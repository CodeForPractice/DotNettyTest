using DotNetRpc.Socketing.BufferManagement;
using DotNetRpc.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetRpc.Socketing
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：ClientSocket.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/11/21 14:51:31
    /// </summary>
    public class ClientSocket
    {
        private EndPoint _hostPoint;
        private Socket _socket;
        private int _receiveBufferSize = 60 * 1024;
        private int _sendBufferSize = 60 * 1024;
        private readonly ManualResetEvent _manualResetEvent;
        private long _pendingMessageCount;
        private int _isClosed = 0;
        private int _sending = 0;

        private MemoryStream _sendingStream;

        private SocketAsyncEventArgs _sendSockerArg;
        private SocketAsyncEventArgs _receiveSocketArg;

        private ConcurrentQueue<IEnumerable<ArraySegment<byte>>> _sendingMessageQueue = new ConcurrentQueue<IEnumerable<ArraySegment<byte>>>();

        public ClientSocket(EndPoint hostPoint)
        {
            _hostPoint = hostPoint;
            _socket = SocketUtil.Create(_sendBufferSize, _receiveBufferSize);
            _manualResetEvent = new ManualResetEvent(false);
            _sendingStream = new MemoryStream();

            _sendSockerArg = new SocketAsyncEventArgs();
            _sendSockerArg.AcceptSocket = _socket;
            _sendSockerArg.Completed += SendSockerArg_Completed;

            _receiveSocketArg = new SocketAsyncEventArgs();
            _receiveSocketArg.AcceptSocket = _socket;
            _receiveSocketArg.Completed += ReceiveSocketArg_Completed;
        }


        public ClientSocket Start()
        {
            var socketArgs = new SocketAsyncEventArgs();
            socketArgs.AcceptSocket = _socket;
            socketArgs.RemoteEndPoint = _hostPoint;
            socketArgs.Completed += Connected_Completed;
            if (_hostPoint != null)
            {
                _socket.Bind(_hostPoint);
            }
            var connectArgs = _socket.ConnectAsync(socketArgs);
            if (!connectArgs)
            {
                ProccessConnected(socketArgs);
            }
            _manualResetEvent.WaitOne();
            return this;
        }

        private void Connected_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProccessConnected(e);
        }

        private void ProccessConnected(SocketAsyncEventArgs e)
        {
            e.Completed -= Connected_Completed;
            e.AcceptSocket = null;
            e.RemoteEndPoint = null;
            e.Dispose();
            if (e.SocketError != SocketError.Success)
            {
                _socket.ShutDownCurrent();
                LogUtil.Info($"socket connet failed.socket error:{e.SocketError}");
                _manualResetEvent.Set();
                return;
            }
            LogUtil.Info($"socket connected,remote endpoint:{_socket.RemoteEndPoint},local endpoint:{_socket.LocalEndPoint}");
            _manualResetEvent.Set();
        }

        public ClientSocket SendMessage(byte[] message)
        {
            if (message == null || message.Length <= 0)
            {
                return this;
            }
            var segmentList = MessageUtil.Packet(message);
            _sendingMessageQueue.Enqueue(segmentList);
            Interlocked.Increment(ref _pendingMessageCount);
            TrySend();
            return this;
        }

        private void TrySend()
        {
            if (_isClosed == 1)
            {
                return;
            }
            if (EnterSending())
            {
                Send();
            }
        }

        private void Send()
        {
            Task.Factory.StartNew(() =>
            {
                _sendingStream.SetLength(0);
                IEnumerable<ArraySegment<byte>> segments;
                while (_sendingMessageQueue.TryDequeue(out segments))
                {
                    Interlocked.Decrement(ref _pendingMessageCount);
                    foreach (var item in segments)
                    {
                        _sendingStream.Write(item.Array, item.Offset, item.Count);
                    }
                    if (_sendingStream.Length >= _sendBufferSize)
                    {
                        break;
                    }
                    if (_sendingStream.Length == 0)
                    {
                        ExitSend();
                        if (_sendingMessageQueue.Count > 0)
                        {
                            TrySend();
                        }
                        return;
                    }

                    try
                    {
                        _sendSockerArg.SetBuffer(_sendingStream.GetBuffer(), 0, (int)_sendingStream.Length);
                        var isSendSuccess = _sendSockerArg.AcceptSocket.SendAsync(_sendSockerArg);
                        if (!isSendSuccess)
                        {
                            ProcessSend(_sendSockerArg);
                        }
                    }
                    catch (Exception ex)
                    {
                        CloseInternal(SocketError.Shutdown, "Socket send error, errorMessage:" + ex.Message, ex);
                        ExitSend();
                    }
                }
            });

        }

        private void SendSockerArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessSend(e);
        }

        private void ProcessSend(SocketAsyncEventArgs e)
        {
            if (_isClosed == 1)
            {
                return;
            }
            if (e.Buffer != null)
            {
                e.SetBuffer(null, 0, 0);
            }
            ExitSend();
            if (e.SocketError == SocketError.Success)
            {
                TrySend();
            }
            else
            {
                CloseInternal(SocketError.Shutdown, "Socket send error.", null);
            }
        }

        private bool EnterSending()
        {
            return Interlocked.CompareExchange(ref _sending, 1, 0) == 0;
        }

        private void ExitSend()
        {
            Interlocked.Exchange(ref _sending, 0);
        }


        private void ReceiveSocketArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessReceived(e);
        }

        private void ProcessReceived(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred == 0 || e.SocketError != SocketError.Success)
            {
                CloseInternal(e.SocketError, e.SocketError != SocketError.Success ? "Socket receive error" : "Socket normal close", null);
                return;
            }
        }



        public void Close()
        {
            CloseInternal(SocketError.Success, "socket normal close", null);
        }


        private void CloseInternal(SocketError socketError, string reason, Exception exception)
        {
            if (Interlocked.CompareExchange(ref _isClosed, 1, 0) == 0)
            {

                _sendSockerArg?.DisposeCurrent(SendSockerArg_Completed);
                _receiveSocketArg?.DisposeCurrent(ReceiveSocketArg_Completed);
                _socket.ShutDownCurrent();
            }
        }
    }
}

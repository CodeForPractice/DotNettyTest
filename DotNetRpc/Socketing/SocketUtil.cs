using DotNetRpc.Utils;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace DotNetRpc.Socketing
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：SocketUtil.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/11/21 14:31:32
    /// </summary>
    public static class SocketUtil
    {
        public static IPAddress GetLocalIPV4()
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(m => m.AddressFamily == AddressFamily.InterNetwork);
        }

        public static Socket Create(int sendBufferSize, int receiveBufferSize)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                NoDelay = true,
                Blocking = false,
                SendBufferSize = sendBufferSize,
                ReceiveBufferSize = receiveBufferSize
            };
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            return socket;
        }

        public static void ShutDownCurrent(this Socket socket)
        {
            if (socket != null)
            {
                ExceptionUtil.EatException(() => socket.Shutdown(SocketShutdown.Both));
                socket.CloseCurrent();
            }
        }

        public static void CloseCurrent(this Socket socket)
        {
            if (socket != null)
            {
                ExceptionUtil.EatException(() => socket.Close(10000));
            }
        }

        public static void DisposeCurrent(this SocketAsyncEventArgs e, EventHandler<SocketAsyncEventArgs> Completed)
        {
            if (e != null)
            {
                ExceptionUtil.EatException(() =>
                {
                    e.Completed -= Completed;
                    e.AcceptSocket = null;
                    e.Dispose();
                });

            }
        }
    }
}
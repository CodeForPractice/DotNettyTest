using DotNetRpc.Message;
using DotNetRpc.Serializer;
using DotNetRpc.TransPort;
using DotNetRpc.Utils;
using DotNetty.Buffers;
using System.Net;
using System.Text;

namespace DotNetRpc
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：RpcClient.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/11/20 10:12:27
    /// </summary>
    public class RpcClient : SelfDisposable
    {
        private bool _isSsl = false;
        private string _certificateFileName;
        private string _certificatePwd;
        private int _bindPort;
        private IPAddress _hostIPAddress;
        private DotNetty.Transport.Channels.IChannel _channel;

        public RpcClient(IPAddress hostIPAddress, int port)
        {
            EnsureUtil.NotNull(hostIPAddress, "hostIPAddress");
            EnsureUtil.Positive(port, "port");
            _hostIPAddress = hostIPAddress;
            _bindPort = port;
        }

        public RpcClient(IPAddress hostIPAddress, int port, bool isSSL, string certificateFileName, string certificatePwd) : this(hostIPAddress, port)
        {
            if (isSSL)
            {
                _isSsl = true;
                _certificateFileName = certificateFileName;
                _certificatePwd = certificatePwd;
            }
        }

        /// <summary>
        /// 获取当前已打开通道
        /// </summary>
        private DotNetty.Transport.Channels.IChannel CurrentChannel
        {
            get
            {
                if (_channel == null)
                {
                    var bootStarp = DotNettyUtil.CreateClientBootstrap("NetRpc", new RpcClientChannelHandle(), isSSL: _isSsl, certificateFileName: _certificateFileName, certificatePwd: _certificatePwd);
                    var task = bootStarp.ConnectAsync(new IPEndPoint(_hostIPAddress, _bindPort));
                    task.Wait();
                    _channel = task.Result;
                }
                return _channel;
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ResponseMessage Send(RequestMessage message)
        {
            SendMessage(message);
            return RpcMessageUtil.GetCallBackMessage(message.MessageId, 5000);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="messageContent">消息内容</param>
        /// <returns>返回消息信息</returns>
        public ResponseMessage Send(string messageContent)
        {
            var requestMessage = new RequestMessage()
            {
                IsNeedReply = true
            };
            SendMessage(requestMessage);
            return RpcMessageUtil.GetCallBackMessage(requestMessage.MessageId, 5000);
        }

        /// <summary>
        /// 发送不需要回复的信息
        /// </summary>
        /// <param name="messageContent">消息内容</param>
        public void SendNotReplayMessage(string messageContent)
        {
            var requestMessage = new RequestMessage()
            {
                IsNeedReply = false
            };
            SendMessage(requestMessage);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="requestMessage">消息信息</param>
        private void SendMessage(RequestMessage requestMessage)
        {
            var bytes = requestMessage.ObjectToBytes();
            IByteBuffer buffer = Unpooled.WrappedBuffer(bytes);
            CurrentChannel.WriteAndFlushAsync(buffer).Wait();
        }

        protected override void DisposeCode()
        {
            _channel.CloseAsync().Wait();
        }
    }
}
using DotNetRpc.TransPort;
using DotNetRpc.Utils;

namespace DotNetRpc
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：RpcServer.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/11/20 13:25:48
    /// </summary>
    public class RpcServer : SelfDisposable
    {
        private bool _isSsl = false;
        private string _certificateFileName;
        private string _certificatePwd;
        private int _bindPort;
        private DotNetty.Transport.Channels.IChannel _channel;

        public RpcServer(int port)
        {
            EnsureUtil.Positive(port, "port");
            _bindPort = port;
        }

        public RpcServer(int port, bool isSSL, string certificateFileName, string certificatePwd) : this(port)
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
                    var bootStarp = DotNettyUtil.CreateServerBootstrap("NetRpc", new RpcServerChannelHandle(), isSSL: _isSsl, certificateFileName: _certificateFileName, certificatePwd: _certificatePwd);
                    var task = bootStarp.BindAsync(_bindPort);
                    task.Wait();
                    _channel = task.Result;
                }
                return _channel;
            }
        }

        /// <summary>
        /// 开启
        /// </summary>
        public void Start()
        {
            LogUtil.Debug($"开始监听端口:{_bindPort.ToString()}");
            var channel = CurrentChannel;
        }

        protected override void DisposeCode()
        {
            _channel?.CloseAsync().Wait();
        }
    }
}
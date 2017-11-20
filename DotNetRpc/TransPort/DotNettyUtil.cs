using DotNetRpc.Utils;
using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Handlers.Tls;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace DotNetRpc.TransPort
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：DotNettyUtil.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/11/20 10:22:19
    /// </summary>
    public sealed class DotNettyUtil
    {
        /// <summary>
        /// 创建客户端启动类
        /// </summary>
        /// <typeparam name="T">消息处理类</typeparam>
        /// <param name="name">绑定通道名字</param>
        /// <param name="handler">处理实例</param>
        /// <param name="isSSL">是否ssl</param>
        /// <param name="certificateFileName">证书路径</param>
        /// <param name="certificatePwd">证书密码</param>
        /// <returns>客户端启动类</returns>
        public static Bootstrap CreateClientBootstrap<T>(string name, T handler, bool isSSL = false, string certificateFileName = null, string certificatePwd = null) where T : ChannelHandlerAdapter
        {
            var gtoup = new MultithreadEventLoopGroup();
            var bootstrap = new Bootstrap();
            bootstrap.Group(gtoup)
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.TcpNodelay, true)
                .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                {
                    IChannelPipeline channelPipeline = channel.Pipeline;
                    if (isSSL)
                    {
                        X509Certificate2 cert = null;
                        string targetHost = null;
                        cert = new X509Certificate2(certificateFileName, certificatePwd);
                        targetHost = cert.GetNameInfo(X509NameType.DnsName, false);
                        channelPipeline.AddLast("tls", new TlsHandler(stream => new SslStream(stream, true, (sender, certificate, chain, errors) => true), new ClientTlsSettings(targetHost)));
                    }
                    channelPipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
                    channelPipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));
                    channelPipeline.AddLast(name, handler);
                }));
            return bootstrap;
        }

        /// <summary>
        /// 创建服务端启动类
        /// </summary>
        /// <typeparam name="T">消息处理类</typeparam>
        /// <param name="name">绑定通道名字</param>
        /// <param name="handler">处理实例</param>
        /// <param name="isSSL">是否ssl</param>
        /// <param name="certificateFileName">证书路径</param>
        /// <param name="certificatePwd">证书密码</param>
        /// <returns>服务端启动类</returns>
        public static ServerBootstrap CreateServerBootstrap<T>(string name, T handler, bool isSSL = false, string certificateFileName = null, string certificatePwd = null) where T : ChannelHandlerAdapter
        {
            IEventLoopGroup bossGroup = new MultithreadEventLoopGroup(100);
            IEventLoopGroup workerGroup = new MultithreadEventLoopGroup(5);
            try
            {
                var bootstrap = new ServerBootstrap();
                bootstrap.Group(bossGroup, workerGroup);
                bootstrap
                    .Channel<TcpServerSocketChannel>()
                    .Option(ChannelOption.SoBacklog, 100)
                    .Handler(new LoggingHandler(LogLevel.INFO))
                    .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;
                        if (isSSL)
                        {
                            X509Certificate2 cert = null;
                            cert = new X509Certificate2(certificateFileName, certificatePwd);
                            pipeline.AddLast("tls", TlsHandler.Server(cert));
                        }
                        pipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
                        pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));
                        pipeline.AddLast(name, handler);
                    }));
                return bootstrap;
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                Task.WaitAll(bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                   workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
                throw;
            }
        }
    }
}
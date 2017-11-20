using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Handlers.Tls;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Infrastructrue;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Rpc.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            RunClient().Wait();

        }

        public static async Task RunClient()
        {
            var gtoup = new MultithreadEventLoopGroup();
            X509Certificate2 cert = null;
            string targetHost = null;
            if (ClientSetting.IsSsl)
            {
                cert = new X509Certificate2(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dotnetty.com.pfx"), "password");
                targetHost = cert.GetNameInfo(X509NameType.DnsName, false);
            }

            var bootstrap = new Bootstrap();
            bootstrap.Group(gtoup)
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.TcpNodelay, true)
                .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                {
                    IChannelPipeline channelPipeline = channel.Pipeline;
                    if (cert != null)
                    {
                        channelPipeline.AddLast("tls", new TlsHandler(stream => new SslStream(stream, true, (sender, certificate, chain, errors) => true), new ClientTlsSettings(targetHost)));
                    }
                    channelPipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
                    channelPipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));
                    channelPipeline.AddLast("echo", new RpcClient(4000));
                }));

            Console.WriteLine("输入任意字符开始发送");
            Console.ReadLine();
            IChannel clientChannel = await bootstrap.ConnectAsync(new IPEndPoint(ClientSetting.Host, ClientSetting.Port));
            Parallel.For(0, 10, i =>
            {
                var requestMessage = new RequestMessage();
                requestMessage.MessageContent = i.ToString();
                requestMessage.IsNeedReply = i % 3 == 0;
                byte[] bytes = Encoding.UTF8.GetBytes(requestMessage.ToJson());
                IByteBuffer buffer = Unpooled.WrappedBuffer(bytes);
                Console.WriteLine("Send " + requestMessage.ToJson());
                clientChannel.WriteAndFlushAsync(buffer).Wait();
                var responseMessage = RpcMessageUtil.GetCallBackMessage(requestMessage.MessageId, 5000);
                if (responseMessage == null)
                {
                    Console.WriteLine("未收到反馈:" + requestMessage.MessageId);
                }
                else
                    Console.WriteLine("Receive " + responseMessage.ToJson());
            });

            await clientChannel.CloseAsync();
            Console.ReadLine();

        }
    }
}

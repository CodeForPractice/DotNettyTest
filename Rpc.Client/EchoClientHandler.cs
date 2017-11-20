using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpc.Client
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：EchoClientHandler.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/11/17 15:07:26
    /// </summary>
    public class EchoClientHandler : ChannelHandlerAdapter
    {
        readonly IByteBuffer _byteBuffer;
        public EchoClientHandler()
        {
            this._byteBuffer = Unpooled.Buffer(ClientSetting.Size);
            byte[] messageBytes = Encoding.UTF8.GetBytes("Hello world");
            this._byteBuffer.WriteBytes(messageBytes);
        }

        public override void ChannelActive(IChannelHandlerContext context) => context.WriteAndFlushAsync(this._byteBuffer);

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var byteBuffer = message as IByteBuffer;
            if (byteBuffer != null)
            {
                Console.WriteLine("Received from server: " + byteBuffer.ToString(Encoding.UTF8));
            }
            context.WriteAsync(message);
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }
}

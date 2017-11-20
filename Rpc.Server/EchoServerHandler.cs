using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructrue;

namespace Rpc.Server
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：EchoServerHandler.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/11/17 15:18:16
    /// </summary>
    public class EchoServerHandler : ChannelHandlerAdapter
    {
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var buffer = message as IByteBuffer;
            if (buffer != null)
            {
                var messageStr = buffer.ToString(Encoding.UTF8);
                Console.WriteLine("Receive "+messageStr);
                var requestMessage = messageStr.ToObj<RequestMessage>();
                var responseMessage = new ResponseMessage();
                responseMessage.MessageId = requestMessage.MessageId;
                responseMessage.MessageContent= requestMessage.MessageContent;
                responseMessage.IsNeedReply= requestMessage.IsNeedReply;

                byte[] bytes = Encoding.UTF8.GetBytes(responseMessage.ToJson());
                IByteBuffer responsebuffer = Unpooled.WrappedBuffer(bytes);
                context.WriteAsync(responsebuffer);
            }
            //context.WriteAsync(message);
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }
}

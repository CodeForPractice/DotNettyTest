using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using System.Text;

namespace Infrastructrue
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：RpcClient.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/11/17 16:12:16
    /// </summary>
    public class RpcClient : ChannelHandlerAdapter
    {
        private readonly IByteBuffer _byteBuffer;

        public RpcClient(int bufferSize)
        {
            RequestMessage requestMessage = new RequestMessage() { MessageContent = "Connect Success", IsNeedReply = false };
            this._byteBuffer = Unpooled.Buffer(bufferSize);
            byte[] messageBytes = Encoding.UTF8.GetBytes(requestMessage.ToJson());
            this._byteBuffer.WriteBytes(messageBytes);
        }

        public override void ChannelActive(IChannelHandlerContext context) => context.WriteAndFlushAsync(this._byteBuffer);

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var byteBuffer = message as IByteBuffer;
            if (byteBuffer != null)
            {
                var str = byteBuffer.ToString(Encoding.UTF8);
                var responseMessage = str.ToObj<ResponseMessage>();
                if (responseMessage.IsNeedReply)
                {
                    RpcMessageUtil.AddCallBackMessage(responseMessage.MessageId, responseMessage);
                }
            }
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }
}
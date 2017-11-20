using DotNetRpc.Message;
using DotNetRpc.Serializer;
using DotNetRpc.Utils;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using System.Text;

namespace DotNetRpc
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：RpcClientChannelHandle.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/11/20 10:46:58
    /// </summary>
    public class RpcClientChannelHandle : ChannelHandlerAdapter
    {
        public RpcClientChannelHandle()
        {
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var byteBuffer = message as IByteBuffer;
            if (byteBuffer != null)
            {
                var str = byteBuffer.ToString(Encoding.UTF8);
                LogUtil.Debug("Receive:" + str);
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
            LogUtil.Error(exception, "ExceptionCaught");
            context.CloseAsync();
        }
    }
}
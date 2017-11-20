using DotNetRpc.Message;
using DotNetRpc.Serializer;
using DotNetRpc.Utils;
using DotNetRpcDependency;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using System.Text;

namespace DotNetRpc
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：RpcServerChannelHandle.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/11/20 13:19:52
    /// </summary>
    public class RpcServerChannelHandle : ChannelHandlerAdapter
    {
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var buffer = message as IByteBuffer;
            if (buffer != null)
            {
                var receiveBytes = new Byte[buffer.ReadableBytes];
                buffer.ReadBytes(receiveBytes);
                var requestMessage = receiveBytes.BytesToObject<RequestMessage>();
                var responseMessage = new ResponseMessage();
                responseMessage.MessageId = requestMessage.MessageId;
                responseMessage.JsonResult = CallMethod(requestMessage).ToJson();
                responseMessage.IsNeedReply = requestMessage.IsNeedReply;
                var replyStr = responseMessage.ToJson();
                LogUtil.Debug("Reply:" + responseMessage.JsonResult);
                byte[] bytes = Encoding.UTF8.GetBytes(replyStr);
                IByteBuffer responsebuffer = Unpooled.WrappedBuffer(bytes);
                context.WriteAsync(responsebuffer);
            }
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            LogUtil.Error(exception, "ExceptionCaught");
            context.CloseAsync();
        }

        public object CallMethod(RequestMessage requestMessage)
        {
            if (requestMessage == null || requestMessage.MethodCallInfo == null)
            {
                return null;
            }
            using (var scope = ContainerManager.BeginLeftScope())
            {
                var instanceType = Type.GetTypeFromHandle(requestMessage.MethodCallInfo.TypeHandle);
                var instance = scope.Resolve(instanceType);
                var method = instanceType.GetMethod(requestMessage.MethodCallInfo.MethodName, requestMessage.MethodCallInfo.ArgumentTypes);
                if (method != null)
                {
                    return method.Invoke(instance, requestMessage.MethodCallInfo.Parameters);
                }
            }
            return null;
        }
    }
}
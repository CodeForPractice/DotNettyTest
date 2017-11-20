using System;

namespace DotNetRpc.Message
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：RequestMessage.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/11/20 11:28:41
    /// </summary>
    [Serializable]
    public class RequestMessage
    {
        public RequestMessage()
        {
            MessageId = Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 消息ID
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// 是否需要回复
        /// </summary>
        public bool IsNeedReply { get; set; } = true;

        /// <summary>
        /// 消息内容
        /// </summary>
        public MethodCallInfo MethodCallInfo { get; set; }
    }
}
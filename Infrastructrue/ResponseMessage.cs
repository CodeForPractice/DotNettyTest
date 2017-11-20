namespace Infrastructrue
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：ResponseMessage.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/11/17 16:41:19
    /// </summary>
    public sealed class ResponseMessage
    {
        public string MessageId { get; set; }

        public bool IsNeedReply { get; set; }

        public string MessageContent { get; set; }
    }
}
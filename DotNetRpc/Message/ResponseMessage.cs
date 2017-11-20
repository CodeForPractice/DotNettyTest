namespace DotNetRpc.Message
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：ResponseMessage.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/11/20 11:07:53
    /// </summary>
    public class ResponseMessage
    {
        /// <summary>
        /// 消息ID（唯一）
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// 是否需要回复
        /// </summary>
        public bool IsNeedReply { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        public string JsonResult { get; set; }
    }
}
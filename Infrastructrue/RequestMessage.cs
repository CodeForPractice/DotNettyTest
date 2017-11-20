using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructrue
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：RequestMessage.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/11/17 16:30:24
    /// </summary>
    public class RequestMessage
    {
        public RequestMessage()
        {
            MessageId = Guid.NewGuid().ToString("N");
        }

        public string MessageId { get; set; }

        public bool IsNeedReply { get; set; } = true;

        public string MessageContent { get; set; }
    }
}

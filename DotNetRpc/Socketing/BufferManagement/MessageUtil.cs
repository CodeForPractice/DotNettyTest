using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetRpc.Socketing.BufferManagement
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：MessageUtil.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/11/21 15:43:37
    /// </summary>
    public class MessageUtil
    {
        /// <summary>
        /// 将消息进行封包处理
        /// </summary>
        /// <param name="messageBytes">处理的消息</param>
        /// <returns></returns>
        public static IEnumerable<ArraySegment<byte>> Packet(byte[] messageBytes)
        {
            if (messageBytes == null)
            {
                throw new ArgumentNullException("messageBytes");
            }
            var messageLength = messageBytes.Length;
            var data = new ArraySegment<byte>(messageBytes, 0, messageLength);
            yield return new ArraySegment<byte>(BitConverter.GetBytes(messageLength));
            yield return data;
        }
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetRpc.Message
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：RpcMessageUtil.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/11/20 11:21:16
    /// </summary>
    public sealed class RpcMessageUtil
    {
        internal static List<ConcurrentDictionary<string, ResponseMessage>> MessageList = new List<ConcurrentDictionary<string, ResponseMessage>>();
        static RpcMessageUtil()
        {
            for (int i = 0; i < 10; i++)
            {
                MessageList.Add(new ConcurrentDictionary<string, ResponseMessage>());
            }
        }

        /// <summary>
        /// 获取返回信息
        /// </summary>
        /// <param name="messageId">发送消息ID</param>
        /// <param name="millisecondsTimeout">超时时间</param>
        /// <returns>返回信息</returns>
        public static ResponseMessage GetCallBackMessage(string messageId, int millisecondsTimeout)
        {
            var cancelToken = new CancellationTokenSource(millisecondsTimeout);
            try
            {
                var task = Task.Run(() =>
                {
                    var concurrentDictionary = MessageList[GetMessageIdHashCode(messageId)];
                    while (!concurrentDictionary.ContainsKey(messageId))
                    {
                        if (!cancelToken.Token.IsCancellationRequested)
                        {
                            Thread.Sleep(1);
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (!cancelToken.Token.IsCancellationRequested)
                    {
                        concurrentDictionary.TryRemove(messageId, out ResponseMessage messageBytes);
                        return messageBytes;
                    }
                    else
                    {
                        return null;
                    }
                }, cancelToken.Token);
                task.Wait();
                return task.Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                cancelToken.Cancel();
                return null;
            }
        }

        /// <summary>
        /// 添加返回信息
        /// </summary>
        /// <param name="messageId">消息ID</param>
        /// <param name="responseMessage">返回内容</param>
        public static void AddCallBackMessage(string messageId, ResponseMessage responseMessage)
        {
            var concurrentDictionary = MessageList[GetMessageIdHashCode(messageId)];
            concurrentDictionary.TryAdd(messageId, responseMessage);
        }

        /// <summary>
        /// 获取消息ID的HashCode
        /// </summary>
        /// <param name="messageId">消息ID</param>
        /// <returns>HashCode</returns>
        private static int GetMessageIdHashCode(string messageId)
        {
            return Math.Abs(messageId.GetHashCode()) % 10;
        }
    }
}
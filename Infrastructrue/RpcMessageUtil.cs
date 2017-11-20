using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructrue
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：RpcMessageUtil.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/11/17 16:16:53
    /// </summary>
    public class RpcMessageUtil
    {
        internal static ConcurrentDictionary<string, ResponseMessage> concurrentDictionary = new ConcurrentDictionary<string, ResponseMessage>();

        public static ResponseMessage GetCallBackMessage(string messageId, int millisecondsTimeout)
        {
            var cancelToken = new CancellationTokenSource(millisecondsTimeout);
            try
            {
                var task = Task.Run(() =>
                 {
                     while (!concurrentDictionary.ContainsKey(messageId))
                     {
                         if (!cancelToken.Token.IsCancellationRequested)
                         {
                             System.Threading.Thread.Sleep(1);
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

        public static void AddCallBackMessage(string messageId, ResponseMessage messageBytes)
        {
            concurrentDictionary.TryAdd(messageId, messageBytes);
        }
    }
}

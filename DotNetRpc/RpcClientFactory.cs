using DotNetRpc.Utils;
using System.Collections.Concurrent;
using System.Net;

namespace DotNetRpc
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：RpcClientFactory.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/11/20 14:16:49
    /// </summary>
    public sealed class RpcClientFactory
    {
        internal static ConcurrentDictionary<string, RpcClient> ClientCache = new ConcurrentDictionary<string, RpcClient>();

        /// <summary>
        /// 根据类型名字获取Client
        /// </summary>
        /// <param name="typeName">类型名字</param>
        /// <returns></returns>
        public static RpcClient GetClient(string typeName)
        {
            LogUtil.Debug($"TypeName:{typeName}");
            return GetClient("127.0.0.1", 11025);
        }

        /// <summary>
        /// 获取Rpc客户端
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static RpcClient GetClient(string ipAddress, int port)
        {
            string key = $"{ipAddress}:{port.ToString()}";
            if (ClientCache.ContainsKey(key))
            {
                return ClientCache[key];
            }
            else
            {
                var rpcClient = new RpcClient(IPAddress.Parse(ipAddress), port);
                ClientCache[key] = rpcClient;
                return rpcClient;
            }
        }

        public static void Close()
        {
            foreach (var item in ClientCache)
            {
                item.Value?.Dispose();
            }
        }
    }
}
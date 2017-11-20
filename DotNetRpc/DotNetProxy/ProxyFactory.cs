using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetRpc.DotNetProxy
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：ProxyFactory.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/11/20 15:01:52
    /// </summary>
    public class ProxyFactory
    {
        public static T Create<T>()
        {
            return (T)(new RpcProxyImpl(typeof(T)).GetTransparentProxy());
        }
    }
}

using System;

namespace DotNetRpc.Utils
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：MethodUtil.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/11/20 14:32:35
    /// </summary>
    public sealed class MethodUtil
    {
        /// <summary>
        /// 获取全部的参数类型
        /// </summary>
        /// <param name="arrArgs">参数列表</param>
        /// <returns>全部的参数类型</returns>
        public static Type[] GetArgTypes(object[] arrArgs)
        {
            if (null == arrArgs)
            {
                return new Type[0];
            }
            Type[] result = new Type[arrArgs.Length];
            for (int i = 0; i < result.Length; ++i)
            {
                if (arrArgs[i] == null)
                {
                    result[i] = null;
                }
                else
                {
                    result[i] = arrArgs[i].GetType();
                }
            }
            return result;
        }
    }
}
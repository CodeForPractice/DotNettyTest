using System;

namespace DotNetRpc.Utils
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：EnsureUtil.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/11/20 10:31:39
    /// </summary>
    public static class EnsureUtil
    {
        public static void NotNull<T>(T argument, string argumentName) where T : class
        {
            if (argument == null)
                throw new ArgumentNullException(argumentName + " should not be null.");
        }

        public static void Positive(int number, string argumentName)
        {
            if (number <= 0)
                throw new ArgumentOutOfRangeException(argumentName, argumentName + " should be positive.");
        }
    }
}
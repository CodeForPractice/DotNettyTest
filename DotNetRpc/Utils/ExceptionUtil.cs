using System;

namespace DotNetRpc.Utils
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：ExceptionUtil.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/11/21 13:09:13
    /// </summary>
    public static class ExceptionUtil
    {
        public static void EatException(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                LogUtil.Warn(ex.ToErrMsg());
            }
        }

        public static T EatException<T>(Func<T> func, T defaultValue = default(T))
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                LogUtil.Warn(ex.ToErrMsg());
                return defaultValue;
            }
        }
    }
}
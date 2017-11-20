namespace DotNetRpc.Logger
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：ILoggerFactory.cs
    /// 接口属性：公共
    /// 类功能描述：ILoggerFactory接口
    /// 创建标识：yjq 2017/11/20 13:09:54
    /// </summary>
    public interface ILoggerFactory
    {
        /// <summary>
        /// 根据loggerName创建ILogger
        /// </summary>
        /// <param name="loggerName">logger名字</param>
        /// <returns>ILogger</returns>
        ILogger Create(string loggerName);
    }
}
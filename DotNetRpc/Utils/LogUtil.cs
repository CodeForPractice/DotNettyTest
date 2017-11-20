using DotNetRpc.Logger;
using DotNetRpc.Logger.NLogger;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace DotNetRpc.Utils
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：LogUtil.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：日志工具类
    /// 创建标识：yjq 2017/11/20 12:01:49
    /// </summary>
    public static class LogUtil
    {
        #region ILogger

        /// <summary>
        /// 获取ILogger
        /// </summary>
        /// <param name="loggerName">记录器名字</param>
        /// <returns>ILogger</returns>
        private static IEnumerable<ILogger> GetLogger(string loggerName = null)
        {
            if (string.IsNullOrWhiteSpace(loggerName))
            {
                loggerName = "DotNetRpc.*";
            }
            yield return new NLoggerFactory().Create(loggerName);
        }

        #endregion ILogger

        /// <summary>
        /// 输出调试日志信息
        /// </summary>
        /// <param name="msg">日志内容</param>
        public static void Debug(string msg, string loggerName = null)
        {
            foreach (var logger in GetLogger(loggerName: loggerName))
            {
                logger.Debug(msg);
            }
        }

        /// <summary>
        /// 输出普通日志信息
        /// </summary>
        /// <param name="msg">日志内容</param>
        public static void Info(string msg, string loggerName = null)
        {
            foreach (var logger in GetLogger(loggerName: loggerName))
            {
                logger.Info(msg);
            }
        }

        /// <summary>
        /// 输出警告日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="loggerName"></param>
        public static void Warn(string msg, string loggerName = null)
        {
            foreach (var logger in GetLogger(loggerName: loggerName))
            {
                logger.Warn(msg);
            }
        }

        /// <summary>
        /// 输出警告日志信息
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="memberName"></param>
        /// <param name="loggerName"></param>
        public static void Warn(Exception ex, [CallerMemberName]string memberName = null, string loggerName = null)
        {
            foreach (var logger in GetLogger(loggerName: loggerName))
            {
                logger.Warn(ex.ToErrMsg(memberName: memberName));
            }
        }

        /// <summary>
        /// 输出错误日志信息
        /// </summary>
        /// <param name="msg">日志内容</param>
        public static void Error(string msg, string loggerName = null)
        {
            foreach (var logger in GetLogger(loggerName: loggerName))
            {
                logger.Error(msg);
            }
        }

        /// <summary>
        /// 输出错误日志信息
        /// </summary>
        /// <param name="ex">异常信息</param>
        public static void Error(Exception ex, [CallerMemberName]string memberName = null, string loggerName = null)
        {
            foreach (var logger in GetLogger(loggerName: loggerName))
            {
                logger.Error(ex.ToErrMsg(memberName: memberName));
            }
        }

        /// <summary>
        /// 输出严重错误日志信息
        /// </summary>
        /// <param name="msg">日志内容</param>
        public static void Fatal(string msg, string loggerName = null)
        {
            foreach (var logger in GetLogger(loggerName: loggerName))
            {
                logger.Fatal(msg);
            }
        }

        /// <summary>
        /// 输出严重错误日志信息
        /// </summary>
        /// <param name="ex">异常信息</param>
        public static void Fatal(Exception ex, [CallerMemberName]string memberName = null, string loggerName = null)
        {
            foreach (var logger in GetLogger(loggerName: loggerName))
            {
                logger.Fatal(ex.ToErrMsg(memberName: memberName));
            }
        }

        /// <summary>
        /// 获取错误异常信息
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="memberName">出现异常的方法名字</param>
        /// <returns>错误异常信息</returns>
        public static string ToErrMsg(this Exception ex, [CallerMemberName]string memberName = null)
        {
            StringBuilder errorBuilder = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(memberName))
            {
                errorBuilder.AppendFormat("CallerMemberName：{0}", memberName).AppendLine();
            }
            errorBuilder.AppendFormat("Message：{0}", ex.Message).AppendLine();
            if (ex.InnerException != null)
            {
                if (!string.Equals(ex.Message, ex.InnerException.Message, StringComparison.OrdinalIgnoreCase))
                {
                    errorBuilder.AppendFormat("InnerException：{0}", ex.InnerException.Message).AppendLine();
                }
            }
            errorBuilder.AppendFormat("Source：{0}", ex.Source).AppendLine();
            errorBuilder.AppendFormat("StackTrace：{0}", ex.StackTrace).AppendLine();
            return errorBuilder.ToString();
        }
    }
}
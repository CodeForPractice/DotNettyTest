using System;

namespace DotNetRpc.Message
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：MethodCallInfo.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：调用方法信息
    /// 创建标识：yjq 2017/11/20 14:38:06
    /// </summary>
    [Serializable]
    public class MethodCallInfo
    {
        public MethodCallInfo() { }

        public RuntimeTypeHandle TypeHandle { get; set; }
        public string ClassName { get; set; }

        public string MethodName { get; set; }

        public object[] Parameters { get; set; }

        public Type[] ArgumentTypes { get; set; }
    }
}
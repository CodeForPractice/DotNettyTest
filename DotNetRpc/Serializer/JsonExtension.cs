using System;

namespace DotNetRpc.Serializer
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：JsonExtension.cs
    /// 类属性：公共类（静态）
    /// 类功能描述：json扩展类
    /// 创建标识：yjq 2017/11/20 11:16:03
    /// </summary>
    public static class JsonExtension
    {
        /// <summary>
        /// 转为json格式的字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象实例</param>
        /// <returns>json格式的字符串</returns>
        public static string ToJson<T>(this T obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 将json格式的字符串转为对象
        /// </summary>
        /// <param name="jsonStr">json格式的字符串</param>
        /// <param name="instanceType">实例类型</param>
        /// <returns></returns>
        public static object ToObj(this string jsonStr, Type instanceType)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(jsonStr, instanceType);
        }

        /// <summary>
        /// 将json格式的字符串转为对象
        /// </summary>
        /// <typeparam name="T">实例类型</typeparam>
        /// <param name="jsonStr">json格式的字符串</param>
        /// <returns></returns>
        public static T ToObj<T>(this string jsonStr)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonStr);
        }
    }
}
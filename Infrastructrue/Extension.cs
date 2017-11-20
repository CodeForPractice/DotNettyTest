namespace Infrastructrue
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：Extension.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/11/17 16:34:05
    /// </summary>
    public static class Extension
    {
        public static string ToJson<T>(this T obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static T ToObj<T>(this string str)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }
    }
}
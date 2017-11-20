using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DotNetRpc.Serializer
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：ByteSerializer.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/11/20 15:24:39
    /// </summary>
    public static class ByteSerializer
    {
        /// <summary> 
        /// 将一个object对象序列化，返回一个byte[]         
        /// </summary> 
        /// <param name="obj">能序列化的对象</param>         
        /// <returns></returns> 
        public static byte[] ObjectToBytes(this object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter(); formatter.Serialize(ms, obj);
                return ms.GetBuffer();
            }
        }

        /// <summary> 
        /// 将一个序列化后的byte[]数组还原         
        /// </summary>
        /// <param name="Bytes"></param>         
        /// <returns></returns> 
        public static T BytesToObject<T>(this byte[] Bytes)
        {
            using (MemoryStream ms = new MemoryStream(Bytes))
            {
                IFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(ms);
            }
        }
    }
}

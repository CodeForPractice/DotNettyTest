using System.Configuration;
using System.Net;

namespace Rpc.Client
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：ClientSetting.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/11/17 14:50:57
    /// </summary>
    public static class ClientSetting
    {
        public static bool IsSsl
        {
            get
            {
                string ssl = ConfigurationManager.AppSettings["ssl"];
                return !string.IsNullOrEmpty(ssl) && bool.Parse(ssl);
            }
        }

        public static IPAddress Host
        {
            get { return IPAddress.Parse(ConfigurationManager.AppSettings["host"]); }
        }

        public static int Port
        {
            get { return int.Parse(ConfigurationManager.AppSettings["port"]); }
        }

        public static int Size
        {
            get { return int.Parse(ConfigurationManager.AppSettings["size"]); }
        }
    }
}
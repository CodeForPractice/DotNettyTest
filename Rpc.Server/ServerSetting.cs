using System.Configuration;

namespace Rpc.Server
{
    /// <summary>
    /// Copyright (C) 2017 yjq 版权所有。
    /// 类名：ServerSetting.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：
    /// 创建标识：yjq 2017/11/17 15:15:51
    /// </summary>
    public static class ServerSetting
    {
        public static bool IsSsl
        {
            get
            {
                string ssl = ConfigurationManager.AppSettings["ssl"];
                return !string.IsNullOrEmpty(ssl) && bool.Parse(ssl);
            }
        }

        public static int Port
        {
            get { return int.Parse(ConfigurationManager.AppSettings["port"]); }
        }
    }
}
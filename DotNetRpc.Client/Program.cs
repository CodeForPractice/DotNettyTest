using DotNetRpc.DotNetProxy;
using DotNetRpc.Message;
using DotNetRpc.TestModel;
using System;

namespace DotNetRpc.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var rpcClient = RpcClientFactory.GetClient("127.0.0.1", 11025);
            //var responseMessage = rpcClient.Send("How Are You?");
            var user = ProxyFactory.Create<IUser>();
            user.GetResponseMesage();
            user.GetResponseMesage(1);
            user.GetResponseMesage("2");
            user.GetResponseMesage(1, "2");
            Console.WriteLine("输入任意键退出");
            Console.ReadLine();
            RpcClientFactory.Close();
        }
    }

}
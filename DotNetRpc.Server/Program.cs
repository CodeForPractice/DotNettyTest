using Autofac;
using DotNetRpc.TestModel;
using DotNetRpcDependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetRpc.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var containerBuilder = new ContainerBuilder();
            ContainerManager.UseAutofacContainer(containerBuilder)
                .RegisterType<IUser, User>(lifeStyle: LifeStyle.PerLifetimeScope)
                ;
            using (RpcServer rpcServer = new RpcServer(11025))
            {
                rpcServer.Start();
                Console.WriteLine("输入任意键退出");
                Console.ReadLine();
            }
        }
    }
}

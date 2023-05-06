using System;
using System.IO;
using Example.Application.Contracts;
using Example.Application.Extensions;
using Example.Infrastructure;

namespace Example.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);// 设置当前工作目录：@".\"

            //JsonHelper.Serializer = NewJsonSerializer.Instance;

            DependencyManager.Register(container =>
            {
                container.AddApplicationDI();
            });

            IMigrationService migrationService = DependencyManager.Container.Resolve<IMigrationService>();
            migrationService.Upgrade();

            Console.WriteLine("Done.");
            Console.ReadLine();
        }
    }
}

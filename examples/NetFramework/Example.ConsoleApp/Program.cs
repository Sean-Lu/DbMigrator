using System;
using System.IO;
using Example.Application.Contracts;
using Example.Application.Extensions;
using Example.Infrastructure;
using Newtonsoft.Json;

namespace Example.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);// 设置当前工作目录：@".\"

            DIManager.Register(container =>
            {
                container.AddApplicationDI();
            });

            IMigrationService migrationService = DIManager.Resolve<IMigrationService>();
            migrationService.Upgrade();

            IMigrationHistoryService migrationHistoryService = DIManager.Resolve<IMigrationHistoryService>();
            var migrationHistoryList = migrationHistoryService.Search(1, 10);
            Console.WriteLine("=========================================================");
            Console.WriteLine($"数据库升级历史（最新10条）：{Environment.NewLine}{JsonConvert.SerializeObject(migrationHistoryList, Formatting.Indented)}");
            Console.WriteLine("=========================================================");

            Console.WriteLine("Done.");
            Console.ReadLine();
        }
    }
}

using System;
using System.IO;
using System.Linq;
using Example.Application.Contracts;
using Example.Application.Extensions;
using Example.Infrastructure;
using Newtonsoft.Json;
using Sean.Core.DbMigrator;

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
            if (!migrationService.Upgrade(DatabaseUpgradeCallback))
            {
                Console.WriteLine("数据库升级失败");
                Console.ReadLine();
                return;
            }

            IMigrationHistoryService migrationHistoryService = DIManager.Resolve<IMigrationHistoryService>();
            var migrationHistoryList = migrationHistoryService.Search(1, 10);
            Console.WriteLine("=========================================================");
            Console.WriteLine($"数据库升级历史（最新10条）：{Environment.NewLine}{JsonConvert.SerializeObject(migrationHistoryList, Formatting.Indented)}");
            Console.WriteLine("=========================================================");

            Console.WriteLine("Done.");
            Console.ReadLine();
        }

        private static void DatabaseUpgradeCallback(MigrationCallbackContext context)
        {
            switch (context.Step)
            {
                case MigrationStep.MigrationNothing:
                    {
                        Console.WriteLine("没有需要执行的数据库升级步骤");
                    }
                    break;
                case MigrationStep.MigrationWaiting:
                    {
                        Console.WriteLine($"[{context.MigrationInfos.Count}]准备执行数据库升级步骤...");
                    }
                    break;
                case MigrationStep.MigrationItemExecuting:
                    {
                        Console.WriteLine($"[{context.MigrationInfoIndex + 1}/{context.MigrationInfos.Count}]正在执行数据库升级步骤[{context.MigrationInfo.Version}]：{context.MigrationInfo.Description}");
                    }
                    break;
                case MigrationStep.MigrationItemExecuted:
                    {
                        Console.WriteLine($"[{context.MigrationInfoIndex + 1}/{context.MigrationInfos.Count}]执行数据库升级步骤{(context.MigrationInfo.Success ? "成功" : "失败")}[{context.MigrationInfo.Version}]：{context.MigrationInfo.Description}");
                    }
                    break;
                case MigrationStep.MigrationComplete:
                    {
                        Console.WriteLine($"数据库升级步骤执行结束：{(!context.MigrationInfos.Any() || context.MigrationInfos.All(c => c.Success) ? "成功" : "失败")}");
                    }
                    break;
            }
        }
    }
}

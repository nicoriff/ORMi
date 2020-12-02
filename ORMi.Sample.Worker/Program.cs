using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ORMi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORMi.Sample.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();

                    services.AddSingleton<IWMIHelper>(new WMIHelper("root\\cimv2"));
                    services.AddSingleton<IWMIWatcher, WMIWatcher>();
                });
    }
}

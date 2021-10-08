using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ORMi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ORMi.Sample.Worker
{
    public class Worker : BackgroundService
    {
        private readonly IWMIHelper _helper;
        private readonly IWMIWatcher _watcher;

        public Worker(IWMIHelper helper, IWMIWatcher watcher)
        {
            _helper = helper;
            _watcher = watcher;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _watcher.Initialize("root\\CimV2", "SELECT * FROM Win32_ProcessStartTrace");
            _watcher.WMIEventArrived += _watcher_WMIEventArrived;
            return Task.CompletedTask;
        }

        private void _watcher_WMIEventArrived(object sender, WMIEventArgs e)
        {
            dynamic process = e.Object;

            Console.WriteLine("New Process: {0} (Pid: {1})", process.ProcessName, process.ProcessID.ToString());
        }
    }
}

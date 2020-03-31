using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMi.Sample.Models
{
    [WMIClass(Name = "Win32_Process", Namespace = "root\\CimV2")]
    public class Process
    {
        public int Handle { get; set; }
        public string Name { get; set; }
        public int ProcessID { get; set; }
        public DateTime CreationDate { get; set; }

        public dynamic GetOwnerSid()
        {
            return WMIMethod.ExecuteMethod(this);
        }

        public ProcessOwner GetOwner()
        {
            return WMIMethod.ExecuteMethod<ProcessOwner>(this);
        }

        public int AttachDebugger()
        {
            return WMIMethod.ExecuteMethod<int>(this);
        }

        public ProcessResult Create(string commandLine, string currentDirectory, string processStartupInformation)
        {
            ProcessResult res = WMIMethod.ExecuteStaticMethod<ProcessResult>(new { CommandLine = commandLine, CurrentDirectory = currentDirectory, ProcessStartupInformation = processStartupInformation});
            return res;
        }
    }

    public class ProcessOwner
    {
        public string Domain { get; set; }
        public int ReturnValue { get; set; }
        public string User { get; set; }
    }


    public class ProcessResult
    {
        public int ProcessId { get; set; }
        public int ReturnValue { get; set; }
    }
}

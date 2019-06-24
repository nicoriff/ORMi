using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMi.Sample.Models
{
    [WMIClass(Name = "Win32_Process", Namespace = "root\\CimV2")]
    public class Process : WMIInstance
    {
        public int Handle { get; set; }
        public string Name { get; set; }
        public int ProcessID { get; set; }

        /// <summary>
        /// Date the process begins executing.
        /// </summary>
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
    }

    public class ProcessOwner
    {
        public string Domain { get; set; }
        public int ReturnValue { get; set; }
        public string User { get; set; }
    }
}

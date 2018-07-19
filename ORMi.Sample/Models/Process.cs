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

        /// <summary>
        /// Date the process begins executing.
        /// </summary>
        public DateTime CreationDate { get; set; }

        public dynamic GetOwner()
        {
            return WMIMethod.ExecuteMethod(this);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMi.Sample.Models
{
    [WMIClass(Name = "Win32_OperatingSystem", Namespace = "root\\CimV2")]
    public class OperatingSystem
    {
        /// <summary>
        /// Date and time the operating system was last restarted.
        /// </summary>
        public DateTime LastBootUpTime { get; private set; }
    }
}

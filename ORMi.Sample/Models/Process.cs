using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMi.Sample.Models
{
    [WMIClass("Win32_ProcessStartTrace")]
    public class Process
    {
        public string ProcessName { get; set; }
        public int ProcessID { get; set; }
    }
}

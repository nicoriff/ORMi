using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMi.Sample.Models
{
    [WMIClass("Win32_Process")]
    public class Process
    {
        public string Name { get; set; }
        public int ProcessID { get; set; }

        public void GetOwner()
        {
            WMIMethod.ExecuteMethod(this);
        }
    }
}

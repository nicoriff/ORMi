using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMi.Sample.Models
{
    [WMIClass(Name = "Lnl_ReaderOutput1", Namespace = "root\\OnGuard")]
    public class Output
    {
        public int PanelID { get; set; }
        public int ReaderID { get; set; }
        public string Hostname { get; set; }
        public string Name { get; set; }

        public dynamic Activate()
        {
            return WMIMethod.ExecuteMethod(this);
        }
    }
}

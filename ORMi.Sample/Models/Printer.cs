using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMi.Sample.Models
{
    [WMIClass(Name = "Win32_Printer", Namespace = "root\\CimV2")]
    public class Printer
    {
        public string DeviceID { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }

        public void RenamePrinter(string newName)
        {
            WMIMethod.ExecuteMethod(this, new { NewPrinterName = newName });
        }

    }
}

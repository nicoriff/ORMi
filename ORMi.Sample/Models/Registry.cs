using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMi.Sample.Models
{
    [WMIClass("RegistryValueChangeEvent")]
    public class Registry
    {
        public string Hive { get; set; }
        public string KeyPath { get; set; }
    }
}

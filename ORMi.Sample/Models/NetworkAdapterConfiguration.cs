using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMi.Sample.Models
{
    [WMIClass("Win32_NetworkAdapterConfiguration")]
    public class NetworkAdapterConfiguration
    {
        public string Caption { get; set; }
        public string Description { get; set; }

        public uint IPConnectionMetric { get; set; }

        public UInt32 InterfaceIndex { get; set; }

        public string WINSScopeID { get; set; }

        public bool SetStatic(string ip, string netmask)
        {
            int retVal = WMIMethod.ExecuteMethod(this, new { IPAddress = new string[] { ip }, SubnetMask = new string[] { netmask } });

            if (retVal != 0)
                Console.WriteLine($"Failed to set network settings with error code {retVal}");

            return retVal == 0;
        }
    }
}

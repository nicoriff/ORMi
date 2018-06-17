using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMi.Sample.Models
{
    [WMIClass("Win32_PnPEntity")]
    public class Device
    {
        public string Name { get; set; }

        public string Description { get; set; }

        [WMIProperty("Status")]
        public string StatusName { get; set; }
    }
}

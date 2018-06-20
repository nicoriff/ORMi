using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMi
{
    public class WMIProperty : Attribute
    {
        public WMIProperty()
        {

        }
        public WMIProperty(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public Boolean SearchKey { get; set; }
    }

    public class WMIClass : Attribute
    {
        public WMIClass()
        {

        }
        public WMIClass(string name)
        {
            Name = name;
        }
        public WMIClass(string name, string wmiNamespace)
        {
            Name = name;
            Namespace = wmiNamespace;
        }

        public string Name { get; set; }
        public string Namespace { get; set; }
    }

    public class WMIIgnore : Attribute
    {

    }
}

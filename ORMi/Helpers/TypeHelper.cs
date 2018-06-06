using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ORMi.Helpers
{
    public static class TypeHelper
    {
        public static object LoadObject(ManagementObject mo, Type t)
        {
            var o = Activator.CreateInstance(t);

            foreach (PropertyInfo p in o.GetType().GetProperties())
            {
                WMIProperty propAtt = p.GetCustomAttribute<WMIProperty>();

                string propertyName = String.Empty;

                if (propAtt != null)
                {
                    propertyName = propAtt.Name;
                }
                else
                {
                    propertyName = p.Name;
                }

                var a = mo.Properties[propertyName].Value;

                p.SetValue(o, Convert.ChangeType(a, p.PropertyType), null);
            }

            return o;
        }

        public static object LoadObject(ManagementBaseObject mo, Type t)
        {
            var o = Activator.CreateInstance(t);

            foreach (PropertyInfo p in o.GetType().GetProperties())
            {
                WMIProperty propAtt = p.GetCustomAttribute<WMIProperty>();

                string propertyName = String.Empty;

                if (propAtt != null)
                {
                    propertyName = propAtt.Name;
                }
                else
                {
                    propertyName = p.Name;
                }

                var a = mo.Properties[propertyName].Value;

                p.SetValue(o, Convert.ChangeType(a, p.PropertyType), null);
            }

            return o;
        }

        public static string GetClassName(object p)
        {
            var dnAttribute = p.GetType().GetCustomAttributes(
                typeof(WMIClass), true
            ).FirstOrDefault() as WMIClass;
            if (dnAttribute != null)
            {
                return dnAttribute.Name;
            }
            else
            {
                return p.GetType().Name;
            }
        }

        public static string GetClassName(Type t)
        {
            var dnAttribute = t.GetCustomAttributes(
                           typeof(WMIClass), true
                       ).FirstOrDefault() as WMIClass;
            if (dnAttribute != null)
            {
                return dnAttribute.Name;
            }
            return null;
        }

        public static ManagementObject GetManagementObject(ManagementClass sourceClass, object obj)
        {
            ManagementObject genericInstance = sourceClass.CreateInstance();

            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
            {
                WMIProperty propAtt = propertyInfo.GetCustomAttribute<WMIProperty>();

                if (propAtt == null)
                {
                    if (propertyInfo.GetValue(obj).GetType() == typeof(DateTime))
                    {
                        genericInstance[propertyInfo.Name.ToUpper()] = ManagementDateTimeConverter.ToDmtfDateTime(Convert.ToDateTime(propertyInfo.GetValue(obj)));
                    }
                    else
                    {
                        genericInstance[propertyInfo.Name.ToUpper()] = propertyInfo.GetValue(obj);
                    }
                }
                else
                {
                    if (propertyInfo.GetValue(obj).GetType() == typeof(DateTime))
                    {
                        genericInstance[propAtt.Name.ToUpper()] = ManagementDateTimeConverter.ToDmtfDateTime(Convert.ToDateTime(propertyInfo.GetValue(obj)));
                    }
                    else
                    {
                        genericInstance[propAtt.Name.ToUpper()] = propertyInfo.GetValue(obj);
                    }
                }
            }

            return genericInstance;
        }

        public static WMISearchKey GetSearchKey(object p)
        {
            WMISearchKey res = null;

            foreach (PropertyInfo propertyInfo in p.GetType().GetProperties())
            {
                WMIProperty propAtt = propertyInfo.GetCustomAttribute<WMIProperty>();

                if (propAtt != null)
                {
                    if (propAtt.SearchKey)
                    {
                        res = new WMISearchKey
                        {
                            Name = propAtt.Name,
                            Value = propertyInfo.GetValue(p)
                        };

                        break;
                    }
                }
            }

            return res;
        }
    }

    public class WMISearchKey
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }
}

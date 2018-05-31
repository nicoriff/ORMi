using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ORMi
{
    public class WMIHelper
    {
        public string Scope { get;set; }

        public WMIHelper(string scope)
        {
            Scope = scope;
        }

        public void AddInstance(object obj)
        {
            try
            {
                WindowsImpersonationContext impersonatedUser = WindowsIdentity.GetCurrent().Impersonate();

                ManagementClass genericClass = new ManagementClass(Scope, GetClassName(obj), null);

                ManagementObject genericInstance = genericClass.CreateInstance();

                Dictionary<string, object> properties = GetPropertiesDictionary(obj);

                foreach (KeyValuePair<string, object> value in properties)
                {
                    if (value.Value.GetType() == typeof(DateTime))
                    {
                        genericInstance[value.Key] = ManagementDateTimeConverter.ToDmtfDateTime(Convert.ToDateTime(value.Value));
                    }
                    else
                    {
                        genericInstance[value.Key] = value.Value.ToString();
                    }
                }

                genericInstance.Put();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateInstance(object obj)
        {
            try
            {
                WindowsImpersonationContext impersonatedUser = WindowsIdentity.GetCurrent().Impersonate();

                string className = GetClassName(obj);
                Dictionary<string, object> properties = GetPropertiesDictionary(obj);

                KeyValuePair<string, object> onGuardKey = GetSearchKey(obj);

                string query = String.Format("SELECT * FROM {0} WHERE {1} = '{2}'", className, onGuardKey.Key, onGuardKey.Value);

                ManagementObjectSearcher searcher;
                searcher = new ManagementObjectSearcher(Scope, query);

                EnumerationOptions options = new EnumerationOptions();
                options.ReturnImmediately = true;

                foreach (ManagementObject m in searcher.Get())
                {
                    foreach (KeyValuePair<string, object> k in properties)
                    {
                        m.Properties[k.Key].Value = k.Value;
                    }

                    m.Put();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RemoveInstance(object obj)
        {
            try
            {
                WindowsImpersonationContext impersonatedUser = WindowsIdentity.GetCurrent().Impersonate();

                string className = GetClassName(obj);

                KeyValuePair<string, object> onGuardKey = GetSearchKey(obj);

                string query = String.Format("SELECT * FROM {0} WHERE {1} = '{2}'", className, onGuardKey.Key, onGuardKey.Value);

                ManagementObjectSearcher searcher;
                searcher = new ManagementObjectSearcher(Scope, query);

                EnumerationOptions options = new EnumerationOptions();
                options.ReturnImmediately = true;

                foreach (ManagementObject m in searcher.Get())
                {
                    m.Delete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RemoveInstance(string query)
        {
            try
            {
                WindowsImpersonationContext impersonatedUser = WindowsIdentity.GetCurrent().Impersonate();

                ManagementObjectSearcher searcher;
                searcher = new ManagementObjectSearcher(Scope, query);

                EnumerationOptions options = new EnumerationOptions();
                options.ReturnImmediately = true;

                foreach (ManagementObject m in searcher.Get())
                {
                    m.Delete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<T> Query<T>()
        {
            List<T> res = new List<T>();

            string nombre = GetClassName(typeof(T));

            string query = String.Format("SELECT * FROM {0}", nombre);

            ManagementObjectSearcher searcher;
            searcher = new ManagementObjectSearcher(Scope, query);

            EnumerationOptions options = new EnumerationOptions();
            options.ReturnImmediately = true;

            ManagementObjectCollection wmiRes = searcher.Get();

            foreach (ManagementObject mo in wmiRes)
            {
                var a = (T)LoadObject(mo, typeof(T));
                res.Add(a);
            }

            return res;
        }

        public IEnumerable<T> Query<T>(string query)
        {
            List<T> res = new List<T>();

            ManagementObjectSearcher searcher;
            searcher = new ManagementObjectSearcher(Scope, query);

            EnumerationOptions options = new EnumerationOptions();
            options.ReturnImmediately = true;

            ManagementObjectCollection wmiRes = searcher.Get();

            foreach (ManagementObject mo in wmiRes)
            {
                var a = (T)LoadObject(mo, typeof(T));
                res.Add(a);
            }

            return res;
        }

        private object LoadObject(ManagementObject mo, Type t)
        {
            var o = Activator.CreateInstance(t);

            foreach (PropertyInfo p in o.GetType().GetProperties())
            {
                WMIProperty propAtt = p.GetCustomAttribute<WMIProperty>();

                var a = mo.Properties[propAtt.Name].Value;

                p.SetValue(o, Convert.ChangeType(a, p.PropertyType), null);
            }

            return o;
        }

        public Dictionary<string, object> GetPropertiesDictionary(object p)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            foreach (PropertyInfo propertyInfo in p.GetType().GetProperties())
            {
                WMIProperty propAtt = propertyInfo.GetCustomAttribute<WMIProperty>();

                if (propAtt != null)
                {
                    if (propAtt.Type == typeof(DateTime))
                    {
                        dic.Add(propAtt.Name, ManagementDateTimeConverter.ToDmtfDateTime(Convert.ToDateTime(propertyInfo.GetValue(p))));
                    }
                    else
                    {
                        dic.Add(propAtt.Name, propertyInfo.GetValue(p));
                    }
                }
            }

            return dic;
        }

        public string GetClassName(object p)
        {
            var dnAttribute = p.GetType().GetCustomAttributes(
                typeof(WMIClass), true
            ).FirstOrDefault() as WMIClass;
            if (dnAttribute != null)
            {
                return dnAttribute.Name;
            }
            return null;
        }

        public string GetClassName(Type t)
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

        public KeyValuePair<string, object> GetSearchKey(object p)
        {
            KeyValuePair<string, object> res = new KeyValuePair<string, object>();

            foreach (PropertyInfo propertyInfo in p.GetType().GetProperties())
            {
                WMIProperty propAtt = propertyInfo.GetCustomAttribute<WMIProperty>();

                if (propAtt != null)
                {
                    if (propAtt.SearchKey)
                    {
                        res = new KeyValuePair<string, object>(propAtt.Name, propertyInfo.GetValue(p));
                        break;
                    }
                }
            }

            return res;
        }
    }
}

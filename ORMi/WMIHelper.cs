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

                ManagementObject genericInstance = GetManagementObject(genericClass, obj);

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

                WMISearchKey key = GetSearchKey(obj);

                if (key.Value != null)
                {
                    string query = String.Format("SELECT * FROM {0} WHERE {1} = '{2}'", GetClassName(obj), key.Name, key.Value);

                    ManagementObjectSearcher searcher;
                    searcher = new ManagementObjectSearcher(Scope, query);

                    EnumerationOptions options = new EnumerationOptions();
                    options.ReturnImmediately = true;

                    ManagementObjectCollection col = searcher.Get();

                    foreach (ManagementObject m in searcher.Get())
                    {
                        foreach (PropertyInfo p in obj.GetType().GetProperties())
                        {
                            WMIProperty propAtt = p.GetCustomAttribute<WMIProperty>();

                            if (propAtt != null)
                            {
                                m[propAtt.Name] = p.GetValue(obj);
                            }
                            else
                            {
                                m[p.Name] = p.GetValue(obj);
                            }
                        }

                        m.Put();
                    }
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

                WMISearchKey key = GetSearchKey(obj);

                string query = String.Format("SELECT * FROM {0} WHERE {1} = '{2}'", className, key.Name, key.Value);

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

        public Task<IEnumerable<T>> QueryAsync<T>()
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

            return Task.FromResult<IEnumerable<T>>(res);
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

        public string GetClassName(object p)
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

        private ManagementObject GetManagementObject(ManagementClass sourceClass, object obj)
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

        public WMISearchKey GetSearchKey(object p)
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

using ORMi.Helpers;
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

        #region CRUD Operations
        public void AddInstance(object obj)
        {
            try
            {
                WindowsImpersonationContext impersonatedUser = WindowsIdentity.GetCurrent().Impersonate();

                ManagementClass genericClass = new ManagementClass(Scope, TypeHelper.GetClassName(obj), null);

                ManagementObject genericInstance = TypeHelper.GetManagementObject(genericClass, obj);

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

                string className = TypeHelper.GetClassName(obj);

                WMISearchKey key = TypeHelper.GetSearchKey(obj);

                if (key.Value != null)
                {
                    string query = String.Format("SELECT * FROM {0} WHERE {1} = '{2}'", TypeHelper.GetClassName(obj), key.Name, key.Value);

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

                string className = TypeHelper.GetClassName(obj);

                WMISearchKey key = TypeHelper.GetSearchKey(obj);

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

            string nombre = TypeHelper.GetClassName(typeof(T));

            string query = String.Format("SELECT * FROM {0}", nombre);

            ManagementObjectSearcher searcher;
            searcher = new ManagementObjectSearcher(Scope, query);

            EnumerationOptions options = new EnumerationOptions();
            options.ReturnImmediately = true;

            ManagementObjectCollection wmiRes = searcher.Get();

            foreach (ManagementObject mo in wmiRes)
            {
                var a = (T)TypeHelper.LoadObject(mo, typeof(T));
                res.Add(a);
            }

            return res;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>()
        {
            return await Task.Run(() => Query<T>());
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
                var a = (T)TypeHelper.LoadObject(mo, typeof(T));
                res.Add(a);
            }

            return res;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string query)
        {
            return await Task.Run(() => Query<T>(query));
        }

        public dynamic QuerySingle<T>(string query)
        {
            dynamic res = null;

            ManagementObjectSearcher searcher;
            searcher = new ManagementObjectSearcher(Scope, query);

            EnumerationOptions options = new EnumerationOptions();
            options.ReturnImmediately = true;

            ManagementObjectCollection wmiRes = searcher.Get();

            foreach (ManagementObject mo in wmiRes)
            {
                res = (T)TypeHelper.LoadObject(mo, typeof(T));
                break;
            }

            return res;
        }

        #endregion

    }
}

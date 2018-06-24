using ORMi.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ORMi
{
    public static class WMIMethod
    {
        public static dynamic ExecuteMethod(object obj)
        {
            WindowsImpersonationContext impersonatedUser = WindowsIdentity.GetCurrent().Impersonate();

            var mth = new StackTrace().GetFrame(1).GetMethod();
            string methodName = mth.Name;

            ManagementClass genericClass = new ManagementClass(TypeHelper.GetNamespace(obj), TypeHelper.GetClassName(obj), null);

            ManagementObject instance = TypeHelper.GetManagementObject(genericClass, obj);

            ManagementBaseObject result = instance.InvokeMethod(methodName, null, null);

            return TypeHelper.LoadDynamicObject(result);
        }

        public static dynamic ExecuteMethod(object obj, dynamic parameters)
        {
            WindowsImpersonationContext impersonatedUser = WindowsIdentity.GetCurrent().Impersonate();

            var mth = new StackTrace().GetFrame(1).GetMethod();
            string methodName = mth.Name;

            ManagementClass genericClass = new ManagementClass(TypeHelper.GetNamespace(obj), TypeHelper.GetClassName(obj), null);

            ManagementObject instance = TypeHelper.GetManagementObject(genericClass, obj);

            ManagementBaseObject inParams = genericClass.GetMethodParameters(methodName);

            foreach (PropertyInfo p in parameters.GetType().GetProperties())
            {
                inParams[p.Name] = p.GetValue(parameters);
            }

            ManagementBaseObject result = instance.InvokeMethod(methodName, inParams, null);

            return TypeHelper.LoadDynamicObject(result);
        }

        public static dynamic ExecuteStaticMethod()
        {
            var mth = new StackTrace().GetFrame(1).GetMethod();
            string methodName = mth.Name;

            Type t = mth.ReflectedType;

            ManagementClass cls = new ManagementClass(TypeHelper.GetNamespace(t), TypeHelper.GetClassName(t), null);

            ManagementBaseObject result = cls.InvokeMethod(methodName, null, null);

            return TypeHelper.LoadDynamicObject(result);
        }

        public static dynamic ExecuteStaticMethod(dynamic parameters)
        {
            var mth = new StackTrace().GetFrame(1).GetMethod();
            string methodName = mth.Name;

            Type t = mth.ReflectedType;

            ManagementClass cls = new ManagementClass(TypeHelper.GetNamespace(t), TypeHelper.GetClassName(t), null);

            ManagementBaseObject inParams = cls.GetMethodParameters(methodName);

            foreach (PropertyInfo p in parameters.GetType().GetProperties())
            {
                inParams[p.Name] = p.GetValue(parameters);
            }

            ManagementBaseObject result = cls.InvokeMethod(methodName, inParams, null);

            return TypeHelper.LoadDynamicObject(result);
        }
    }
}

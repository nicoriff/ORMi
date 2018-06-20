using ORMi.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace ORMi
{
    public static class WMIMethod
    {
        public static dynamic ExecuteMethod(object obj)
        {
            var mth = new StackTrace().GetFrame(1).GetMethod();
            string methodName = mth.Name;

            ManagementClass genericClass = new ManagementClass(TypeHelper.GetNamespace(obj), TypeHelper.GetClassName(obj), null);

            ManagementObject instance = TypeHelper.GetManagementObject(genericClass, obj);

            object res = instance.InvokeMethod(methodName, null);

            return null;
        }

        public static dynamic ExecuteStaticMethod()
        {
            var mth = new StackTrace().GetFrame(1).GetMethod();

            string className = mth.ReflectedType.Name;
            string methodName = mth.Name;

            Type t = mth.ReflectedType;


            try// Get the client's SMS_Client class.
            {
                ManagementClass cls = new ManagementClass(TypeHelper.GetNamespace(t), TypeHelper.GetClassName(t), null);

                ManagementBaseObject result = cls.InvokeMethod("GetAssignedSite", null, null);

                //// Display current site code.
                //Console.WriteLine(outSiteParams["sSiteCode"].ToString());

                //// Set up current site code as input parameter for SetAssignedSite.
                //ManagementBaseObject inParams = cls.GetMethodParameters("SetAssignedSite");
                //inParams["sSiteCode"] = outSiteParams["sSiteCode"].ToString();

                //// Assign the Site code.
                //ManagementBaseObject outMPParams = cls.InvokeMethod("SetAssignedSite", inParams, null);
            }
            catch (ManagementException e)
            {
                throw new Exception("Failed to execute method", e);
            }


            return null;
        }
    }
}

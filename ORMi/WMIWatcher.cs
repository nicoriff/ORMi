using ORMi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ORMi
{
    public class WMIWatcher
    {
        ManagementEventWatcher watcher;
        private string _scope;
        private string _query;
        private Type _type;

        public delegate void WMIEventHandler(object sender, WMIEventArgs e);
        public event WMIEventHandler WMIEventArrived;

        /// <summary>
        /// Creates a WMI Event watcher based on custom query
        /// </summary>
        /// <param name="scope">Desired Scope</param>
        /// <param name="query">Query to be watch</param>
        /// <param name="type">Type of result</param>
        public WMIWatcher(string scope, string query, Type type)
        {
            _scope = scope;
            _query = query;
            _type = type;

            CreateWatcher();
        }

        /// <summary>
        /// Creates a WMI Event watcher based on the WMIClass atribute that has been set to the desired Type
        /// </summary>
        /// <param name="scope">Desired Scope</param>
        /// <param name="type">Query to be watch</param>
        public WMIWatcher(string scope, Type type)
        {
            _scope = scope;
            _query = String.Format("SELECT * FROM {0}", TypeHelper.GetClassName(type));
            _type = type;

            CreateWatcher();
        }

        /// <summary>
        /// Create a WMI Event Watcher
        /// </summary>
        private void CreateWatcher()
        {
            watcher = new ManagementEventWatcher(_scope, _query);
            watcher.EventArrived += Watcher_EventArrived;
            watcher.Start();
        }

        private void Watcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            object o = TypeHelper.LoadObject(e.NewEvent, _type);

            WMIEventArrived(this, new WMIEventArgs { Object = o });
        }
    }

    public class WMIEventArgs : EventArgs
    {
        public object Object { get; set; }
    }
}

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

        public WMIWatcher(string scope, string query, Type type)
        {
            _scope = scope;
            _query = query;
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

        public event EventHandler WMIEventArrived;
    }

    public class WMIEventArgs : EventArgs
    {
        public object Object { get; set; }
    }
}

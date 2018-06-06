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
        private string _query;
        private Type _type;

        public WMIWatcher(string query, Type type)
        {
            _query = query;
            _type = type;

            CreateWatcher(query);
        }

        private void CreateWatcher(string query)
        {
            ManagementEventWatcher watcher = new ManagementEventWatcher(query);
            watcher.EventArrived += Watcher_EventArrived;
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

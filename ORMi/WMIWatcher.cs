﻿using ORMi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace ORMi
{
    public sealed class WMIWatcher : IDisposable
    {
        ManagementEventWatcher watcher;
        private readonly string _scope;
        private readonly string _query;
        private readonly Type _type;

        public delegate void WMIEventHandler(object sender, WMIEventArgs e);
        public event WMIEventHandler WMIEventArrived;

        /// <summary>
        /// Creeates a WMI Watcher for the specified query. It returns a dynamic object.
        /// </summary>
        /// <param name="scope">Desired Scope</param>
        /// <param name="query">Query to be watch</param>
        /// <param name="options">Connection options. If null, default options are used</param>
        public WMIWatcher(string scope, string query, ConnectionOptions options = null)
        {
            _scope = scope;
            _query = query;

            CreateWatcher(options);
        }

        /// <summary>
        /// Creates a WMI Event watcher based on the WMIClass atribute that has been set to the desired Type
        /// </summary>
        /// <param name="scope">Desired Scope</param>
        /// <param name="type">Type of object that will initiate the watch</param>
        /// <param name="options">Connection options. If null, default options are used</param>
        public WMIWatcher(string scope, Type type, ConnectionOptions options = null)
        {
            _scope = scope;
            _query = String.Format("SELECT * FROM {0}", TypeHelper.GetClassName(type));
            _type = type;

            CreateWatcher(options);
        }

        /// <summary>
        /// Creates a WMI Event watcher based on custom query
        /// </summary>
        /// <param name="scope">Desired Scope</param>
        /// <param name="query">Query to be watch</param>
        /// <param name="type">Type of result</param>
        /// <param name="options">Connection options. If null, default options are used</param>
        public WMIWatcher(string scope, string query, Type type, ConnectionOptions options = null)
        {
            _scope = scope;
            _query = query;
            _type = type;

            CreateWatcher(options);
        }

        /// <summary>
        /// Create a WMI Event Watcher
        /// </summary>
        private void CreateWatcher(ConnectionOptions options = null)
        {
            watcher?.Dispose();
            watcher = new ManagementEventWatcher(_scope, _query);

            if (options != null)
            {
                watcher.Scope.Options = options;
            }

            watcher.EventArrived += Watcher_EventArrived;
            watcher.Start();
        }

        private void Watcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            if (_type != null)
            {
                object o = TypeHelper.LoadObject(e.NewEvent, _type);

                WMIEventArrived(this, new WMIEventArgs { Object = o });
            }
            else
            {
                dynamic d = TypeHelper.LoadDynamicObject(e.NewEvent);

                WMIEventArrived(this, new WMIEventArgs { Object = d });
            }
        }

        /// <summary>
        /// Starts the current WMI Event watcher
        /// </summary>
        public void StartWatcher()
        {
            if (watcher != null)
            {
                watcher.Start();
            }
            else
            {
                throw new NullReferenceException("The WMI Watcher is not initialized");
            }
        }

        /// <summary>
        /// Stops the current WMI Event watcher
        /// </summary>
        public void StopWatcher()
        {
            if (watcher != null)
            {
                watcher.Stop();
            }
            else
            {
                throw new NullReferenceException("The WMI Watcher is not initialized");
            }
        }

        public void Dispose()
        {
            ((IDisposable)watcher).Dispose();
        }
    }

    public class WMIEventArgs : EventArgs
    {
        public object Object { get; set; }
    }
}

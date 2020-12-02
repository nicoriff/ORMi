using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace ORMi.Interfaces
{
    public interface IWMIWatcher
    {
        event WMIWatcher.WMIEventHandler WMIEventArrived;

        /// <summary>
        /// Disposes the WMIWatcher object.
        /// </summary>
        void Dispose();

        /// <summary>
        /// Initializes the WMIWatcher with the desired parameters.
        /// </summary>
        /// <param name="scope">Desired Scope</param>
        /// <param name="query">Query to be watch</param>
        /// <param name="type">Type of result</param>
        /// <param name="options">Connection options. If null, default options are used</param>
        void Initialize(string scope, string query, Type type = null, ConnectionOptions options = null);

        /// <summary>
        /// Starts the current WMI Event watcher
        /// </summary>
        void StartWatcher();

        /// <summary>
        /// Stops the current WMI Event watcher
        /// </summary>
        void StopWatcher();
    }
}

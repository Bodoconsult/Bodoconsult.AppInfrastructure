// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;
using EmergencyShutdown.Business.Interfaces;

namespace EmergencyShutdown.Business.Handler
{
    /// <summary>
    /// Runs all required shutdowns
    /// </summary>
    public class ShutdownHandler : IShutdownHandler
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public ShutdownHandler()
        {
            StatusMessageDelegate= StatusMessageDelegateInternal;
        }

        /// <summary>
        /// List of all servers to shut down
        /// </summary>
        public List<IServer> Servers { get; } = new();

        /// <summary>
        /// Status changed event
        /// </summary>
        public StatusMessageDelegate StatusMessageDelegate { get; set; }

        /// <summary>
        /// Load a workload
        /// </summary>
        /// <param name="workload">Current workload</param>
        public void LoadWorkload(Workload workload)
        {
            Servers.AddRange(workload.Servers);
        }

        private void SendMessage(string message)
        {
            var x = StatusMessageDelegate;
            x(message);
        }

        private void StatusMessageDelegateInternal(string message)
        {
            Console.WriteLine(message);
        }

    }
}

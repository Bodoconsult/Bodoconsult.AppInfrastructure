// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace EmergencyShutdown.Business.Interfaces
{
    /// <summary>
    /// Current workload
    /// </summary>
    public class Workload
    {
        /// <summary>
        /// List of all servers to shut down
        /// </summary>
        public List<IServer> Servers { get; } = new();
    }
}

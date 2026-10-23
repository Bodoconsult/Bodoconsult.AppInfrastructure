// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using EmergencyShutdown.Business.Interfaces;

namespace EmergencyShutdown.Business.ServerSystems
{
    /// <summary>
    /// Create a dummy server for tdd testing
    /// </summary>
    public class DummyServerSystem : BaseServerSystem
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        /// <param name="server">Server to shut down</param>
        /// <param name="shutdownConfig">Current shutdown config</param>
        public DummyServerSystem(IServer server, ShutdownConfig shutdownConfig) : base(server, shutdownConfig)
        { }

        /// <summary>
        /// Shutdown server
        /// </summary>
        /// <returns>true if success</returns>
        public override bool Shutdown()
        {
            Console.WriteLine("Server is shutting down...");
            Thread.Sleep(1000);
            Console.WriteLine("Server shutdown!");
            return true;
        }
    }
}

// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using EmergencyShutdown.Business.Interfaces;
using EmergencyShutdown.Business.Servers;
using EmergencyShutdown.Business.ServerSystems;

namespace EmergencyShutdown.Business.Factories;

/// <summary>
/// Factory creating <see cref="DummyServerSystem"/> instances
/// </summary>
public class DummyServerSystemFactory : IServerSystemFactory
{
    /// <summary>
    /// Create a server system instance dependent on the given
    /// </summary>
    /// <param name="server">Server to shut down</param>
    /// <param name="shutdownConfig">Current shutdown config</param>
    /// <returns><see cref="IServerSystem"/> instance</returns>
    public IServerSystem CreateInstance(IServer server, ShutdownConfig shutdownConfig)
    {
        return server is not DummyServer inst ? 
            throw new ArgumentException("Server is not of type Dummy") : 
            new DummyServerSystem(inst, shutdownConfig);
    }
}
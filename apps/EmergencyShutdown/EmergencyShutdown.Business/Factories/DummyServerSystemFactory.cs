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
    /// <param name="server"></param>
    /// <returns></returns>
    public IServerSystem CreateInstance(IServer server)
    {
        if (server is not DummyServer inst)
        {
            throw new ArgumentException("Server is not of type Dummy");
        }

        return new DummyServerSystem(inst);
    }
}
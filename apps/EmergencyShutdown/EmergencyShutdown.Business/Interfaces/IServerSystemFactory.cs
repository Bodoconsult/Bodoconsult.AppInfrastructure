// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace EmergencyShutdown.Business.Interfaces;

/// <summary>
/// Interface for server systems
/// </summary>
public interface IServerSystemFactory
{
    /// <summary>
    /// Create a server system instance dependent on the given
    /// </summary>
    /// <param name="server"></param>
    /// <returns></returns>
    IServerSystem CreateInstance(IServer server);
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace EmergencyShutdown.Business.Interfaces;

/// <summary>
/// Interface for the shutdown of different types of server systems
/// </summary>
public interface IServerSystem
{
    /// <summary>
    /// Shutdown server
    /// </summary>
    /// <returns>True on success else false</returns>
    bool Shutdown();

    /// <summary>
    /// Data of the server to shut down
    /// </summary>
    IServer Server { get;  }

    /// <summary>
    /// Current shutdown config
    /// </summary>
    ShutdownConfig ShutdownConfig { get; }
}
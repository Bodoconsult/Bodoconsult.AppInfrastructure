// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using EmergencyShutdown.Business.Interfaces;

namespace EmergencyShutdown.Business.ServerSystems;

/// <summary>
/// Base class for shutdown of server systems
/// </summary>
public abstract class BaseServerSystem: IServerSystem
{
    /// <summary>
    /// default ctor
    /// </summary>
    /// <param name="server">Server instance</param>
    /// <param name="shutdownConfig">Current shutdown config</param>
    protected BaseServerSystem(IServer server, ShutdownConfig shutdownConfig)
    {
        Server = server;
        ShutdownConfig = shutdownConfig;
    }

    /// <summary>
    /// Shutdown server
    /// </summary>
    /// <returns>True on success else false</returns>
    public virtual bool Shutdown()
    {
        throw new NotSupportedException("Override in derived classes");
    }

    /// <summary>
    /// Data of the server to shut down
    /// </summary>

    public IServer Server { get; internal set; }

    /// <summary>
    /// Current shutdown config
    /// </summary>
    public ShutdownConfig ShutdownConfig { get; }
}
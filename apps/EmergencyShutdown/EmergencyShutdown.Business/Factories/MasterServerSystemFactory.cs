// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using EmergencyShutdown.Business.Interfaces;
using EmergencyShutdown.Business.Servers;

namespace EmergencyShutdown.Business.Factories;

/// <summary>
/// Master factory for creating <see cref="IServerSystem"/> instances
/// </summary>
public class MasterServerSystemFactory : IMasterServerSystemFactory
{
    private readonly Dictionary<string, IServerSystemFactory> _factories = new();

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="windowsShellProcessManager">Shell process manager for Windows OS</param>
    public MasterServerSystemFactory(IShellProcessManager windowsShellProcessManager)
    {
        _factories.Add(nameof(DummyServer), new DummyServerSystemFactory());
        _factories.Add(nameof(WindowsServer), new WindowsServerSystemFactory(windowsShellProcessManager));
    }
    
    /// <summary>
    /// Create a server system instance dependent on the given
    /// </summary>
    /// <param name="server">Server to shut down</param>
    /// <param name="shutdownConfig">Current shutdown config</param>
    /// <returns><see cref="IServerSystem"/> instance</returns>
    public IServerSystem? CreateInstance(IServer server, ShutdownConfig shutdownConfig)
    {
        var typeName = server.GetType().Name;

        if (_factories.ContainsKey(typeName))
        {
            return null;
        }

        var factory = _factories[typeName];
        return factory.CreateInstance(server, shutdownConfig);
    }
}
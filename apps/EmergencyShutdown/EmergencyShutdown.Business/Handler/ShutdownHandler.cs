// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;
using EmergencyShutdown.Business.Interfaces;

namespace EmergencyShutdown.Business.Handler;

/// <summary>
/// Runs all required shutdowns
/// </summary>
public class ShutdownHandler : IShutdownHandler
{
    private readonly IMasterServerSystemFactory _masterServerSystemFactory;
    private readonly ShutdownConfig _shutdownConfig;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="masterServerSystemFactory">Master server system factory</param>
    /// <param name="shutdownConfig">Current shutdown config</param>
    public ShutdownHandler(IMasterServerSystemFactory masterServerSystemFactory, ShutdownConfig shutdownConfig)
    {
        _masterServerSystemFactory = masterServerSystemFactory;
        _shutdownConfig = shutdownConfig;
        StatusMessageDelegate = StatusMessageDelegateInternal;
    }

    /// <summary>
    /// List of all servers to shut down
    /// </summary>
    public List<IServer> Servers { get; } = new();

    /// <summary>
    /// Servers processed
    /// </summary>
    public int Processed { get; private set; }

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
        Processed = 0;
        Servers.Clear();
        Servers.AddRange(workload.Servers);
    }

    /// <summary>
    /// Process the current workload
    /// </summary>
    public void ProcessWorkload()
    {
        foreach (var server in Servers)
        {
            var serverSystem = _masterServerSystemFactory.CreateInstance(server, _shutdownConfig);

            if (serverSystem is null)
            {
                continue;

            }

            if (serverSystem.Shutdown())
            {
                Processed++;
            }
        }
    }

    private void SendMessage(string message)
    {
        StatusMessageDelegate.Invoke(message);
    }

    private void StatusMessageDelegateInternal(string message)
    {
        Console.WriteLine(message);
    }

}
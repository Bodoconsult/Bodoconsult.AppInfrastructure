// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;

namespace EmergencyShutdown.Business.Interfaces;

/// <summary>
/// Interface for shutdown handler
/// </summary>
public interface IShutdownHandler
{
    /// <summary>
    /// List of all servers to shut down
    /// </summary>
    List<IServer> Servers { get; }

    /// <summary>
    /// Status changed event
    /// </summary>
    StatusMessageDelegate StatusMessageDelegate { get; set; }

    /// <summary>
    /// Load a workload
    /// </summary>
    /// <param name="workload">Current workload</param>
    void LoadWorkload(Workload workload);
}
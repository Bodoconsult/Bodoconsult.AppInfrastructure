// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for implementing the management of sync running non-blocking processes. As long as the process IDs are unique instance can be singleton
/// </summary>
public interface ISyncProcessManager<TKey, T> : IDisposable where T: class
{
    /// <summary>
    /// Is queue with the sync running orders empty
    /// </summary>
    bool IsSyncRunningOrderEmpty { get; }

    /// <summary>
    /// Add an order to the sync execution queue
    /// </summary>
    /// <param name="processId">Key value of the rpocess to run sync</param>
    /// <param name="timeout">Timeout in ms</param>
    SyncProcessData<TKey, T> AddSyncProcess(TKey processId, int timeout);

    /// <summary>
    /// Remove a sync execution order from  sync execution queue
    /// </summary>
    /// <param name="processId">Key value of the process to run sync</param>
    void RemoveSyncProcess(TKey processId);

    /// <summary>
    /// Get the sync running execution data for an order
    /// </summary>
    /// <param name="processId">Key value of the rpocess to run sync</param>
    /// <returns>Sync running execution data or null</returns>
    SyncProcessData<TKey, T>? GetSyncProcessDataForProcess(TKey processId);
}
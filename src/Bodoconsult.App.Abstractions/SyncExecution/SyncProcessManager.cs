// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using System.Collections.Concurrent;

namespace Bodoconsult.App.Abstractions.SyncExecution;

/// <summary>
/// Current implementation of <see cref="ISyncProcessManager{TKey,T}"/> for processes identified by a Guid
/// </summary>
public class SyncProcessManager<TKey, T> : ISyncProcessManager<TKey, T> where T : class
{
    /// <summary>
    /// The current execution list of sync running orders.
    /// Do not access SyncExecutionQueue directly.
    /// Always take a "copy" of the list with i.e. _syncExecutionQueue.Select or _syncExecutionQueue.ToList to avoid multithreading iusses
    /// </summary>
    private readonly ConcurrentDictionary<TKey, SyncProcessData<TKey, T>> _syncExecutionQueue = new();

    /// <summary>
    /// Is queue with the sync running orders empty
    /// </summary>
    public bool IsSyncRunningOrderEmpty => _syncExecutionQueue.IsEmpty;

    /// <summary>
    /// Add an order to the sync execution queue
    /// </summary>
    /// <param name="processId">GUID of the rpocess to run sync</param>
    /// <param name="timeout">Timeout in ms</param>
    public SyncProcessData<TKey, T> AddSyncProcess(TKey processId, int timeout)
    {
        var syncData = new SyncProcessData<TKey, T>(processId, timeout);
        _syncExecutionQueue.TryAdd(processId, syncData);
        return syncData;
    }

    /// <summary>
    /// Remove a sync execution order from  sync execution queue
    /// </summary>
    /// <param name="processId">GUID of the rpocess to run sync</param>
    public void RemoveSyncProcess(TKey processId)
    {
        _syncExecutionQueue.TryRemove(processId, out var syncData);
        syncData?.Dispose();
    }

    /// <summary>
    /// Get the sync running execution data for an order
    /// </summary>
    /// <param name="processId">GUID of the rpocess to run sync</param>
    /// <returns>Sync running execution data or null</returns>
    public SyncProcessData<TKey, T> GetSyncProcessDataForProcess(TKey processId)
    {
        var success = _syncExecutionQueue.TryRemove(processId, out var syncData);
        return !success ? null : syncData;
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        _syncExecutionQueue.Clear();
    }
}

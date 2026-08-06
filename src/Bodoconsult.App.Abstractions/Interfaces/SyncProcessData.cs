// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Helper class for running process identified by a key value in a sync manner
/// </summary>
public class SyncProcessData<TKey, T> : IDisposable where T: class
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public SyncProcessData(TKey processId, int timeout)
    {
        ProcessId = processId;
        TaskCompletionSource = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);

        // Create the CancellationTokenSource to implement timeout for sync running
        var cts = new CancellationTokenSource(timeout + 100);
        cts.Token.Register(() =>
        {

            if (TaskCompletionSource is not
                {
                    Task:
                    {
                        IsCompleted: false, IsCanceled: false, IsFaulted: false, IsCompletedSuccessfully: false
                    }
                })
            {
                return;
            }

            //TaskCompletionSource?.SetResult(null);

        });

        CancellationTokenSource = cts;
    }

    /// <summary>
    /// Create a task to wait unitl order finished or timeout
    /// </summary>
    /// <returns>Task to wait for</returns>
    public Task<T> CreateWaitingTask()
    {
        ArgumentNullException.ThrowIfNull(TaskCompletionSource);

        // Now wait
        return TaskCompletionSource.Task;
    }

    /// <summary>
    /// Set the result for the task
    /// </summary>
    /// <param name="data">Return value for the waiting task</param>
    public void SetResult(T data)
    {
        TaskCompletionSource?.TrySetResult(data);
    }

    /// <summary>
    /// Process ID
    /// </summary>
    public TKey ProcessId { get; }

    /// <summary>
    /// CancellationTokenSource used for running an order in a sync manner
    /// </summary>
    public CancellationTokenSource? CancellationTokenSource { get; private set; }

    /// <summary>
    /// TaskCompletionSource used for running an order in a sync manner
    /// </summary>
    public TaskCompletionSource<T>? TaskCompletionSource { get; private set; }

    /// <summary>
    /// Current BT request data connect to this process
    /// </summary>
    public IBusinessTransactionRequestData? BusinessTransactionRequestData { get; set; }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        try
        {
            if (CancellationTokenSource != null)
            {
                if (!CancellationTokenSource.IsCancellationRequested)
                {
                    CancellationTokenSource.Cancel(false);
                }

                CancellationTokenSource.Dispose();
            }

            CancellationTokenSource = null;
            TaskCompletionSource = null;
        }
        catch //(Exception e)
        {
            // Do nothing
        }
    }
}
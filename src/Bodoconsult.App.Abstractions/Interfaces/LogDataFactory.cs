// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

using BufferPool;

/// <summary>
/// Factory for <see cref="LogData"/> instances with integrated buffer pool to reduce GC pressure
/// </summary>
public class LogDataFactory: ILogDataFactory
{

    /// <summary>
    /// Current buffer pool instance
    /// </summary>
    private readonly BufferPool<LogData> _bufferPool;

    /// <summary>
    /// Default ctor
    /// </summary>
    public LogDataFactory()
    {
        _bufferPool = new BufferPool<LogData>(() => new LogData());
    }

    /// <summary>
    /// The current number of instances in the <see cref="LogData"/> buffer pool
    /// </summary>
    public int CurrentNumberOfInstancesInPool => _bufferPool.LengthOfQueue;

    /// <summary>
    /// Allocate the buffer pool
    /// </summary>
    /// <param name="numberOfInstances">Number of instances to pre-allocate in the buffer pool</param>
    public void AllocateBufferPool(int numberOfInstances)
    {
        _bufferPool.Allocate(numberOfInstances);
    }

    /// <summary>
    /// Dequeue an instance of <see cref="LogData"/> to use it for logging
    /// </summary>
    /// <returns>Instance of <see cref="LogData"/></returns>
    public LogData DequeueInstance()
    {
        var logData = _bufferPool.Dequeue();
        logData.LogDate = DateTime.Now;
        return logData;
    }

    /// <summary>
    /// Reset the logdata entity and give it back to pool
    /// </summary>
    /// <param name="logData"></param>
    public void EnqueueInstance(LogData logData)
    {
        logData.Reset();
        _bufferPool.Enqueue(logData);
    }
}
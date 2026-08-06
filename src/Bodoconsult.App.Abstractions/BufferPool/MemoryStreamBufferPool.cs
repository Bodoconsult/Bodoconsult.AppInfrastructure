// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.Concurrent;

namespace Bodoconsult.App.Abstractions.BufferPool;

/// <summary>
/// Buffer pool is used to store reusable <see cref="MemoryStream"/>> instances to reduce GC pressure
/// </summary>
public class MemoryStreamBufferPool
{
    private readonly Func<MemoryStream> _factoryMethod = () => new MemoryStream();
    private readonly ConcurrentQueue<MemoryStream> _queue = new();

    /// <summary>
    /// The current length of the internal queue
    /// </summary>
    public int LengthOfQueue => _queue.Count;

    /// <summary>
    /// Pre-allocate a certain number of objects stored in the pool
    /// </summary>
    /// <param name="numberOfInstances">Number of objects stored in the pool</param>
    public void Allocate(int numberOfInstances)
    {
        for (var i = 0; i < numberOfInstances; i++)
        {
            _queue.Enqueue(_factoryMethod());
        }
    }

    /// <summary>
    /// Dequeue an object to use from buffer pool
    /// </summary>
    /// <returns>Instance of type T or null if an error happend</returns>
    public MemoryStream Dequeue()
    {
        // Debug.Print($"LogPool DEQUEUE{_queue.Count}");

        var success = _queue.TryDequeue(out var buffer);

        if (success && buffer != null)
        {
            return buffer;
        }

        buffer = _factoryMethod();
        return buffer;
    }

    /// <summary>
    /// Queue a used object back to the buffer pool
    /// </summary>
    /// <param name="buffer">Reusable object to store in the pool</param>
    public void Enqueue(MemoryStream buffer)
    {
        buffer.Position = 0;
        buffer.SetLength(0);
        _queue.Enqueue(buffer);
        // Debug.Print($"LogPool ENQUEUE{_queue.Count}");
    }

    /// <summary>
    /// Clear the buffer pool to avoid blocking memory
    /// </summary>
    public void Clear()
    {
        for (var i = 0; i < _queue.Count; i++)
        {
            _queue.TryDequeue(out _);
        }
    }
}
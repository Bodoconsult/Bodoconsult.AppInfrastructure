// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.Concurrent;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.BufferPool;

/// <summary>
/// Buffer pool is used to store resetable objects implementing <see cref="IResetable"/>to reduce GC pressure
/// </summary>
/// <typeparam name="T">Type of the object class to store</typeparam>
public class BufferPoolResetable<T> where T : IResetable
{
    private Func<T> _factoryMethod;
    private readonly ConcurrentQueue<T> _queue = new();

    /// <summary>
    /// Default ctor. Use <see cref="LoadFactoryMethod"/> to load a custimized load method
    /// </summary>
    public BufferPoolResetable()
    {
        // Do nothing
    }

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="factoryMethod">Factory method for object creation</param>
    public BufferPoolResetable(Func<T> factoryMethod)
    {
        _factoryMethod = factoryMethod;
    }

    /// <summary>
    /// Load the factory method
    /// </summary>
    /// <param name="factoryMethod">Factory method</param>
    public void LoadFactoryMethod(Func<T> factoryMethod)
    {
        _factoryMethod = factoryMethod;
    }

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
    public T Dequeue()
    {
        // Debug.Print($"LogPool DEQUEUE{_queue.Count}");

        var success = _queue.TryDequeue(out var buffer);

        if (success && buffer != null)
        {
            return buffer;
        }

        buffer = _factoryMethod();
        return buffer ?? default;
    }

    /// <summary>
    /// Queue a used object back to the buffer pool
    /// </summary>
    /// <param name="buffer">Reusable object to store in the pool</param>
    public void Enqueue(T buffer)
    {
        buffer.Reset();
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
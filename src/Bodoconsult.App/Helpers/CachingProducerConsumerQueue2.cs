// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.Concurrent;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Helpers;

/// <summary>
/// Thread-safe implementation for a <see cref="ICachingProducerConsumerQueue2{TType}"/>. Supports one or many producers but only one consumer.
/// </summary>
public class CachingProducerConsumerQueue2<T> : ICachingProducerConsumerQueue2<T> where T : struct
{
    private readonly Lock _cacheLock = new();
    private readonly List<T> _cache = new(100);

    private Thread _consumerThread;

    /// <summary>
    /// Cache size
    /// </summary>
    public int CacheSize { get; set; } = 100;

    /// <summary>
    /// Thread priority
    /// </summary>
    public ThreadPriority ThreadPriority { get; set; } = ThreadPriority.Normal;

    /// <summary>
    /// Contains the internal queue
    /// </summary>
    public BlockingCollection<List<T>> InternalQueue;

    /// <summary>
    /// The delegate to consume each item added to the queue
    /// </summary>
    public ConsumerTaskDelegate<List<T>> ConsumerTaskDelegate { get; set; }

    /// <summary>
    /// Is the queue started?
    /// </summary>
    public bool IsActivated { get; private set; }

    /// <summary>
    /// Enqueue an item to the internal queue for processing as soon as possible
    /// </summary>
    /// <param name="item">Item to add to the queue</param>
    public void Enqueue(T item)
    {
        //if (InternalQueue == null)
        //{
        //    throw new ArgumentException("InternalQueue is null. Run StartConsumer() first!");
        //}
        //try
        //{
        if (InternalQueue == null || InternalQueue.IsCompleted)
        {
            return;
        }

        List<T> data = null;

        lock (_cacheLock)
        {
            if (_cache.Count == CacheSize - 1)
            {
                data = new List<T>(_cache.Count + 1);
                data.AddRange(_cache);
                data.Add(item);
                _cache.Clear();
            }
            else
            {
                _cache.Add(item);
            }
        }

        if (data == null)
        {
            return;
        }

        InternalQueue.Add(data);

        //}
        //catch //(Exception e)
        //{
        //    // Do nothing
        //}
    }

    /// <summary>
    /// Enqueue a list of items to the internal queue for processing as soon as possible
    /// </summary>
    /// <param name="items">Items to add to the queue</param>
    public void Enqueue(List<T> items)
    {
        if (InternalQueue == null || InternalQueue.IsCompleted)
        {
            return;
        }

        List<T> data = null;

        lock (_cacheLock)
        {
            if (_cache.Count >= CacheSize - items.Count)
            {
                data = new List<T>(_cache.Count + 1);
                data.AddRange(_cache);
                data.AddRange(items);
                _cache.Clear();
            }
            else
            {
                foreach (var item in items)
                {
                    _cache.Add(item);
                }
            }
        }

        if (data == null)
        {
            return;
        }

        InternalQueue.Add(data);
    }

    /// <summary>
    /// Start the consumer thread
    /// </summary>
    public void StartConsumer()
    {
        if (ConsumerTaskDelegate == null)
        {
            throw new ArgumentNullException(nameof(ConsumerTaskDelegate));
        }

        InternalQueue = new BlockingCollection<List<T>>();

        _consumerThread = new Thread(RunInternal)
        {
            Priority = ThreadPriority,
            IsBackground = true
        };
        _consumerThread.Start();

        IsActivated = true;
    }

    /// <summary>
    /// Internal consumer method. If queue does not have any items InternalQueue.GetConsumingEnumerable waits for new items!!!!
    /// </summary>
    private void RunInternal()
    {
        if (InternalQueue == null)
        {
            return;
        }

        foreach (var item in InternalQueue.GetConsumingEnumerable())
        {
            ConsumerTaskDelegate.Invoke(item);
        }
    }

    /// <summary>
    /// Stop the consumer thread
    /// </summary>
    public void StopConsumer()
    {
        // Claer the cache
        lock (_cacheLock)
        {
            if (_cache.Count > 0)
            {
                var data = new List<T>(_cache.Count + 1);
                data.AddRange(_cache);
                _cache.Clear();

                InternalQueue.Add(data);
            }
        }

        // Now stop queue
        InternalQueue?.CompleteAdding();

        //Thread.Sleep(50);
        if (_consumerThread is { IsAlive: true })
        {
            _consumerThread?.Join();
        }
        IsActivated = false;
        InternalQueue?.Dispose();
        InternalQueue = null;
        ConsumerTaskDelegate = null;
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        StopConsumer();

        IsActivated = false;
        _consumerThread = null;
    }
}
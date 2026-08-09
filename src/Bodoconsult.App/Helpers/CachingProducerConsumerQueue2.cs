// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

// https://deniskyashif.com/2020/01/07/csharp-channels-part-3/

using Bodoconsult.App.Abstractions.Interfaces;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace Bodoconsult.App.Helpers;

/// <summary>
/// Thread-safe implementation for a <see cref="ICachingProducerConsumerQueue2{TType}"/>. Supports one or many producers but only one consumer.
/// </summary>
public class CachingProducerConsumerQueue2<T> : ICachingProducerConsumerQueue2<T> where T : struct
{
    private readonly ConcurrentQueue<T> _cache = new();

    private CancellationTokenSource _cancellationTokenSource = new();

    private Task? _task;

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
    public Channel<T[]> InternalQueue { get; private set; } = Channel.CreateBounded<T[]>(new BoundedChannelOptions(100)
    {
        SingleReader = true,
        SingleWriter = false,
        FullMode = BoundedChannelFullMode.Wait
    });

    /// <summary>
    /// The delegate to consume each item added to the queue
    /// </summary>
    public ConsumerTaskDelegate<T[]> ConsumerTaskDelegate { get; set; } = _ => { };

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
        if (!IsActivated)
        {
            return;
        }

        _cache.Enqueue(item);

        var count = _cache.Count;
        if (count <= CacheSize)
        {
            return;
        }

        Flush();
    }

    /// <summary>
    /// Enqueue a list of items to the internal queue for processing as soon as possible
    /// </summary>
    /// <param name="items">Items to add to the queue</param>
    public void Enqueue(IEnumerable<T> items)
    {
        if (!IsActivated)
        {
            return;
        }

        foreach (var item in items)
        {
            _cache.Enqueue(item);
        }

        var count = _cache.Count;
        if (count <= CacheSize)
        {
            return;
        }

        Flush();
    }

    /// <summary>
    /// Start the consumer thread
    /// </summary>
    public void StartConsumer()
    {
        _cancellationTokenSource.Dispose();

        InternalQueue.Writer.TryComplete();

        InternalQueue = Channel.CreateBounded<T[]>(new BoundedChannelOptions(CacheSize)
        {
            SingleReader = true,
            SingleWriter = false,
            FullMode = BoundedChannelFullMode.Wait
        });
        _cancellationTokenSource = new CancellationTokenSource();

        var wait = new AutoResetEvent(false);

        _task = Task.Factory.StartNew(async () =>
        {
            _ = await RunInternal(wait);
        }, TaskCreationOptions.LongRunning);

        wait.WaitOne(1000);

        IsActivated = true;
    }

    /// <summary>
    /// Internal consumer method. If queue does not have any items InternalQueue.GetConsumingEnumerable waits for new items!!!!
    /// </summary>
    private async Task<bool> RunInternal(AutoResetEvent wait)
    {
        Thread.CurrentThread.Priority = ThreadPriority;

        wait.Set();

        var reader = InternalQueue.Reader;

        await foreach (var item in reader.ReadAllAsync(_cancellationTokenSource.Token))
        {
            ConsumerTaskDelegate.Invoke(item);
        }

        return true;
    }

    /// <summary>
    /// Stop the consumer thread
    /// </summary>
    public void StopConsumer()
    {
        IsActivated = false;

        // Flush the cache
        Flush();

        Task.Delay(200).Wait();

        InternalQueue.Writer.TryComplete();

        InternalQueue.Reader.Completion.Wait(60000);

        _task?.Wait(60000);

        _cancellationTokenSource.Cancel(false);

        ConsumerTaskDelegate = _ => { };
    }

    /// <summary>
    /// Flush the cache to <see cref="ICachingProducerConsumerQueue{T}.ConsumerTaskDelegate"/>
    /// </summary>
    public void Flush()
    {
        if (_cache.IsEmpty)
        {
            return;
        }

        InternalQueue.Writer.TryWrite(_cache.ToArray());
        _cache.Clear();
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        StopConsumer();
        IsActivated = false;
    }
}
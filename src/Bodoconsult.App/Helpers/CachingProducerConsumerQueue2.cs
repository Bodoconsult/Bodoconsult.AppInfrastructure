// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

// https://deniskyashif.com/2020/01/07/csharp-channels-part-3/

using Bodoconsult.App.Abstractions.Interfaces;
using System.Diagnostics;
using System.Threading.Channels;

namespace Bodoconsult.App.Helpers;

/// <summary>
/// Thread-safe implementation for a <see cref="ICachingProducerConsumerQueue2{TType}"/>. Supports one or many producers but only one consumer.
/// </summary>
public class CachingProducerConsumerQueue2<T> : ICachingProducerConsumerQueue2<T> where T : struct
{
    private readonly Lock _cacheLock = new();
    private readonly List<T> _cache = new(100);
    private CancellationTokenSource _cancellationTokenSource = new();
    private readonly WatchDog _watchDog;
    private Task? _task;

    /// <summary>
    /// Default ctor
    /// </summary>
    public CachingProducerConsumerQueue2()
    {
        _watchDog = new WatchDog(Runner, 20);
        _watchDog.StartWatchDog();
    }

    private void Runner()
    {
        int count;
        lock (_cacheLock)
        {
            count = _cache.Count;
        }

        Debug.Print($"Runner {count}");

        if (count >= CacheSize)
        {
            Flush();
        }
    }

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
    public Channel<List<T>> InternalQueue { get; private set; } = Channel.CreateBounded<List<T>>(new BoundedChannelOptions(100)
    {
        SingleReader = true,
        SingleWriter = false,
        FullMode = BoundedChannelFullMode.Wait
    });

    /// <summary>
    /// The delegate to consume each item added to the queue
    /// </summary>
    public ConsumerTaskDelegate<List<T>> ConsumerTaskDelegate { get; set; } = _ => { };

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

        lock (_cacheLock)
        {
            _cache.Add(item);
            var count = _cache.Count;
            if (count <= CacheSize)
            {
                return;
            }

            InternalQueue.Writer.TryWrite(_cache.ToList());
            _cache.Clear();
        }
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

        lock (_cacheLock)
        {
            _cache.AddRange(items);
            var count = _cache.Count;
            if (count <= CacheSize)
            {
                return;
            }

            InternalQueue.Writer.TryWrite(_cache.ToList());
            _cache.Clear();
        }
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

        InternalQueue.Writer.TryComplete();

        InternalQueue = Channel.CreateBounded<List<T>>(new BoundedChannelOptions(CacheSize)
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

        //while (await reader.WaitToReadAsync(_cancellationTokenSource.Token))
        //{
        //    while (reader.TryRead(out var item))
        //    {
        //        ConsumerTaskDelegate.Invoke(item);
        //    }
        //}

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
        // Clear the cache
        List<T> data;

        lock (_cacheLock)
        {
            data = new List<T>(_cache.Count);
            data.AddRange(_cache);
            _cache.Clear();
        }

        if (data.Count == 0)
        {
            return;
        }

        InternalQueue.Writer.TryWrite(data);
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        StopConsumer();

        _watchDog.StopWatchDog();

        IsActivated = false;
    }
}
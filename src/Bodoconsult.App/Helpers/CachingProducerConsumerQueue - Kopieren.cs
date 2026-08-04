//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

//using Bodoconsult.App.Abstractions.Interfaces;
//using System.Collections.Concurrent;
//using System.Diagnostics;

//namespace Bodoconsult.App.Helpers;

///// <summary>
///// Thread-safe implementation for a <see cref="ICachingProducerConsumerQueue{TType}"/>. Supports one or many producers but only one consumer.
///// </summary>
//public class CachingProducerConsumerQueue<T> : ICachingProducerConsumerQueue<T> where T : class
//{
//    private readonly Lock _cacheLock = new();
//    private readonly List<T> _cache = new(100);
//    private CancellationTokenSource _cancellationTokenSource;
//    private readonly WatchDog _watchDog;

//    /// <summary>
//    /// Default ctor
//    /// </summary>
//    public CachingProducerConsumerQueue()
//    {
//        _watchDog = new WatchDog(Runner, 20);
//        _watchDog.StartWatchDog();
//    }

//    private void Runner()
//    {
//        int count;
//        lock (_cacheLock)
//        {
//            count = _cache.Count;
//        }

//        Debug.Print($"Runner {count}");

//        if (count >= CacheSize)
//        {
//            Flush();
//        }
//    }

//    /// <summary>
//    /// Cache size
//    /// </summary>
//    public int CacheSize { get; set; } = 100;

//    /// <summary>
//    /// Thread priority
//    /// </summary>
//    public ThreadPriority ThreadPriority { get; set; } = ThreadPriority.Normal;

//    /// <summary>
//    /// Contains the internal queue
//    /// </summary>
//    public BlockingCollection<List<T>> InternalQueue;

//    /// <summary>
//    /// The delegate to consume each item added to the queue
//    /// </summary>
//    public ConsumerTaskDelegate<List<T>> ConsumerTaskDelegate { get; set; }

//    /// <summary>
//    /// Is the queue started?
//    /// </summary>
//    public bool IsActivated { get; private set; }

//    /// <summary>
//    /// Enqueue an item to the internal queue for processing as soon as possible
//    /// </summary>
//    /// <param name="item">Item to add to the queue</param>
//    public void Enqueue(T item)
//    {
//        if (!IsActivated)
//        {
//            return;
//        }

//        lock (_cacheLock)
//        {
//            _cache.Add(item);
//        }
//    }

//    /// <summary>
//    /// Enqueue a list of items to the internal queue for processing as soon as possible
//    /// </summary>
//    /// <param name="items">Items to add to the queue</param>
//    public void Enqueue(List<T> items)
//    {
//        if (!IsActivated)
//        {
//            return;
//        }

//        if (items.Count < CacheSize)
//        {
//            lock (_cacheLock)
//            {
//                _cache.AddRange(items);
//            }
//        }
//        else
//        {
//            var list = new List<T>(CacheSize);
//            for (var i = 0; i < items.Count; i += CacheSize)
//            {
//                list = i + CacheSize >= items.Count ?
//                    items.GetRange(i, items.Count - i) :
//                    items.GetRange(i, CacheSize);

//                if (list.Count <= 0)
//                {
//                    continue;
//                }

//                lock (_cacheLock)
//                {
//                    _cache.AddRange(list);
//                }

//                list.Clear();
//            }
//            list.Clear();
//        }
//    }

//    /// <summary>
//    /// Start the consumer thread
//    /// </summary>
//    public void StartConsumer()
//    {
//        if (ConsumerTaskDelegate == null)
//        {
//            throw new ArgumentNullException(nameof(ConsumerTaskDelegate));
//        }

//        InternalQueue = new BlockingCollection<List<T>>();
//        _cancellationTokenSource = new CancellationTokenSource();

//        var wait = new AutoResetEvent(false);

//        Task.Factory.StartNew(() =>
//        {
//            RunInternal(wait);
//        }, TaskCreationOptions.LongRunning);

//        wait.WaitOne(1000);

//        IsActivated = true;
//    }

//    /// <summary>
//    /// Internal consumer method. If queue does not have any items InternalQueue.GetConsumingEnumerable waits for new items!!!!
//    /// </summary>
//    private void RunInternal(AutoResetEvent wait)
//    {
//        if (InternalQueue == null)
//        {
//            return;
//        }

//        Thread.CurrentThread.Priority = ThreadPriority;

//        wait.Set();

//        foreach (var item in InternalQueue.GetConsumingEnumerable(_cancellationTokenSource.Token))
//        {
//            ConsumerTaskDelegate.Invoke(item);
//        }
//    }

//    private bool IsCompleted()
//    {
//        return InternalQueue?.IsCompleted ?? true;
//    }

//    /// <summary>
//    /// Stop the consumer thread
//    /// </summary>
//    public void StopConsumer()
//    {
//        IsActivated = false;

//        // Flush the cache
//        Flush();

//        // Now stop queue
//        InternalQueue?.CompleteAdding();

//        //Thread.Sleep(50);
//        Wait.Until(IsCompleted);
//        _cancellationTokenSource?.Cancel(false);
        
//        InternalQueue?.Dispose();
//        InternalQueue = null;
//        ConsumerTaskDelegate = null;
//    }

//    /// <summary>
//    /// Flush the cache to <see cref="ICachingProducerConsumerQueue{T}.ConsumerTaskDelegate"/>
//    /// </summary>
//    public void Flush()
//    {
//        // Clear the cache
//        if (InternalQueue == null || InternalQueue.IsCompleted)
//        {
//            return;
//        }

//        List<T> data;

//        lock (_cacheLock)
//        {
//            data = new List<T>(_cache.Count);
//            data.AddRange(_cache);
//            _cache.Clear();
//        }

//        if (data.Count == 0)
//        {
//            return;
//        }

//        InternalQueue.Add(data);
//    }

//    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
//    public void Dispose()
//    {
//        StopConsumer();

//        _watchDog.StopWatchDog();

//        IsActivated = false;
//    }
//}
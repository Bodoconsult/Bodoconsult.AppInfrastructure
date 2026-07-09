// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using Bodoconsult.App.Abstractions.Interfaces;
using System.Timers;
using Bodoconsult.App.Helpers;

namespace Bodoconsult.App.DataCollectionServices;

/// <summary>
/// Current implementation of <see cref="IDataCollectionService&lt;T&gt;"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public class DataCollectionService<T> : IDataCollectionService<T> where T : class
{
    private System.Timers.Timer _aTimer;
    private readonly Lock _dataLock = new();
    private readonly Lock _isActiveLock = new();
    private readonly ProducerConsumerQueue<List<T>> _queue = new();

    /// <summary>
    /// Do not use directly
    /// </summary>
    private bool _isActive;

    /// <summary>
    /// Defauult ctor
    /// </summary>
    /// <param name="forwardCollectDataDelegate">Delegate to forward collected data from an <see cref="IDataCollectionService&lt;T&gt;"/> implementation after collection time has passed</param>
    public DataCollectionService(ForwardCollectDataDelegate<T> forwardCollectDataDelegate)
    {
        ForwardCollectDataDelegate = forwardCollectDataDelegate;
        _queue.ConsumerTaskDelegate = ConsumerTaskDelegate;
    }

    private void ConsumerTaskDelegate(List<T> data)
    {
        ForwardCollectDataDelegate.Invoke(data);
    }

    /// <summary>
    /// Delegate to forward collected data from an <see cref="IDataCollectionService&lt;T&gt;"/> implementation after collection time has passed
    /// </summary>
    public ForwardCollectDataDelegate<T> ForwardCollectDataDelegate { get; }

    /// <summary>
    /// The time period the service is collecting data in ms. The service is collecting data every <see cref="CollectionInterval"/> ms for this period of time. <see cref="CollectionInterval"/> must be bigger than <see cref="CollectionTime"/>. Default: 1000ms
    /// </summary>
    public int CollectionTime { get; set; } = 1000;

    /// <summary>
    /// The time interval the service is collecting data for a period of <see cref="CollectionTime"/>> ms in ms. Default: 5000ms
    /// </summary>
    public int CollectionInterval { get; set; } = 5000;

    /// <summary>
    /// Is the service active currently
    /// </summary>
    public bool IsActive
    {
        get
        {
            lock (_isActiveLock)
            {
                return _isActive;
            }
        }
        private set
        {
            lock (_isActiveLock)
            {
                _isActive = value;
            }
        }
    }

    /// <summary>
    /// The currently collected data
    /// </summary>
    public List<T> Data { get; } = new();

    /// <summary>
    /// Start the data collection
    /// </summary>
    public void Start()
    {
        if (CollectionInterval < CollectionTime + 500)
        {
            throw new ArgumentException("Collection interval must be bigger by 500ms at least than Collection period");
        }

        _queue.StartConsumer();

        _aTimer = new System.Timers.Timer(CollectionInterval);
        // Hook up the Elapsed event for the timer. 
        _aTimer.Elapsed += OnTimedEvent;
        _aTimer.AutoReset = true;
        _aTimer.Enabled = true;
    }

    private void OnTimedEvent(object sender, ElapsedEventArgs e)
    {
        Debug.Print("Collecting...");
        IsActive = true;
        Task.Delay(CollectionTime).GetAwaiter().GetResult();
        IsActive = false;

        Debug.Print("Collecting stopped");

        var data = Data.ToList();
        Data.Clear();

        _queue.Enqueue(data);
    }

    /// <summary>
    /// Stop the data collection
    /// </summary>
    public void Stop()
    {
        _queue.StopConsumer();

        _aTimer?.Stop();
        _aTimer?.Dispose();
        _aTimer = null;
    }

    /// <summary>
    /// Add an item to the data collection if service is activated
    /// </summary>
    /// <param name="item">Item to collect</param>
    public void Add(T item)
    {
        if (!IsActive)
        {
            Debug.Print("Item not added");
            return;
        }

        lock (_dataLock)
        {
            Data.Add(item);
        }

        Debug.Print("Add item");
    }

    /// <summary>
    /// Add a list of items to the data collection if service is activated
    /// </summary>
    /// <param name="items">List of items to collect</param>
    public void Add(List<T> items)
    {
        if (!IsActive)
        {
            return;
        }

        lock (_dataLock)
        {
            Data.AddRange(items);
        }
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        _queue.StopConsumer();
        _queue.Dispose();

        _aTimer?.Dispose();
    }
}
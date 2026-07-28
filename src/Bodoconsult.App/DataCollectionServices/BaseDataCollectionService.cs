// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using System.Timers;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Helpers;

namespace Bodoconsult.App.DataCollectionServices;

/// <summary>
/// Base class for <see cref="IDataCollectionService&lt;T&gt;"/> implementations
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseDataCollectionService<T> : IDataCollectionService<T> where T : class
{
    private System.Timers.Timer _aTimer;
    private readonly Lock _dataLock = new();
    private readonly Lock _isActiveLock = new();
    
    /// <summary>
    /// The internal queue
    /// </summary>
    protected readonly ProducerConsumerQueue<List<T>> Queue = new();

    /// <summary>
    /// Do not use directly
    /// </summary>
    private bool _isActive;

    /// <summary>
    /// The internal delegate to handle data in the queue
    /// </summary>
    /// <param name="data">Data in the queue</param>
    protected void ConsumerTaskDelegate(List<T> data)
    {
        ForwardCollectDataDelegate.Invoke(data);
    }

    /// <summary>
    /// Default ctor
    /// </summary>
    protected BaseDataCollectionService(ForwardCollectDataDelegate<T> forwardCollectDataDelegate)
    {
        Queue.ConsumerTaskDelegate = ConsumerTaskDelegate;
        ForwardCollectDataDelegate = forwardCollectDataDelegate;
    }

    /// <summary>
    /// Delegate to forward collected data from an <see cref="IDataCollectionService&lt;T&gt;"/> implementation after collection time has passed
    /// </summary>
    public ForwardCollectDataDelegate<T> ForwardCollectDataDelegate { get; protected set; }

    /// <summary>
    /// The time interval the service is collecting data for a period in ms. Default: 5000ms
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
        protected set
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
    public virtual void Start()
    {
        Queue.StartConsumer();

        _aTimer = new System.Timers.Timer(CollectionInterval);
        // Hook up the Elapsed event for the timer. 
        _aTimer.Elapsed += OnTimedEvent;
        _aTimer.AutoReset = true;
        _aTimer.Enabled = true;
    }

    /// <summary>
    /// Timer event
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">EventArgs</param>
    /// <exception cref="NotSupportedException"></exception>
    protected virtual void OnTimedEvent(object sender, ElapsedEventArgs e)
    {
        throw new NotSupportedException("Override in derived classes");
    }

    /// <summary>
    /// Stop the data collection
    /// </summary>
    public void Stop()
    {
        Queue.StopConsumer();

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

    /// <summary>
    /// Set the property <see cref="IsActive"/> to true. Do NOT use in production code. Intedned only for unit tests
    /// </summary>
    public void SetIsActive()
    {
        IsActive = true;
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        Queue.StopConsumer();
        Queue.Dispose();

        _aTimer?.Dispose();
    }
}
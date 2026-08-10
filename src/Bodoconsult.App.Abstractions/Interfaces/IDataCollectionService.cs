// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Delegate to forward collected data from an <see cref="IDataCollectionService&lt;T&gt;"/> implementation after collection time has passed
/// </summary>
/// <typeparam name="T">Type of data to collect</typeparam>
/// <param name="data">Collected data</param>
public delegate void ForwardCollectDataDelegate<in T>(IReadOnlyList<T> data) where T : class;

/// <summary>
/// Interface for data collection services. A data collection service is a service collection data if activated from a data stream in time intervals for a certain time period.
/// A data stream is not a real stream implementing a C# stream type.
/// It is more a steam of data coming in from an external source in distinct messages.
/// </summary>
public interface IDataCollectionService<T>: IDisposable where T : class
{
    /// <summary>
    /// Delegate to forward collected data from an <see cref="IDataCollectionService&lt;T&gt;"/> implementation after collection time has passed
    /// </summary>
    ForwardCollectDataDelegate<T> ForwardCollectDataDelegate { get; }

    ///// <summary>
    ///// The time period the service is collecting data in ms. The service is collecting data every <see cref="CollectionInterval"/> ms for this period of time. <see cref="CollectionInterval"/> must be bigger than <see cref="CollectionTime"/>
    ///// </summary>
    //int CollectionTime { get; set; }

    /// <summary>
    /// The time interval the service is collecting data for a certain period in ms. 
    /// </summary>
    int CollectionInterval { get; set; }

    /// <summary>
    /// Is the service active currently
    /// </summary>
    bool IsActive { get; }

    /// <summary>
    /// The currently collected data
    /// </summary>
    List<T> Data { get; }

    /// <summary>
    /// Start the data collection
    /// </summary>
    void Start();

    /// <summary>
    /// Stop the data collection
    /// </summary>
    void Stop();

    /// <summary>
    /// Add an item to the data collection if service is activated
    /// </summary>
    /// <param name="item">Item to collect</param>
    void Add(T item);

    /// <summary>
    /// Add a list of items to the data collection if service is activated
    /// </summary>
    /// <param name="items">List of items to collect</param>
    void Add(List<T> items);
}

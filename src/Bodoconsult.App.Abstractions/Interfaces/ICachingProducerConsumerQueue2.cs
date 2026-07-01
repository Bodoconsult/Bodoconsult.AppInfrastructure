// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Implements a thread-safe generic producer consumer based pattern cachin data using threads: Use it for structs
/// </summary>
public interface ICachingProducerConsumerQueue2<T> where T : struct
{
    /// <summary>
    /// Cache size
    /// </summary>
    int CacheSize { get; set; }

    /// <summary>
    /// Thread priority
    /// </summary>
    ThreadPriority ThreadPriority { get; set; }

    /// <summary>
    /// The delegate to consume each item added to the queue
    /// </summary>
    ConsumerTaskDelegate<List<T>> ConsumerTaskDelegate { get; set; }

    /// <summary>
    /// Is the queue started?
    /// </summary>
    bool IsActivated { get; }

    /// <summary>
    /// Enqueue an item to the internal queue for processing as soon as possible
    /// </summary>
    /// <param name="item">Item to add to the queue</param>
    void Enqueue(T item);

    /// <summary>
    /// Enqueue a list of items to the internal queue for processing as soon as possible
    /// </summary>
    /// <param name="items">Items to add to the queue</param>
    void Enqueue(List<T> items);

    /// <summary>
    /// Start the consumer thread
    /// </summary>
    void StartConsumer();

    /// <summary>
    /// Stop the consumer thread
    /// </summary>
    void StopConsumer();

    /// <summary>
    /// Flush the cache to <see cref="ConsumerTaskDelegate"/>
    /// </summary>
    void Flush();

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    void Dispose();
}
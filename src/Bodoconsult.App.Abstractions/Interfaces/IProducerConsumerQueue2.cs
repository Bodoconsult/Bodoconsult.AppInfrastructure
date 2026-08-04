// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// A delegate for consumer task used in <see cref="IProducerConsumerQueue{TType}"/>. Supports many producers but only one consumer.
/// </summary>
/// <typeparam name="T">A class type</typeparam>
/// <param name="value">Current instance of TType</param>
public delegate void ConsumerTaskDelegate2<in T>(T value) where T : struct;

/// <summary>
/// Implements a thread-safe generic producer consumer based pattern using threads. Use it for structs
/// </summary>
public interface IProducerConsumerQueue2<T> : IDisposable where T : struct
{
    /// <summary>
    /// The delegate to consume each item added to the queue
    /// </summary>
    ConsumerTaskDelegate2<T> ConsumerTaskDelegate { get; set; }

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
    /// Enqueue a list of itema to the internal queue for processing as soon as possible
    /// </summary>
    /// <param name="items">List of items to add to the queue</param>
    void Enqueue(IEnumerable<T> items);

    /// <summary>
    /// Start the consumer thread
    /// </summary>
    void StartConsumer();

    /// <summary>
    /// Stop the consumer thread
    /// </summary>
    void StopConsumer();
}
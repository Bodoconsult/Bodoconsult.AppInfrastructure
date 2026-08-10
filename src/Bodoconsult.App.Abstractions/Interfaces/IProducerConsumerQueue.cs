// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// A delegate for consumer task used in <see cref="IProducerConsumerQueue{TType}"/>. Supports many producers but only one consumer.
/// </summary>
/// <typeparam name="T">A class type</typeparam>
/// <param name="data">Current instance of TType</param>
public delegate void ConsumerTaskDelegate<in T>(T data) where T : class;

/// <summary>
/// A delegate for consumer task used in <see cref="IProducerConsumerQueue{TType}"/>. Supports many producers but only one consumer.
/// </summary>
/// <typeparam name="T">A class type</typeparam>
/// <param name="data">Current instance of TType</param>
public delegate Task ConsumerTaskDelegateAsync<in T>(T data) where T : class;

/// <summary>
/// Implements a thread-safe generic producer consumer based pattern using threads: Use it for classes
/// </summary>
public interface IProducerConsumerQueue<T>: IDisposable where T: class
{
    /// <summary>
    /// Capacity of the queue
    /// </summary>
    int Capacity { get; set; }
    
    /// <summary>
    /// The delegate to consume each item added to the queue
    /// </summary>
    ConsumerTaskDelegate<T> ConsumerTaskDelegate { get; set; }

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
    void Enqueue(IReadOnlyList<T> items);

    /// <summary>
    /// Start the consumer thread
    /// </summary>
    void StartConsumer();

    /// <summary>
    /// Stop the consumer thread
    /// </summary>
    void StopConsumer();

}
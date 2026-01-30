// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.Concurrent;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Helpers;

/// <summary>
/// Thread-safe implementation for a <see cref="IProducerConsumerQueue{TType}"/>. Supports one or many producers but only one consumer.
/// </summary>
public class ProducerConsumerQueue2<T> : IProducerConsumerQueue2<T> where T : struct
{

    private Thread _consumerThread;


    /// <summary>
    /// Contains the internal queue
    /// </summary>
    public BlockingCollection<T> InternalQueue;

    /// <summary>
    /// The delegate to consume each item added to the queue
    /// </summary>
    public ConsumerTaskDelegate2<T> ConsumerTaskDelegate { get; set; }

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
        try
        {
            if (InternalQueue == null || InternalQueue.IsCompleted)
            {
                return;
            }
            InternalQueue.Add(item);
        }
        catch //(Exception e)
        {
            // Do nothing
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

        InternalQueue = new BlockingCollection<T>();

        _consumerThread = new Thread(RunInternal)
        {
            //Priority = _threadPriority,
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
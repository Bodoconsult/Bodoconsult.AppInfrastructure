//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

//using Bodoconsult.App.Abstractions.Interfaces;
//using System.Collections.Concurrent;

//namespace Bodoconsult.App.Helpers;

///// <summary>
///// Thread-safe implementation for a <see cref="IProducerConsumerQueue{TType}"/>. Supports one or many producers but only one consumer.
///// </summary>
//public class ProducerConsumerQueue<T> : IProducerConsumerQueue<T> where T : class
//{
//    private CancellationTokenSource _cancellationTokenSource;

//    /// <summary>
//    /// Thread priority
//    /// </summary>
//    public ThreadPriority ThreadPriority { get; set; } = ThreadPriority.Normal;

//    /// <summary>
//    /// Contains the internal queue
//    /// </summary>
//    public BlockingCollection<T> InternalQueue;

//    /// <summary>
//    /// The delegate to consume each item added to the queue
//    /// </summary>
//    public ConsumerTaskDelegate<T> ConsumerTaskDelegate { get; set; }

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
//        if (!IsActivated || InternalQueue.IsCompleted)
//        {
//            return;
//        }
//        InternalQueue.Add(item);
//    }

//    /// <summary>
//    /// Enqueue a list of itema to the internal queue for processing as soon as possible
//    /// </summary>
//    /// <param name="items">List of items to add to the queue</param>
//    public void Enqueue(IList<T> items)
//    {
//        if (!IsActivated || InternalQueue.IsCompleted)
//        {
//            return;
//        }

//        foreach (var tItem in items)
//        {
//            InternalQueue.Add(tItem);
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

//        InternalQueue = new BlockingCollection<T>();

//        _cancellationTokenSource = new CancellationTokenSource();
//        Task.Factory.StartNew(RunInternal, TaskCreationOptions.LongRunning);

//        IsActivated = true;
//    }

//    /// <summary>
//    /// Internal consumer method. If queue does not have any items InternalQueue.GetConsumingEnumerable waits for new items!!!!
//    /// </summary>
//    private void RunInternal()
//    {
//        if (InternalQueue == null)
//        {
//            return;
//        }

//        Thread.CurrentThread.Priority = ThreadPriority;

//        foreach (var item in InternalQueue.GetConsumingEnumerable(_cancellationTokenSource.Token))
//        {
//            ConsumerTaskDelegate.Invoke(item);
//        }
//    }

//    private bool IsCompleted()
//    {
//        return InternalQueue.IsCompleted;
//    }

//    /// <summary>
//    /// Stop the consumer thread
//    /// </summary>
//    public void StopConsumer()
//    {
//        IsActivated = false;
//        InternalQueue?.CompleteAdding();
//        Wait.Until(IsCompleted);
//        _cancellationTokenSource?.Cancel(false);
//        InternalQueue?.Dispose();
//        InternalQueue = null;
//        ConsumerTaskDelegate = null;
//    }

//    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
//    public void Dispose()
//    {
//        StopConsumer();

//        IsActivated = false;
//    }
//}
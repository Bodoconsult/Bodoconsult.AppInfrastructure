// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace Bodoconsult.App.Helpers;

/// <summary>
/// Thread-safe implementation for a <see cref="IProducerConsumerQueueAsync2{TType}"/>. Supports one or many producers but only one consumer.
/// </summary>
public class ProducerConsumerQueueAsync2<T> : IProducerConsumerQueueAsync2<T> where T : struct
{
    private CancellationTokenSource _cancellationTokenSource;

    /// <summary>
    /// Thread priority
    /// </summary>
    public ThreadPriority ThreadPriority { get; set; } = ThreadPriority.Normal;
    /// <summary>
    /// Contains the internal queue
    /// </summary>
    public Channel<T> InternalQueue;

    /// <summary>
    /// Capacity of the queue
    /// </summary>
    public int Capacity { get; set; } = 100;

    /// <summary>
    /// The delegate to consume each item added to the queue
    /// </summary>
    public ConsumerTaskDelegateAsync2<T> ConsumerTaskDelegate { get; set; }

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
        if (InternalQueue == null)
        {
            return;
        }
        InternalQueue.Writer.TryWrite(item);
    }

    /// <summary>
    /// Enqueue a list of itema to the internal queue for processing as soon as possible
    /// </summary>
    /// <param name="items">List of items to add to the queue</param>
    public void Enqueue(IEnumerable<T> items)
    {
        if (InternalQueue == null)
        {
            return;
        }

        var writer = InternalQueue.Writer;

        foreach (var tItem in items)
        {
            writer.TryWrite(tItem);
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

        InternalQueue = Channel.CreateBounded<T>(new BoundedChannelOptions(Capacity)
        {
            SingleReader = true,
            SingleWriter = false,
            FullMode = BoundedChannelFullMode.Wait
        });

        _cancellationTokenSource = new CancellationTokenSource();
        Task.Factory.StartNew(async () =>
        {
            await RunInternal();
        }, TaskCreationOptions.LongRunning);

        IsActivated = true;
    }

    /// <summary>
    /// Internal consumer method. If queue does not have any items InternalQueue.GetConsumingEnumerable waits for new items!!!!
    /// </summary>
    private async Task RunInternal()
    {
        if (InternalQueue == null)
        {
            return;
        }

        Thread.CurrentThread.Priority = ThreadPriority;

        var reader = InternalQueue.Reader;

        while (await reader.WaitToReadAsync(_cancellationTokenSource.Token))
        {
            while (reader.TryRead(out var item))
            {
                await ConsumerTaskDelegate.Invoke(item);
            }
        }
    }

    /// <summary>
    /// Stop the consumer thread
    /// </summary>
    public void StopConsumer()
    {
        IsActivated = false;
        _cancellationTokenSource?.Cancel(false);
        InternalQueue?.Writer.TryComplete();

        Task.Delay(200).Wait();

        InternalQueue = null;
        ConsumerTaskDelegate = null;
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        StopConsumer();

        IsActivated = false;
    }
}
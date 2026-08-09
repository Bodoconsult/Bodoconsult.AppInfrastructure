// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using System.Threading.Channels;

namespace Bodoconsult.App.Helpers;

/// <summary>
/// Thread-safe implementation for a <see cref="IProducerConsumerQueue{TType}"/>. Supports one or many producers but only one consumer.
/// </summary>
public class ProducerConsumerQueue<T> : IProducerConsumerQueue<T> where T : class
{
    private CancellationTokenSource _cancellationTokenSource = new();

    /// <summary>
    /// Contains the internal queue
    /// </summary>
    private Channel<T> _internalQueue = Channel.CreateBounded<T>(new BoundedChannelOptions(100)
    {
        SingleReader = true,
        SingleWriter = false,
        FullMode = BoundedChannelFullMode.Wait
    });

    /// <summary>
    /// Thread priority
    /// </summary>
    public ThreadPriority ThreadPriority { get; set; } = ThreadPriority.Normal;


    /// <summary>
    /// Capacity of the queue
    /// </summary>
    public int Capacity { get; set; } = 100;

    /// <summary>
    /// The delegate to consume each item added to the queue
    /// </summary>
    public ConsumerTaskDelegate<T> ConsumerTaskDelegate { get; set; } = _ => { };

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
        if (!IsActivated)
        {
            return;
        }
        _internalQueue.Writer.TryWrite(item);
    }

    /// <summary>
    /// Enqueue a list of itema to the internal queue for processing as soon as possible
    /// </summary>
    /// <param name="items">List of items to add to the queue</param>
    public void Enqueue(IEnumerable<T> items)
    {
        if (!IsActivated)
        {
            return;
        }

        var writer = _internalQueue.Writer;

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
        if (ConsumerTaskDelegate is null)
        {
            throw new ArgumentNullException(nameof(ConsumerTaskDelegate));
        }

        _internalQueue.Writer.TryComplete();

        _internalQueue = Channel.CreateBounded<T>(new BoundedChannelOptions(Capacity)
        {
            SingleReader = true,
            SingleWriter = false,
            FullMode = BoundedChannelFullMode.Wait
        });

        _cancellationTokenSource.Dispose();

        _cancellationTokenSource = new CancellationTokenSource();
        Task.Factory.StartNew(RunInternal, TaskCreationOptions.LongRunning);

        IsActivated = true;
    }

    /// <summary>
    /// Internal consumer method. If queue does not have any items InternalQueue.GetConsumingEnumerable waits for new items!!!!
    /// </summary>
    private async Task<bool> RunInternal()
    {
        Thread.CurrentThread.Priority = ThreadPriority;

        var reader = _internalQueue.Reader;

        while (await reader.WaitToReadAsync(_cancellationTokenSource.Token))
        {
            while (reader.TryRead(out var item))
            {
                ConsumerTaskDelegate.Invoke(item);
            }
        }

        return true;
    }

    /// <summary>
    /// Stop the consumer thread
    /// </summary>
    public void StopConsumer()
    {
        IsActivated = false;
        _cancellationTokenSource.Cancel(false);
        _internalQueue.Writer.TryComplete();

        Task.Delay(200).Wait();

        ConsumerTaskDelegate = _ => { };
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        StopConsumer();

        IsActivated = false;
    }
}
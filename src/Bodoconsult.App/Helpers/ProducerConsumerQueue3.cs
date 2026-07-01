// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Threading.Channels;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Helpers;

/// <summary>
/// Thread-safe implementation for a <see cref="IProducerConsumerQueue{TType}"/> based on channels. Supports one or many producers but only one consumer.
/// </summary>
public class ProducerConsumerQueue3<T> : IProducerConsumerQueue<T> where T : class
{
    private readonly Channel<T> _channel;
    private CancellationTokenSource _cancellationTokenSource;

    /// <summary>
    /// The delegate to consume each item added to the queue
    /// </summary>
    public ConsumerTaskDelegate<T> ConsumerTaskDelegate { get; set; }

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
        _channel.Writer.TryWrite(item);
    }

    /// <summary>
    /// Enqueue a list of itema to the internal queue for processing as soon as possible
    /// </summary>
    /// <param name="items">List of items to add to the queue</param>
    public void Enqueue(IList<T> items)
    {
        foreach (var item in items)
        {
            _channel.Writer.TryWrite(item);
        }
    }

    /// <summary>
    ///  Default ctor
    /// </summary>
    public ProducerConsumerQueue3()
    {
        _channel = Channel.CreateUnbounded<T>();
    }

    /// <summary>
    /// Start the consumer thread
    /// </summary>
    public void StartConsumer()
    {
        ArgumentNullException.ThrowIfNull(ConsumerTaskDelegate);

        _cancellationTokenSource = new CancellationTokenSource();

        Task.Run(async () =>
        {
            await foreach (var message in _channel.Reader.ReadAllAsync(_cancellationTokenSource.Token))
            {
                ConsumerTaskDelegate.Invoke(message);
            }
        });

        IsActivated = true;
    }

    /// <summary>
    /// Stop the consumer thread
    /// </summary>
    public void StopConsumer()
    {
        IsActivated = false;

         _channel.Writer.TryComplete();

        _cancellationTokenSource?.Cancel(false);
        
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        StopConsumer();
    }
}
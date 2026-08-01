// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using System.Timers;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.DataCollectionServices;

/// <summary>
/// Current implementation of <see cref="IDataCollectionService&lt;T&gt;"/> using a counter <see cref="CollectionCounter"/>
/// </summary>
/// <typeparam name="T">Type of data to collect</typeparam>
public class CounterDataCollectionService<T> : BaseDataCollectionService<T> where T : class
{
    /// <summary>
    /// Defauult ctor
    /// </summary>
    /// <param name="forwardCollectDataDelegate">Delegate to forward collected data from an <see cref="IDataCollectionService&lt;T&gt;"/> implementation after collection time has passed</param>
    public CounterDataCollectionService(ForwardCollectDataDelegate<T> forwardCollectDataDelegate) : base(forwardCollectDataDelegate)
    { }

    /// <summary>
    /// The number of counts the service is collecting data in ms. The service is collecting data every CollectionInterval ms until the number of counts is reached
    /// </summary>
    public int CollectionCounter { get; set; } = 10;

    /// <summary>
    /// Timer event
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">EventArgs</param>
    protected override void OnTimedEvent(object sender, ElapsedEventArgs e)
    {
        Debug.Print("Collecting...");
        var cancellationTokenSource = new CancellationTokenSource(CollectionInterval - 20);
        
        IsActive = true;

        while (Data.Count < CollectionCounter)
        {
            Task.Delay(15, cancellationTokenSource.Token).GetAwaiter().GetResult();
            if (cancellationTokenSource.IsCancellationRequested)
            {
                break;
            }
        }

        IsActive = false;

        Debug.Print("Collecting stopped");
        var data = Data.ToList();
        Data.Clear();

        if (data.Count == 0)
        {
            return;
        }
        Queue.Enqueue(data);
    }
}
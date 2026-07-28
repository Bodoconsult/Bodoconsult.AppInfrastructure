// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using Bodoconsult.App.Abstractions.Interfaces;
using System.Timers;

namespace Bodoconsult.App.DataCollectionServices;

/// <summary>
/// Current implementation of <see cref="IDataCollectionService&lt;T&gt;"/> using a time period <see cref="CollectionTime"/>
/// </summary>
/// <typeparam name="T">Type of data to collect</typeparam>
public class TimePeriodDataCollectionService<T> : BaseDataCollectionService<T> where T : class
{
    /// <summary>
    /// Defauult ctor
    /// </summary>
    /// <param name="forwardCollectDataDelegate">Delegate to forward collected data from an <see cref="IDataCollectionService&lt;T&gt;"/> implementation after collection time has passed</param>
    public TimePeriodDataCollectionService(ForwardCollectDataDelegate<T> forwardCollectDataDelegate) : base(forwardCollectDataDelegate)
    { }

    /// <summary>
    /// The time period the service is collecting data in ms. The service is collecting data every CollectionInterval ms for this period of time. CollectionInterval must be bigger than <see cref="CollectionTime"/>. Default: 1000ms
    /// </summary>
    public int CollectionTime { get; set; } = 1000;

    /// <summary>
    /// Start the data collection
    /// </summary>
    public override void Start()
    {
        if (CollectionInterval < CollectionTime + 500)
        {
            throw new ArgumentException("Collection interval must be bigger by 500ms at least than Collection period");
        }

        base.Start();
    }

    /// <summary>
    /// Timer event
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">EventArgs</param>
    protected override void OnTimedEvent(object sender, ElapsedEventArgs e)
    {
        Debug.Print("Collecting...");
        IsActive = true;
        Task.Delay(CollectionTime).GetAwaiter().GetResult();
        IsActive = false;

        Debug.Print("Collecting stopped");

        var data = Data.ToList();
        Data.Clear();

        Queue.Enqueue(data);
    }
}
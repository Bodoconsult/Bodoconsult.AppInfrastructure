// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics.Tracing;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for app wide event tracing
/// </summary>
public interface IAppEventSource
{
    /// <summary>
    /// Add an event source provider to the app event source
    /// </summary>
    /// <param name="provider">Event source provider to add</param>
    void AddProvider(IEventSourceProvider provider);

    /// <summary>
    /// Event counters to be loaded in the app event source
    /// </summary>
    Dictionary<string, EventCounter> EventCounters { get; }

    /// <summary>
    /// Incrementing event counters to be loaded in the app event source
    /// </summary>
    Dictionary<string, IncrementingEventCounter> IncrementingEventCounters { get; }

    /// <summary>
    /// Polling counters to be loaded in the app event source
    /// </summary>
    Dictionary<string, PollingCounter> PollingCounters { get; }

    /// <summary>
    /// Incrementing polling counters to be loaded in the app event source
    /// </summary>
    Dictionary<string, IncrementingPollingCounter> IncrementingPollingCounters { get; }

    /// <summary>
    /// Report a value as metric value to an <see cref="EventCounter"/>
    /// </summary>
    /// <param name="name">Name of the counter</param>
    /// <param name="value">Value to report</param>
    void ReportMetric(string name, float value);

    /// <summary>
    /// Report a value of 1 as increment to an <see cref="IncrementingEventCounter"/>
    /// </summary>
    /// <param name="name">Name of the counter</param>
    void ReportIncrement(string name);

    /// <summary>
    /// Report a value as increment to an <see cref="IncrementingEventCounter"/>
    /// </summary>
    /// <param name="name">Name of the counter</param>
    /// <param name="value">Value to report as increment</param>
    void ReportIncrement(string name, float value);

    /// <summary>
    /// Get an <see cref="EventCounter"/> instance by its name
    /// </summary>
    /// <param name="name">Name of the requested instance</param>
    /// <returns><see cref="EventCounter"/> instance or null</returns>
    EventCounter GetMetricEventCounter(string name);

    /// <summary>
    /// Get an <see cref="IncrementingEventCounters"/> instance by its name
    /// </summary>
    /// <param name="name">Name of the requested instance</param>
    /// <returns><see cref="EventCounter"/> instance or null</returns>
    IncrementingEventCounter GetIncrementEventCounter(string name);

}
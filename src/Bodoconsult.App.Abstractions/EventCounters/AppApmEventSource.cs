// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics.Tracing;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Abstractions.EventCounters;

/// <summary>
/// App event source for APM
/// </summary>
[EventSource(Name = "App-APM")]
public sealed class AppApmEventSource : EventSource, IAppEventSource
{
    private readonly IAppLoggerProxy _appLogger;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appLogger">Current app logger</param>
    public AppApmEventSource(IAppLoggerProxy appLogger)
    {
        _appLogger = appLogger;
    }

    /// <summary>
    /// Add an event source provider to the app event source
    /// </summary>
    /// <param name="provider">Event source provider to add</param>
    public void AddProvider(IEventSourceProvider provider)
    {

        provider.AddEventCounters(this);

        provider.AddIncrementingEventCounters(this);

        provider.AddPollingCounters(this);

        provider.AddIncrementingPollingCounters(this);

    }

    /// <summary>
    /// Event counters to be loaded in the app event source
    /// </summary>
    public Dictionary<string, EventCounter> EventCounters { get; } = new();

    /// <summary>
    /// Incrementing event counters to be loaded in the app event source
    /// </summary>
    public Dictionary<string, IncrementingEventCounter> IncrementingEventCounters { get; } = new();

    /// <summary>
    /// Polling counters to be loaded in the app event source
    /// </summary>
    public Dictionary<string, PollingCounter> PollingCounters { get; } = new();

    /// <summary>
    /// Incrementing polling counters to be loaded in the app event source
    /// </summary>
    public Dictionary<string, IncrementingPollingCounter> IncrementingPollingCounters { get; } = new();

    /// <summary>
    /// Report a value as metric value to an <see cref="EventCounter"/>
    /// </summary>
    /// <param name="name">Name of the counter</param>
    /// <param name="value">Value to report</param>
    public void ReportMetric(string name, float value)
    {
        if (!EventCounters.TryGetValue(name, out var counterInstance))
        {
            _appLogger.LogError($"Event counter {name} does NOT exist");
            return;

            //counterInstance = new EventCounter(name, this);
            //EventCounters.Add(name, counterInstance);
        }
        counterInstance?.WriteMetric(value);
    }

    /// <summary>
    /// Report a value as increment to an <see cref="IncrementingEventCounter"/>
    /// </summary>
    /// <param name="name">Name of the counter</param>
    /// <param name="value">Value to report as increment</param>
    public void ReportIncrement(string name, float value)
    {
        if (!IncrementingEventCounters.TryGetValue(name, out var counterInstance))
        {
            _appLogger.LogError($"Event counter {name} does NOT exist");
            return;
            //counterInstance = new IncrementingEventCounter(name, this);
            //IncrementingEventCounters.Add(name, counterInstance);
        }
        counterInstance?.Increment(value);
    }

    /// <summary>
    /// Get an <see cref="EventCounter"/> instance by its name
    /// </summary>
    /// <param name="name">Name of the requested instance</param>
    /// <returns><see cref="EventCounter"/> instance or null</returns>
    public EventCounter? GetMetricEventCounter(string name)
    {
        if (EventCounters.TryGetValue(name, out var counterInstance))
        {
            return counterInstance;
        }
        _appLogger.LogError($"Event counter {name} does NOT exist");
        return null;
    }

    /// <summary>
    /// Get an <see cref="IAppEventSource.IncrementingEventCounters"/> instance by its name
    /// </summary>
    /// <param name="name">Name of the requested instance</param>
    /// <returns><see cref="EventCounter"/> instance or null</returns>
    public IncrementingEventCounter? GetIncrementEventCounter(string name)
    {
        if (IncrementingEventCounters.TryGetValue(name, out var counterInstance))
        {
            return counterInstance;
        }

        _appLogger.LogError($"Event counter {name} does NOT exist");
        return null;
    }

    /// <summary>
    /// Report a value of 1 as increment to an <see cref="IncrementingEventCounter"/>
    /// </summary>
    /// <param name="name">Name of the counter</param>
    public void ReportIncrement(string name)
    {
        if (!IncrementingEventCounters.TryGetValue(name, out var counterInstance))
        {
            _appLogger.LogError($"Event counter {name} does NOT exist");
            return;

            //counterInstance = new IncrementingEventCounter(name, this);
            //IncrementingEventCounters.Add(name, counterInstance);
        }
        counterInstance?.Increment();
    }


}
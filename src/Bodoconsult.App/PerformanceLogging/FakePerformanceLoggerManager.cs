// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.PerformanceLogging;

/// <summary>
/// Fake implementation of <see cref="IPerformanceLoggerManager"/>
/// </summary>
public class FakePerformanceLoggerManager : IPerformanceLoggerManager
{
    /// <summary>
    /// The delay after the runner method was running in milliseconds
    /// </summary>
    public int DelayUntilNextRunnerFired { get; set; }

    /// <summary>
    /// Current status message delegate to be called from the <see cref="IPerformanceLoggerManager.Log"/> method
    /// </summary>
    public StatusMessageDelegate StatusMessageDelegate { get; set; } = _ => { };

    /// <summary>
    /// Start the performance logging
    /// </summary>
    public void StartLogging()
    {
        // Do nothing
    }

    /// <summary>
    /// Stop the performance logging
    /// </summary>
    public void StopLogging()
    {
        // Do nothing
    }

    /// <summary>
    /// Log the performance data
    /// </summary>
    public void Log()
    {
        // Do nothing
    }
}
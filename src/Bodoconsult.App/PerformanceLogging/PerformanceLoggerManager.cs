// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Helpers;

namespace Bodoconsult.App.PerformanceLogging;

/// <summary>
/// Current implementation of <see cref="IPerformanceLoggerManager"/>
/// </summary>
public class PerformanceLoggerManager: IPerformanceLoggerManager
{
    private IWatchDog? _watchDog;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="performanceLogger">Current performance logger implementation</param>
    public PerformanceLoggerManager(IPerformanceLogger performanceLogger)
    {
        PerformanceLogger = performanceLogger;
    }

    /// <summary>
    /// Current instance of a performance logger
    /// </summary>
    public IPerformanceLogger PerformanceLogger { get; }

    /// <summary>
    /// Current status message delegate to be called from the <see cref="IPerformanceLoggerManager.Log"/> method
    /// </summary>
    public StatusMessageDelegate StatusMessageDelegate { get; set; } = _ => { };

    /// <summary>
    /// Start the performance logging
    /// </summary>
    public void StartLogging()
    {
        _watchDog ??= new WatchDog(Log, DelayUntilNextRunnerFired);

        _watchDog.StartWatchDog();

        PerformanceLogger.StartLogger();
    }

    /// <summary>
    /// The delay after the runner method was running in milliseconds. Default value is 1000ms.
    /// </summary>
    public int DelayUntilNextRunnerFired { get; set; } = 1000;

    /// <summary>
    /// Stop the performance logging
    /// </summary>
    public void StopLogging()
    {
        _watchDog?.StopWatchDog();

        PerformanceLogger.StopLogger();
    }

    /// <summary>
    /// Log the performance data
    /// </summary>
    public void Log()
    {
        StatusMessageDelegate.Invoke(PerformanceLogger.GetCountersAsString() ?? string.Empty);
    }
}
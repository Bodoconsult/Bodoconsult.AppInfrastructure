// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.PerformanceLogging;

/// <summary>
/// Fake implementation of <see cref="IPerformanceLogger"/>
/// </summary>
public class FakePerformanceLogger : IPerformanceLogger
{
    /// <summary>
    /// Get important counters as formatted string
    /// </summary>
    /// <returns>String with performance counter data</returns>
    public string? GetCountersAsString()
    {
        return null;
    }

    /// <summary>
    /// Stop the logger
    /// </summary>
    public void StopLogger()
    {
        // Do nothing
    }

    /// <summary>
    /// Start the logger
    /// </summary>
    public void StartLogger()
    {
        // Do nothing
    }
}
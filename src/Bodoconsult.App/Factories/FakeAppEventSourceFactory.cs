// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// License MIT

using Bodoconsult.App.Abstractions.EventCounters;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Logging;
using Microsoft.Extensions.Logging;

namespace Bodoconsult.App.Factories;

/// <summary>
/// Factory for a fake implementation of <see cref="IAppEventSource"/>
/// </summary>
public class FakeAppEventSourceFactory : IAppEventSourceFactory
{
    private IAppEventSource? _appApmEventSource;

    /// <summary>
    /// Create an instance of a <see cref="FakeAppEventSource"/>
    /// </summary>
    /// <returns><see cref="FakeAppEventSource"/> instance</returns>
    public IAppEventSource CreateInstance()
    {
        if (_appApmEventSource != null)
        {
            return _appApmEventSource;
        }

        _appApmEventSource = new FakeAppEventSource();

        return _appApmEventSource;
    }
}

/// <summary>
/// Factory for creating <see cref="AppLoggerProxy"/> instances
/// </summary>
public class AppLoggerProxyFactory : IAppLoggerProxyFactory
{
    /// <summary>
    /// Create an instance of a <see cref="IAppLoggerProxy"/> implementation
    /// </summary>
    /// <param name="loggerFactory">Current logger factory to use</param>
    /// <param name="logDataFactory">Current logdata factory. Should be singleton instance for loggers</param>
    /// <returns>An instance of a <see cref="IAppLoggerProxy"/></returns>
    public IAppLoggerProxy CreateInstance(ILoggerFactory loggerFactory, ILogDataFactory logDataFactory)
    {
        var result = new AppLoggerProxy(loggerFactory, logDataFactory);
        result.LogInformation("Logging started");
        return result;
    }
}
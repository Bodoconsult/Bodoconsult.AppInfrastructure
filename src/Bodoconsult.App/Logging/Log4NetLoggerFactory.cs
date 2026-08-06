// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

/*
log4net by The Apache Software Foundation

log4net is a tool to help the programmer output log statements to a variety of output targets.
    In case of problems with an application, it is helpful to enable logging so that the problem
can be located. With log4net it is possible to enable logging at runtime without modifying the
application binary. The log4net package is designed so that log statements can remain in
shipped code without incurring a high performance cost. It follows that the speed of logging
(or rather not logging) is crucial.

At the same time, log output can be so voluminous that it quickly becomes overwhelming.
One of the distinctive features of log4net is the notion of hierarchical loggers.
Using these loggers it is possible to selectively control which log statements are output
at arbitrary granularity.

log4net is designed with two distinct goals in mind: speed and flexibility

License: Apache - 2.0

License - Url: https://licenses.nuget.org/Apache-2.0

Project - Url: https://github.com/apache/logging-log4net
*/

using Microsoft.Extensions.Logging;

namespace Bodoconsult.App.Logging;

/// <summary>
/// Simple logger factory for Log4Net (loads settings from app.config)
/// </summary>
public class Log4NetLoggerFactory : ILoggerFactory
{

    private ILogger? _logger;

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        _logger = null;
    }

    /// <summary>
    /// Filename of the Log4Net logging without path
    /// </summary>
    public string ConfigFileName { get; set; } = "log4net.config";

    /// <summary>
    /// Creates a new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance.
    /// </summary>
    /// <param name="categoryName">The category name for messages produced by the logger.</param>
    /// <returns>The <see cref="T:Microsoft.Extensions.Logging.ILogger" />.</returns>
    public ILogger CreateLogger(string categoryName)
    {
        return _logger ??= new Log4NetLogger(categoryName, ConfigFileName);
    }

    /// <summary>
    /// Adds an <see cref="T:Microsoft.Extensions.Logging.ILoggerProvider" /> to the logging system.
    /// </summary>
    /// <param name="provider">The <see cref="T:Microsoft.Extensions.Logging.ILoggerProvider" />.</param>
    public void AddProvider(ILoggerProvider provider)
    {
        throw new NotSupportedException();
    }
}
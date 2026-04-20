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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Bodoconsult.App.Logging;

/// <summary>
/// Extension methods for Log4Net lgging
/// </summary>
public static class Log4NetExtensions
{
    /// <summary>
    /// Add Log4Net logging to logger factory
    /// </summary>
    /// <param name="factory">Current logger factory</param>
    /// <param name="log4NetConfigFile">Config file to use</param>
    /// <returns>Logger factory</returns>
    /// <exception cref="ArgumentNullException">Throws if factory is null</exception>
    public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, string log4NetConfigFile)
    {
        ArgumentNullException.ThrowIfNull(factory);

        using var p = new Log4NetProvider(log4NetConfigFile);
        factory.AddProvider(p);

        return factory;
    }

    /// <summary>
    /// Add Log4Net logging to logger factory
    /// </summary>
    /// <param name="factory">Current logger factory</param>
    /// <returns>Logger factory</returns>
    /// <exception cref="ArgumentNullException">Throws if factory is null</exception>
    public static ILoggerFactory AddLog4Net(this ILoggerFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        var s = Environment.ProcessPath;
        ArgumentNullException.ThrowIfNull(s);

        // ReSharper disable once AssignNullToNotNullAttribute
        s = Path.Combine(new FileInfo(s).DirectoryName, "log4net.config");

        using var p = new Log4NetProvider(s);
        factory.AddProvider(p);

        return factory;
    }


    /// <summary>
    /// Adds a Log4Net logger provider named to the factory
    /// </summary>
    /// <param name="builder">The extension method argument.</param>
    public static ILoggingBuilder AddLog4Net(this ILoggingBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, Log4NetProvider>());
        return builder;
    }
}
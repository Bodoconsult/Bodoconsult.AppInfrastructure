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

using System.Collections.Concurrent;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace Bodoconsult.App.Logging;

/// <summary>
/// Implementation of <see cref="ILoggerProvider"/> for Log4Net
/// </summary>
public class Log4NetProvider : ILoggerProvider
{
    private readonly string _log4NetConfigFile;

    private readonly ConcurrentDictionary<string, Log4NetLogger> _loggers = new();

    /// <summary>
    /// Default ctor
    /// </summary>
    public Log4NetProvider()
    {
        var s = Environment.ProcessPath;
        ArgumentNullException.ThrowIfNull(s); ;
        // ReSharper disable once AssignNullToNotNullAttribute
        s = Path.Combine(new FileInfo(s).DirectoryName, "log4net.config");
        _log4NetConfigFile = s;
    }

    /// <summary>
    /// Ctor with a Log4Net file path to load
    /// </summary>
    /// <param name="log4NetConfigFile">Log4Net file path to</param>
    public Log4NetProvider(string log4NetConfigFile)
    {
        _log4NetConfigFile = log4NetConfigFile;
    }

    /// <summary>
    /// Creates a new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance.
    /// </summary>
    /// <param name="categoryName">The category name for messages produced by the logger.</param>
    /// <returns>The instance of <see cref="T:Microsoft.Extensions.Logging.ILogger" /> that was created.</returns>
    public ILogger CreateLogger(string categoryName)
    {
        var impl = CreateLoggerImplementation(categoryName);

        impl.IsEnabled(LogLevel.Trace);


        return _loggers.GetOrAdd(categoryName, CreateLoggerImplementation);
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    protected virtual void Dispose(bool disposing)
    {

        if (disposing)
        {
            //#pragma warning disable 
            try
            {
                _loggers.Clear();
                    
            }
#pragma warning disable CA1031
            catch //(Exception e)
            {
                // ignored
            }
#pragma warning restore CA1031
        }
    }

    private Log4NetLogger CreateLoggerImplementation(string name)
    {
        var l = new Log4NetLogger(name, Parselog4NetConfigFile(_log4NetConfigFile));
        return l;
    }

    private static XmlElement Parselog4NetConfigFile(string filename)
    {
        var log4NetConfig = new XmlDocument();

        using (var s = File.OpenRead(filename))
        {
            log4NetConfig.Load(s);
        }

        return log4NetConfig["log4net"];
    }
}
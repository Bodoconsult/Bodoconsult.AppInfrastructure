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

// https://stackify.com/log4net-guide-dotnet-logging/

using System.Diagnostics;
using System.Reflection;
using System.Xml;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using log4net.Repository;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Bodoconsult.App.Logging;

/// <summary>
/// <see cref="Microsoft.Extensions.Logging.ILogger"/> implementation for Log4Net
/// </summary>
public class Log4NetLogger : ILogger
{
    ////private readonly string _name;
    ////private readonly XmlElement _xmlElement;
    private ILog _log;

    #region Ctors

    /// <summary>
    /// Default ctor: setup for a Log4Net logger with default name logfile.log for the log file
    /// </summary>
    public Log4NetLogger()
    {
        var assPath = Environment.ProcessPath;
        ArgumentNullException.ThrowIfNull(assPath);

        var dir = new FileInfo(assPath).DirectoryName;
        ArgumentNullException.ThrowIfNull(dir);

        var fileName = Path.Combine(dir, "logfile.log");

        InitLoggerFromCode(fileName);
    }

    /// <summary>
    /// Setup for a Log4Net logger
    /// </summary>
    /// <param name="fileName">Full file path to the log file</param>
    public Log4NetLogger(string fileName)
    {
        InitLoggerFromCode(fileName);
    }

    private void InitLoggerFromCode(string fileName)
    {
        var fi = new FileInfo(fileName);

        var plainFileName = fi.Name.Replace(fi.Extension, string.Empty);

        var layout = new PatternLayout("%message%newline");
        var filter = new LevelMatchFilter { LevelToMatch = Level.All };
        filter.ActivateOptions();

        var appender = new RollingFileAppender
        {
            File = fileName,
            ImmediateFlush = true,
            AppendToFile = true,
            RollingStyle = RollingFileAppender.RollingMode.Size,
            MaxFileSize = 50 * 1024 * 1024,
            MaxSizeRollBackups = 20,
            StaticLogFileName = true,
            LockingModel = new FileAppender.MinimalLock(),
            Name = $"RollingFileAppender_{plainFileName}"
        };
        appender.AddFilter(filter);
        appender.Layout = layout;

        try
        {
            appender.ActivateOptions();
        }
        catch //(Exception e)
        {
            // Do nothing: may throw on Android due not supported mutex
        }
            
        var repositoryName = $"{plainFileName}Repository";
        var loggerName = $"{plainFileName} Logger";
        try
        {
            var repos = LoggerManager.GetAllRepositories();

            if (repos.All(x => x.Name != repositoryName))
            {
                var repository = LoggerManager.CreateRepository(repositoryName);
                BasicConfigurator.Configure(repository, appender);
            }

        }
        catch (Exception e)
        {
            Debug.Print(e.Message);
            //logger.LogError("Error when initializing monitor logger: {0}",e);
        }
        _log = LogManager.GetLogger(repositoryName, loggerName);

    }

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="name"></param>
    /// <param name="configFileName"></param>

    public Log4NetLogger(string name, string configFileName)
    {
        var s = Environment.ProcessPath;
        ArgumentNullException.ThrowIfNull(s);
        
        var dir = new FileInfo(s).DirectoryName;
        ArgumentNullException.ThrowIfNull(dir);

        var filePath = Path.Combine(dir, configFileName);

        var xmlElement = ParseLog4NetConfigFile(filePath);

        InitLoggerFromXml(name, xmlElement);

    }

    /// <summary>
    /// Ctor to provide XML content to configure the Log4Net logger
    /// </summary>
    /// <param name="name"></param>
    /// <param name="xmlElement"></param>
    public Log4NetLogger(string name, XmlElement xmlElement)
    {
        InitLoggerFromXml(name, xmlElement);
        //_log.Fatal("log4net init successful");
    }

    /// <summary>
    /// Ctor to provide XML content to configure the Log4Net logger
    /// </summary>
    /// <param name="name"></param>
    /// <param name="xmlElement"></param>
    /// <param name="repoName">Repository name</param>
    public Log4NetLogger(string name, XmlElement xmlElement, string repoName)
    {
        InitLoggerFromXml(name, xmlElement, repoName);
        //_log.Fatal("log4net init successful");
    }

    private void InitLoggerFromXml(string name, XmlElement xmlElement, string repoName = null)
    {
        ILoggerRepository loggerRepository;

        if (repoName == null)
        {
            loggerRepository = LogManager.CreateRepository(Assembly.GetCallingAssembly(), typeof(Hierarchy));
            XmlConfigurator.Configure(loggerRepository, xmlElement);

            _log = LogManager.GetLogger(loggerRepository.Name, name);
        }
        else
        {
            var repositoryName = repoName;
            var loggerName = repoName;
            try
            {
                var repos = LoggerManager.GetAllRepositories();

                if (repos.All(x => x.Name != repositoryName))
                {
                    loggerRepository = LoggerManager.CreateRepository(repositoryName);
                    XmlConfigurator.Configure(loggerRepository, xmlElement);
                }
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
                //logger.LogError("Error when initializing monitor logger: {0}",e);
            }
            _log = LogManager.GetLogger(repositoryName, loggerName);
        }
    }

    #endregion 


    /// <summary>
    /// Parse a Log4Net config file
    /// </summary>
    /// <param name="filename">Log4Net config filename</param>
    /// <returns>XML element</returns>
    public static XmlElement ParseLog4NetConfigFile(string filename)
    {
        var log4NetConfig = new XmlDocument();

        using (var s = File.OpenRead(filename))
        {
            log4NetConfig.Load(s);
        }

        var node = log4NetConfig["log4net"];

        if (node != null)
        {
            return node;
        }

        node = log4NetConfig["configuration"];

        if (node == null)
        {
            return null;
        }

        node = node["log4net"];
        return node;
    }


    /// <summary>Begins a logical operation scope.</summary>
    /// <param name="state">The identifier for the scope.</param>
    /// <typeparam name="TState">The type of the state to begin scope for.</typeparam>
    /// <returns>An <see cref="T:System.IDisposable" /> that ends the logical operation scope on dispose.</returns>
    public IDisposable BeginScope<TState>(TState state)
    {
        return null;
    }

    /// <summary>
    /// Is logging enabled for a certain log level
    /// </summary>
    /// <param name="logLevel">Log level</param>
    /// <returns>True if logging is enabled for the requested log level else false</returns>
    /// <exception cref="ArgumentOutOfRangeException">Log level not existing</exception>
    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Critical => _log.IsFatalEnabled,
            LogLevel.Debug or LogLevel.Trace => _log.IsDebugEnabled,
            LogLevel.Error => _log.IsErrorEnabled,
            LogLevel.Information => _log.IsInfoEnabled,
            LogLevel.Warning => _log.IsWarnEnabled,
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel))
        };
    }

    /// <summary>Writes a log entry.</summary>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="eventId">Id of the event.</param>
    /// <param name="state">The entry to be written. Can be also an object.</param>
    /// <param name="exception">The exception related to this entry.</param>
    /// <param name="formatter">Function to create a <see cref="T:System.String" /> message of the <paramref name="state" /> and <paramref name="exception" />.</param>
    /// <typeparam name="TState">The type of the object to be written.</typeparam>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
        Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        ArgumentNullException.ThrowIfNull(formatter);

        var message = formatter(state, exception);

        if (string.IsNullOrEmpty(message) && exception == null)
        {
            return;
        }

        switch (logLevel)
        {
            case LogLevel.Critical:
                _log.Fatal(message);
                break;
            case LogLevel.Debug:
            case LogLevel.Trace:
                _log.Debug(message);
                break;
            case LogLevel.Error:
                _log.Error(message);
                break;
            case LogLevel.Information:
                _log.Info(message);
                break;
            case LogLevel.Warning:
                _log.Warn(message);
                break;
            default:
                _log.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
                _log.Info(message, exception);
                break;
        }
    }
}
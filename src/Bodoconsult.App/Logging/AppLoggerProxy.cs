// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Bodoconsult.App.Logging;

/// <summary>
/// Current implementation of <see cref="IAppLoggerProxy"/>.
/// Collect logging messages and write them in a separate job
/// </summary>
public class AppLoggerProxy : IAppLoggerProxy
{
    private readonly ILogDataFactory _logDataFactory;

    /// <summary>
    /// Current logger factory instance
    /// </summary>
    public ILoggerFactory LoggerFactory { get; private set; }

    private IProducerConsumerQueue<LogData> _logMessages;

    private ILogger _logger;

    /// <summary>
    /// Default ctor
    /// </summary>
    public AppLoggerProxy(ILoggerFactory loggerFactory, ILogDataFactory logDataFactory)
    {
        LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        _logger = loggerFactory.CreateLogger(string.Intern("Default"));
        BaseCtor();

        _logDataFactory = logDataFactory;
    }


    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="loggerFactory">Logger factory</param>
    /// <param name="logDataFactory">Logger data factory></param>
    /// <param name="appType">App type string</param>
    /// <exception cref="ArgumentNullException"></exception>
    public AppLoggerProxy(ILoggerFactory loggerFactory, ILogDataFactory logDataFactory, string appType)
    {
        LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        _logger = loggerFactory.CreateLogger(appType);
        BaseCtor();

        _logDataFactory = logDataFactory;
    }


    private void BaseCtor()
    {
        StartLogging();
    }

    /// <summary>
    /// Update logger factory. This method is intended to be used when at app start the logger factory has to changed from the interim one to the one used in the DI container
    /// </summary>
    /// <param name="loggerFactory"></param>
    public void UpdateILoggerFactory(ILoggerFactory loggerFactory)
    {
        StopLogging();
        LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        _logger = LoggerFactory.CreateLogger(string.Intern("Default"));
        StartLogging();
    }

    /// <summary>
    /// Write a log entry to the logger. Not intended for direct usage. Public only for unit testing!
    /// </summary>
    /// <param name="logData">Current data to log</param>
    public void WriteLogEntry(LogData logData)
    {
        try
        {
            var fileName = FileSystemHelper.GetFileNameWithoutExtension(logData.SourceFile);

            var sb = new StringBuilder();
            sb.Append(string.Empty);
            FormatArgs(logData.Args, sb);

            if (logData.Exception == null)
            {
                _logger.Log(logData.LogLevel,
                    logData.EventId,
                    null,
                    "{LogDate:yyyy.MM.dd HH:mm:ss.fffffff} - Thread {ThreadId} - {LogLevel} - {fileName}.{SourceMethod}.R{logData.SourceRowNumber} - {Message}{sb}",
                    logData.LogDate, logData.ThreadId, logData.LogLevel, fileName, logData.SourceMethod, logData.SourceRowNumber, logData.Message, sb.ToString().TrimEnd());
            }
            else
            {
                _logger.Log(logData.LogLevel,
                    logData.EventId,
                    null,
                    "{logData.LogDate:yyyy.MM.dd HH:mm:ss.fffffff} - Thread {logData.ThreadId} - {logData.LogLevel} - {fileName}.{logData.SourceMethod}.R{logData.SourceRowNumber} - {logData.Message}: {logData.Exception}{sb}",
                    logData.LogDate, logData.ThreadId, logData.LogLevel, fileName, logData.SourceMethod, logData.SourceRowNumber, logData.Message, logData.Exception, sb.ToString().TrimEnd());
            }

                
            //: $"{logData.LogDate:yyyy.MM.dd HH:mm:ss.fffffff} - {logData.LogLevel} - {fileName}.{logData.SourceMethod}.R{logData.SourceRowNumber} - {logData.Message}: {logData.Exception.Message}{(string.IsNullOrEmpty(logData.Exception.StackTrace) ? "" : $"\r\n{logData.Exception.StackTrace}")}\r\n{FormatArgs(logData.Args)}");
        }
        catch
        {
            Debug.Print("Logger error");
        }

        _logDataFactory.EnqueueInstance(logData);
    }

    /// <summary>Formats and writes a debug log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogDebug(0, exception, "Error while processing request from {Address}", address)</example>
    public void LogDebug(
        EventId eventId,
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {

        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Debug;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;


        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes a debug log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogDebug(0, exception, "Error while processing request from {Address}", address)</example>
    public void LogDebug(
        EventId eventId,
        Exception exception,
        string message,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Debug;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);

    }



    /// <summary>Formats and writes a debug log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogDebug(0, "Processing request from {Address}", address)</example>
    public void LogDebug(
        EventId eventId,
        string message,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = LogLevel.Debug;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }


    /// <summary>Formats and writes a debug log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogDebug(0, "Processing request from {Address}", address)</example>
    public void LogDebug(
        EventId eventId,
        string message,
        object[] args = null,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = LogLevel.Debug;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes a debug log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogDebug(exception, "Error while processing request from {Address}", address)</example>
    public void LogDebug(
        Exception exception,
        string message,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Debug;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes a debug log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogDebug(exception, "Error while processing request from {Address}", address)</example>
    public void LogDebug(
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Debug;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes a debug log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogDebug("Processing request from {Address}", address)</example>
    public void LogDebug(string message,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0
    )
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = LogLevel.Debug;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes a debug log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogDebug("Processing request from {Address}", address)</example>
    public void LogDebug(string message,
        object[] args,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0
    )
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = LogLevel.Debug;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        _logMessages?.Enqueue(log);
    }


    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogTrace(0, exception, "Error while processing request from {Address}", address)</example>
    public void LogTrace(
        EventId eventId,
        Exception exception,
        string message,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Trace;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogTrace(0, exception, "Error while processing request from {Address}", address)</example>
    public void LogTrace(
        EventId eventId,
        Exception exception,
        string message,
        object[] args = null,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Trace;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogTrace(0, "Processing request from {Address}", address)</example>
    public void LogTrace(
        EventId eventId,
        string message,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = LogLevel.Trace;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogTrace(0, "Processing request from {Address}", address)</example>
    public void LogTrace(
        EventId eventId,
        string message,
        object[] args,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = LogLevel.Trace;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogTrace(exception, "Error while processing request from {Address}", address)</example>
    public void LogTrace(
        Exception exception,
        string message,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Trace;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogTrace(exception, "Error while processing request from {Address}", address)</example>
    public void LogTrace(
        Exception exception,
        string message,
        object[] args = null,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Trace;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogTrace("Processing request from {Address}", address)</example>
    public void LogTrace(string message,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = LogLevel.Trace;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogTrace("Processing request from {Address}", address)</example>
    public void LogTrace(string message,
        object[] args,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = LogLevel.Trace;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogInformation(0, exception, "Error while processing request from {Address}", address)</example>
    public void LogInformation(
        EventId eventId,
        Exception exception,
        string message,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Information;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogInformation(0, exception, "Error while processing request from {Address}", address)</example>
    public void LogInformation(
        EventId eventId,
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Information;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogInformation(0, "Processing request from {Address}", address)</example>
    public void LogInformation(
        EventId eventId,
        string message,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = LogLevel.Information;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogInformation(0, "Processing request from {Address}", address)</example>
    public void LogInformation(
        EventId eventId,
        string message,
        object[] args,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = LogLevel.Information;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogInformation(exception, "Error while processing request from {Address}", address)</example>
    public void LogInformation(
        Exception exception,
        string message,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Information;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogInformation(exception, "Error while processing request from {Address}", address)</example>
    public void LogInformation(
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Information;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogInformation("Processing request from {Address}", address)</example>
    public void LogInformation(string message,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = LogLevel.Information;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }

    /// <summary>
    /// Create a message with a special timestamp
    /// </summary>
    /// <param name="message">Message to log</param>
    /// <param name="timeStamp">Timestamp</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    public void LogInformation(string message,
        DateTime timeStamp,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogDate = timeStamp;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogInformation("Processing request from {Address}", address)</example>
    public void LogInformation(string message,
        object[] args,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = LogLevel.Information;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogWarning(0, exception, "Error while processing request from {Address}", address)</example>
    public void LogWarning(
        EventId eventId,
        Exception exception,
        string message,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Warning;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogWarning(0, exception, "Error while processing request from {Address}", address)</example>
    public void LogWarning(
        EventId eventId,
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Warning;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogWarning(0, "Processing request from {Address}", address)</example>
    public void LogWarning(
        EventId eventId,
        string message,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = LogLevel.Warning;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogWarning(0, "Processing request from {Address}", address)</example>
    public void LogWarning(
        EventId eventId,
        string message,
        object[] args,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = LogLevel.Warning;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogWarning(exception, "Error while processing request from {Address}", address)</example>
    public void LogWarning(
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Warning;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogWarning(exception, "Error while processing request from {Address}", address)</example>
    public void LogWarning(
        Exception exception,
        string message,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Warning;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogWarning("Processing request from {Address}", address)</example>
    public void LogWarning(string message,
        object[] args,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = LogLevel.Warning;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        _logMessages?.Enqueue(log);
    }


    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogWarning("Processing request from {Address}", address)</example>
    public void LogWarning(string message,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = LogLevel.Warning;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogError("Error while processing request from 123", exception)</example>
    public void LogError(string message, Exception exception, string memberName = null, string filepath = null,
        int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.LogLevel = LogLevel.Error;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogError(0, exception, "Error while processing request from {Address}", address)</example>
    public void LogError(
        EventId eventId,
        Exception exception,
        string message,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Error;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogError(0, exception, "Error while processing request from {Address}", address)</example>
    public void LogError(
        EventId eventId,
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Error;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogError(0, "Processing request from {Address}", address)</example>
    public void LogError(
        EventId eventId,
        string message,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = LogLevel.Error;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogError(0, "Processing request from {Address}", address)</example>
    public void LogError(
        EventId eventId,
        string message,
        object[] args = null,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = LogLevel.Error;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogError(exception, "Error while processing request from {Address}", address)</example>
    public void LogError(
        Exception exception,
        string message,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Error;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogError(exception, "Error while processing request from {Address}", address)</example>
    public void LogError(
        Exception exception,
        string message,
        object[] args = null,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Error;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogError("Processing request from {Address}", address)</example>
    public void LogError(string message,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = LogLevel.Error;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogError("Processing request from {Address}", address)</example>
    public void LogError(string message,
        object[] args,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = LogLevel.Error;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogCritical(0, exception, "Error while processing request from {Address}", address)</example>
    public void LogCritical(
        EventId eventId,
        Exception exception,
        string message,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Critical;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }


    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogCritical(0, exception, "Error while processing request from {Address}", address)</example>
    public void LogCritical(
        EventId eventId,
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Critical;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogCritical(0, "Processing request from {Address}", address)</example>
    public void LogCritical(
        EventId eventId,
        string message,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = LogLevel.Critical;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogCritical(0, "Processing request from {Address}", address)</example>
    public void LogCritical(
        EventId eventId,
        string message,
        object[] args,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = LogLevel.Critical;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogCritical(exception, "Error while processing request from {Address}", address)</example>
    public void LogCritical(
        Exception exception,
        string message,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Critical;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogCritical(exception, "Error while processing request from {Address}", address)</example>
    public void LogCritical(
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = LogLevel.Critical;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogCritical("Processing request from {Address}", address)</example>
    public void LogCritical(string message,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = LogLevel.Critical;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }

    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogCritical("Processing request from {Address}", address)</example>
    public void LogCritical(string message,
        object[] args,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = LogLevel.Critical;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        _logMessages?.Enqueue(log);
    }

    /// <summary>
    /// Formats and writes a log message at the specified log level.
    /// </summary>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="message">Format string of the log message.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    public void Log(
        LogLevel logLevel,
        string message,
        object[] args = null,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = logLevel;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        _logMessages?.Enqueue(log);
    }

    /// <summary>
    /// Formats and writes a log message at the specified log level.
    /// </summary>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="message">Format string of the log message.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    public void Log(
        LogLevel logLevel,
        string message,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Message = message;
        log.LogLevel = logLevel;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }

    /// <summary>
    /// Formats and writes a log message at the specified log level.
    /// </summary>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    public void Log(
        LogLevel logLevel,
        EventId eventId,
        string message,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = logLevel;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }

    /// <summary>
    /// Formats and writes a log message at the specified log level.
    /// </summary>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    public void Log(
        LogLevel logLevel,
        EventId eventId,
        string message,
        object[] args = null,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.EventId = eventId;
        log.Message = message;
        log.LogLevel = logLevel;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        _logMessages?.Enqueue(log);
    }


    /// <summary>
    /// Formats and writes a log message at the specified log level.
    /// </summary>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>

    public void Log(
        LogLevel logLevel,
        Exception exception,
        string message,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = logLevel;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;

        _logMessages?.Enqueue(log);
    }

    /// <summary>
    /// Formats and writes a log message at the specified log level.
    /// </summary>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>

    public void Log(
        LogLevel logLevel,
        Exception exception,
        string message,
        object[] args = null,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string filepath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        var log = _logDataFactory.DequeueInstance();
        log.Exception = exception;
        log.Message = message;
        log.LogLevel = logLevel;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = args;

        _logMessages?.Enqueue(log);
    }


    /// <summary>
    /// Start the logging
    /// </summary>
    public void StartLogging()
    {
        if (_logMessages != null)
        {
            StopLogging();
        }

        _logMessages = new ProducerConsumerQueue<LogData>
        {
            ConsumerTaskDelegate = WriteLogEntry
        };
        _logMessages.StartConsumer();
    }

    /// <summary>
    /// Stop the logging
    /// </summary>
    public void StopLogging()
    {
        if (_logMessages == null)
        {
            return;
        }
        _logMessages.StopConsumer();
        _logMessages = null;
    }


    /// <summary>
    /// Get Current stack as customized string
    /// @see LogStackTrace
    /// </summary>
    private static string CurrentStackCustomizedLog()
    {
        var lStackLog = new StringBuilder();
        var lCurrentStack = new StackTrace(true);
        // the true value is used to include source file info

        for (var x = 0; x < lCurrentStack.FrameCount; ++x)
        {
            var lMethodCall = lCurrentStack.GetFrame(x);
            if (IsMethodToBeIncluded(lMethodCall))
                lStackLog.AppendLine(MethodCallLog(lMethodCall));
        }
        return lStackLog.ToString();
    }

    /// <summary>
    /// This method is used to keep Logger methods out of the returned log
    /// (the methods actually included in a StackTrace
    /// depend on compiler optimizations).
    /// @see LogStackTrace
    /// </summary>
    private static bool IsMethodToBeIncluded(StackFrame pStackMethod)
    {
        var lMethod = pStackMethod.GetMethod();
        return lMethod.DeclaringType != typeof(AppLoggerProxy);
    }


    /// <summary>
    /// Instead of visiting each field of stackFrame,
    /// the StackFrame.ToString() method could be used, 
    /// but the returned text would not include the class name.
    /// @see LogStackTrace
    /// </summary>
    private static string MethodCallLog(StackFrame pMethodCall)
    {
        var lMethodCallLog = new StringBuilder();

        var lMethod = pMethodCall.GetMethod();
        lMethodCallLog.Append(lMethod.DeclaringType);
        lMethodCallLog.Append(".");
        lMethodCallLog.Append(pMethodCall.GetMethod().Name);

        var lMethodParameters = lMethod.GetParameters();
        lMethodCallLog.Append("(");
        for (var x = 0; x < lMethodParameters.Length; ++x)
        {
            if (x > 0)
            {
                lMethodCallLog.Append(", ");
            }

            var lMethodParameter = lMethodParameters[x];
            lMethodCallLog.Append(lMethodParameter.ParameterType.Name);
            lMethodCallLog.Append(" ");
            lMethodCallLog.Append(lMethodParameter.Name);
        }
        lMethodCallLog.Append(")");

        var lSourceFileName = pMethodCall.GetFileName();
        if (string.IsNullOrEmpty(lSourceFileName))
        {
            return lMethodCallLog.ToString();
        }

        lMethodCallLog.Append(" in ");
        lMethodCallLog.Append(lSourceFileName);
        lMethodCallLog.Append(": line ");
        lMethodCallLog.Append(pMethodCall.GetFileLineNumber());

        return lMethodCallLog.ToString();
    }

    /// <summary>
    /// Log message and StackTrace with the current log level
    /// </summary>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="message">Format string of the log message.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    public void LogStackTrace(LogLevel logLevel, string message, string memberName = null, string filepath = null, int lineNumber = 0)
    {
        var lLogMessage = new StringBuilder();

        // format pretty message
        if (!string.IsNullOrEmpty(message))
        {
            lLogMessage.Append(message);
            lLogMessage.AppendLine();
        }
        lLogMessage.Append("======================================================================");
        lLogMessage.AppendLine();
        lLogMessage.Append("CallStack:");
        lLogMessage.AppendLine();
        lLogMessage.Append(CurrentStackCustomizedLog());
        lLogMessage.Append("======================================================================");


        var log = _logDataFactory.DequeueInstance();
        log.Exception = null;
        log.Message = lLogMessage.ToString();
        log.LogLevel = logLevel;
        log.SourceFile = filepath;
        log.SourceMethod = memberName;
        log.SourceRowNumber = lineNumber;
        log.Args = [];

        _logMessages?.Enqueue(log);
    }

    ///// <summary>
    ///// Formats and writes a log message at the specified log level.
    ///// </summary>
    ///// <param name="logLevel">Entry will be written on this level.</param>
    ///// <param name="eventId">The event id associated with the log.</param>
    ///// <param name="exception">The exception to log.</param>
    ///// <param name="message">Format string of the log message.</param>
    ///// <param name="args">An object array that contains zero or more objects to format.</param>
    //public  void Log(
    //  LogLevel logLevel,
    //  EventId eventId,
    //  Exception exception,
    //  string message,
    //  params object[] args)
    //{
    //    //if (logger == null)
    //    //    throw new ArgumentNullException(nameof(logger));
    //    //logger.Log<FormattedLogValues>(logLevel, eventId, new FormattedLogValues(message, args), exception, LoggerExtensions._messageFormatter);
    //}


    #region Helper methods

    /// <summary>
    /// Format args as JSON string
    /// </summary>
    /// <param name="args">Args delivered by the method caller</param>
    /// <param name="s">String to add the args as text</param>
    /// <returns>JSON formatted string</returns>
    public static string FormatArgs(object[] args, StringBuilder s)
    {

        if (args == null || args.Length == 0)
        {
            return string.Empty;
        }

        s.AppendLine(string.Empty);
        foreach (var arg in args)
        {
            if (arg is Exception e)
            {
                s.AppendLine($"{e.GetType().Name}: {e.Message} {e.StackTrace}: " //+ Globals.JsonSerializeNice(e)
                             );
            }
            else if (arg != null)
            {
                ObjectHelper.GetObjectPropertiesAsString(arg, s);
            }
        }

        return s.Replace(string.Intern("00000000-0000-0000-0000-000000000000"), string.Intern("Empty")).ToString();

    }

    #endregion
    
    /// <summary>
    /// Dispose the logger
    /// </summary>
    /// <param name="disposing">Is disposing requested</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }
        StopLogging();
        LoggerFactory?.Dispose();
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    //~AppLoggerProxy()
    //{
    //    Dispose(false);
    //}
}
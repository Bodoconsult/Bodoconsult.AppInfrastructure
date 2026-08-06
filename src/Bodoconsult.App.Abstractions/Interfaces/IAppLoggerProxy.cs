// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for logging services
/// </summary>
public interface IAppLoggerProxy : IDisposable
{
    /// <summary>
    /// Current logger factory instance
    /// </summary>
    ILoggerFactory LoggerFactory { get; }

    /// <summary>
    /// Update logger factory. This method is intended to be used when at app start the logger factory has to changed from the interim one to the one used in the DI container
    /// </summary>
    /// <param name="loggerFactory"></param>
    void UpdateILoggerFactory(ILoggerFactory loggerFactory);

    /// <summary>Formats and writes a debug log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogDebug(0, exception, "Error while processing request from {Address}", address)</example>
    void LogDebug(
        EventId eventId,
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a debug log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogDebug(0, exception, "Error while processing request from {Address}", address)</example>
    void LogDebug(
        EventId eventId,
        Exception exception,
        string message,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a debug log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogDebug(0, "Processing request from {Address}", address)</example>
    void LogDebug(
        EventId eventId,
        string message,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);


    /// <summary>Formats and writes a debug log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogDebug(0, "Processing request from {Address}", address)</example>
    void LogDebug(
        EventId eventId,
        string message,
        object[] args,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a debug log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogDebug(exception, "Error while processing request from {Address}", address)</example>
    void LogDebug(
        Exception exception,
        string message,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a debug log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogDebug(exception, "Error while processing request from {Address}", address)</example>
    void LogDebug(
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a debug log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogDebug("Processing request from {Address}", address)</example>
    void LogDebug(string message,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);


    /// <summary>Formats and writes a debug log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogDebug("Processing request from {Address}", address)</example>
    void LogDebug(string message,
        object[] args,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogTrace(0, exception, "Error while processing request from {Address}", address)</example>
    void LogTrace(
        EventId eventId,
        Exception exception,
        string message,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogTrace(0, exception, "Error while processing request from {Address}", address)</example>
    void LogTrace(
        EventId eventId,
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);


    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogTrace(0, "Processing request from {Address}", address)</example>
    void LogTrace(
        EventId eventId,
        string message,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogTrace(0, "Processing request from {Address}", address)</example>
    void LogTrace(
        EventId eventId,
        string message,
        object[] args,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogTrace(exception, "Error while processing request from {Address}", address)</example>
    void LogTrace(
        Exception exception,
        string message,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogTrace(exception, "Error while processing request from {Address}", address)</example>
    void LogTrace(
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogTrace("Processing request from {Address}", address)</example>
    void LogTrace(string message,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a trace log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogTrace("Processing request from {Address}", address)</example>
    void LogTrace(string message,
        object[] args,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogInformation(0, exception, "Error while processing request from {Address}", address)</example>
    void LogInformation(
        EventId eventId,
        Exception exception,
        string message,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogInformation(0, exception, "Error while processing request from {Address}", address)</example>
    void LogInformation(
        EventId eventId,
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogInformation(0, "Processing request from {Address}", address)</example>
    void LogInformation(
        EventId eventId,
        string message,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogInformation(0, "Processing request from {Address}", address)</example>
    void LogInformation(
        EventId eventId,
        string message,
        object[] args,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogInformation(exception, "Error while processing request from {Address}", address)</example>
    void LogInformation(
        Exception exception,
        string message,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogInformation(exception, "Error while processing request from {Address}", address)</example>
    void LogInformation(
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);


    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogInformation("Processing request from {Address}", address)</example>
    void LogInformation(string message,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>
    /// Create a message with a special timestamp
    /// </summary>
    /// <param name="message">Message to log</param>
    /// <param name="timeStamp">Timestamp</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    void LogInformation(string message,
        DateTime timeStamp,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes an informational log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogInformation("Processing request from {Address}", address)</example>
    void LogInformation(string message,
        object[] args,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogWarning(0, exception, "Error while processing request from {Address}", address)</example>
    void LogWarning(
        EventId eventId,
        Exception exception,
        string message,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogWarning(0, exception, "Error while processing request from {Address}", address)</example>
    void LogWarning(
        EventId eventId,
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogWarning(0, "Processing request from {Address}", address)</example>
    void LogWarning(
        EventId eventId,
        string message,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogWarning(0, "Processing request from {Address}", address)</example>
    void LogWarning(
        EventId eventId,
        string message,
        object[] args,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogWarning(exception, "Error while processing request from {Address}", address)</example>
    void LogWarning(
        Exception exception,
        string message,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogWarning(exception, "Error while processing request from {Address}", address)</example>
    void LogWarning(
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogWarning("Processing request from {Address}", address)</example>
    void LogWarning(string message,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a warning log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogWarning("Processing request from {Address}", address)</example>
    void LogWarning(string message,
        object[] args,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);


    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogError("Error while processing request from 123", exception)</example>
    void LogError(
        string message,
        Exception exception,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);


    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogError(0, exception, "Error while processing request from {Address}", address)</example>
    void LogError(
        EventId eventId,
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogError(0, "Processing request from {Address}", address)</example>
    void LogError(
        EventId eventId,
        string message,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogError(0, "Processing request from {Address}", address)</example>
    void LogError(
        EventId eventId,
        string message,
        object[] args,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogError(exception, "Error while processing request from {Address}", address)</example>
    void LogError(
        Exception exception,
        string message,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogError(exception, "Error while processing request from {Address}", address)</example>
    void LogError(
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogError("Processing request from {Address}", address)</example>
    void LogError(string message,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes an error log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogError("Processing request from {Address}", address)</example>
    void LogError(string message,
        object[] args,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogCritical(0, exception, "Error while processing request from {Address}", address)</example>
    void LogCritical(
        EventId eventId,
        Exception exception,
        string message,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogCritical(0, exception, "Error while processing request from {Address}", address)</example>
    void LogCritical(
        EventId eventId,
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogCritical(0, "Processing request from {Address}", address)</example>
    void LogCritical(
        EventId eventId,
        string message,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogCritical(0, "Processing request from {Address}", address)</example>
    void LogCritical(
        EventId eventId,
        string message,
        object[] args,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogCritical(exception, "Error while processing request from {Address}", address)</example>
    void LogCritical(
        Exception exception,
        string message,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogCritical(exception, "Error while processing request from {Address}", address)</example>
    void LogCritical(
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogCritical("Processing request from {Address}", address)</example>
    void LogCritical(string message,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>Formats and writes a critical log message.</summary>
    /// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <example>logger.LogCritical("Processing request from {Address}", address)</example>
    void LogCritical(string message,
        object[] args,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>
    /// Formats and writes a log message at the specified log level.
    /// </summary>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="message">Format string of the log message.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    void Log(
        LogLevel logLevel,
        string message,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>
    /// Formats and writes a log message at the specified log level.
    /// </summary>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="message">Format string of the log message.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    void Log(
        LogLevel logLevel,
        string message,
        object[] args,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>
    /// Formats and writes a log message at the specified log level.
    /// </summary>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    void Log(
        LogLevel logLevel,
        EventId eventId,
        string message,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

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
    void Log(
        LogLevel logLevel,
        EventId eventId,
        string message,
        object[] args,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>
    /// Formats and writes a log message at the specified log level.
    /// </summary>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    void Log(
        LogLevel logLevel,
        Exception exception,
        string message,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);

    /// <summary>
    /// Formats and writes a log message at the specified log level.
    /// </summary>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message.</param>
    /// <param name="memberName">Calling method name (filled automatically by compiler)</param>
    /// <param name="filepath">Calling file name (filled automatically by compiler)</param>
    /// <param name="lineNumber">Calling method line number (filled automatically by compiler)</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    void Log(
        LogLevel logLevel,
        Exception exception,
        string message,
        object[] args,
        [CallerMemberName] string? memberName = "",
        [CallerFilePath] string? filepath = "",
        [CallerLineNumber] int lineNumber = 0);


    /// <summary>
    /// Start the logging
    /// </summary>
    void StartLogging();

    /// <summary>
    /// Stop the logging
    /// </summary>
    void StopLogging();
}
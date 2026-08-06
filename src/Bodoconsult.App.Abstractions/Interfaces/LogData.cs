// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Microsoft.Extensions.Logging;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Log data entity class
/// </summary>
public sealed class LogData : IResetable
{
    /// <summary>
    /// The timestamp the log entry was originally created
    /// </summary>
    public DateTime LogDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Actual log level
    /// </summary>
    public LogLevel LogLevel { get; set; } = LogLevel.Information;

    /// <summary>
    /// The thread ID creating this instance. Default: Environment.CurrentManagedThreadId
    /// </summary>
    public int ThreadId { get; set; } = Environment.CurrentManagedThreadId;

    /// <summary>
    /// Source file
    /// </summary>
    public string? SourceFile { get; set; } = string.Empty;

    /// <summary>
    /// Source method
    /// </summary>
    public string? SourceMethod { get; set; } = string.Empty;

    /// <summary>
    /// Source row number
    /// </summary>
    public int SourceRowNumber { get; set; }

    /// <summary>
    /// Message to log
    /// </summary>
    public string? Message { get; set; } = string.Empty;

    /// <summary>
    /// Exception to log
    /// </summary>
    public Exception? Exception { get; set; }

    /// <summary>
    /// EventId to log
    /// </summary>
    public EventId EventId { get; set; }

    /// <summary>
    /// Args delivered from the caller
    /// </summary>
    public object[]? Args { get; set; }

    /// <summary>
    /// Reset the class to default values
    /// </summary>
    public void Reset()
    {
        LogDate = DateTime.Now;
        LogLevel = LogLevel.Information;
        SourceFile = string.Empty;
        SourceMethod = string.Empty;
        Exception = null;
        Args = null;
        Exception = null;
        Message = string.Empty;
        SourceMethod = string.Empty;
    }
}
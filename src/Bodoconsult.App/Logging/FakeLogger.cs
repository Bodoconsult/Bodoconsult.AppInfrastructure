// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Bodoconsult.App.Logging;

/// <summary>
/// Delegate for faking the log event
/// </summary>
/// <param name="message">Message to log</param>
public delegate void FakeLogDelegate(string message);


/// <summary>
/// Fake implementation for <see cref="ILogger"/>
/// </summary>
public class FakeLogger : ILogger, IDisposable
{
        
    private readonly string _categoryName;

    /// <summary>
    /// Minimum log level for the fake logger
    /// </summary>
    public IList<LogLevel> AllowedLogLevel { get; } = new List<LogLevel>
    {
        LogLevel.Critical, 
        LogLevel.Error, 
        LogLevel.Warning,
        //LogLevel.Information
    };
        
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="categoryName">Category name to be written in log output</param>
    public FakeLogger(string categoryName)
    {
        _categoryName = categoryName;
    }

    /// <summary>
    /// Delegate for faking the log event
    /// </summary>
    public FakeLogDelegate FakeLogDelegate { get; set; } = DummyFakeLogDelegate;

    private static void DummyFakeLogDelegate(string message)
    {

    }

    /// <summary>Writes a log entry.</summary>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="eventId">Id of the event.</param>
    /// <param name="state">The entry to be written. Can be also an object.</param>
    /// <param name="exception">The exception related to this entry.</param>
    /// <param name="formatter">Function to create a <see cref="System.String" /> message of the <paramref name="state" /> and <paramref name="exception" />.</param>
    /// <typeparam name="TState">The type of the object to be written.</typeparam>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var msg = $"{_categoryName}´: {logLevel}: {state} {exception?.Message}";
        Debug.Print(msg);
        FakeLogDelegate.Invoke(msg);
    }

    /// <summary>
    /// Checks if the given <paramref name="logLevel" /> is enabled.
    /// </summary>
    /// <param name="logLevel">level to be checked.</param>
    /// <returns><c>true</c> if enabled.</returns>
    public bool IsEnabled(LogLevel logLevel)
    {
        return AllowedLogLevel.Contains(logLevel);
    }

    /// <summary>Begins a logical operation scope.</summary>
    /// <param name="state">The identifier for the scope.</param>
    /// <typeparam name="TState">The type of the state to begin scope for.</typeparam>
    /// <returns>An <see cref="System.IDisposable" /> that ends the logical operation scope on dispose.</returns>
    public IDisposable BeginScope<TState>(TState state) where TState : notnull
    {
        return this;
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Dispose the logger
    /// </summary>
    /// <param name="disposing">Is disposing requested</param>
    protected virtual void Dispose(bool disposing)
    {
        // Do nothing
    }
}
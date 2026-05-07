// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Bodoconsult.App.Logging;

/// <summary>
/// Fake logger factory
/// </summary>
public class FakeLoggerFactory : ILoggerFactory
{

    private ILogger _logger;

    /// <summary>
    /// Default ctor
    /// </summary>
    public FakeLoggerFactory()
    {
        FakeLogDelegate = LogMessage;
    }



    /// <summary>
    /// List for all logged messages
    /// </summary>
    public IList<string> LoggedMessages { get; } = new List<string>();


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

    /// <summary>
    /// Creates a new <see cref="ILogger" /> instance.
    /// </summary>
    /// <param name="categoryName">The category name for messages produced by the logger.</param>
    /// <returns>The <see cref="ILogger" />.</returns>
    public ILogger CreateLogger(string categoryName)
    {

        if (_logger != null)
        {
            return _logger;
        }

        _logger = new FakeLogger(categoryName)
        {
            FakeLogDelegate = FakeLogDelegate
        };

        return _logger;
    }

    /// <summary>
    /// Adds an <see cref="ILoggerProvider" /> to the logging system.
    /// </summary>
    /// <param name="provider">The <see cref="ILoggerProvider" />.</param>
    public void AddProvider(ILoggerProvider provider)
    {
        //Do nothing
    }

    /// <summary>
    /// Delegate for faking the log event
    /// </summary>
    public FakeLogDelegate FakeLogDelegate { get; set; }



    /// <summary>
    /// Fakes the writing of an log entry
    /// </summary>
    /// <param name="message">Message to log</param>
    private void LogMessage(string message)
    {
        //Trace.TraceInformation(message);
        LoggedMessages.Add(message);
    }
}
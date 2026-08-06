// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Logging;
using Microsoft.Extensions.Logging;

namespace Bodoconsult.App.Benchmarking;

/// <summary>
/// Server logger factory for benchmark logging. Implementation by Freddy Darsonville
/// </summary>
public class BenchLoggerFactory : IMonitorLoggerFactory
{

    private ILogger? _logger;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="fileName">Current full file path to log in</param>
    public BenchLoggerFactory(string fileName)
    {
        FileName = fileName;
    }


    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        _logger = null;
    }

    /// <summary>
    /// Creates a new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance.
    /// </summary>
    /// <param name="categoryName">The category name for messages produced by the logger.</param>
    /// <returns>The <see cref="T:Microsoft.Extensions.Logging.ILogger" />.</returns>
    public ILogger CreateLogger(string categoryName)
    {
        // Use caching
        if (_logger != null)
        {
            return _logger;
        }
          
        _logger = new Log4NetLogger(FileName);
        return _logger;
    }

    /// <summary>
    /// Adds an <see cref="T:Microsoft.Extensions.Logging.ILoggerProvider" /> to the logging system.
    /// </summary>
    /// <param name="provider">The <see cref="T:Microsoft.Extensions.Logging.ILoggerProvider" />.</param>
    public void AddProvider(ILoggerProvider provider)
    {
        // Do nothing
    }

    /// <summary>
    /// Current logging config: NOT used for the current implementation
    /// </summary>
    public LoggingConfig? LoggingConfig { get; set; }

    /// <summary>
    /// Full file path of the log file
    /// </summary>
    public string FileName { get; }
}
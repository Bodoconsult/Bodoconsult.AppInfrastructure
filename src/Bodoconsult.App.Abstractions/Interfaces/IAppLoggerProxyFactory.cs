// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Microsoft.Extensions.Logging;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for creating an instance of a <see cref="IAppLoggerProxy"/> implementation
/// </summary>
public interface IAppLoggerProxyFactory
{
    /// <summary>
    /// Create an instance of a <see cref="IAppLoggerProxy"/> implementation
    /// </summary>
    /// <param name="loggerFactory">Current logger factory to use</param>
    /// <param name="logDataFactory">Current logdata factory. Should be singleton instance for loggers</param>
    /// <returns>An instance of a <see cref="IAppLoggerProxy"/></returns>
    public IAppLoggerProxy CreateInstance(ILoggerFactory loggerFactory, ILogDataFactory logDataFactory);
}
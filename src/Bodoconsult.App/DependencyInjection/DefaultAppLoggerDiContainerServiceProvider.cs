// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

using Bodoconsult.App.Abstractions.DependencyInjection;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Logging;
using Microsoft.Extensions.Logging;

namespace Bodoconsult.App.DependencyInjection;

/// <summary>
/// Provider adding default app logging services configured by appsettings.json by default
/// </summary>
public class DefaultAppLoggerDiContainerServiceProvider: IDefaultAppLoggerDiContainerServiceProvider
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public DefaultAppLoggerDiContainerServiceProvider(LoggingConfig loggingConfig, IAppLoggerProxy logger)
    {
        LoggingConfig = loggingConfig;
        Logger = logger;
    }

    /// <summary>
    /// Current logging config
    /// </summary>
    public LoggingConfig LoggingConfig { get;  }

    /// <summary>
    /// Current app logger
    /// </summary>
    public IAppLoggerProxy Logger { get; }

    /// <summary>
    /// Add DI container services to a DI container
    /// </summary>
    /// <param name="diContainer">Current DI container</param>
    public void AddServices(DiContainer diContainer)
    {
        diContainer.ServiceCollection.AddDefaultLogger(LoggingConfig);
        diContainer.AddSingletonInstance(Logger);
    }

    /// <summary>
    /// Late bind DI container references to avoid circular DI references
    /// </summary>
    /// <param name="diContainer">Current DI container</param>
    public void LateBindObjects(DiContainer diContainer)
    {
        var loggerFactory = diContainer.Get<ILoggerFactory>();
        var logger = diContainer.Get<IAppLoggerProxy>();

        logger.UpdateILoggerFactory(loggerFactory);
    }
}
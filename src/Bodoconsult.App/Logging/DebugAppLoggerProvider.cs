// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bodoconsult.App.Logging;

/// <summary>
/// Debug window implementation for creating default logger
/// </summary>
public class DebugAppLoggerProvider : IDefaultAppLoggerProvider
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public DebugAppLoggerProvider()
    {
        LoggingConfig = new LoggingConfig
        {
            MinimumLogLevel = LogLevel.Debug,
            LogDataFactory = new LogDataFactory()
        };

        LoggingConfig.Filters.Add("Default", LogLevel.Debug);
        LoggingConfig.Filters.Add("Microsoft", LogLevel.Warning);
        LoggingConfig.Filters.Add("Microsoft.EntityFrameworkCore", LogLevel.Warning);
    }

    /// <summary>
    /// Current app configuration provider
    /// </summary>
    public IAppConfigurationProvider? AppConfigurationProvider => null;

    /// <summary>
    /// Current logging config
    /// </summary>
    public LoggingConfig LoggingConfig { get; }

    /// <summary>
    /// The app default logger instance create by the provider
    /// </summary>
    public IAppLoggerProxy? DefaultLogger { get; private set; }

    /// <summary>
    /// Load the logging settings from <see cref="IAppConfigurationProvider.Configuration"/>
    /// </summary>
    public void LoadLoggingConfigFromConfiguration()
    {
        // Do nothing
    }

    /// <summary>
    /// Load <see cref="IDefaultAppLoggerProvider.DefaultLogger"/> from <see cref="IDefaultAppLoggerProvider.LoggingConfig"/>
    /// </summary>
    public void LoadDefaultLogger()
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddDefaultLogger(LoggingConfig);

        var logFactory = serviceCollection.BuildServiceProvider()
            .GetService<ILoggerFactory>();

        ArgumentNullException.ThrowIfNull(logFactory);

        DefaultLogger = new AppLoggerProxy(logFactory, LoggingConfig.LogDataFactory);
    }
}
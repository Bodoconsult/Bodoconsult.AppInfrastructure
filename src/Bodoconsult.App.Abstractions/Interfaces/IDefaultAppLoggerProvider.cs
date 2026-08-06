// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for creating the default logger of the application
/// </summary>
public interface IDefaultAppLoggerProvider
{
    /// <summary>
    /// Current app configuration provider
    /// </summary>
    IAppConfigurationProvider? AppConfigurationProvider { get;  }

    /// <summary>
    /// Current logging config
    /// </summary>
    LoggingConfig LoggingConfig { get; }

    /// <summary>
    /// The app default logger instance create by the provider
    /// </summary>
    IAppLoggerProxy? DefaultLogger { get; }

    /// <summary>
    /// Load the logging settings from <see cref="IAppConfigurationProvider.Configuration"/>
    /// </summary>
    void LoadLoggingConfigFromConfiguration();

    /// <summary>
    /// Load <see cref="DefaultLogger"/> from <see cref="LoggingConfig"/>
    /// </summary>
    void LoadDefaultLogger();


}
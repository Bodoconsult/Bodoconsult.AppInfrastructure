// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for app start provider implementations generating the most basic app features like AppStartParameter, app configuration handling from appsettings.json and app default logging
/// </summary>
public interface IAppStartProvider
{
    /// <summary>
    /// Global app settings
    /// </summary>
    IAppGlobals AppGlobals { get; }

    /// <summary>
    /// Current <see cref="IAppConfigurationProvider"/> instance to use
    /// </summary>
    IAppConfigurationProvider? AppConfigurationProvider { get; }

    /// <summary>
    /// Current instance of <see cref="IDefaultAppLoggerProvider"/> to use
    /// </summary>
    IDefaultAppLoggerProvider? DefaultAppLoggerProvider { get; set; }

    /// <summary>
    /// Current logger provider instances to use for logger creation
    /// </summary>
    public IList<ILoggerProviderConfigurator>? LoggerProviderConfigurators { get; set; }

    /// <summary>
    /// Load the default app configuration provider reading from appsettings.json
    /// </summary>
    void LoadConfigurationProvider();

    /// <summary>
    /// Load the default app start
    /// </summary>
    void LoadAppStartParameter();

    /// <summary>
    /// Load the current <see cref="IDefaultAppLoggerProvider"/> implementation
    /// </summary>
    void LoadDefaultAppLoggerProvider();

    /// <summary>
    /// Set central values in <see cref="AppGlobals"/> instance
    /// </summary>
    void SetValuesInAppGlobal();
}
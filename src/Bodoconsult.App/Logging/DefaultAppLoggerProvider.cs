// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

using System.Diagnostics;
using Bodoconsult.App.Abstractions.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Bodoconsult.App.Logging;

/// <summary>
/// Default implementation creating default logger from appsettings.json file
/// </summary>
public class DefaultAppLoggerProvider : IDefaultAppLoggerProvider
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public DefaultAppLoggerProvider(IAppConfigurationProvider appConfigurationProvider, LoggingConfig loggingConfig)
    {
        AppConfigurationProvider = appConfigurationProvider;
        LoggingConfig = loggingConfig;
    }

    /// <summary>
    /// Current app configuration provider
    /// </summary>
    public IAppConfigurationProvider? AppConfigurationProvider { get; }

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
        LoggingConfig.LogDataFactory = new LogDataFactory();

        var config = AppConfigurationProvider!.ReadLoggingSection();

        ArgumentNullException.ThrowIfNull(config, "Config section is appsettings.json is missing");

        var kids = config.GetChildren().ToList();

        AddMinimumLogLevel(kids);

        AddFilters(kids);

        AddLoggerProviders(kids);
    }

    /// <summary>
    /// Load <see cref="IDefaultAppLoggerProvider.DefaultLogger"/> from <see cref="IDefaultAppLoggerProvider.LoggingConfig"/>
    /// </summary>
    public void LoadDefaultLogger()
    {
        DefaultLogger = AppLoggerExtensions.GetDefaultAppLoggerProxy(LoggingConfig);
    }

    private void AddLoggerProviders(List<IConfigurationSection> kids)
    {
        foreach (var configurator in LoggingConfig.LoggerProviderConfigurators)
        {
            var section = kids.FirstOrDefault(item => item.Key == configurator.SectionNameAppSettingsJson);
            if (section == null)
            {
                return;
            }

            configurator.Section = section;
        }
    }

    private void AddFilters(List<IConfigurationSection> kids)
    {
        var section = kids.FirstOrDefault(item => item.Key == "LogLevel");

        if (section == null)
        {
            return;
        }

        // Add filters from config
        var logLevels = section.GetChildren();
        foreach (var logLevel in logLevels)
        {
            Enum.TryParse(logLevel.Value, ignoreCase: true, result: out LogLevel logLevelValue);
            if (!LoggingConfig.Filters.TryAdd(logLevel.Key, logLevelValue))
            {
                Debug.Print($"LogLevel {logLevel.Key} already exists");
            }
        }
    }

    private void AddMinimumLogLevel(List<IConfigurationSection> kids)
    {
        // Add minimum log level from config
        var minLevel = kids.FirstOrDefault(x => x.Key == "MinimumLogLevel");

        if (minLevel == null)
        {
            return;
        }

        Enum.TryParse(minLevel.Value, ignoreCase: true, result: out LogLevel logLevelValue);
        LoggingConfig.MinimumLogLevel = logLevelValue;
    }



}
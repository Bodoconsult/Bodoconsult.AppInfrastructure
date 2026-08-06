// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Bodoconsult.App.Logging.LoggingConfigurators;

/// <summary>
/// Configures a Log4Net logger
/// </summary>
public class Log4NetLoggingProviderConfigurator : ILoggerProviderConfigurator
{
    /// <summary>
    /// The name of the section in the appsettings.json file
    /// </summary>
    public string SectionNameAppSettingsJson => "Log4Net";

    /// <summary>
    /// The configuration section from appsettings.json or null if not existing
    /// </summary>
    public IConfigurationSection? Section { get; set; }

    /// <summary>
    /// Add the DI container service used for the current logger provider
    /// </summary>
    /// <param name="builder">Current logging builder</param>
    /// <param name="loggingConfig">Current logging config</param>
    public void AddServices(ILoggingBuilder builder, LoggingConfig loggingConfig)
    {
        builder.AddLog4Net();
    }
}
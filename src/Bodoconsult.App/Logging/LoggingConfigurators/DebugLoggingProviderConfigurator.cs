// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics;
using Bodoconsult.App.Abstractions.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Bodoconsult.App.Logging.LoggingConfigurators;

/// <summary>
/// Configures a DEBUG window logger
/// </summary>
public class DebugLoggingProviderConfigurator : ILoggerProviderConfigurator
{
    /// <summary>
    /// The name of the section in the appsettings.json file
    /// </summary>
    public string SectionNameAppSettingsJson => "Debug";

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
        // Debug
        if (Debugger.IsAttached)
        {
            builder.AddDebug();
        }
    }
}
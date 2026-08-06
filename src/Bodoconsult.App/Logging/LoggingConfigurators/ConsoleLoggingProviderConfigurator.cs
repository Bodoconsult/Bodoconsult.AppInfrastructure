// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Bodoconsult.App.Logging.LoggingConfigurators;

/// <summary>
/// Configures a console window logger
/// </summary>
public class ConsoleLoggingProviderConfigurator : ILoggerProviderConfigurator
{
    /// <summary>
    /// The name of the section in the appsettings.json file
    /// </summary>
    public string SectionNameAppSettingsJson => "Console";

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
        ArgumentNullException.ThrowIfNull(Section);

        var oValue = Section.GetChildren().FirstOrDefault(x => x.Key == "DisableColors");
        if (oValue is { Value: not null })
        {
            ConsoleConfigurationSettings.ColorBehavior =
                !oValue.Value.Equals("FALSE", StringComparison.InvariantCultureIgnoreCase)
                    ? LoggerColorBehavior.Enabled
                    : LoggerColorBehavior.Disabled;
        }

        oValue = Section.GetChildren().FirstOrDefault(x => x.Key == "IncludeScopes");
        if (oValue is { Value: not null })
        {
            ConsoleConfigurationSettings.IncludeScopes = oValue.Value.ToUpperInvariant() != "FALSE";
        }

        builder.AddSimpleConsole(x =>
        {
            x.ColorBehavior = loggingConfig.ConsoleConfigurationSettings.ColorBehavior;
            x.IncludeScopes = loggingConfig.ConsoleConfigurationSettings.IncludeScopes;

        });
    }

    /// <summary>
    /// Console config configuration
    /// </summary>
    public SimpleConsoleFormatterOptions ConsoleConfigurationSettings { get; } = new();
}
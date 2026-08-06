// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Microsoft.Extensions.Configuration;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for app configuration provider
/// </summary>
public interface IAppConfigurationProvider
{
    /// <summary>
    /// Full path to the JSON config file to use for the current app
    /// </summary>
    string ConfigFile { get; }

    /// <summary>
    /// Current configuration loaded from <see cref="ConfigFile"/>
    /// </summary>
    IConfigurationRoot? Configuration { get; }

    /// <summary>
    /// Load <see cref="Configuration"/> from <see cref="ConfigFile"/>
    /// </summary>
    /// <returns>Config object</returns>
    void LoadConfigurationFromConfigFile();

    /// <summary>
    /// Load the default connection from <see cref="Configuration"/> from section ConnectionStrings value DefaultConnection
    /// </summary>
    string ReadDefaultConnection();

    /// <summary>
    /// Read the logging section
    /// </summary>
    /// <returns>Logging section</returns>
    IConfigurationSection? ReadLoggingSection();

    /// <summary>
    /// Read the app start parameter section
    /// </summary>
    /// <returns>App start parameter section</returns>
    IConfigurationSection? ReadAppStartParameterSection();

    /// <summary>
    /// Read a section by its name
    /// </summary>
    /// <param name="sectionName">Section name requested</param>
    /// <returns>Section</returns>
    IConfigurationSection? ReadConfigurationSection(string sectionName);
}
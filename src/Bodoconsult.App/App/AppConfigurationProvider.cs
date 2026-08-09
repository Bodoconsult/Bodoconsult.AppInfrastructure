// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Bodoconsult.App;

/// <summary>
/// Current implementation of <see cref="IAppConfigurationProvider"/>
/// </summary>
public class AppConfigurationProvider : IAppConfigurationProvider
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public AppConfigurationProvider(string configFile)
    {
        ConfigFile = configFile;
    }

    /// <summary>
    /// Full path to the JSON config file to use for the current app
    /// </summary>
    public string ConfigFile { get; }

    /// <summary>
    /// Current configuration loaded from <see cref="IAppConfigurationProvider.ConfigFile"/>
    /// </summary>
    public IConfigurationRoot? Configuration { get; private set; }

    /// <summary>
    /// Load <see cref="IAppConfigurationProvider.Configuration"/> from <see cref="IAppConfigurationProvider.ConfigFile"/>
    /// </summary>
    /// <returns>Config object</returns>
    public void LoadConfigurationFromConfigFile()
    {
        //#if DEBUG
        //            ConfigFile = File.Exists(Path.Combine(path, "appsettings.Development.json")) ?
        //                "appsettings.Development.json" :
        //                "appsettings.json";
        //#endif

        var configFilePath = new FileInfo(ConfigFile).DirectoryName;
        if (configFilePath is null)
        {
            throw new ArgumentNullException(nameof(configFilePath));
        }


        Configuration = new ConfigurationBuilder()
            .SetBasePath(configFilePath)
            .AddJsonFile(ConfigFile)
            //.AddEnvironmentVariables()
            //.AddCommandLine(args)

            .Build();
    }

    /// <summary>
    /// Load the default connection from <see cref="IAppConfigurationProvider.Configuration"/> from section ConnectionStrings value DefaultConnection
    /// </summary>
    public string ReadDefaultConnection()
    {
        return Configuration?.GetSection("ConnectionStrings")["DefaultConnection"] ?? string.Empty;
    }

    /// <summary>
    /// Read the logging section
    /// </summary>
    /// <returns>Logging section</returns>
    public IConfigurationSection? ReadLoggingSection()
    {
        return Configuration?.GetSection("Logging");
    }

    /// <summary>
    /// Read the app start parameter section
    /// </summary>
    /// <returns>App start parameter section</returns>
    public IConfigurationSection? ReadAppStartParameterSection()
    {
        return Configuration?.GetSection("AppStartParameter");
    }


    /// <summary>
    /// Read a section by its name
    /// </summary>
    /// <param name="sectionName">Section name requested</param>
    /// <returns>Section</returns>
    public IConfigurationSection? ReadConfigurationSection(string sectionName)
    {
        return Configuration?.GetSection(sectionName);
    }
}
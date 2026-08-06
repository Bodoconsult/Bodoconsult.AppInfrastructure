// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Logging;
using Microsoft.Extensions.Configuration;

namespace Bodoconsult.App;

/// <summary>
/// Default app start provider reading configuration, creating app start parameter and creating logger as defined in configuration
/// </summary>
public class DefaultAppStartProvider : IAppStartProvider
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Current app globals</param>
    public DefaultAppStartProvider(IAppGlobals appGlobals)
    {
        AppGlobals = appGlobals;
    }

    /// <summary>
    /// Global app settings
    /// </summary>
    public IAppGlobals AppGlobals { get; }

    /// <summary>
    /// Current <see cref="IAppConfigurationProvider"/> instance to use
    /// </summary>
    public IAppConfigurationProvider? AppConfigurationProvider { get; private set; }

    /// <summary>
    /// Current instance of <see cref="IDefaultAppLoggerProvider"/> to use
    /// </summary>
    public IDefaultAppLoggerProvider? DefaultAppLoggerProvider { get; set; }

    /// <summary>
    /// Current logger provider instances to use for logger creation
    /// </summary>
    public IList<ILoggerProviderConfigurator>? LoggerProviderConfigurators { get; set; }

    /// <summary>
    /// Load the default app configuration provider reading from appsettings.json
    /// </summary>
    public void LoadConfigurationProvider()
    {
        if (string.IsNullOrEmpty(AppGlobals.AppStartParameter.ConfigFile))
        {
            throw new ArgumentNullException(nameof(AppGlobals.AppStartParameter.ConfigFile), "Config file name may not be null or empty");
        }

        AppConfigurationProvider = new AppConfigurationProvider(AppGlobals.AppStartParameter.ConfigFile);
        AppConfigurationProvider.LoadConfigurationFromConfigFile();

        AppGlobals.ConfigurationRoot = AppConfigurationProvider.Configuration;
    }

    /// <summary>
    /// Load the default app start
    /// </summary>
    public void LoadAppStartParameter()
    {
        if (AppConfigurationProvider == null)
        {
            throw new ArgumentNullException(nameof(AppConfigurationProvider));
        }

        //AppGlobals.AppStartParameter ??= new AppStartParameter();

        var asp = AppGlobals.AppStartParameter;

        if (string.IsNullOrEmpty(AppGlobals.AppStartParameter.DefaultConnectionString))
        {
            asp.DefaultConnectionString = AppConfigurationProvider.ReadDefaultConnection();
        }

        if (AppGlobals is IAppGlobalsWithDatabase withDatabase)
        {
            if (withDatabase.ContextConfig != null)
            {
                withDatabase.ContextConfig.ConnectionString = asp.DefaultConnectionString;
            }
        }

        var section = AppConfigurationProvider.ReadAppStartParameterSection();
        if (section == null)
        {
            return;
        }

        // Read AppName
        asp.AppName = ReadStringProperty(section, "AppName", asp.AppName) ?? "MyApp";
        asp.AppFolderName = ReadStringProperty(section, "AppFolderName", asp.AppFolderName ?? string.Empty) ?? "MyApp";
        asp.IpAddress = ReadStringProperty(section, "IpAddress", asp.IpAddress ?? string.Empty);
        asp.Port = ReadIntProperty(section, "Port", asp.Port);
        asp.NumberOfBackupsToKeep = ReadIntProperty(section, "NumberOfBackupsToKeep", asp.NumberOfBackupsToKeep);
        asp.BackupPath = ReadStringProperty(section, "BackupPath", asp.BackupPath ?? string.Empty);

        switch (asp)
        {
            case I2NetworkDevicesAppStartParameter asp2:
                asp2.IpAddress2 = ReadStringProperty(section, "IpAddress2", asp2.IpAddress2);
                asp2.Port2 = ReadIntProperty(section, "Port2", asp2.Port2);
                break;
            case I3NetworkDevicesAppStartParameter asp3:
                asp3.IpAddress2 = ReadStringProperty(section, "IpAddress2", asp3.IpAddress2);
                asp3.Port2 = ReadIntProperty(section, "Port2", asp3.Port2);

                asp3.IpAddress3 = ReadStringProperty(section, "IpAddress3",asp3.IpAddress2);
                asp3.Port3 = ReadIntProperty(section, "Port3", asp3.Port3);
                break;
        }
    }

    /// <summary>
    /// Read a string value from a config section
    /// </summary>
    /// <param name="section">Section</param>
    /// <param name="propertyName">Property name</param>
    /// <param name="currentValue">Current value to keep if config section does not provide a value</param>
    /// <returns>String value</returns>
    public static string? ReadStringProperty(IConfigurationSection section, string propertyName, string? currentValue)
    {
        var calue = section[propertyName];
        return !string.IsNullOrEmpty(calue) ? calue : currentValue;
    }

    /// <summary>
    /// Read a boolean value from a config section
    /// </summary>
    /// <param name="section">Section</param>
    /// <param name="propertyName">Property name</param>
    /// <param name="currentValue">Current value to keep if config section does not provide a value</param>
    /// <returns>Boolean value</returns>
    public static bool ReadBoolProperty(IConfigurationSection section, string propertyName, bool currentValue)
    {
        var calue = section[propertyName];
        if (string.IsNullOrEmpty(calue))
        {
            return currentValue;
        }

        try
        {
            return Convert.ToBoolean(calue);
        }
        catch // (Exception e)
        {
            return false;
        }
    }

    /// <summary>
    /// Read an int value from a config section
    /// </summary>
    /// <param name="section">Section</param>
    /// <param name="propertyName">Property name</param>
    /// <param name="currentValue">Current value to keep if config section does not provide a value</param>
    /// <returns>Int value</returns>
    public static int ReadIntProperty(IConfigurationSection section, string propertyName, int currentValue)
    {
        var calue = section[propertyName];
        if (string.IsNullOrEmpty(calue))
        {
            return currentValue;
        }

        try
        {
            return Convert.ToInt32(calue);
        }
        catch // (Exception e)
        {
            return 0;
        }
    }

    /// <summary>
    /// Read an int value from a config section
    /// </summary>
    /// <param name="section">Section</param>
    /// <param name="propertyName">Property name</param>
    /// <param name="currentValue">Current value to keep if config section does not provide a value</param>
    /// <returns>Int value</returns>
    public static long ReadLongProperty(IConfigurationSection section, string propertyName, long currentValue)
    {
        var calue = section[propertyName];
        if (string.IsNullOrEmpty(calue))
        {
            return currentValue;
        }

        try
        {
            return Convert.ToInt64(calue);
        }
        catch // (Exception e)
        {
            return 0;
        }
    }

    /// <summary>
    /// Load the current <see cref="IDefaultAppLoggerProvider"/> implementation
    /// </summary>
    public void LoadDefaultAppLoggerProvider()
    {
        ArgumentNullException.ThrowIfNull(AppConfigurationProvider);
        ArgumentNullException.ThrowIfNull(AppGlobals.LoggingConfig);

        DefaultAppLoggerProvider = new DefaultAppLoggerProvider(AppConfigurationProvider, AppGlobals.LoggingConfig);
        DefaultAppLoggerProvider.LoadLoggingConfigFromConfiguration();
        DefaultAppLoggerProvider.LoadDefaultLogger();
    }

    /// <summary>
    /// Set central values in <see cref="IAppGlobals"/> instance
    /// </summary>
    public void SetValuesInAppGlobal()
    {
        ArgumentNullException.ThrowIfNull(DefaultAppLoggerProvider);

        AppGlobals.Logger = DefaultAppLoggerProvider.DefaultLogger;
        AppGlobals.LoggingConfig = DefaultAppLoggerProvider.LoggingConfig;
        AppGlobals.AppStartParameter.DataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), AppGlobals.AppStartParameter.AppFolderName ?? "MyApp");
        AppGlobals.AppStartParameter.LogfilePath = AppGlobals.AppStartParameter.DataPath;
    }
}
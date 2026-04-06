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
    public IAppConfigurationProvider AppConfigurationProvider { get; private set; }

    /// <summary>
    /// Current instance of <see cref="IDefaultAppLoggerProvider"/> to use
    /// </summary>
    public IDefaultAppLoggerProvider DefaultAppLoggerProvider { get; set; }

    /// <summary>
    /// Current logger provider instances to use for logger creation
    /// </summary>
    public IList<ILoggerProviderConfigurator> LoggerProviderConfigurators { get; set; }

    /// <summary>
    /// Load the default app configuration provider reading from appsettings.json
    /// </summary>
    public void LoadConfigurationProvider()
    {
        AppConfigurationProvider = new AppConfigurationProvider(AppGlobals.AppStartParameter.ConfigFile);
        AppConfigurationProvider.LoadConfigurationFromConfigFile();
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

        AppGlobals.AppStartParameter ??= new AppStartParameter();

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
        asp.AppName = ReadStringProperty(section, "AppName");
        asp.AppFolderName = ReadStringProperty(section, "AppFolderName") ?? "MyApp";
        asp.IpAddress = ReadStringProperty(section, "IpAddress");
        asp.Port = ReadIntProperty(section, "Port");
        asp.NumberOfBackupsToKeep = ReadIntProperty(section, "NumberOfBackupsToKeep");
        asp.BackupPath = ReadStringProperty(section, "BackupPath");

        switch (asp)
        {
            case I2NetworkDevicesAppStartParameter asp2:
                asp2.IpAddress2 = ReadStringProperty(section, "IpAddress2");
                asp2.Port2 = ReadIntProperty(section, "Port2");
                break;
            case I3NetworkDevicesAppStartParameter asp3:
                asp3.IpAddress2 = ReadStringProperty(section, "IpAddress2");
                asp3.Port2 = ReadIntProperty(section, "Port2");

                asp3.IpAddress3 = ReadStringProperty(section, "IpAddress3");
                asp3.Port3 = ReadIntProperty(section, "Port3");
                break;
        }
    }

    private static string ReadStringProperty(IConfigurationSection section, string propertyName)
    {
        var calue = section[propertyName];
        return !string.IsNullOrEmpty(calue) ? calue : null;
    }

    private static int ReadIntProperty(IConfigurationSection section, string propertyName)
    {
        var calue = section[propertyName];
        if (string.IsNullOrEmpty(calue))
        {
            return 0;
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
    /// Load the current <see cref="IDefaultAppLoggerProvider"/> implementation
    /// </summary>
    public void LoadDefaultAppLoggerProvider()
    {
        DefaultAppLoggerProvider = new DefaultAppLoggerProvider(AppConfigurationProvider, AppGlobals.LoggingConfig);
        DefaultAppLoggerProvider.LoadLoggingConfigFromConfiguration();
        DefaultAppLoggerProvider.LoadDefaultLogger();
    }

    /// <summary>
    /// Set central values in <see cref="IAppGlobals"/> instance
    /// </summary>
    public void SetValuesInAppGlobal()
    {
        AppGlobals.Logger = DefaultAppLoggerProvider.DefaultLogger;
        AppGlobals.LoggingConfig = DefaultAppLoggerProvider.LoggingConfig;
        AppGlobals.LogDataFactory = AppGlobals.LoggingConfig.LogDataFactory;
        AppGlobals.AppStartParameter.DataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), AppGlobals.AppStartParameter.AppFolderName);
        AppGlobals.AppStartParameter.LogfilePath = AppGlobals.AppStartParameter.DataPath;
    }
}
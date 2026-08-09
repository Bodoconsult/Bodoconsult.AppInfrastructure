//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

//using Bodoconsult.App.Interfaces;
//using Bodoconsult.App.Logging;

//namespace Bodoconsult.App
//{
//    /// <summary>
//    /// App start provider used for unit testing reading configuration, creating app start parameter and creating logger suitable for unit tests
//    /// </summary>
//    public class DebugAppStartProvider : IAppStartProvider
//    {
//        /// <summary>
//        /// The default app configuration file
//        /// </summary>
//        public string ConfigFile { get; set; }

//        /// <summary>
//        /// Current <see cref="IAppConfigurationProvider"/> instance to use
//        /// </summary>
//        public IAppConfigurationProvider AppConfigurationProvider { get; private set; }

//        /// <summary>
//        /// Current <see cref="IAppStartParameter"/> to use
//        /// </summary>
//        public IAppStartParameter AppStartParameter { get; private set; }

//        /// <summary>
//        /// Current instance of <see cref="IDefaultAppLoggerProvider"/> to use
//        /// </summary>
//        public IDefaultAppLoggerProvider DefaultAppLoggerProvider { get; set; }

//        /// <summary>
//        /// Current logger provider instances to use for logger creation
//        /// </summary>
//        public IList<ILoggerProviderConfigurator> LoggerProviderConfigurators { get; set; }


//        /// <summary>
//        /// Load the default app configuration provider reading from appsettings.json
//        /// </summary>
//        public void LoadConfigurationProvider()
//        {
//            AppConfigurationProvider = new AppConfigurationProvider();
//            AppConfigurationProvider.LoadConfigurationFromConfigFile();
//        }

//        /// <summary>
//        /// Load the default app start
//        /// </summary>
//        public void LoadAppStartParameter()
//        {
//            if (AppConfigurationProvider is null)
//            {
//                throw new ArgumentNullException(nameof(AppConfigurationProvider));
//            }

//            AppStartParameter = new AppStartParameter
//            {
//                DefaultConnectionString = AppConfigurationProvider.ReadDefaultConnection()
//            };

//        }

//        /// <summary>
//        /// Load the default app start
//        /// </summary>
//        public void LoadAppStartParameter(IAppStartParameter appStartParameter)
//        {
//            if (AppConfigurationProvider is null)
//            {
//                throw new ArgumentNullException(nameof(AppConfigurationProvider));
//            }

//            AppStartParameter = appStartParameter ?? throw new ArgumentNullException(nameof(appStartParameter));
//            AppStartParameter.DefaultConnectionString = AppConfigurationProvider.ReadDefaultConnection();
//        }

//        /// <summary>
//        /// Load the current <see cref="IDefaultAppLoggerProvider"/> implementation
//        /// </summary>
//        public void LoadDefaultAppLoggerProvider()
//        {
//            DefaultAppLoggerProvider = new DebugAppLoggerProvider();
//            DefaultAppLoggerProvider.LoadLoggingConfigFromConfiguration();
//            DefaultAppLoggerProvider.LoadDefaultLogger();
//        }

//        /// <summary>
//        /// Set central values in <see cref="IAppGlobals"/> instance
//        /// </summary>
//        /// <param name="appInstance">Current app globals instance</param>
//        public void SetValuesInAppGlobal(IAppGlobals appInstance)
//        {
//            appInstance.AppStartParameter = AppStartParameter;
//            appInstance.Logger = DefaultAppLoggerProvider.DefaultLogger;
//            appInstance.LoggingConfig = DefaultAppLoggerProvider.LoggingConfig;
//            appInstance.LogDataFactory = appInstance.LoggingConfig.LogDataFactory;

//            appInstance.AppStartParameter.DataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), AppStartParameter.AppFolderName);
//            appInstance.AppStartParameter.LogfilePath = appInstance.AppStartParameter.DataPath;
//        }
//    }
//}
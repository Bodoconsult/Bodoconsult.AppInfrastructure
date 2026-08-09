// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Logging.LoggingConfigurators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Bodoconsult.App.Logging;

/// <summary>
/// Helper functionality for easy usage of logging
/// </summary>
public static class AppLoggerExtensions
{
    /// <summary>
    /// Add a default logger
    /// </summary>
    /// <param name="serviceCollection">Current service collection</param>
    /// <param name="loggingConfig">Current logger configuration</param>

    public static void AddDefaultLogger(this IServiceCollection serviceCollection, LoggingConfig loggingConfig)
    {
        serviceCollection.AddLogging(builder =>
            {
                // Clear all default providers
                builder.ClearProviders();

                // Add minimum log level from config
                builder.SetMinimumLevel(loggingConfig.MinimumLogLevel);

                // Add filters from config
                foreach (var filter in loggingConfig.Filters)
                {
                    var key = string.Equals(filter.Key, "DEFAULT", StringComparison.InvariantCultureIgnoreCase) ? null : filter.Key;
                    builder.AddFilter(key, filter.Value);
                }

                // Add the providers found activated in appsettings.json
                foreach (var c in loggingConfig.LoggerProviderConfigurators.Where(x => x.Section != null))
                {
                    c.AddServices(builder, loggingConfig);
                }
            }
        );
    }

    /// <summary>
    /// Add a default logger
    /// </summary>
    /// <param name="serviceCollection">Current service collection</param>
    /// <param name="loggingConfig">Current logger configuration</param>
    /// <param name="monitorLogFilename">Current monitor log filename</param>
    public static void AddMonitorLogger(this IServiceCollection serviceCollection, LoggingConfig loggingConfig, string monitorLogFilename)
    {
        serviceCollection.AddLogging(builder =>
            {
                // Clear all default providers
                builder.ClearProviders();

                // Add minimum log level from config
                builder.SetMinimumLevel(loggingConfig.MinimumLogLevel);

                // Add filters from config
                foreach (var filter in loggingConfig.Filters)
                {
                    var key = string.Equals(filter.Key, "DEFAULT", StringComparison.InvariantCultureIgnoreCase) ? null : filter.Key;
                    builder.AddFilter(key, filter.Value);
                }

                // Add the providers found activated in appsettings.json without Log4Net
                foreach (var c in loggingConfig.LoggerProviderConfigurators.Where(x => x.Section != null))
                {
                    if (c is Log4NetLoggingProviderConfigurator)
                    {
                        continue;
                    }
                    c.AddServices(builder, loggingConfig);
                }

                // Now add monitor logger
                var mon = new Log4NetMonitorProvider(monitorLogFilename);
                builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider>(mon));
            }
        );
    }

    /// <summary>
    /// Create the configured default logger factory
    /// </summary>
    /// <param name="loggingConfig">Current logging configuration</param>
    /// <returns>Configured default logger</returns>
    public static ILoggerFactory? GetDefaultLogger(LoggingConfig loggingConfig)
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddDefaultLogger(loggingConfig);

        var logFactory = serviceCollection.BuildServiceProvider()
            .GetService<ILoggerFactory>();

        return logFactory;
    }

    /// <summary>
    /// Create the configured monitor logger factory
    /// </summary>
    /// <param name="loggingConfig">Current logging configuration</param>
    /// <param name="monitorLogFilename">Current monitor log filename</param>
    /// <returns>Configured default logger</returns>
    public static ILoggerFactory? GetMonitorLogger(LoggingConfig loggingConfig, string monitorLogFilename)
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddMonitorLogger(loggingConfig, monitorLogFilename);

        var logFactory = serviceCollection.BuildServiceProvider()
            .GetService<ILoggerFactory>();

        return logFactory;
    }

    /// <summary>
    /// Get a fake app logger proxy
    /// </summary>
    /// <returns></returns>
    public static IAppLoggerProxy GetFakeAppLoggerProxy()
    {
        return new AppLoggerProxy(new FakeLoggerFactory(), new LogDataFactory());
    }

    /// <summary>
    /// Get a default app logger proxy
    /// </summary>
    /// <returns></returns>
    public static IAppLoggerProxy GetDefaultAppLoggerProxy(LoggingConfig loggingConfig)
    {
        ArgumentNullException.ThrowIfNull(loggingConfig.LogDataFactory);

        var logger = GetDefaultLogger(loggingConfig);

        ArgumentNullException.ThrowIfNull(logger);

        return new AppLoggerProxy(logger, loggingConfig.LogDataFactory);
    }

    /// <summary>
    /// Get a monitor logger proxy
    /// </summary>
    /// <returns></returns>
    public static IAppLoggerProxy GetMonitorAppLoggerProxy(LoggingConfig loggingConfig, string monitorLogFilename)
    {
        ArgumentNullException.ThrowIfNull(loggingConfig.LogDataFactory);

        var logger = GetMonitorLogger(loggingConfig, monitorLogFilename);

        ArgumentNullException.ThrowIfNull(logger);

        return new AppLoggerProxy(logger, loggingConfig.LogDataFactory);
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.DependencyInjection;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Benchmarking;
using Bodoconsult.App.CentralServices;
using Bodoconsult.App.Factories;
using Bodoconsult.App.Interfaces;
using Bodoconsult.App.Logging;
using Microsoft.Extensions.Logging;

namespace Bodoconsult.App.DependencyInjection;

/// <summary>
/// Loads basic DI services configuration 1 used my most apps to DI container. Intended mainly for production
/// The following services are loaded:
///     - IAppLoggerProxy as central logger instance
///     - IAppLoggerProxyFactory for logging for creating specialized logfiles
///     - IAppBenchProxy for benchmarking
///     - IGeneralAppManagementManager for general app management
///     - IAppDateService for app date management
///     
/// 
/// </summary>
public class BasicAppServicesConfig1ContainerServiceProvider : IDiContainerServiceProvider
{
    private readonly string _benchmarkFileName;
    private readonly IAppGlobals _appGlobals;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Current app globals</param>
    public BasicAppServicesConfig1ContainerServiceProvider(IAppGlobals appGlobals)
    {
        _appGlobals = appGlobals;

        var fi = new FileInfo(_appGlobals.AppStartParameter.AppExe);

        _benchmarkFileName = Path.Combine(_appGlobals.AppStartParameter.DataPath, $"{fi.Name}_Benchmark.csv");
    }

    /// <summary>
    /// Add DI container services to a DI container
    /// </summary>
    /// <param name="diContainer">Current DI container</param>
    public void AddServices(DiContainer diContainer)
    {
        // Logging
        diContainer.AddSingletonInstance(_appGlobals.LogDataFactory);
        diContainer.AddSingleton<IAppLoggerProxyFactory, AppLoggerProxyFactory>();

        // Load an existing logger as central app logger
        if (_appGlobals.Logger != null)
        {
            diContainer.AddSingletonInstance(_appGlobals.Logger);
        }
        // ToDo: Load fresh app logger

        // Benchmark
        var benchProxy = AppBenchProxy.CreateAppBenchProxy(_benchmarkFileName, _appGlobals.LogDataFactory);
        diContainer.AddSingletonInstance(benchProxy);

        // General app management
        diContainer.AddSingleton<IGeneralAppManagementService, GeneralAppManagementService>();
        diContainer.AddSingleton<IGeneralAppManagementManager, GeneralAppManagementManager>();

        // App date time service
        diContainer.AddSingleton<IAppDateService, AppDateService>();

        // AppEventListener 
        diContainer.AddSingleton<IAppEventListener, DummyAppEventListener>();

        // Event source factory
        diContainer.AddSingleton<IAppEventSourceFactory, FakeAppEventSourceFactory>();
    }

    /// <summary>
    /// Late bind DI container references to avoid circular DI references
    /// </summary>
    /// <param name="diContainer"></param>
    public void LateBindObjects(DiContainer diContainer)
    {
        var appLogger = diContainer.Get<IAppLoggerProxy>();

        appLogger.LogInformation($"Benchmark starts logging to {_benchmarkFileName}...");

        // Set logger to current logger factory
        var loggerFactory = diContainer.Get<ILoggerFactory>();
        appLogger.UpdateILoggerFactory(loggerFactory);
    }
}
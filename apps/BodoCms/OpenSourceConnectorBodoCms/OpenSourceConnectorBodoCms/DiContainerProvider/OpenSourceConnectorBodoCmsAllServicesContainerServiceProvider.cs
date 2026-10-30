// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.DependencyInjection;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Benchmarking;
using Bodoconsult.App.Factories;
using Bodoconsult.Database.Interfaces;
using Bodoconsult.Database.SqlClient;
using Microsoft.Extensions.Logging;
using OpenSourceConnectorBodoCms.Business.App;
using OpenSourceConnectorBodoCms.Business.Services;
using System.Runtime.Versioning;
using OpenSourceConnectorBodoCms.Business.Interfaces;

namespace OpenSourceConnectorBodoCms.DiContainerProvider;

/// <summary>
/// Load all specific OpenSourceConnectorBodoCms services to DI container. Intended mainly for production
/// </summary>
[SupportedOSPlatform("windows10.0.17763")]
public class OpenSourceConnectorBodoCmsAllServicesContainerServiceProvider : IDiContainerServiceProvider
{
    private readonly string _benchmarkFileName;
    private readonly IAppGlobals _appGlobals;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Current app globals</param>
    public OpenSourceConnectorBodoCmsAllServicesContainerServiceProvider(IAppGlobals appGlobals)
    {
        AppStartParameter = appGlobals.AppStartParameter;
        _appGlobals = appGlobals;
        
        ArgumentNullException.ThrowIfNull(appGlobals.AppStartParameter.DataPath);

        _benchmarkFileName = Path.Combine(appGlobals.AppStartParameter.DataPath, "OpenSourceConnectorBodoCms_Benchmark.csv");
    }

    /// <summary>
    /// Current app start parameter
    /// </summary>
    public IAppStartParameter AppStartParameter { get; }

    /// <summary>
    /// Add DI container services to a DI container
    /// </summary>
    /// <param name="diContainer">Current DI container</param>
    public void AddServices(DiContainer diContainer)
    {
        // Factories to create tower instance related objects (should be singletons)
        diContainer.AddSingletonInstance(_appGlobals.LogDataFactory);
        diContainer.AddSingleton<IAppLoggerProxyFactory, AppLoggerProxyFactory>();

        // benchmark
        var benchProxy = AppBenchProxy.CreateAppBenchProxy(_benchmarkFileName, _appGlobals.LogDataFactory);
        diContainer.AddSingletonInstance(benchProxy);

        //// General app management
        //diContainer.AddSingleton<IGeneralAppManagementService, GeneralAppManagementService>();
        //diContainer.AddSingleton<IGeneralAppManagementManager, GeneralAppManagementManager>();

        // Services
        ArgumentException.ThrowIfNullOrEmpty(_appGlobals.AppStartParameter.DefaultConnectionString);
        var db = new SqlServerDatabaseService(_appGlobals.AppStartParameter.DefaultConnectionString);

        diContainer.AddSingletonInstance((IDatabaseService)db);
        diContainer.AddSingleton<IExportService, ExportService>();
        //diContainer.AddSingleton<IWebsiteBuilder, WebsiteBuilder>();
        //diContainer.AddSingleton<IOpenSourceConnectorBodoCmsHandler, OpenSourceConnectorBodoCmsHandler>();

        // Load all other services required for the app now
        diContainer.AddSingleton<IApplicationService, OpenSourceConnectorBodoCmsService>();
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

        //// Example 1: Load the job scheduler now
        //var scheduler = diContainer.Get<IJobSchedulerManagementDelegate>();
        //scheduler.StartJobScheduler();

        //// Example 2: Load business transactions
        //var btl = diContainer.Get<IBusinessTransactionLoader>();
        //btl.LoadProviders();

    }
}
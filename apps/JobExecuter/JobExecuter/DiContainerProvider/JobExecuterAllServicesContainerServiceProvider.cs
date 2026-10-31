// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App;
using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Benchmarking;
using Bodoconsult.App.Factories;
using Bodoconsult.App.Interfaces;
using Bodoconsult.Web.Mail.Interfaces;
using Bodoconsult.Web.Mail.Mailers;
using JobExecuter.Dal;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Runtime.Versioning;
using JobExecuter.Dal.App;
using DiContainer = Bodoconsult.App.Abstractions.DependencyInjection.DiContainer;

namespace JobExecuter.DiContainerProvider;

/// <summary>
/// Load all specific BodoFileTransfer services to DI container. Intended mainly for production
/// </summary>
[SupportedOSPlatform("windows10.0.17763")]
public class JobExecuterAllServicesContainerServiceProvider : IDiContainerServiceProvider
{
    private readonly string _benchmarkFileName = Path.Combine("C:\\ProgramData\\JobExecuter", "BodoFileTransfer_Benchmark.csv");
    private readonly IAppGlobals _appGlobals;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals"></param>
    public JobExecuterAllServicesContainerServiceProvider(IAppGlobals appGlobals)
    {
        AppStartParameter = appGlobals.AppStartParameter;
        _appGlobals = appGlobals;
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

        // General app management
        diContainer.AddSingleton<IGeneralAppManagementService, GeneralAppManagementService>();
        diContainer.AddSingleton<IGeneralAppManagementManager, GeneralAppManagementManager>();

        // Services
        diContainer.AddSingleton<IJobHandler, JobHandler>();
        
        // Load all other services required for the app now
        diContainer.AddSingleton<IApplicationService, JobExecuterService>();
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
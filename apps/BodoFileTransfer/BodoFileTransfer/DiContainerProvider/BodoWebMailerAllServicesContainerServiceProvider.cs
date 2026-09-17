// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App;
using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Benchmarking;
using Bodoconsult.App.Factories;
using Bodoconsult.App.Interfaces;
using Bodoconsult.Web.Mail.Interfaces;
using Bodoconsult.Web.Mail.Mailers;
using BodoFileTransfer.Business.App;
using BodoFileTransfer.Business.DataHandling;
using BodoFileTransfer.Business.FolderHandling;
using BodoFileTransfer.Business.Interfaces;
using BodoFileTransfer.Business.Model;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Runtime.Versioning;
using DiContainer = Bodoconsult.App.Abstractions.DependencyInjection.DiContainer;

namespace BodoFileTransfer.DiContainerProvider;

/// <summary>
/// Load all specific BodoFileTransfer services to DI container. Intended mainly for production
/// </summary>
[SupportedOSPlatform("windows10.0.17763.0")]
public class BodoFileTransferAllServicesContainerServiceProvider : IDiContainerServiceProvider
{
    private readonly string _benchmarkFileName = Path.Combine("C:\\ProgramData\\BodoFileTransfer", "BodoFileTransfer_Benchmark.csv");

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appStartParameter"></param>
    /// <param name="licenseMissingDelegate"></param>
    public BodoFileTransferAllServicesContainerServiceProvider(IAppStartParameter appStartParameter, LicenseMissingDelegate licenseMissingDelegate)
    {
        AppStartParameter = appStartParameter;
        LicenseMissingDelegate = licenseMissingDelegate;
    }

    /// <summary>
    /// Current app start parameter
    /// </summary>
    public IAppStartParameter AppStartParameter { get; }

    /// <summary>
    /// Current <see cref="LicenseMissingDelegate"/>
    /// </summary>
    public LicenseMissingDelegate LicenseMissingDelegate { get; set; }

    /// <summary>
    /// Add DI container services to a DI container
    /// </summary>
    /// <param name="diContainer">Current DI container</param>
    public void AddServices(DiContainer diContainer)
    {
        // Factories to create tower instance related objects (should be singletons)
        diContainer.AddSingletonInstance(Globals.Instance.LogDataFactory);
        diContainer.AddSingleton<IAppLoggerProxyFactory, AppLoggerProxyFactory>();

        // benchmark
        var benchProxy = AppBenchProxy.CreateAppBenchProxy(_benchmarkFileName, Globals.Instance.LogDataFactory);
        diContainer.AddSingletonInstance(benchProxy);

        // General app management
        diContainer.AddSingleton<IGeneralAppManagementService, GeneralAppManagementService>();
        diContainer.AddSingleton<IGeneralAppManagementManager, GeneralAppManagementManager>();

        // Services
        diContainer.AddSingleton<IMailer, O365Mailer>();
        diContainer.AddSingleton<IDataHandler, DataHandler>();
        diContainer.AddSingleton<IFolderHandlerConfig, FolderHandlerConfig>();
        diContainer.AddSingleton<IFolderHandler, FolderHandler>();
        
        // Load all other services required for the app now
        diContainer.AddSingleton<IApplicationService, BodoFileTransferService>();

        // ...

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
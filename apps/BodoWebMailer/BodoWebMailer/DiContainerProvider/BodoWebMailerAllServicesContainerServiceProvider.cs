// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using Bodoconsult.App;
using Bodoconsult.App.Abstractions.DependencyInjection;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Benchmarking;
using Bodoconsult.App.Factories;
using Bodoconsult.App.Interfaces;
using BodoWebMailer.Business.App;
using BodoWebMailer.Business.Interfaces;
using BodoWebMailer.Business.Services;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Runtime.Versioning;
using Bodoconsult.Web.Mail.Interfaces;
using Bodoconsult.Web.Mail.Mailers;

namespace BodoWebMailer.DiContainerProvider;

/// <summary>
/// Load all specific BodoWebMailer services to DI container. Intended mainly for production
/// </summary>
[SupportedOSPlatform("windows10.0.17763")]
public class BodoWebMailerAllServicesContainerServiceProvider : IDiContainerServiceProvider
{
    private readonly string _benchmarkFileName;
    private readonly IBodoWebMailerAppGlobals _appGlobals;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Current app globals</param>
    public BodoWebMailerAllServicesContainerServiceProvider(IAppGlobals appGlobals)
    {
        AppStartParameter = appGlobals.AppStartParameter;

        if (appGlobals is IBodoWebMailerAppGlobals globals)
        {
            _appGlobals = globals;
        }
        else
        {
            throw new ArgumentException("appGlobals is not IBodoWebMailerAppGlobals");
        }
        
        ArgumentNullException.ThrowIfNull(appGlobals.AppStartParameter.DataPath);

        _benchmarkFileName = Path.Combine(appGlobals.AppStartParameter.DataPath, "BodoWebMailer_Benchmark.csv");
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
        diContainer.AddSingleton<IMailStorageService, SqlServerMailStorageService>();

        if (_appGlobals.UseSmtp)
        {
            diContainer.AddSingleton<IMailer, SmtpMailer>();
        }
        else
        {
            diContainer.AddSingleton<IMailer, O365Mailer>();
        }
        
        diContainer.AddSingleton<IMailHandler, MailHandler>();

        // Load all other services required for the app now
        diContainer.AddSingleton<IApplicationService, BodoWebMailerService>();

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
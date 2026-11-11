// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.DependencyInjection;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Benchmarking;
using Bodoconsult.App.Factories;
using Bodoconsult.Database.Interfaces;
using Bodoconsult.Database.SqlClient;
using Bodoconsult.Drawing.SkiaSharp.Factories;
using Bodoconsult.Drawing.SkiaSharp.Interfaces;
using BodoWebReporting.Dal.App;
using BodoWebReporting.Dal.DataHandling;
using BodoWebReporting.Dal.Factories;
using BodoWebReporting.Dal.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Runtime.Versioning;
using BodoWebReporting.Dal.ReportHandling;

namespace BodoWebReporting.DiContainerProvider;

/// <summary>
/// Load all specific BodoWebReporting services to DI container. Intended mainly for production
/// </summary>
[SupportedOSPlatform("windows10.0.17763")]
public class BodoWebReportingAllServicesContainerServiceProvider : IDiContainerServiceProvider
{
    //private readonly string _benchmarkFileName;
    private readonly IAppGlobals _appGlobals;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Current app globals</param>
    public BodoWebReportingAllServicesContainerServiceProvider(IAppGlobals appGlobals)
    {
        AppStartParameter = appGlobals.AppStartParameter;
        _appGlobals = appGlobals;

        ArgumentNullException.ThrowIfNull(appGlobals.AppStartParameter.DataPath);

        //_benchmarkFileName = Path.Combine(appGlobals.AppStartParameter.DataPath, "BodoWebReporting_Benchmark.csv");
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

        //// benchmark
        //var benchProxy = AppBenchProxy.CreateAppBenchProxy(_benchmarkFileName, _appGlobals.LogDataFactory);
        //diContainer.AddSingletonInstance(benchProxy);

        //// General app management
        //diContainer.AddSingleton<IGeneralAppManagementService, GeneralAppManagementService>();
        //diContainer.AddSingleton<IGeneralAppManagementManager, GeneralAppManagementManager>();

        // Services
        ArgumentException.ThrowIfNullOrEmpty(_appGlobals.AppStartParameter.DefaultConnectionString);
        var db = new SqlClientConnManager(_appGlobals.AppStartParameter.DefaultConnectionString);
        db.SetCommandTimeout(60000);

        diContainer.AddSingletonInstance((IConnManager)db);
        diContainer.AddSingleton<IBitmapServiceFactory, BitmapServiceFactory>();
        diContainer.AddSingleton<IDataHandler, DataHandler>();
        
        diContainer.AddSingleton<IWebChartExporterFactory, WebChartExporterFactory>();
        diContainer.AddSingleton<IWebChartHandler, WebChartHandler>();

        diContainer.AddSingleton<IWebTableExporterFactory, WebTableExporterFactory>();
        diContainer.AddSingleton<IWebTableHandler, WebTableHandler>();

        diContainer.AddSingleton<IWebReportExporterFactory, WebReportExporterFactory>();
        diContainer.AddSingleton<IWebReportHandler, WebReportHandler>();
        
        diContainer.AddSingleton<IWebReportingHandler, WebReportingHandler>();

        // Load all other services required for the app now
        diContainer.AddSingleton<IApplicationService, BodoWebReportingService>();
    }

    /// <summary>
    /// Late bind DI container references to avoid circular DI references
    /// </summary>
    /// <param name="diContainer"></param>
    public void LateBindObjects(DiContainer diContainer)
    {
        var appLogger = diContainer.Get<IAppLoggerProxy>();

        // Set logger to current logger factory
        var loggerFactory = diContainer.Get<ILoggerFactory>();
        appLogger.UpdateILoggerFactory(loggerFactory);
    }
}
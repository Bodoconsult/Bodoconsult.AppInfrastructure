// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.DependencyInjection;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Wpf.Interfaces;
using WpfApp1.AppData;
using WpfApp1.ViewModels;

namespace WpfApp1.DiContainerProvider;

/// <summary>
/// Load all specific WpfApp1 services to DI container. Intended mainly for production
/// </summary>
public class WpfApp1AllServicesContainerServiceProvider : IDiContainerServiceProvider
{
    /// <summary>
    /// Add DI container services to a DI container
    /// </summary>
    /// <param name="diContainer">Current DI container</param>
    public void AddServices(DiContainer diContainer)
    {
        // Main view model
        diContainer.AddSingleton<IMainWindowViewModel, WpfApp1MainWindowViewModel>();

        // Load all other services required for the app now
        diContainer.AddSingleton<IApplicationService, WpfApp1Service>();

        // ...
    }

    /// <summary>
    /// Late bind DI container references to avoid circular DI references
    /// </summary>
    /// <param name="diContainer"></param>
    public void LateBindObjects(DiContainer diContainer)
    {
        //// Example 1: Load the job scheduler now
        //var scheduler = diContainer.Get<IJobSchedulerManagementDelegate>();
        //scheduler.StartJobScheduler();

        //// Example 2: Load business transactions
        //var btl = diContainer.Get<IBusinessTransactionLoader>();
        //btl.LoadProviders();
    }
}
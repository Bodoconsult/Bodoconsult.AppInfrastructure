// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.DependencyInjection;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Logging;
using Bodoconsult.App.Wpf.Interfaces;
using Bodoconsult.App.Wpf.ReactiveUI.Interfaces;
using ReactiveUI;
using WpfReactiveUiDemoApp.AppData;
using WpfReactiveUiDemoApp.ViewLocation;
using WpfReactiveUiDemoApp.ViewModels;

namespace WpfReactiveUiDemoApp.DiContainerProvider;

/// <summary>
/// Load all specific WpfReactiveUiDemoApp services to DI container. Intended mainly for production
/// </summary>
public class WpfReactiveUiDemoAppAllServicesContainerServiceProvider : IDiContainerServiceProvider
{
    /// <summary>
    /// Add DI container services to a DI container
    /// </summary>
    /// <param name="diContainer">Current DI container</param>
    public void AddServices(DiContainer diContainer)
    {
        // AppEventListener 
        diContainer.AddSingleton<IAppEventListener, AppEventListener>();

        // Load all other services required for the app now
        diContainer.AddSingleton<IRegionManager>(new RegionManager());
        diContainer.AddTransient<IMainWindowViewModel, WpfReactiveUiDemoAppMainWindowViewModel>();
        //diContainer.AddTransient<ViewModel1, ViewModel1>();
        //diContainer.AddTransient<ViewModel2, ViewModel2>();

        //diContainer.AddSingleton<IViewLocator, SimpleViewLocator>(); 
        diContainer.AddSingleton<IApplicationService, WpfReactiveUiDemoAppService>();

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
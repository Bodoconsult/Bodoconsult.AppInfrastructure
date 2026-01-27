// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.DependencyInjection;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Logging;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.Wpf.ReactiveUI.Regions;
using WpfReactiveUiDemoApp.AppData;
using WpfReactiveUiDemoApp.ViewModels;
using WpfReactiveUiDemoApp.Views;

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
        
        // Regions manager with all window types loaded with regions
        var rm = new WpfRegionManager();
        rm.RegisterWindow<MainWindow, WpfReactiveUiDemoAppMainWindowViewModel>(["DocumentRegion", "MenuRegion"], null);
        rm.RegisterWindow<Window1, Window1ViewModel>(["DocumentRegion", "MenuRegion"], () => new Window1());

        diContainer.AddSingleton<IRegionManager>(rm);

        // View models
        diContainer.AddTransient<WpfReactiveUiDemoAppMainWindowViewModel, WpfReactiveUiDemoAppMainWindowViewModel>();
        diContainer.AddTransient<FirstViewModel, FirstViewModel>();

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
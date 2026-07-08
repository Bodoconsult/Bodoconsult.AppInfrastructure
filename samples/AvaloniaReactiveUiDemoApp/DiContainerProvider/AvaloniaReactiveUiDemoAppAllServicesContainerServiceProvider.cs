// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.DependencyInjection;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Logging;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.Avalonia.ReactiveUI.Regions;
using AvaloniaReactiveUiDemoApp.AppData;
using AvaloniaReactiveUiDemoApp.ViewModels;
using Bodoconsult.App.Avalonia.Helpers;
using Bodoconsult.App.Avalonia.ReactiveUI;
using Bodoconsult.App.Avalonia.ReactiveUI.Views;
using Bodoconsult.App.ReactiveUI.ViewModels;
using Bodoconsult.App.Avalonia.Interfaces;
using Bodoconsult.App.Avalonia.ReactiveUI.ViewModels;

namespace AvaloniaReactiveUiDemoApp.DiContainerProvider;

/// <summary>
/// Load all specific AvaloniaReactiveUiDemoApp services to DI container. Intended mainly for production
/// </summary>
public class AvaloniaReactiveUiDemoAppAllServicesContainerServiceProvider : IDiContainerServiceProvider
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
        var rm = new AvaloniaRegionManager();
        rm.RegisterWindow<MainWindow, AvaloniaReactiveUiDemoAppMainWindowViewModel>(["DocumentRegion", "MenuRegion"], null);
        rm.RegisterWindow<Views.Window1, Window1ViewModel>(["DocumentRegion", "MenuRegion"], () => new Views.Window1());

        diContainer.AddSingleton<IRegionManager>(rm);

        // View models
        diContainer.AddSingleton<LogoViewModel, LogoViewModel>();
        diContainer.AddTransient<AvaloniaReactiveUiDemoAppMainWindowViewModel, AvaloniaReactiveUiDemoAppMainWindowViewModel>();
        diContainer.AddTransient<FirstViewModel, FirstViewModel>();
        diContainer.AddTransient<CopyrightViewModel, CopyrightViewModel>();

        //diContainer.AddTransient<ViewModel1, ViewModel1>();
        //diContainer.AddTransient<ViewModel2, ViewModel2>();

        //diContainer.AddSingleton<IViewLocator, SimpleViewLocator>(); 
        diContainer.AddSingleton<IApplicationService, AvaloniaReactiveUiDemoAppService>();

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

        var vm = diContainer.Get<LogoViewModel>();

        var exe = Environment.ProcessPath;

        if (exe == null)
        {
            return;
        }

        var fi = new FileInfo(exe);

        var fileName = Path.Combine( fi.DirectoryName ?? "", "logo.jpg");

        vm.LoadLogoFromFile(fileName);
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.App.Abstractions.DependencyInjection;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Wpf.AppStarter;
using Microsoft.Extensions.Hosting;
using Splat.Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using Bodoconsult.App.Wpf.Interfaces;
using ReactiveUI.Builder;

namespace Bodoconsult.App.Wpf.ReactiveUI.App;

/// <summary>
/// Base class for <see cref="IAppBuilder"/> implementations running a background service but not using GRPC
/// </summary>
public class BaseWpfReactiveUiAppBuilder: BaseAppBuilder
{

    private readonly IHostBuilder _builder;
    private IHost _host;
    private List<Assembly> _viewAssemblies;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Current app globals</param>
    /// <param name="args">Current app args from command line</param>
    public BaseWpfReactiveUiAppBuilder(IAppGlobals appGlobals, List<Assembly> viewAssemblies, string[] args = null) : base(appGlobals)
    {
        _viewAssemblies = viewAssemblies;

        // Prepare the service builder instance
       _builder = DiContainer.CreateHost();

        //_builder.ConfigureServices(services =>
        //{
        //    services.UseMicrosoftDependencyResolver();
        //    //var resolver = AppLocator.CurrentMutable;
        //    //resolver.InitializeSplat();
        //    //resolver.InitializeReactiveUI();
        //});

        AppGlobals.DiContainer = new DiContainer();
    }

    /// <summary>
    /// Register DI container services
    /// </summary>
    public override void RegisterDiServices()
    {
        DiContainerServiceProviderPackage.AddServices(AppGlobals.DiContainer);

        AppGlobals.DiContainer.AddSingleton<IAppBuilder>(this);
    }

    /// <summary>
    /// Start the application
    /// </summary>
    public override void StartApplication()
    {
        //_builder.Services.AddHostedService<BackgroundServiceAppStarter>();

        var dpr = new MicrosoftDependencyResolver(AppGlobals.DiContainer.ServiceCollection);

        var appB = dpr.CreateReactiveUIBuilder()
            .WithWpf(); // Register WPF platform services

        foreach (var ass in _viewAssemblies)
        {
            appB.WithViewsFromAssembly(ass);
        }

        var h = appB.BuildApp();

        AppGlobals.DiContainer.BuildServiceProvider();

        _builder.ConfigureServices(services =>
        {
            foreach (var service in AppGlobals.DiContainer.ServiceCollection)
            {
                services.Add(service);
            }
        });

        _host = _builder.Build();
        //_host.Services.UseMicrosoftDependencyResolver();
        AppGlobals.DiContainer.LoadServiceProvider(_host.Services);

        DiContainerServiceProviderPackage.LateBindObjects(AppGlobals.DiContainer);

        // Create the viewmodel now
        var viewModel = CreateViewModel();

        // Inject it to UI
        var appStarter = new WpfStarterUi(this, viewModel);
        AppStarter = appStarter;

        // Run as singleton app
        if (AppGlobals.AppStartParameter.IsSingletonApp && appStarter.IsAnotherInstance)
        {
            Console.WriteLine($"Another instance of {AppGlobals.AppStartParameter.AppName} is already running! Press any key to proceed!");
            Console.ReadLine();
            Environment.Exit(0);
            return;
        }

        appStarter.Start();

        appStarter.Wait();


        //StartApplicationService();

    }

    /// <summary>
    /// Stops the application
    /// </summary>
    public override void StopApplication()
    {
        AppGlobals.EventWaitHandle?.Reset();
        ApplicationServer?.StopApplication();
        _host?.StopAsync();
    }

    /// <summary>
    /// Create the view model for the main window
    /// </summary>
    public virtual IMainWindowViewModel CreateViewModel()
    {
        throw new NotSupportedException();
    }
}
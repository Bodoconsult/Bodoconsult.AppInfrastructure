// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.App.Abstractions.DependencyInjection;
using Bodoconsult.App.Abstractions.Interfaces;
using Microsoft.Extensions.Hosting;
using ReactiveUI;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace Bodoconsult.App.Wpf.ReactiveUI.App;

/// <summary>
/// Base class for <see cref="IAppBuilder"/> implementations running a background service but not using GRPC
/// </summary>
public class BaseWpfReactiveUiAppBuilder: BaseAppBuilder
{

    private readonly IHostBuilder _builder;
    private IHost _host;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Current app globals</param>
    /// <param name="args">Current app args from command line</param>
    public BaseWpfReactiveUiAppBuilder(IAppGlobals appGlobals, string[] args = null) : base(appGlobals)
    {
        // Prepare the service builder instance
        _builder = DiContainer.CreateHost();

        _builder.ConfigureServices(services =>
        {
            services.UseMicrosoftDependencyResolver();
            var resolver = AppLocator.CurrentMutable;
            resolver.InitializeSplat();
            resolver.InitializeReactiveUI();
        });

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

        _builder.ConfigureServices(services =>
        {
            foreach (var service in AppGlobals.DiContainer.ServiceCollection)
            {
                services.Add(service);
            }
        });

        _host = _builder.Build();
        AppGlobals.DiContainer.LoadServiceProvider(_host.Services);

        DiContainerServiceProviderPackage.LateBindObjects(AppGlobals.DiContainer);
            

        StartApplicationService();

        _host.Run();
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
}
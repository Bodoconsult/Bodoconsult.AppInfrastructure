// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.BackgroundService.App;
using WorkerService1.DiContainerProvider;

namespace WorkerService1;

public class WorkerService1AppBuilder : BaseBackgroundServiceAppBuilder
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Global app settings</param>
    public WorkerService1AppBuilder(IAppGlobals appGlobals) : base(appGlobals)
    {
        ConfigureHostBuilder(ConfigureAction);
    }

    private static void ConfigureAction(HostApplicationBuilder hostApplicationBuilder)
    {
        hostApplicationBuilder.Services.AddWindowsService(options =>
        {
            options.ServiceName = "WorkerService1";
        });
    }

    /// <summary>
    /// Load the <see cref="IAppBuilder.DiContainerServiceProviderPackage"/>
    /// </summary>
    public override void LoadDiContainerServiceProviderPackage()
    {
        var factory = new WorkerService1ProductionDiContainerServiceProviderPackageFactory(AppGlobals);
        DiContainerServiceProviderPackage = factory.CreateInstance();
    }
}
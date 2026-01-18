// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.DependencyInjection;

namespace WpfReactiveUiDemoApp.DiContainerProvider;

/// <summary>
/// Load all the complete package of WorkerService1 services based on GRPC to DI container. Intended mainly for production
/// </summary>
public class WpfReactiveUiDemoAppAllServicesDiContainerServiceProviderPackage : BaseDiContainerServiceProviderPackage
{

    public WpfReactiveUiDemoAppAllServicesDiContainerServiceProviderPackage(IAppGlobals appGlobals) : base(appGlobals)
    {

        DoNotBuildDiContainer = true;

        // Basic app services
        IDiContainerServiceProvider provider = new BasicAppServicesConfig1ContainerServiceProvider(appGlobals);
        ServiceProviders.Add(provider);

        // Performance measurement
        provider = new ApmDiContainerServiceProvider(appGlobals.AppStartParameter, appGlobals.StatusMessageDelegate);
        ServiceProviders.Add(provider);

        // App default logging
        provider = new DefaultAppLoggerDiContainerServiceProvider(appGlobals.LoggingConfig, appGlobals.Logger);
        ServiceProviders.Add(provider);

        // SWorkerService1 specific services
        provider = new WpfReactiveUiDemoAppAllServicesContainerServiceProvider();
        ServiceProviders.Add(provider);
    }

}
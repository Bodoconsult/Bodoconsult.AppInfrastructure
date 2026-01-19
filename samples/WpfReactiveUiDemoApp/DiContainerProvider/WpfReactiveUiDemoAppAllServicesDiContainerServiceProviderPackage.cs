// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.DependencyInjection;
using Bodoconsult.I18N.DependencyInjection;
using WpfReactiveUiDemoApp.I18N;

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

        // I18N
        var factory = new WpfReactiveUiDemoAppI18NFactory();
        provider = new I18NDiContainerServiceProvider(factory);
        ServiceProviders.Add(provider);

        // App specific services
        provider = new WpfReactiveUiDemoAppAllServicesContainerServiceProvider();
        ServiceProviders.Add(provider);
    }

}
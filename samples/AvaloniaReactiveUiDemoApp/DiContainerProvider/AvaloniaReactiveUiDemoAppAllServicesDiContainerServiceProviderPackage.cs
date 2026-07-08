// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.DependencyInjection;
using Bodoconsult.I18N.DependencyInjection;
using AvaloniaReactiveUiDemoApp.I18N;

namespace AvaloniaReactiveUiDemoApp.DiContainerProvider;

/// <summary>
/// Load all the complete package of WorkerService1 services based on GRPC to DI container. Intended mainly for production
/// </summary>
public class AvaloniaReactiveUiDemoAppAllServicesDiContainerServiceProviderPackage : BaseDiContainerServiceProviderPackage
{

    public AvaloniaReactiveUiDemoAppAllServicesDiContainerServiceProviderPackage(IAppGlobals appGlobals) : base(appGlobals)
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
        var factory = new AvaloniaReactiveUiDemoAppI18NFactory();
        provider = new I18NDiContainerServiceProvider(factory);
        ServiceProviders.Add(provider);

        // App specific services
        provider = new AvaloniaReactiveUiDemoAppAllServicesContainerServiceProvider();
        ServiceProviders.Add(provider);
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.DependencyInjection;

namespace WpfApp1.DiContainerProvider;

/// <summary>
/// Load all the complete package of WpfApp1 services based on GRPC to DI container. Intended mainly for production
/// </summary>
public class WpfApp1AllServicesDiContainerServiceProviderPackage : BaseDiContainerServiceProviderPackage
{

    public WpfApp1AllServicesDiContainerServiceProviderPackage(IAppGlobals appGlobals,
        StatusMessageDelegate statusMessageDelegate, LicenseMissingDelegate licenseMissingDelegate) : base(appGlobals)
    {
        // Basic app services like logging etc.
        IDiContainerServiceProvider provider = new BasicAppServicesConfig2ContainerServiceProvider(appGlobals);
        ServiceProviders.Add(provider);

        // Performance measurement
        provider = new ApmDiContainerServiceProvider(appGlobals.AppStartParameter, statusMessageDelegate);
        ServiceProviders.Add(provider);

        //// App default logging
        //provider = new DefaultAppLoggerDiContainerServiceProvider(appGlobals.LoggingConfig, appGlobals.Logger);
        //ServiceProviders.Add(provider);

        // SWpfApp1 specific services
        provider = new WpfApp1AllServicesContainerServiceProvider();
        ServiceProviders.Add(provider);
    }

}
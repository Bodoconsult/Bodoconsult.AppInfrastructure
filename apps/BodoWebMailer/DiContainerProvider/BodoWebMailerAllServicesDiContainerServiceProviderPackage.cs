// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.DependencyInjection;
using System.Runtime.Versioning;

namespace BodoWebMailer.DiContainerProvider;

/// <summary>
/// Load all the complete package of BodoWebMailer services based on GRPC to DI container. Intended mainly for production
/// </summary>
[SupportedOSPlatform("windows10.0.17763.0")]
public class BodoWebMailerAllServicesDiContainerServiceProviderPackage : BaseDiContainerServiceProviderPackage
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals"></param>
    /// <param name="statusMessageDelegate"></param>
    /// <param name="licenseMissingDelegate"></param>
    public BodoWebMailerAllServicesDiContainerServiceProviderPackage(IAppGlobals appGlobals,
        StatusMessageDelegate statusMessageDelegate, LicenseMissingDelegate licenseMissingDelegate) : base(appGlobals)
    {
        ArgumentNullException.ThrowIfNull(appGlobals.Logger);

        // Basic app services
        IDiContainerServiceProvider provider = new BasicAppServicesConfig1ContainerServiceProvider(appGlobals);
        ServiceProviders.Add(provider);

        // App default logging
        provider = new DefaultAppLoggerDiContainerServiceProvider(appGlobals.LoggingConfig, appGlobals.Logger);
        ServiceProviders.Add(provider);

        // BodoWebMailer specific services
        provider = new BodoWebMailerAllServicesContainerServiceProvider(appGlobals.AppStartParameter, licenseMissingDelegate);
        ServiceProviders.Add(provider);
    }
}
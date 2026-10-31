// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Runtime.Versioning;
using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.DependencyInjection;

namespace JobExecuter.DiContainerProvider;

/// <summary>
/// Load all the complete package of BodoFileTransfer services to DI container. Intended mainly for production
/// </summary>
[SupportedOSPlatform("windows10.0.17763")]
public class JobExecuterAllServicesDiContainerServiceProviderPackage : BaseDiContainerServiceProviderPackage
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals"></param>
    public JobExecuterAllServicesDiContainerServiceProviderPackage(IAppGlobals appGlobals) : base(appGlobals)
    {
        ArgumentNullException.ThrowIfNull(appGlobals.Logger);

        // Basic app services
        IDiContainerServiceProvider provider = new BasicAppServicesConfig1ContainerServiceProvider(appGlobals);
        ServiceProviders.Add(provider);

        // App default logging
        provider = new DefaultAppLoggerDiContainerServiceProvider(appGlobals.LoggingConfig, appGlobals.Logger);
        ServiceProviders.Add(provider);

        // BodoFileTransfer specific services
        provider = new JobExecuterAllServicesContainerServiceProvider(appGlobals);
        ServiceProviders.Add(provider);
    }
}
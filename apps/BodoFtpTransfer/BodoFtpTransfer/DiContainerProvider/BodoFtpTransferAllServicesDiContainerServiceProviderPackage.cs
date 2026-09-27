// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Runtime.Versioning;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.DependencyInjection;

namespace BodoFtpTransfer.DiContainerProvider;

/// <summary>
/// Load all the complete package of BodoFtpTransfer services to DI container. Intended mainly for production
/// </summary>
[SupportedOSPlatform("windows10.0.17763.0")]
public class BodoFtpTransferAllServicesDiContainerServiceProviderPackage : BaseDiContainerServiceProviderPackage
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals"></param>
    public BodoFtpTransferAllServicesDiContainerServiceProviderPackage(IAppGlobals appGlobals) : base(appGlobals)
    {
        ArgumentNullException.ThrowIfNull(appGlobals.Logger);

        // Basic app services
        IDiContainerServiceProvider provider = new BasicAppServicesConfig1ContainerServiceProvider(appGlobals);
        ServiceProviders.Add(provider);

        // App default logging
        provider = new DefaultAppLoggerDiContainerServiceProvider(appGlobals.LoggingConfig, appGlobals.Logger);
        ServiceProviders.Add(provider);

        // BodoFtpTransfer specific services
        provider = new BodoFtpTransferAllServicesContainerServiceProvider(appGlobals.AppStartParameter);
        ServiceProviders.Add(provider);
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Runtime.Versioning;
using Bodoconsult.App.Abstractions.Interfaces;

namespace OpenSourceConnectorBodoCms.DiContainerProvider;

/// <summary>
/// The current DI container used for production 
/// </summary>
[SupportedOSPlatform("windows10.0.17763")]
public class OpenSourceConnectorBodoCmsProductionDiContainerServiceProviderPackageFactory : IDiContainerServiceProviderPackageFactory
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public OpenSourceConnectorBodoCmsProductionDiContainerServiceProviderPackageFactory(IAppGlobals appGlobals)
    {
        AppGlobals = appGlobals;
    }

    /// <summary>
    /// App globals
    /// </summary>
    public IAppGlobals AppGlobals { get; }

    /// <summary>
    /// Create an instance of <see cref="IDiContainerServiceProviderPackage"/>. Should be a singleton instance
    /// </summary>
    /// <returns>Singleton instance of <see cref="IDiContainerServiceProviderPackage"/></returns>
    public IDiContainerServiceProviderPackage CreateInstance()
    {
        return new OpenSourceConnectorBodoCmsAllServicesDiContainerServiceProviderPackage(AppGlobals);
    }
}
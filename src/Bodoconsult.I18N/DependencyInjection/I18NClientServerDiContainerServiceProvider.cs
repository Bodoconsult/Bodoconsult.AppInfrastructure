// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.DependencyInjection;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.I18N.DependencyInjection;

/// <summary>
/// DI container service provider for loading I18N client server instances. The following interfaces and delegates are loaded in the DI container:
/// - II18NServer as singleton
/// - II18NClient as scoped instances
///  </summary>
public class I18NClientServerDiContainerServiceProvider : IDiContainerServiceProvider
{
    private readonly II18NServerFactory _i18NServerFactory;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="i18NServerFactory">Current factory for a configured I18N instance</param>
    public I18NClientServerDiContainerServiceProvider(II18NServerFactory i18NServerFactory)
    {
        _i18NServerFactory = i18NServerFactory;
    }

    /// <summary>
    /// Add DI container services to a DI container
    /// </summary>
    /// <param name="diContainer">Current DI container</param>
    public void AddServices(DiContainer diContainer)
    {
        var i18N = _i18NServerFactory.CreateInstance();
        diContainer.AddSingleton(i18N);
        diContainer.AddScoped<II18NClient, I18NClient>();
    }

    /// <summary>
    /// Late bind DI container references to avoid circular DI references
    /// </summary>
    /// <param name="diContainer">Current DI container</param>
    public void LateBindObjects(DiContainer diContainer)
    {
        // Do nothing
    }
}
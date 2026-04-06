// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.DependencyInjection;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.I18N.DependencyInjection;

/// <summary>
/// DI container service provider for loading a dummy I18N instance. The following interfaces and delegates are loaded in the DI container:
/// - II18N
/// - TranslateDelegate
/// - TranslateWithParamsDelegate
///  </summary>
public class DummyI18NDiContainerServiceProvider : IDiContainerServiceProvider
{
    /// <summary>
    /// Add DI container services to a DI container
    /// </summary>
    /// <param name="diContainer">Current DI container</param>
    public void AddServices(DiContainer diContainer)
    {
        I18N.IsDummyRequested = true;
        var i18N = I18N.Current;
        diContainer.AddSingleton(i18N);
        diContainer.AddSingleton<TranslateDelegate>(i18N.Translate);
        diContainer.AddSingleton<TranslateWithParamsDelegate>(i18N.Translate);
    }

    /// <summary>
    /// Late bind DI container references to avoid circular DI references. Does nothing here
    /// </summary>
    /// <param name="diContainer">Current DI container</param>
    public void LateBindObjects(DiContainer diContainer)
    {
        // Do nothing
    }
}
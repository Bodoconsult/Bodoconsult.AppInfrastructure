// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.I18N.DependencyInjection;
using Bodoconsult.I18N.Test.LocalesProviderPackages;

namespace Bodoconsult.I18N.Test.Samples;

/// <summary>
/// Factory to create a fully configured I18N factory using a locales provider package
/// </summary>
public class TestI18NFactory2 : BaseI18NFactory
{
    /// <summary>
    /// Creating a configured II18N instance
    /// </summary>
    /// <returns>An II18N instance</returns>
    public override II18N CreateInstance()
    {
        // Set the fallback language
        I18NInstance.SetFallbackLocale("en");

        // Load a provider
        var sample = new SampleLocalesProviderPackage();
        sample.LoadLocalesProviders(I18NInstance);

        // Load more providers or packages if necessary
        // ...

        // Init instance with langugae from running thread
        I18NInstance.Init();

        // Return the instance
        return I18NInstance;
    }
}
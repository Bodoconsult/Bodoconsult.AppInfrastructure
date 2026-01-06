// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.I18N.DependencyInjection;
using Bodoconsult.I18N.LocalesProviders;
using Bodoconsult.I18N.Test.Helpers;

namespace Bodoconsult.I18N.Test.Samples;

/// <summary>
/// Factory to create a fully configured I18N factory using providers directly
/// </summary>
public class TestI18NServerFactory : BaseI18NServerFactory
{
    /// <summary>
    /// Creating a configured II18N instance
    /// </summary>
    /// <returns>An II18N instance</returns>
    public override II18NServer CreateInstance()
    {
        // Load a provider
        ILocalesProvider provider = new I18NEmbeddedResourceLocalesProvider(TestHelper.CurrentAssembly,
            "Bodoconsult.I18N.Test.Samples.Locales");

        I18NServerInstance.AddProvider(provider);

        // Add provider 2
        provider = new I18NEmbeddedResourceLocalesProvider(TestHelper.CurrentAssembly,
            "Bodoconsult.I18N.Test.Locales");

        I18NServerInstance.AddProvider(provider);

        // Load more providers or packages if necessary
        // ...

        // Return the instance
        return I18NServerInstance;
    }
}
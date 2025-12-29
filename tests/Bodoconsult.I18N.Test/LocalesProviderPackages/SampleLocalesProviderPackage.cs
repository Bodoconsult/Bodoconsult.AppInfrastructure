// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.I18N.LocalesProviders;

namespace Bodoconsult.I18N.Test.LocalesProviderPackages;

/// <summary>
/// Sample implementation of an <see cref="ILocalesProviderPackage"/>
/// </summary>
internal class SampleLocalesProviderPackage: BaseLocalesProviderPackage
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public SampleLocalesProviderPackage()
    {
        var assembly = typeof(SampleLocalesProviderPackage).Assembly;

        // Load a provider
        ILocalesProvider provider = new I18NEmbeddedResourceLocalesProvider(assembly, "Bodoconsult.I18N.Test.Samples.Locales");

        LocalesProviders.Add(provider);

        // Add provider 2
        provider = new I18NEmbeddedResourceLocalesProvider(assembly, "Bodoconsult.I18N.Test.Locales");

        LocalesProviders.Add(provider);
    }
}
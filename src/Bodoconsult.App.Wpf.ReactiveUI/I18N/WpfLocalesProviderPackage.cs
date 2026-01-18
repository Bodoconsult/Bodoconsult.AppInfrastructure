// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.I18N.LocalesProviders;

namespace Bodoconsult.App.Wpf.ReactiveUI.I18N;

/// <summary>
/// Load language ressources from Bodoconsult.App.Wpf.ReactiveUI
/// </summary>
public class WpfLocalesProviderPackage: BaseLocalesProviderPackage
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public WpfLocalesProviderPackage()
    {
        var assembly = typeof(WpfLocalesProviderPackage).Assembly;

        // Load a provider
        var provider = new I18NEmbeddedResourceLocalesProvider(assembly, "Bodoconsult.App.Wpf.ReactiveUI.Resources.Localization");
        LocalesProviders.Add(provider);
    }
}
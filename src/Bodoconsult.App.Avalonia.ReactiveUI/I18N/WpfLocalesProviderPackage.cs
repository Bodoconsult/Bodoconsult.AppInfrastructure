// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.I18N.LocalesProviders;

namespace Bodoconsult.App.Avalonia.ReactiveUI.I18N;

/// <summary>
/// Load language ressources from Bodoconsult.App.Avalonia.ReactiveUI
/// </summary>
public class AvaloniaLocalesProviderPackage: BaseLocalesProviderPackage
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public AvaloniaLocalesProviderPackage()
    {
        var assembly = typeof(AvaloniaLocalesProviderPackage).Assembly;

        // Load a provider
        var provider = new I18NEmbeddedResourceLocalesProvider(assembly, "Bodoconsult.App.Avalonia.ReactiveUI.Resources.Localization");
        LocalesProviders.Add(provider);
    }
}
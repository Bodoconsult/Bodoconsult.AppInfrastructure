// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Reflection;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.I18N.DependencyInjection;
using Bodoconsult.I18N.LocalesProviders;

namespace WpfApp1.Factories;


/// <summary>
/// Factory to create a fully configured I18N factory
/// </summary>
public class I18NFactory : BaseI18NFactory
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
        var provider = new I18NEmbeddedResourceLocalesProvider(Assembly.GetEntryAssembly(),
            "WpfApp1.Locales");

        I18NInstance.AddProvider(provider);

        // Load more providers if necessary
        // ...

        // Init instance with language from running thread
        I18NInstance.Init();

        // Return the instance
        return I18NInstance;
    }
}


// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Base class for <see cref="ILocalesProviderPackage"/> implementations
/// </summary>
public abstract class BaseLocalesProviderPackage:ILocalesProviderPackage
{
    /// <summary>
    /// List of all locales providers in this package
    /// </summary>
    public IList<ILocalesProvider> LocalesProviders { get; } = new List<ILocalesProvider>();

    /// <summary>
    /// Load the providers in an existing <see cref="II18N"/> instance
    /// </summary>
    /// <param name="i18Ninstance">Existing <see cref="II18N"/> instance to load the locales providers in</param>
    public void LoadLocalesProviders(II18N i18Ninstance)
    {
        foreach (var localesProvider in LocalesProviders)
        {
            i18Ninstance.AddProvider(localesProvider);
        }
    }
}
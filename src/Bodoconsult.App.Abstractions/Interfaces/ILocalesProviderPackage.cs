// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for creating <see cref="ILocalesProvider"/> packages for assemblies to simplify the locales providers handling for these assemblies
/// </summary>
public interface ILocalesProviderPackage
{
    /// <summary>
    /// List of all locales providers in this package
    /// </summary>
    IList<ILocalesProvider> LocalesProviders { get; }

    /// <summary>
    /// Load the providers in an existing <see cref="II18N"/> instance
    /// </summary>
    /// <param name="i18Ninstance">Existing <see cref="II18N"/> instance to load the locales providers in</param>
    void LoadLocalesProviders(II18N i18Ninstance);
}
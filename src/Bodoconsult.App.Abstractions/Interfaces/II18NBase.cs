// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Base interface for I18N implementations
/// </summary>
public interface II18NBase
{
    /// <summary>
    /// Reset all providers
    /// </summary>
    /// <returns>Current I18N instance for FluentAPI</returns>
    void Reset();

    /// <summary>
    ///  Add a provider as data source for translations
    /// </summary>
    /// <param name="provider">Provider for translation data</param>
    /// <returns>Current I18N instance for FluentAPI</returns>
    void AddProvider(ILocalesProvider provider);

    /// <summary>
    /// All loaded providers
    /// </summary>
    List<ILocalesProvider> Providers { get; }
}
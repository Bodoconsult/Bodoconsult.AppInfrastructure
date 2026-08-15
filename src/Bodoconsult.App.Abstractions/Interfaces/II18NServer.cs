// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for I18N server classes. Instances have to be loaded as singleton in the app
/// </summary>
public interface II18NServer : II18NBase, IDisposable
{
    /// <summary>
    /// Fallback locale to use
    /// </summary>
    string FallBackLocale { get; set; }

    /// <summary>
    /// Current loaded locales
    /// </summary>
    IReadOnlyList<string> Locales { get; }

    /// <summary>
    /// Symbol to show that translation was not found
    /// </summary>
    public string NotFoundSymbol { get; set; }

    /// <summary>
    /// Reset all providers (fluid version)
    /// </summary>
    /// <returns>Current I18N instance for FluentAPI</returns>
    public II18NServer Reset2();

    /// <summary>
    ///  Add a provider as data source for translations (fluid version)
    /// </summary>
    /// <param name="provider">Provider for translation data</param>
    /// <returns>Current I18N instance for FluentAPI</returns>
    public II18NServer AddProvider2(ILocalesProvider provider);

    /// <summary>
    /// Set a logger action
    /// </summary>
    /// <param name="output">Logger action</param>
    II18NServer SetLogger(Action<string> output);

    /// <summary>
    /// Set the not-found-symbol
    /// </summary>
    /// <param name="symbol">Symbol to set as not-found-symbol</param>
    II18NServer SetNotFoundSymbol(string symbol);
}
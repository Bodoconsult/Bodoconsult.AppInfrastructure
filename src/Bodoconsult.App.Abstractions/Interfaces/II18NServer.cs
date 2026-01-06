// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.ComponentModel;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for I18N server classes. Instances have to be loaded as singleton in the app
/// </summary>
public interface II18NServer : IDisposable
{
    /// <summary>
    /// Fallback locale to use
    /// </summary>
    string FallBackLocale { get; set; }

    /// <summary>
    /// All loaded providers
    /// </summary>
    IList<ILocalesProvider> Providers { get; }

    /// <summary>
    /// Symbol to show that translation was not found
    /// </summary>
    public string NotFoundSymbol { get; set; }

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

    /// <summary>
    ///  Add a provider as data source for translations
    /// </summary>
    /// <param name="provider">Provider for translation data</param>
    /// <returns>Current I18N instance for FluentAPI</returns>
    II18NServer AddProvider(ILocalesProvider provider);

    /// <summary>
    /// Reset all providers
    /// </summary>
    II18NServer Reset();
}
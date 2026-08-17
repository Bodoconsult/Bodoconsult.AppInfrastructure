// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Base interface for localization data providers
/// </summary>
public interface ILocalesProvider
{
    /// <summary>
    /// Current logger action
    /// </summary>
    Action<string>? Logger { get; }

    /// <summary>
    /// Set a logger action to enable logging
    /// </summary>
    /// <param name="logger">Logger action</param>
    /// <returns>Current provider</returns>
    ILocalesProvider SetLogger(Action<string> logger);

    /// <summary>
    /// All available resource items
    /// </summary>
    IDictionary<string, string> LocaleItems { get; }

    /// <summary>
    /// Register all available resource items
    /// </summary>
    void RegisterLocaleItems();

    /// <summary>
    /// Load key value pairs for string translations in a translation dictionary.
    /// If a key is already contained in the translation dictionary it should not be added again.
    /// </summary>
    /// <param name="language">Requested language</param>
    /// <returns>Translation dictionary with key value pairs in.</returns>
    IDictionary<string, string> LoadLocaleItem(string language );
}
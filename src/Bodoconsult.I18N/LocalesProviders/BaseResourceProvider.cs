// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System;
using System.Collections.Generic;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.I18N.LocalesProviders;

/// <summary>
/// Base resource provider class
/// </summary>
public class BaseResourceProvider : ILocalesProvider
{
    /// <summary>
    /// Current logger action
    /// </summary>
    public Action<string> Logger { get; private set; }

    /// <summary>
    /// All available resource items
    /// </summary>
    public IDictionary<string, string> LocaleItems { get; } = new Dictionary<string, string>();

    /// <summary>
    /// Set a logger action to enable logging
    /// </summary>
    /// <param name="logger">Logger action</param>
    /// <returns>Current provider</returns>
    public ILocalesProvider SetLogger(Action<string> logger)
    {
        Logger = logger;
        return this;
    }

    /// <summary>
    /// Register all available resource items
    /// </summary>
    public virtual void RegisterLocaleItems()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Load key value pairs for string translations in a translation dictionary.
    /// If a key is already contained in the translation dictionary it should not be added again.
    /// </summary>
    /// <param name="language">Requested language</param>
    /// <returns>Translation dictionary with key value pairs in.</returns>
    public virtual IDictionary<string, string> LoadLocaleItem(string language)
    {
        throw new NotImplementedException();
    }
}

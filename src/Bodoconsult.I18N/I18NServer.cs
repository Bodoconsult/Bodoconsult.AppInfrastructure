// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.I18N;

/// <summary>
/// Current implementation of <see cref="II18NServer"/> holding translation resources. Instances have to be loaded as singleton in the app
/// </summary>
public class I18NServer : II18NServer
{
    private readonly List<string> _locales = [];
    private Action<string> _logger;
    private string _notFoundSymbol = "?";

    /// <summary>
    /// Fallback locale to use
    /// </summary>
    public string FallBackLocale { get; set; } 

    /// <summary>
    /// All loaded providers
    /// </summary>
    public List<ILocalesProvider> Providers { get; } = [];

    /// <summary>
    /// Current loaded locales
    /// </summary>
    public IReadOnlyList<string> Locales => _locales.ToList();

    /// <summary>
    /// Set a logger action
    /// </summary>
    /// <param name="output">Logger action</param>
    public II18NServer SetLogger(Action<string> output)
    {
        _logger = output;
        return this;
    }

    /// <summary>
    /// Set the symbol to wrap a key when not found. ie: if you set "##", a not found key will
    /// be translated as "##key##". 
    /// The default symbol is "?"
    /// </summary>
    public II18NServer SetNotFoundSymbol(string symbol)
    {
        if (!string.IsNullOrEmpty(symbol))
        {
            _notFoundSymbol = symbol;
        }
        return this;
    }

    /// <summary>
    /// Symbol to show that translation was not found
    /// </summary>
    public string NotFoundSymbol
    {
        get => _notFoundSymbol;
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                _notFoundSymbol = value;
            }
        }
    }

    /// <summary>
    /// Reset all providers
    /// </summary>
    /// <returns>Current I18N instance for FluentAPI</returns>
    public void Reset()
    {
        _locales.Clear();
        Providers.Clear();
    }

    /// <summary>
    /// Reset all providers (fluid version)
    /// </summary>
    /// <returns>Current I18N instance for FluentAPI</returns>
    public II18NServer Reset2()
    {
        Reset();
        return this;
    }

    /// <summary>
    ///  Add a provider as data source for translations
    /// </summary>
    /// <param name="provider">Provider for translation data</param>
    /// <returns>Current I18N instance for FluentAPI</returns>
    public void AddProvider(ILocalesProvider provider)
    {

        if (provider is null)
        {
            throw new I18NException(ErrorMessages.ProviderNull);
        }

        Providers.Add(provider);

        provider.RegisterLocaleItems();

        foreach (var lo in provider.LocaleItems.Keys)
        {
            if (_locales.Any(x => x == lo))
            {
                continue;
            }

            _locales.Add(lo);
        }
    }

    /// <summary>
    ///  Add a provider as data source for translations (fluid version)
    /// </summary>
    /// <param name="provider">Provider for translation data</param>
    /// <returns>Current I18N instance for FluentAPI</returns>
    public II18NServer AddProvider2(ILocalesProvider provider)
    {

        if (provider is null)
        {
            throw new I18NException(ErrorMessages.ProviderNull);
        }

        Providers.Add(provider);

        provider.RegisterLocaleItems();

        foreach (var lo in provider.LocaleItems.Keys)
        {
            if (_locales.Any(x => x == lo))
            {
                continue;
            }

            _locales.Add(lo);
        }

        return this;
    }

    #region Load stuff

    //private void LoadLocale(string locale)
    //{
    //    locale = LocaleHelper.CheckLocale(_locales, locale);

    //    if (!_locales.Contains(locale))
    //    {
    //        throw new I18NException($"Locale '{locale}' is not available", new KeyNotFoundException());
    //    }

    //    _translations.Clear();

    //    // Get the translations from each provider 
    //    foreach (var localesProvider in Providers)
    //    {

    //        // Check if locale or a relative locale exists for the provider
    //        var useLocale = LocaleHelper.CheckLocale(localesProvider.LocaleItems, locale);

    //        if (string.IsNullOrEmpty(useLocale) && !string.IsNullOrEmpty(_fallbackLocale))
    //        {
    //            useLocale = _fallbackLocale;
    //            useLocale = LocaleHelper.CheckLocale(localesProvider.LocaleItems, useLocale);

    //            if (string.IsNullOrEmpty(useLocale))
    //            {
    //                continue;
    //            }
    //        }

    //        if (string.IsNullOrEmpty(useLocale))
    //        {
    //            continue;
    //        }

    //        var localTranslations = localesProvider.LoadLocaleItem(useLocale);

    //        foreach (var localTranslation in localTranslations)
    //        {
    //            if (_translations.Any(x => x.Key == localTranslation.Key))
    //            {
    //                Log($"Provider {localesProvider}: key already exists: {localTranslation.Key}");
    //                continue;
    //            }

    //            _translations.Add(localTranslation.Key, localTranslation.Value);
    //        }

    //    }

    //    LogTranslations();

    //    _locale = locale;

    //    // Update bindings to indexer (useful for MVVM)
    //    NotifyPropertyChanged("Item[]");

    //}


    #endregion

    #region Helpers

    /// <summary>
    /// Get the default locale
    /// </summary>
    /// <returns>Default local as string</returns>
    public string GetDefaultLocale()
    {
        var currentCulture = CultureInfo.CurrentCulture;

        // only available in runtime (not from PCL) on the simulator
        // var threeLetterIsoName = currentCulture.GetType().GetRuntimeProperty("ThreeLetterISOLanguageName").GetValue(currentCulture);
        // var threeLetterWindowsName = currentCulture.GetType().GetRuntimeProperty("ThreeLetterWindowsLanguageName").GetValue(currentCulture);

        var matchingLocale = _locales.FirstOrDefault(x => x.Equals(currentCulture.Name)) ?? _locales.FirstOrDefault(x => x.Equals(currentCulture.TwoLetterISOLanguageName));

        return matchingLocale;

        // ISO 639-1 two-letter code. i.e: "es"
        // || x.Key.Equals(threeLetterIsoName) // ISO 639-2 three-letter code. i.e: "spa"
        // || x.Key.Equals(threeLetterWindowsName)); // "ESP"
    }

    #endregion

    #region Logging

    private void Log(string trace) => _logger?.Invoke($"[I18NServer] {trace}");

    #endregion

    #region Cleanup

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        _locales?.Clear();
        Log("Server disposed");
        _logger = null;
    }

    #endregion
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.I18N.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Bodoconsult.I18N;

/// <summary>
/// Current implementation of an <see cref="II18NClient"/> instance for scoped access to I18N
/// </summary>
public class I18NClient : II18NClient
{
    private readonly Dictionary<string, string> _translations = new();
    private readonly IList<string> _locales;
    private bool _throwWhenKeyNotFound;
    private Action<string> _logger;
    private string _locale;


    private void NotifyPropertyChanged(string info) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="i18NServer">Current I18N server instance</param>
    public I18NClient(II18NServer i18NServer)
    {
        I18NServer = i18NServer;
        _locales = I18NServer.Locales.ToList();
    }

    // PropertyChanged
    /// <summary>Occurs when a property value changes.</summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// The current loaded locale name (can be two letter ISO-code or a culture name like "es-ES")
    /// </summary>
    public string Locale
    {
        get => _locale;
        set
        {
            if (_locale == value)
            {
                Log($"{value} is the current locale. No actions will be taken");
                return;
            }

            LoadLocale(value);

            NotifyPropertyChanged(nameof(Locale));
            NotifyPropertyChanged(nameof(Language));
        }
    }

    /// <summary>
    /// The current loaded Language, if any
    /// </summary>
    public PortableLanguage Language
    {
        get => Languages?.FirstOrDefault(x => x.Locale.Equals(Locale));
        set
        {
            if (Language.Locale == value.Locale)
            {
                Log($"{value.DisplayName} is the current language. No actions will be taken");
                return;
            }

            LoadLocale(value.Locale);

            NotifyPropertyChanged(nameof(Locale));
            NotifyPropertyChanged(nameof(Language));
        }
    }

    /// <summary>
    /// A list of supported languages
    /// </summary>
    public IReadOnlyList<PortableLanguage> Languages => _locales?.Select(x => new PortableLanguage
        {
            Locale = x,
            DisplayName = TranslateOrNull(x) ?? new CultureInfo(x).NativeName.CapitalizeFirstCharacter()
        })
        .ToArray();

    /// <summary>
    /// Use the indexer to translate keys. If you need string formatting, use <code>Translate()</code> instead
    /// </summary>
    public string this[string key] => Translate(key);

    /// <summary>
    /// Current <see cref="II18NServer"/> server
    /// </summary>
    public II18NServer I18NServer { get; }

    /// <summary>
    /// Set a logger action
    /// </summary>
    /// <param name="output">Logger action</param>
    /// <returns>Current <see cref="II18N"/> instance</returns>
    public II18NClient SetLogger(Action<string> output)
    {
        _logger = output;
        return this;
    }

    /// <summary>
    /// Set the locale that will be loaded in case the system language is not supported
    /// </summary>
    public II18NClient SetFallbackLocale(string locale)
    {
        I18NServer.FallBackLocale = locale;
        return this;
    }

    /// <summary>
    /// Set that an exception should be thrown if a key was not found. Intended for testing
    /// </summary>
    /// <param name="enabled">Enable exception throwing on key not found</param>
    /// <returns>Current <see cref="II18N"/> instance</returns>
    public II18NClient SetThrowWhenKeyNotFound(bool enabled)
    {
        _throwWhenKeyNotFound = enabled;
        return this;
    }

    /// <summary>
    /// Initialize the system with the thread language
    /// </summary>
    public II18NClient Init()
    {
        var l = GetDefaultLocale();
        Locale = l;
        return this;
    }

    #region Load stuff

    private void LoadLocale(string locale)
    {
        locale = LocaleHelper.CheckLocale(_locales, locale);

        if (!_locales.Contains(locale))
        {
            throw new I18NException($"Locale '{locale}' is not available", new KeyNotFoundException());
        }

        _translations.Clear();

        // Get the translations from each provider 
        foreach (var localesProvider in I18NServer.Providers.ToArray())
        {

            // Check if locale or a relative locale exists for the provider
            var useLocale = LocaleHelper.CheckLocale(localesProvider.LocaleItems, locale);

            if (string.IsNullOrEmpty(useLocale) && !string.IsNullOrEmpty(I18NServer.FallBackLocale))
            {
                useLocale = I18NServer.FallBackLocale;
                useLocale = LocaleHelper.CheckLocale(localesProvider.LocaleItems, useLocale);

                if (string.IsNullOrEmpty(useLocale))
                {
                    continue;
                }
            }

            if (string.IsNullOrEmpty(useLocale))
            {
                continue;
            }

            var localTranslations = localesProvider.LoadLocaleItem(useLocale);

            foreach (var localTranslation in localTranslations)
            {
                if (_translations.Any(x => x.Key == localTranslation.Key))
                {
                    Log($"Provider {localesProvider}: key already exists: {localTranslation.Key}");
                    continue;
                }

                _translations.Add(localTranslation.Key, localTranslation.Value);
            }

        }

        LogTranslations();

        _locale = locale;

        // Update bindings to indexer (useful for MVVM)
        NotifyPropertyChanged("Item[]");

    }

    #endregion

    #region Translations

    /// <summary>
    /// Translate the given key. If key is not existing an empty string is returned
    /// </summary>
    /// <param name="key">Key to translate</param>
    /// <returns>Translated key as string</returns>
    public string Translate(string key)
    {
        if (_translations.TryGetValue(key, out var translate))
        {
            return translate;
        }

        if (_throwWhenKeyNotFound)
        {
            throw new KeyNotFoundException($"[{nameof(I18N)}] key '{key}' not found in the current language '{_locale}'");
        }

        return $"{I18NServer.NotFoundSymbol}{key}{I18NServer.NotFoundSymbol}";
    }


    /// <summary>
    /// Translate the given key. If key is not existing an empty string is returned
    /// </summary>
    /// <param name="key">Key to translate</param>
    /// <param name="args">Optional args</param>
    /// <returns>Translated key as string</returns>
    public string Translate(string key, params object[] args)
    {
        if (_translations.ContainsKey(key))
        {
            return args.Length == 0
                ? _translations[key]
                : string.Format(_translations[key], args);
        }

        if (_throwWhenKeyNotFound)
        {
            throw new KeyNotFoundException($"[{nameof(I18N)}] key '{key}' not found in the current language '{_locale}'");
        }

        return $"{I18NServer.NotFoundSymbol}{key}{I18NServer.NotFoundSymbol}";
    }

    /// <summary>
    /// Get a translation from a key, formatting the string with the given params, if any. 
    /// It will return null when the translation is not found
    /// </summary>
    public string TranslateOrNull(string key, params object[] args) =>
        _translations.ContainsKey(key)
            ? (args.Length == 0 ? _translations[key] : string.Format(_translations[key], args))
            : null;

    /// <summary>
    /// Convert Enum Type values to a Dictionary&lt;TEnum, string&gt; where the key is the Enum value and the string is the translated value.
    /// </summary>
    public Dictionary<TEnum, string> TranslateEnumToDictionary<TEnum>()
    {
        var type = typeof(TEnum);
        var dic = new Dictionary<TEnum, string>();

        foreach (var value in Enum.GetValues(type))
        {
            var name = Enum.GetName(type, value);
            dic.Add((TEnum)value, Translate($"{type.Name}.{name}"));
        }

        return dic;
    }

    /// <summary>
    /// Convert Enum Type values to a List of translated strings
    /// </summary>
    public IReadOnlyList<string> TranslateEnumToList<TEnum>()
    {
        var type = typeof(TEnum);

        return (from object value in Enum.GetValues(type)
                select Enum.GetName(type, value)
                into name
                select Translate($"{type.Name}.{name}"))
            .ToArray();
    }

    /// <summary>
    /// Converts Enum Type values to a List of <code>Tuple&lt;TEnum, string&gt;</code> where the Item2 (string) is the enum value translation
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    public IReadOnlyList<Tuple<TEnum, string>> TranslateEnumToTupleList<TEnum>()
    {
        var type = typeof(TEnum);
        var list = new List<Tuple<TEnum, string>>();

        foreach (var value in Enum.GetValues(type))
        {
            var name = Enum.GetName(type, value);
            var tuple = new Tuple<TEnum, string>((TEnum)value, Translate($"{type.Name}.{name}"));
            list.Add(tuple);
        }

        return list;
    }

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

    private void LogTranslations()
    {
        Log("========== I18NPortable translations ==========");
        foreach (var item in _translations)
            Log($"{item.Key} = {item.Value}");
        Log("====== I18NPortable end of translations =======");
    }

    private void Log(string trace)
        => _logger?.Invoke($"[{nameof(I18N)}] {trace}");

    #endregion

    #region Cleanup

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        if (PropertyChanged != null)
        {
            foreach (var @delegate in PropertyChanged.GetInvocationList())
            {
                PropertyChanged -= (PropertyChangedEventHandler)@delegate;
            }

            PropertyChanged = null;
        }

        _translations?.Clear();
        _locales?.Clear();
        _locale = null;

        Log("Client disposed");

        _logger = null;
    }

    #endregion
}
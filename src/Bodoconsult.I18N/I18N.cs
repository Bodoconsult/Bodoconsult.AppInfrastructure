// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.I18N.Helpers;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeMadeStatic.Global
// ReSharper disable UnusedMethodReturnValue.Global

namespace Bodoconsult.I18N;

/// <summary>
/// Implementation of <see cref="II18N"/>
/// </summary>
public class I18N : II18N
{
    #region Singleton factory

    // Thread-safe implementation of singleton pattern
    private static Lazy<I18N> _instance;

    /// <summary>
    /// Get a singleton instance of I18N
    /// </summary>
    /// <returns></returns>
    public static I18N Current
    {
        get
        {
            try
            {
                _instance ??= new Lazy<I18N>(() => new I18N());
                return _instance.Value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }

    #endregion


    private readonly Dictionary<string, string> _translations = new();

    /// <summary>
    /// All loaded providers
    /// </summary>
    public List<ILocalesProvider> Providers { get; } = new List<ILocalesProvider>();

    private readonly List<string> _locales = [];
    private bool _throwWhenKeyNotFound;
    private string _notFoundSymbol = "?";
    private string _fallbackLocale;
    private Action<string> _logger;

    private void NotifyPropertyChanged(string info) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));

    /// <summary>
    /// Default ctor not accessible form outside the class
    /// </summary>
    private I18N()
    { }

    // PropertyChanged
    /// <summary>Occurs when a property value changes.</summary>
    public event PropertyChangedEventHandler PropertyChanged;


    /// <summary>
    /// Use the indexer to translate keys. If you need string formatting, use <code>Translate()</code> instead
    /// </summary>
    public string this[string key] => Translate(key);

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

    private string _locale;

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
    /// A list of supported languages
    /// </summary>
    public List<PortableLanguage> Languages => _locales?.Select(x => new PortableLanguage
    {
        Locale = x,
        DisplayName = TranslateOrNull(x) ?? new CultureInfo(x).NativeName.CapitalizeFirstCharacter()
    })
        .ToList();


    #region Fluent API

    /// <summary>
    /// Set the symbol to wrap a key when not found. ie: if you set "##", a not found key will
    /// be translated as "##key##". 
    /// The default symbol is "?"
    /// </summary>
    public II18N SetNotFoundSymbol(string symbol)
    {
        if (!string.IsNullOrEmpty(symbol))
        {
            _notFoundSymbol = symbol;
        }
        return this;
    }

    /// <summary>
    /// Enable I18N logs with an action
    /// </summary>
    /// <param name="output">Action to be invoked as the output of the logger</param>
    public II18N SetLogger(Action<string> output)
    {
        _logger = output;
        return this;
    }

    /// <summary>
    /// Throw an exception whenever a key is not found in the locale file (fail early, fail fast)
    /// </summary>
    public II18N SetThrowWhenKeyNotFound(bool enabled)
    {
        _throwWhenKeyNotFound = enabled;
        return this;
    }

    /// <summary>
    /// Set the locale that will be loaded in case the system language is not supported
    /// </summary>
    public II18N SetFallbackLocale(string locale)
    {
        _fallbackLocale = locale;
        return this;
    }

    /// <summary>
    /// Reset all providers
    /// </summary>
    /// <returns>Current I18N instance for FluentAPI</returns>
    public II18N Reset()
    {
        _locales.Clear();
        _translations?.Clear();
        Providers.Clear();
        return this;
    }

    /// <summary>
    ///  Add a provider as data source for translations
    /// </summary>
    /// <param name="provider">Provider for translation data</param>
    /// <returns>Current I18N instance for FluentAPI</returns>
    public II18N AddProvider(ILocalesProvider provider)
    {

        if (provider == null)
        {
            throw new I18NException(ErrorMessages.ProviderNull);
        }

        Providers.Add(provider);

        provider.RegisterLocalesItems();

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

    //public II18N AddLocaleReader(ILocaleReader reader, string extension)
    //{
    //    if(reader == null)
    //        throw new I18NException(ErrorMessages.ReaderNull);

    //    if(string.IsNullOrEmpty(extension))
    //        throw new I18NException(ErrorMessages.ReaderExtensionNeeded);

    //    if(!extension.StartsWith("."))
    //        throw new I18NException(ErrorMessages.ReaderExtensionStartWithDot);

    //    if(extension.Length < 2)
    //        throw new I18NException(ErrorMessages.ReaderExtensionOneChar);

    //    if(extension.Split('.').Length - 1 > 1)
    //        throw new I18NException(ErrorMessages.ReaderExtensionJustOneDot);

    //    if(_readers.Any(x => x.Item2.Equals(extension)))
    //        throw new I18NException(ErrorMessages.ReaderExtensionTwice);

    //    if(_readers.Any(x => x.Item1 == reader))
    //        throw new I18NException(ErrorMessages.ReaderTwice);

    //    _readers.Add(new Tuple<ILocaleReader, string>(reader, extension));

    //    return this;
    //}

    ///// <summary>
    ///// Call this when your app starts
    ///// ie: <code>I18N.Current.Init(GetType().GetTypeInfo().Assembly);</code>
    ///// </summary>
    ///// <param name="hostAssembly">The assembly that hosts the locale text files</param>
    //public II18N Init(Assembly hostAssembly)
    //{
    //    if (_readers.FirstOrDefault(x => x.Item1 is TextKvpReader && x.Item2 == ".txt") == null)
    //    {
    //        _readers.Insert(0, new Tuple<ILocaleReader, string>(new TextKvpReader(), ".txt"));
    //    }

    //    var knownFileExtensions = _readers.Select(x => x.Item2);

    //    foreach (var provider in _providers)
    //    {
    //        provider.Dispose();
    //    }

    //    _providers.Clear(); // temporal until other providers are implemented

    //    if (_providers.FirstOrDefault(x => x is EmbeddedResourceProvider) == null)
    //    {
    //        var resourcesFolder = _resourcesFolder ?? "Locales";
    //        var defaultProvider = new EmbeddedResourceProvider(hostAssembly, resourcesFolder, knownFileExtensions)
    //            .SetLogger(Log)
    //            .Init();

    //        _providers.Add(defaultProvider);
    //    }

    //    var localeTuples = _providers.First().GetAvailableLocales().ToList();
    //    _locales = localeTuples.Select(x => x.Item1).ToList();
    //    _localeFileExtensionMap = localeTuples.ToDictionary(x => x.Item1, x => x.Item2);

    //    var localeToLoad = GetDefaultLocale();

    //    if (string.IsNullOrEmpty(localeToLoad))
    //    {
    //        if (!string.IsNullOrEmpty(_fallbackLocale) && _locales.Contains(_fallbackLocale))
    //        {
    //            localeToLoad = _fallbackLocale;
    //            Log($"Loading fallback locale: {_fallbackLocale}");
    //        }
    //        else
    //        {
    //            localeToLoad = _locales.ElementAt(0);
    //            Log($"Loading first locale on the list: {localeToLoad}");
    //        }
    //    }
    //    else
    //    {
    //        Log($"Default locale from current culture: {localeToLoad}");
    //    }

    //    LoadLocale(localeToLoad);

    //    NotifyPropertyChanged(nameof(Locale));
    //    NotifyPropertyChanged(nameof(Language));

    //    return this;
    //}

    #endregion

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
        foreach (var localesProvider in Providers)
        {

            // Check if locale or a relative locale exists for the provider
            var useLocale = LocaleHelper.CheckLocale(localesProvider.LocaleItems, locale);

            if (string.IsNullOrEmpty(useLocale) && !string.IsNullOrEmpty(_fallbackLocale))
            {
                useLocale = _fallbackLocale;
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

        return $"{_notFoundSymbol}{key}{_notFoundSymbol}";
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

        return $"{_notFoundSymbol}{key}{_notFoundSymbol}";
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
    public List<string> TranslateEnumToList<TEnum>()
    {
        var type = typeof(TEnum);

        return (from object value in Enum.GetValues(type)
                select Enum.GetName(type, value)
                into name
                select Translate($"{type.Name}.{name}"))
            .ToList();
    }

    /// <summary>
    /// Converts Enum Type values to a List of <code>Tuple&lt;TEnum, string&gt;</code> where the Item2 (string) is the enum value translation
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    public List<Tuple<TEnum, string>> TranslateEnumToTupleList<TEnum>()
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

    /// <summary>
    /// Initialize the system with the thread language
    /// </summary>
    public II18N Init()
    {
        var l = GetDefaultLocale();
        Locale = l;
        return this;
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

        _instance = null;

        Log("Disposed");

        _logger = null;
    }

    #endregion
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.ComponentModel;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.I18N;

/// <summary>
/// Mock class for <see cref="II18N"/>
/// </summary>
public class I18NMock : II18N
{
    #region Singleton factory

    // Thread-safe implementation of singleton pattern
    private static Lazy<I18NMock>? _instance;

    /// <summary>
    /// Get a singleton instance of I18N
    /// </summary>
    /// <returns></returns>
    public static I18NMock Current
    {
        get
        {
            try
            {
                _instance ??= new Lazy<I18NMock>(() => new I18NMock());
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

    /// <summary>
    /// Default ctor not accessible form outside the class
    /// </summary>
    private I18NMock()
    { }

    //private void NotifyPropertyChanged(string info) =>
    //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));

    /// <summary>Occurs when a property value changes.</summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose() { }

    /// <summary>
    /// Indexer to translate string. Intended for usage with MVVM / WPF / Xamarin
    /// </summary>
    /// <param name="key">String key to translate</param>
    /// <returns>Translated string</returns>
    public string this[string key] => throw new NotImplementedException();

    /// <summary>
    /// Current locale language as <see cref="PortableLanguage"/> instance
    /// </summary>
    public PortableLanguage? Language { get; set; }

    /// <summary>
    /// Current locale
    /// </summary>
    public string? Locale { get; set; }

    /// <summary>
    /// Available languages found by the providers. 
    /// </summary>
    public IReadOnlyList<PortableLanguage> Languages { get; set; } = [];

    /// <summary>
    /// Set the not-found-symbol
    /// </summary>
    /// <param name="symbol">Symbol to set as not-found-symbol</param>
    /// <returns></returns>
    public II18N SetNotFoundSymbol(string symbol) => throw new NotImplementedException();

    /// <summary>
    /// Set a logger action
    /// </summary>
    /// <param name="output">Logger action</param>
    /// <returns>Current <see cref="II18N"/> instance</returns>
    public II18N SetLogger(Action<string> output) => throw new NotImplementedException();

    /// <summary>
    /// Set that an exception should be thrown if a key was not found. Intended for testing
    /// </summary>
    /// <param name="enabled">Enable exception throwing on key not found</param>
    /// <returns>Current <see cref="II18N"/> instance</returns>
    public II18N SetThrowWhenKeyNotFound(bool enabled) => throw new NotImplementedException();

    /// <summary>
    /// Set a fallback locale
    /// </summary>
    /// <param name="locale">Requested fallback locale</param>
    /// <returns>Current <see cref="II18N"/> instance</returns>
    public II18N SetFallbackLocale(string locale) => throw new NotImplementedException();

    /// <summary>
    /// Reset all providers
    /// </summary>
    /// <returns>Current I18N instance for FluentAPI</returns>
    public void Reset()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///  Add a provider as data source for translations
    /// </summary>
    /// <param name="provider">Provider for translation data</param>
    /// <returns>Current I18N instance for FluentAPI</returns>
    public void AddProvider(ILocalesProvider provider)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Reset all providers (fluid version)
    /// </summary>
    /// <returns>Current I18N instance for FluentAPI</returns>
    public II18N Reset2()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///  Add a provider as data source for translations (fluid version)
    /// </summary>
    /// <param name="provider">Provider for translation data</param>
    /// <returns>Current I18N instance for FluentAPI</returns>
    public II18N AddProvider2(ILocalesProvider provider)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// All loaded providers
    /// </summary>
    public List<ILocalesProvider> Providers { get; } = [];

    /// <summary>
    /// Get the default locale
    /// </summary>
    /// <returns>Default local as string</returns>
    public string GetDefaultLocale() => throw new NotImplementedException();

    /// <summary>
    /// Translate the given key. If key is not existing an empty string is returned
    /// </summary>
    /// <param name="key">Key to translate</param>
    /// <returns>Translated key as string</returns>
    public string Translate(string key) => "mocked translation";

    /// <summary>
    /// Translate the given key. If key is not existing an empty string is returned
    /// </summary>
    /// <param name="key">Key to translate</param>
    /// <param name="args">Optinal args</param>
    /// <returns>Translated key as string</returns>
    public string Translate(string key, params object[] args) => "mocked translation";

    /// <summary>
    /// Translate the given key. If key is not existing null is returned
    /// </summary>
    /// <param name="key">Key to translate</param>
    /// <param name="args">Optinal args</param>
    /// <returns>Translated key as string or null</returns>
    public string TranslateOrNull(string key, params object[] args) => throw new NotImplementedException();

    /// <summary>
    /// Translate an enum to a dictionary
    /// </summary>
    /// <typeparam name="TEnum">Enum</typeparam>
    /// <returns>Dictionary with translated enum values</returns>
    public Dictionary<TEnum, string> TranslateEnumToDictionary<TEnum>() where TEnum : notnull => throw new NotImplementedException();

    /// <summary>
    /// Translate an enum to a list
    /// </summary>
    /// <typeparam name="TEnum">Enum</typeparam>
    /// <returns>List with translated enum values</returns>
    public List<string> TranslateEnumToList<TEnum>() => throw new NotImplementedException();

    /// <summary>
    /// Translate an enum to a list of <see cref="Tuple"/>>
    /// </summary>
    /// <typeparam name="TEnum">Enum</typeparam>
    /// <returns>List with translated enum values as<see cref="Tuple"/> instances</returns>
    public List<Tuple<TEnum, string>> TranslateEnumToTupleList<TEnum>() => throw new NotImplementedException();

    /// <summary>
    /// Initialize the system with the thread language
    /// </summary>
    public II18N Init()
    {
        // Do nothing
        return this;
    }

    /// <summary>
    /// Unload
    /// </summary>
    /// <exception cref="NotImplementedException">Not implemented</exception>
    public void Unload() => throw new NotImplementedException();
}
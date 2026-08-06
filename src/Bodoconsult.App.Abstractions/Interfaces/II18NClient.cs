// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.ComponentModel;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for I18N client classes providing for scoped access to I18N
/// </summary>
public interface II18NClient: INotifyPropertyChanged, IDisposable
{
    /// <summary>
    /// Indexer to translate string. Intended for usage with MVVM / WPF / Xamarin
    /// </summary>
    /// <param name="key">String key to translate</param>
    /// <returns>Translated string</returns>
    string this[string key] { get; }

    /// <summary>
    /// Current <see cref="II18NServer"/> server
    /// </summary>
    II18NServer I18NServer { get; }

    /// <summary>
    /// Current locale language as <see cref="PortableLanguage"/> instance
    /// </summary>
    PortableLanguage Language { get; set; }

    /// <summary>
    /// Current locale
    /// </summary>
    string Locale { get; set; }

    /// <summary>
    /// Available languages found by the providers. 
    /// </summary>
    List<PortableLanguage> Languages { get; }

    /// <summary>
    /// Set a logger action
    /// </summary>
    /// <param name="output">Logger action</param>
    /// <returns>Current <see cref="II18N"/> instance</returns>
    II18NClient SetLogger(Action<string> output);

    /// <summary>
    /// Set that an exception should be thrown if a key was not found. Intended for testing
    /// </summary>
    /// <param name="enabled">Enable exception throwing on key not found</param>
    /// <returns>Current <see cref="II18N"/> instance</returns>
    II18NClient SetThrowWhenKeyNotFound(bool enabled);

    /// <summary>
    /// Initialize the system with the thread language
    /// </summary>
    II18NClient Init();

    /// <summary>
    /// Translate the given key. If key is not existing an empty string is returned
    /// </summary>
    /// <param name="key">Key to translate</param>
    /// <returns>Translated key as string</returns>
    string Translate(string key);

    /// <summary>
    /// Translate the given key. If key is not existing an empty string is returned
    /// </summary>
    /// <param name="key">Key to translate</param>
    /// <param name="args">Optional args</param>
    /// <returns>Translated key as string</returns>
    string Translate(string key, params object[] args);

    /// <summary>
    /// Translate the given key. If key is not existing null is returned
    /// </summary>
    /// <param name="key">Key to translate</param>
    /// <param name="args">Optinal args</param>
    /// <returns>Translated key as string or null</returns>
    string TranslateOrNull(string key, params object[] args);

    /// <summary>
    /// Translate an enum to a dictionary
    /// </summary>
    /// <typeparam name="TEnum">Enum</typeparam>
    /// <returns>Dictionary with translated enum values</returns>
    Dictionary<TEnum, string> TranslateEnumToDictionary<TEnum>() where TEnum : notnull;

    /// <summary>
    /// Translate an enum to a list
    /// </summary>
    /// <typeparam name="TEnum">Enum</typeparam>
    /// <returns>List with translated enum values</returns>
    List<string> TranslateEnumToList<TEnum>();

    /// <summary>
    /// Translate an enum to a list of <see cref="Tuple"/>>
    /// </summary>
    /// <typeparam name="TEnum">Enum</typeparam>
    /// <returns>List with translated enum values as<see cref="Tuple"/> instances</returns>
    List<Tuple<TEnum, string>> TranslateEnumToTupleList<TEnum>();
}
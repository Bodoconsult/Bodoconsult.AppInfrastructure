// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Reflection;
using Bodoconsult.App.Abstractions.Helpers;
using Bodoconsult.I18N.LocalesProviders;

namespace Bodoconsult.I18N.ResourceProviders;

/// <summary>
/// Loads localization resources from embedded resource in an assemblies folder.
/// This folder should contain only I18N formatted resources.
/// I18N formatted means UTF8 encode text files with the name schema {lanuage code}.txt. Samples: en.txt, de.txt, es.txt, de-DE.txt, en-Us.txt, ...
/// </summary>
/// 
public class I18NEmbeddedResourceProvider: BaseResourceProvider
{

    private readonly Assembly _assembly;

    private readonly string _resourceFolder;


    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="assembly">Assembly to load the locales from</param>
    /// <param name="resourceFolder">Folder relative to app path the locales are stored in. Locales file must be named Culture.XX.xaml with XX being the language identifier</param>
    public I18NEmbeddedResourceProvider(Assembly assembly, string resourceFolder)
    {
        _assembly = assembly;
        _resourceFolder = resourceFolder;

        if (!_resourceFolder.EndsWith('.'))
        {
            _resourceFolder += ".";
        }
    }


    /// <summary>
    /// Register all available resource items
    /// </summary>
    public override void RegisterLocaleItems()
    {
        var len = _resourceFolder.Length;

        var localeResources = _assembly.GetManifestResourceNames().Where(x => x.StartsWith(_resourceFolder, StringComparison.OrdinalIgnoreCase));

        foreach (var locales in localeResources)
        {
            var key = locales.Substring(len, locales.Length - len - 4).ToUpperInvariant();

            var kvp = new KeyValuePair<string, string>(key, locales);

            LocaleItems.Add(kvp);
        }
    }


    /// <summary>
    /// Load key value pairs for string translations in a translation dictionary.
    /// If a key is already contained in the translation dictionary it should not be added again.
    /// </summary>
    /// <param name="language">Requested language</param>
    /// <returns>Translation dictionary with key value pairs in.</returns>
    public override IDictionary<string, string> LoadLocaleItem(string language)
    {
        IDictionary<string, string> translations = new Dictionary<string, string>();

        // Check if language exists
        var success = LocaleItems.TryGetValue(language.ToUpperInvariant(), out var result);

        if (!success || result is null)
        {
            return translations;
        }

        var content = ResourceHelper.GetTextResource(_assembly, result);

        if (content is null)
        {
            return translations;
        }

        var lines = content.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            var s = line.Split('=');

            var p = new KeyValuePair<string, string>(s[0].Trim().ToUpperInvariant(), s[1].Trim());

            translations.Add(p);
        }

        return translations;
    }
}
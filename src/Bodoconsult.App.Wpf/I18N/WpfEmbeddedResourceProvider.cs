// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using Bodoconsult.App.Wpf.Models;
using Bodoconsult.I18N.LocalesProviders;

namespace Bodoconsult.App.Wpf.I18N;

/// <summary>
/// Provider for embedded WPF resources
/// </summary>
public class WpfEmbeddedResourceProvider : BaseResourceProvider
{
    private readonly string _resourceFolder;

    private readonly Assembly _assembly;


    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="assembly">Assembly to load the locales from</param>
    /// <param name="resourceFolder">Folder relative to app path the locales are stored in. Locales file must be named Culture.XX.xaml with XX being the language identifier</param>
    public WpfEmbeddedResourceProvider(Assembly assembly, string resourceFolder)
    {
        _assembly = assembly;
        _resourceFolder = resourceFolder;

        if (_resourceFolder.StartsWith('/'))
        {
            _resourceFolder = _resourceFolder[1..];
        }

        if (_resourceFolder.EndsWith('/'))
        {
            _resourceFolder = _resourceFolder[..^1];
        }

    }

    /// <summary>
    /// Register all available resource items
    /// </summary>
    public override void RegisterLocaleItems()
    {

        var assName = _assembly.GetName().Name;

        var lResourceContainerName = $"{assName}.g";
        var lResourceManager = new ResourceManager(lResourceContainerName, _assembly);

        var lResourceSet = lResourceManager.GetResourceSet(Thread.CurrentThread.CurrentCulture, true, true);

        if (lResourceSet == null)
        {
            return;
        }

        foreach (DictionaryEntry lEesource in lResourceSet)
        {
            if (lEesource.ToString() == null)
            {
                continue;
            }

            Debug.Print(_resourceFolder);

            Debug.Print(lEesource.Key.ToString());

            var key = lEesource.Key.ToString().Split(',')[0]
                .Replace($"{_resourceFolder}/", string.Empty, StringComparison.InvariantCultureIgnoreCase).Replace(".baml", string.Empty, StringComparison.InvariantCultureIgnoreCase);

            Debug.Print(key);

            if (!key.StartsWith("culture.", StringComparison.InvariantCultureIgnoreCase))
            {
                continue;
            }

            var path = $"pack://application:,,,/{assName};component/{_resourceFolder}/{key}.xaml";
            var kvp = new KeyValuePair<string, string>(key.Replace("culture.", string.Empty), path);

            LocaleItems.Add(kvp);
        }
    }


    /// <summary>
    /// Load key value pairs for string translations in a translation dictionary.
    /// If a key is already contained in the translation dictionary it should not be added again.
    /// </summary>
    /// <param name="language">Requested language</param>
    /// <returns>Translation dictionary with key value pairs in.</returns>
    public override IDictionary<string, string> LoadLocaleItem(string language )
    {
        var translations = new Dictionary<string, string>();


        // Check if language exists
        var success = LocaleItems.TryGetValue(language, out var path);

        if (!success)
        {
            return translations;
        }

        string s = System.IO.Packaging.PackUriHelper.UriSchemePack;

        var uri = new Uri(path, UriKind.RelativeOrAbsolute);

        var rd = new SharedResourceDictionary
        {
            Source = uri
        };

        foreach (var key in rd.Keys)
        {
            if (key == null)
            {
                continue;
            }

            var value = rd[key];
            if (value == null)
            {
                continue;
            }

            var keyValue = (string)key;
            translations.Add(keyValue, value.ToString());
        }

        return translations;
    }
}
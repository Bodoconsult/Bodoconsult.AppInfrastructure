// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Bodoconsult.I18N.LocalesProviders;

/// <summary>
/// Loads JSON localization resources from embedded resource in an assemblies subfolder.
/// JSON must contain a List(string, string) object
/// </summary>
/// 
public class JsonListFileLocalesProvider : BaseResourceProvider
{

    //private readonly Assembly _assembly;

    private readonly string _resourceFolder;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="resourceFolder">Full ressource folder path the locales are stored in. Locales file must be named Culture.XX.xaml with XX being the language identifier</param>
    public JsonListFileLocalesProvider(string resourceFolder)
    {
        if (string.IsNullOrEmpty(resourceFolder))
        {
            return;
        }

        _resourceFolder = resourceFolder;
    }

    /// <summary>
    /// Register all available resource items
    /// </summary>
    public override void RegisterLocaleItems()
    {
        if (string.IsNullOrEmpty(_resourceFolder)) return;

        var dir = new DirectoryInfo(_resourceFolder);

        foreach (var locale in dir.GetFiles().Where(x => x.Name.EndsWith(".json", StringComparison.InvariantCultureIgnoreCase)))
        {
            var key = locale.Name.Replace(locale.Extension, string.Empty);

            var kvp = new KeyValuePair<string, string>(key, locale.FullName);

            LocaleItems.Add(kvp);
        }
    }


    /// <summary>
    /// Load key value pairs for string translations in a translation dictionary.
    /// If a key is already contained in the translation dictionary it should not be added again.
    /// </summary>
    /// <param name="language">Requested language</param>
    /// <returns>Translation dictionary with key value pairs in.
    /// </returns>
    public override IDictionary<string, string> LoadLocaleItem(string language)
    {
        var translations = new Dictionary<string, string>();

        // Check if language exists
        var success = LocaleItems.TryGetValue(language, out var result);

        if (!success)
        {
            return translations;
        }

        var json = File.ReadAllText(result);

        var content = JsonSerializer.Deserialize<List<JsonKvp>>(json)
            .ToDictionary(x => x.Key.Trim(), x => x.Value.Trim().UnescapeLineBreaks());

        foreach (var kvp in content)
        {

            if (translations.Any(x => x.Key == kvp.Key))
            {
                continue;
            }

            translations.Add(kvp.Key, kvp.Value);
        }

        return translations;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"{GetType().Name}({_resourceFolder})";
    }
}
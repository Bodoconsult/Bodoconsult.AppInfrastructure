using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Bodoconsult.I18N.Helpers;

namespace Bodoconsult.I18N.LocalesProviders;

/// <summary>
/// Loads JSON localization resources from embedded resource in an assemblies subfolder.
/// JSON must contain a List(string, string) object
/// </summary>
/// 
public class JsonListEmbeddedResourceLocalesProvider : BaseResourceProvider
{

    private readonly Assembly _assembly;

    private readonly string _resourceFolder;


    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="assembly">Current assembly</param>
    /// <param name="resourceFolder">Ressource folder name</param>
    public JsonListEmbeddedResourceLocalesProvider(Assembly assembly, string resourceFolder)
    {
        _assembly = assembly;
        _resourceFolder = resourceFolder;

        if (!_resourceFolder.EndsWith(".")) _resourceFolder += ".";
    }


    /// <summary>
    /// Register all available resource items
    /// </summary>
    public override void RegisterLocaleItems()
    {


        var len = _resourceFolder.Length;

        var localeResources = _assembly.GetManifestResourceNames().Where(x => x.StartsWith(_resourceFolder, StringComparison.InvariantCultureIgnoreCase) &&
                                                                              x.EndsWith(".json", StringComparison.InvariantCultureIgnoreCase));

        foreach (var locales in localeResources)
        {
            var key = locales.Substring(len, locales.Length - len - 5);

            var kvp = new KeyValuePair<string, string>(key, locales);

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

        if (!success) return translations;

        var json = FileHelper.GetTextResource(_assembly, result);

        var content = JsonSerializer.Deserialize<List<JsonKvp>>(json)
            .ToDictionary(x => x.Key.Trim(), x => x.Value.Trim().UnescapeLineBreaks());

        foreach (var kvp in content)
        {

            if (translations.Any(x => x.Key == kvp.Key)) continue;

            translations.Add(kvp.Key, kvp.Value);
        }

        return translations;
    }

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
        return $"{GetType().Name}({_resourceFolder})";
    }




}
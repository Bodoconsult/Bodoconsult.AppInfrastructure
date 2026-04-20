// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bodoconsult.App.Abstractions.Helpers;
using Bodoconsult.I18N.Helpers;

namespace Bodoconsult.I18N.LocalesProviders;

/// <summary>
/// Loads localization resources from embedded resource in an assemblies subfolder.
/// This folder should contain only I18N formatted resources.
/// I18N formatted means UTF8 encode text files with the name schema {lanuage code}.txt. Samples: en.txt, de.txt, es.txt, de-DE.txt, en-Us.txt, ...
/// Content must be formatted as {key} = {value}. Number of spaces before and behind equality char = don't matter. Use # for comments
/// </summary>
/// 
public class CsvEmbeddedResourceLocalesProvider : BaseResourceProvider
{

    private readonly Assembly _assembly;

    private readonly string _resourceFolder;


    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="assembly">Current assembly</param>
    /// <param name="resourceFolder">Ressource folder name</param>
    public CsvEmbeddedResourceLocalesProvider(Assembly assembly, string resourceFolder)
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

        var localeResources = _assembly.GetManifestResourceNames().Where(x => x.StartsWith(_resourceFolder, StringComparison.InvariantCultureIgnoreCase) &&
                                                                              x.EndsWith(".csv", StringComparison.InvariantCultureIgnoreCase)).OrderBy(x => x);

        foreach (var locales in localeResources)
        {
            var key = locales.Substring(len, locales.Length - len - 4);

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
    public override IDictionary<string, string>  LoadLocaleItem(string language )
    {

        IDictionary<string, string> translations = new Dictionary<string, string>();

        // Check if language exists
        var success = LocaleItems.TryGetValue(language, out var result);

        if (!success)
        {
            return translations;
        }

        var content = ResourceHelper.GetTextResource(_assembly, result);

        var lines = content.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

        string key = null;
        string value = null;

        foreach (var line in lines)
        {
            var isEmpty = string.IsNullOrWhiteSpace(line);
            var isComment = !isEmpty && line.Trim().StartsWith('#');
            var isKeyValuePair = !isEmpty && !isComment && line.Contains(';');

            if ((isEmpty || isComment || isKeyValuePair) && key != null && value != null)
            {
                translations.Add(key, value);

                key = null;
                value = null;
            }

            if (isEmpty || isComment)
            {
                continue;
            }

            if (isKeyValuePair)
            {
                var kvp = line.Split([';'], 2);

                key = kvp[0].Trim();
                value = kvp[1].Trim().UnescapeLineBreaks();
            }
            else if (key != null && value != null)
            {
                value = value + Environment.NewLine + line.Trim().UnescapeLineBreaks();
            }
            //if (line.StartsWith("#")) continue;

            //var s = line.Split('=');

            //var p = new KeyValuePair<string, string>(s[0].Trim(), s[1].Trim().UnescapeLineBreaks());


        }

        if (key != null && value != null)
        {
            translations.Add(key, value);
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
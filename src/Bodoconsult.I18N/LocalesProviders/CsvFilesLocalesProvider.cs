// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bodoconsult.I18N.LocalesProviders;

/// <summary>
/// Loads localization resources from embedded resource in an assemblies subfolder.
/// This folder should contain only CSV formatted resources.
/// I18N formatted means UTF8 encode text files with the name schema {lanuage code}.csv. Samples: en.csv, de.csv, es.csv, de-DE.csv, en-Us.csv, ...
/// Content must be formatted as {key};{value}
/// </summary>
/// <remarks>Pay attention if your target OS supports external resource files</remarks>
/// 
public class CsvFileLocalesProvider : BaseResourceProvider
{

    //private readonly Assembly _assembly;

    private readonly string _resourceFolder;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="resourceFolder">Full ressource folder path the locales are stored in. Locales file must be named Culture.XX.xaml with XX being the language identifier</param>
    public CsvFileLocalesProvider(string resourceFolder)
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
        if (string.IsNullOrEmpty(_resourceFolder))
        {
            return;
        }

        var dir = new DirectoryInfo(_resourceFolder);

        foreach (var locale in dir.GetFiles().Where(x => x.Name.EndsWith(".csv", StringComparison.InvariantCultureIgnoreCase)))
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

        var content = File.ReadAllText(result);

        var lines = content.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

        string key = null;
        string value = null;

        foreach (var line in lines)
        {
            var isEmpty = string.IsNullOrWhiteSpace(line);
            var isComment = !isEmpty && line.Trim().StartsWith("#");
            var isKeyValuePair = !isEmpty && !isComment && line.Contains(";");

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
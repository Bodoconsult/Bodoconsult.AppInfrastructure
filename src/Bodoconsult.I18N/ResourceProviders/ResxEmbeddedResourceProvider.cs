// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Bodoconsult.I18N.LocalesProviders;

namespace Bodoconsult.I18N.ResourceProviders;

/// <summary>
/// Reading I18N resources from resx files
/// </summary>
public class ResxEmbeddedResourceProvider : BaseResourceProvider
{
    private readonly Assembly _assembly;
    private readonly string _resourcePath;
    private readonly Dictionary<string, ResourceSet> _cultures = new();

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="assembly">Assembly to load the resources from</param>
    /// <param name="resourcePath">Resource path in the assembly</param>
    public ResxEmbeddedResourceProvider(Assembly assembly, string resourcePath)
    {
        _assembly = assembly;
        _resourcePath = resourcePath;
    }

    /// <summary>
    /// Register all available resource items
    /// </summary>
    public override void RegisterLocaleItems()
    {

        var rm = new ResourceManager(_resourcePath, _assembly);

        var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
        foreach (var culture in cultures)
        {
            var ietf = culture.IetfLanguageTag.ToUpperInvariant();

            if (culture.LCID == 127)
            {
                ietf = "EN-US";
            }

            try
            {
                var rs = rm.GetResourceSet(culture, true, false);

                if (rs is null)
                {
                    continue;
                }

                Debug.Print($"{ietf}{rs.GetString("Test.Message1")}");

                if (_cultures.TryAdd(ietf, rs))
                {
                    var kvp = new KeyValuePair<string, string>(ietf.ToUpperInvariant(), ietf);

                    LocaleItems.Add(kvp);
                }
            }
            catch
            {
                // Do nothing
            }
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
        var translations = new Dictionary<string, string>();

        // Check if language exists
        var success = _cultures.TryGetValue(language.ToUpperInvariant(), out var result);

        if (!success)
        {
            success = _cultures.TryGetValue("EN-US", out result);

            if (!success)
            {
                return translations;
            }
        }

        if (result is null)
        {
            return translations;
        }

        foreach (DictionaryEntry entry in result)
        {
            var key = entry.Key.ToString() ?? "";
            var translation = result.GetString(entry.Key.ToString() ?? "");

            if (translation is null)
            {
                continue;
            }
            translations.Add(key, translation);
        }

        return translations;
    }
}
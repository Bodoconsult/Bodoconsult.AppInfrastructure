// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.I18N.Helpers;

/// <summary>
/// Helper class for locale handling 
/// </summary>
public static class LocaleHelper
{
    /// <summary>
    /// Check if a language directly exists or if there is a similar language.
    /// </summary>
    /// <param name="resourceItems"></param>
    /// <param name="requestedLanguage"></param>
    /// <returns>Language code if a fitting laguage code is existing else null.</returns>
    public static string? CheckLocale(IDictionary<string, string> resourceItems, string requestedLanguage)
    {
        // Check if language exists
        var success = resourceItems.Keys.Contains(requestedLanguage);

        if (success)
        {
            return requestedLanguage;
        }

        var shortLanguage = requestedLanguage[..2];
        success = resourceItems.Keys.Contains(shortLanguage);

        if (success)
        {
            return shortLanguage;
        }

        shortLanguage += "-";

        var item = resourceItems.FirstOrDefault(x => x.Key.StartsWith(shortLanguage));

        // ReSharper disable once UsageOfDefaultStructEquality
        return item.Equals(default(KeyValuePair<string, string>)) ? null : item.Key;
    }

    /// <summary>
    /// Check if a language directly exists or if there is a similar language.
    /// </summary>
    /// <param name="localeItems"></param>
    /// <param name="requestedLanguage"></param>
    /// <returns>Language code if a fitting laguage code is existing else null.</returns>
    public static string? CheckLocale(IList<string> localeItems, string requestedLanguage)
    {
        // Check if language exists
        var success = localeItems.Contains(requestedLanguage);

        if (success)
        {
            return requestedLanguage;
        }

        var shortLanguage = requestedLanguage[..2];
        success = localeItems.Contains(shortLanguage);

        if (success)
        {
            return shortLanguage;
        }

        shortLanguage += "-";

        var item = localeItems.FirstOrDefault(x => x.StartsWith(shortLanguage));
        return item;
    }
}
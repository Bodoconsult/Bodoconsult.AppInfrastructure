// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Reflection;

namespace Bodoconsult.I18N;

/// <summary>
/// Extensions methods for strings for I18N
/// </summary>
public static class I18NStringExtensions
{
    /// <summary>
    /// Get a translation from a key, formatting the string with the given params, if any
    /// </summary>
    public static string? Translate(this string key, params object[] args)
        => I18N.Current.Translate(key, args);

    /// <summary>
    /// Get a translation from a key, formatting the string with the given params, if any. 
    /// It will return null when the translation is not found
    /// </summary>
    public static string? TranslateOrNull(this string key, params object[] args)
        => I18N.Current.TranslateOrNull(key, args);

    /// <summary>
    /// Capitalize the first letter of a string
    /// </summary>
    /// <param name="s">Input string</param>
    /// <returns>Output string with first char capitalized</returns>
    public static string CapitalizeFirstCharacter(this string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return s;
        }

        return s.Length == 1 ? s.ToUpper() : $"{s.Remove(1).ToUpper()}{s[1..]}";
    }

    /// <summary>
    /// Unescape line breaks
    /// </summary>
    /// <param name="str">String to unescape</param>
    /// <returns>Unescaped string</returns>
    public static string UnescapeLineBreaks(this string str)
        => str
            .Replace("\\r\\n", "\\n")
            .Replace("\\n", Environment.NewLine)
            .Replace("\r\n", "\n")
            .Replace("\n", Environment.NewLine);

    /// <summary>
    /// Translates an Enum value.
    /// 
    /// i.e: <code>var dog = Animals.Dog.Translate()</code> will give "perro" if the locale
    /// text file contains a line with "Animal.Dog = perro"
    /// </summary>
    public static string? Translate(this Enum value)
    {
        var fieldInfo = value.GetType().GetRuntimeField(value.ToString());

        if (fieldInfo is null)
        {
            return value.ToString();
        }

        var fieldName = fieldInfo.FieldType.Name;

        return $"{fieldName}.{value}".Translate();
    }
}
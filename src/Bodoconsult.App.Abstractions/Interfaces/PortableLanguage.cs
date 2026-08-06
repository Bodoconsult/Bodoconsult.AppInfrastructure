// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Portable language
/// </summary>
public class PortableLanguage
{
    /// <summary>
    /// Language identifier like en, en-us etc.
    /// </summary>
    public string Locale { get; set; } = string.Empty;

    /// <summary>
    /// Dislay name of the language
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;


    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => DisplayName;
}
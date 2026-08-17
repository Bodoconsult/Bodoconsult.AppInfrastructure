// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Text.Json.Serialization;

namespace Bodoconsult.I18N.LocalesProviders;

/// <summary>
/// I18N JSON key-value-pait
/// </summary>
public class JsonKvp
{
    /// <summary>
    /// Key
    /// </summary>
    [JsonPropertyName("key")]
    public string? Key { get; set; }

    /// <summary>
    /// Value
    /// </summary>
    [JsonPropertyName("value")]
    public string? Value { get; set; }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.Interfaces;

/// <summary>
/// Am object to inject translated messages for a error code identifier
/// </summary>
public class ErrorCodeSetTranslation
{
    /// <summary>
    /// Identifier to recognize the error code set from code
    /// </summary>
    public string? Identifier { get; set; }

    /// <summary>
    /// The error message bound to the identifier
    /// </summary>
    public string? Message { get; set; }
}
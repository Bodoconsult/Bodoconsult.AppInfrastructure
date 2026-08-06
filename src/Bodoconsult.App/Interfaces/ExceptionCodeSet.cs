// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.Interfaces;

/// <summary>
/// A set of information bound to a error identifier like error code and error message
/// </summary>
public class ErrorCodeSet
{

    /// <summary>
    /// Identifier to recognize the error code set from code
    /// </summary>
    public string? Identifier { get; set; }

    /// <summary>
    /// The error code bound to the identifier
    /// </summary>
    public int ErrorCode { get; set; }

    /// <summary>
    /// The error code the identifier is bound to on the app next level
    /// </summary>
    public int NextLevelErrorCode { get; set; }

    /// <summary>
    /// The error message bound to the identifier
    /// </summary>
    public string? Message { get; set; }

}
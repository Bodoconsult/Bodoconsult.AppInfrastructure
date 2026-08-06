// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App;

/// <summary>
/// License missing event args
/// </summary>
public class LicenseMissingArgs : EventArgs
{
    /// <summary>
    /// Current error message
    /// </summary>
    public string? ErrorMessage { get; set; }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.DataProtection;

namespace Bodoconsult.App.DataProtection.ConsoleTools;

/// <summary>
/// Basic class for credentials for accessing websites etc.
/// </summary>
public class BasicCredentials
{
    /// <summary>
    /// Name of the credential set
    /// </summary>
    [DataProtectionKey]
    public string Name { get; set; } = "Credentials";

    /// <summary>
    /// Username
    /// </summary>
    [DataProtectionSecret]
    public string? Username { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    [DataProtectionSecret]
    public string? Password { get; set; }

    /// <summary>
    /// URL
    /// </summary>
    [DataProtectionSecret]
    public string? Url { get; set; }
}
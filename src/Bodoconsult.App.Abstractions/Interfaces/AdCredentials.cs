// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.DataProtection;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Basic class for Active Directory (AD) based computer access etc.
/// </summary>
public class AdCredentials
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
    /// Domain
    /// </summary>
    [DataProtectionSecret]
    public string? Domain { get; set; }
}
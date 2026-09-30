// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Security;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for credentials used by <see cref="ICredentialManager"/> implementations
/// </summary>
public interface ICredentials
{
    /// <summary>
    /// Unique target name
    /// </summary>
    string TargetName { get; }

    /// <summary>
    /// Username
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    public SecureString? Password { get; set; }

    /// <summary>
    /// Type of the credential
    /// </summary>
    CredentialType Type { get; }
}
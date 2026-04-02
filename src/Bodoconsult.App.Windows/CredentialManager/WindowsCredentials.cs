// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

// Copyright (c) 2020 Widauer Patrick. All rights reserved.

// https://github.com/a-ctor/WindowsCredentialManager

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Windows.CredentialManager;

/// <summary>
/// Current impl of <see cref="ICredentials"/> on current Win32 systems
/// </summary>
public class WindowsCredentials : ICredentials
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="targetName">Unique target name</param>
    /// <param name="type">Type of credential</param>
    /// <exception cref="ArgumentNullException">Throws if targetname if null</exception>
    public WindowsCredentials(string targetName, CredentialType type)
    {
        TargetName = targetName ?? throw new ArgumentNullException(nameof(targetName));

        if (targetName == string.Empty)
        {
            throw new Win32Exception("Empty credential name is not allowed");
        }

        Type = type;
        LastModified = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Unique target name
    /// </summary>
    public string TargetName { get; }

    /// <summary>
    /// Username
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    public SecureString Password { get; set; }

    /// <summary>
    /// Type of the credential
    /// </summary>
    public CredentialType Type { get; }

    /// <summary>
    /// Comment
    /// </summary>
    public string Comment { get; set; }

    /// <summary>
    /// Last modification data
    /// </summary>
    public DateTimeOffset LastModified { get; set; }

    /// <summary>
    /// Persistence
    /// </summary>
    public CredentialPersistence Persistence { get; set; } = CredentialPersistence.Session;

    /// <summary>
    /// Attributes
    /// </summary>
    public List<CredentialAttribute> Attributes { get; set; } = [];

    /// <summary>
    /// Target alias
    /// </summary>
    public string TargetAlias { get; set; }


}
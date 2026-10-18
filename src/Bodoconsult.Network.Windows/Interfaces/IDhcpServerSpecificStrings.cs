// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Network.Windows.Interfaces;

/// <summary>
/// Intrface for specific strings
/// </summary>
public interface IDhcpServerSpecificStrings
{
    /// <summary>
    /// Default user class name
    /// </summary>
    string DefaultUserClassName { get; }

    /// <summary>
    /// Default vendor class name
    /// </summary>
    string DefaultVendorClassName { get; }
}
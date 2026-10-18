// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Network.Windows.Dhcp.Options;

namespace Bodoconsult.Network.Windows.Interfaces;

/// <summary>
/// Interface for DHCP server option elements
/// </summary>
public interface IDhcpServerOptionElement : IEquatable<IDhcpServerOptionElement>
{
    /// <summary>
    /// Type of the option element
    /// </summary>
    DhcpServerOptionElementType Type { get; }

    /// <summary>
    /// Option value
    /// </summary>
    object Value { get; }

    /// <summary>
    /// Option value formatted
    /// </summary>
    string ValueFormatted { get; }
}
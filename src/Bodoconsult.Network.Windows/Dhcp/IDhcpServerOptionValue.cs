// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Network.Windows.Dhcp;

/// <summary>
/// Interface for option value implementations
/// </summary>
public interface IDhcpServerOptionValue
{
    /// <summary>
    /// Class name
    /// </summary>
    string ClassName { get; }

    /// <summary>
    /// Option
    /// </summary>
    IDhcpServerOption Option { get; }

    /// <summary>
    /// Option ID
    /// </summary>
    int OptionId { get; }

    /// <summary>
    /// The associated DHCP Server
    /// </summary>
    IDhcpServer Server { get; }

    /// <summary>
    /// Option values
    /// </summary>
    IEnumerable<IDhcpServerOptionElement> Values { get; }

    /// <summary>
    /// Vendor name
    /// </summary>
    string VendorName { get; }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Network.Windows.Dhcp;

namespace Bodoconsult.Network.Windows.Interfaces;

/// <summary>
/// Interface for binding elements
/// </summary>
public interface IDhcpServerBindingElement
{
    /// <summary>
    /// DHCP_IP_ADDRESS value that specifies the IP address assigned to the ethernet adapter of the DHCP server.
    /// </summary>
    DhcpServerIpAddress AdapterPrimaryIpAddress { get; }

    /// <summary>
    /// DHCP_IP_ADDRESS value that specifies the subnet IP mask used by this ethernet adapter.
    /// </summary>
    DhcpServerIpMask AdapterSubnetAddress { get; }

    /// <summary>
    /// The binding specified in this structure cannot be modified.
    /// </summary>
    bool CantModify { get; }

    /// <summary>
    /// Unicode string that specifies the name assigned to this network interface device.
    /// </summary>
    string InterfaceDescription { get; }

    /// <summary>
    /// The interface ID as GUID
    /// </summary>
    Guid InterfaceGuidId { get; }

    /// <summary>
    /// Specifies the network interface device ID.
    /// </summary>
    byte[] InterfaceId { get; }

    /// <summary>
    /// Specifies whether or not this binding is set on the DHCP server. If TRUE, the binding is set; if FALSE, it is not.
    /// </summary>
    bool IsBound { get; }

    /// <summary>
    /// The associated DHCP Server
    /// </summary>
    IDhcpServer Server { get; }
}
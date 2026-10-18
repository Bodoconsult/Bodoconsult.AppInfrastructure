// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Network.Windows.Dhcp;
using Bodoconsult.Network.Windows.Dhcp.Client;

namespace Bodoconsult.Network.Windows.Interfaces;

/// <summary>
/// DHCP reservation
/// </summary>
public interface IDhcpServerScopeReservation
{
    /// <summary>
    /// IP address
    /// </summary>
    DhcpServerIpAddress Address { get; }

    /// <summary>
    /// Allowed client types
    /// </summary>
    DhcpServerClientTypes AllowedClientTypes { get; }

    /// <summary>
    /// DHCP client
    /// </summary>
    IDhcpServerClient Client { get; }

    /// <summary>
    /// Hardware address
    /// </summary>
    DhcpServerHardwareAddress HardwareAddress { get; }

    /// <summary>
    /// DHCP options
    /// </summary>
    IDhcpServerScopeReservationOptionValueCollection Options { get; }

    /// <summary>
    /// The associated DHCP scope
    /// </summary>
    IDhcpServerScope Scope { get; }

    /// <summary>
    /// The associated DHCP server
    /// </summary>
    IDhcpServer Server { get; }

    /// <summary>
    /// Delete the scope reservation
    /// </summary>
    void Delete();
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.Network.Windows.Dhcp;
using Bodoconsult.Network.Windows.Dhcp.Client;

namespace Bodoconsult.Network.Windows.Interfaces;

/// <summary>
/// Interface for collections of DHCP reservations
/// </summary>
public interface IDhcpServerScopeReservationCollection : IEnumerable<IDhcpServerScopeReservation>
{
    /// <summary>
    /// The associated DHCP scope
    /// </summary>
    IDhcpServerScope Scope { get; }

    /// <summary>
    /// The associated DHCP server
    /// </summary>
    IDhcpServer Server { get; }

    /// <summary>
    /// Add a reservation
    /// </summary>
    /// <param name="client">DHCP client</param>
    /// <returns>New DHCP reservation</returns>
    IDhcpServerScopeReservation AddReservation(IDhcpServerClient client);

    /// <summary>
    /// Add a reservation
    /// </summary>
    /// <param name="address">IP address</param>
    /// <param name="hardwareAddress">Hardware address</param>
    /// <returns>New DHCP reservation</returns>
    IDhcpServerScopeReservation AddReservation(DhcpServerIpAddress address, DhcpServerHardwareAddress hardwareAddress);

    /// <summary>
    /// Add a reservation
    /// </summary>
    /// <param name="address">IP address</param>
    /// <param name="hardwareAddress">Hardware address</param>
    /// <param name="allowedClientTypes">Allowed client types</param>
    /// <returns>New DHCP reservation</returns>
    IDhcpServerScopeReservation AddReservation(DhcpServerIpAddress address, DhcpServerHardwareAddress hardwareAddress, DhcpServerClientTypes allowedClientTypes);
}
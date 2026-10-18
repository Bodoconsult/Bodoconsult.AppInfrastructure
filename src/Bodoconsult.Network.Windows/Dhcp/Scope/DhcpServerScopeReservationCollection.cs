// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.Network.Windows.Dhcp.Client;
using Bodoconsult.Network.Windows.Interfaces;
using System.Collections;

namespace Bodoconsult.Network.Windows.Dhcp.Scope;

/// <summary>
/// Collection of DHCP reservations
/// </summary>
public class DhcpServerScopeReservationCollection : IDhcpServerScopeReservationCollection
{
    /// <summary>
    /// The associated DHCP server
    /// </summary>
    public DhcpServer Server { get; }

    /// <summary>
    /// The associated DHCP server
    /// </summary>
    IDhcpServer IDhcpServerScopeReservationCollection.Server => Server;

    /// <summary>
    /// The associated DHCP scope
    /// </summary>
    public DhcpServerScope Scope { get; }

    /// <summary>
    /// The associated DHCP scope
    /// </summary>
    IDhcpServerScope IDhcpServerScopeReservationCollection.Scope => Scope;

    internal DhcpServerScopeReservationCollection(DhcpServerScope scope)
    {
        Server = scope.Server;
        Scope = scope;
    }

    /// <summary>
    /// Enumerates a list of reservations associated with the DHCP Scope
    /// </summary>
    public IEnumerator<IDhcpServerScopeReservation> GetEnumerator()
        => DhcpServerScopeReservation.GetReservations(Scope).GetEnumerator();

    /// <summary>
    /// Enumerates a list of reservations associated with the DHCP Scope
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    /// <summary>
    /// Creates a DHCP scope reservation from a client
    /// </summary>
    /// <param name="client">A DHCP client to convert to a reservation</param>
    /// <returns>The scope reservation</returns>
    public IDhcpServerScopeReservation AddReservation(IDhcpServerClient client)
    {
        if (!Scope.IpRange.Contains(client.IpAddress))
            throw new ArgumentOutOfRangeException(nameof(client), "The client address is not within the IP range of the scope");

        return DhcpServerScopeReservation.CreateReservation(Scope, client.IpAddress, client.HardwareAddress);
    }
    /// <summary>
    /// Creates a DHCP scope reservation
    /// </summary>
    /// <param name="address">IP Address to reserve</param>
    /// <param name="hardwareAddress">Hardware address (MAC address) of client associated with this reservation</param>
    /// <returns>The scope reservation</returns>
    public IDhcpServerScopeReservation AddReservation(DhcpServerIpAddress address, DhcpServerHardwareAddress hardwareAddress)
        => DhcpServerScopeReservation.CreateReservation(Scope, address, hardwareAddress);
    /// <summary>
    /// Creates a DHCP scope reservation
    /// </summary>
    /// <param name="address">IP Address to reserve</param>
    /// <param name="hardwareAddress">Hardware address (MAC address) of client associated with this reservation</param>
    /// <param name="allowedClientTypes">Protocols this reservation supports</param>
    /// <returns>The scope reservation</returns>
    public IDhcpServerScopeReservation AddReservation(DhcpServerIpAddress address, DhcpServerHardwareAddress hardwareAddress, DhcpServerClientTypes allowedClientTypes)
        => DhcpServerScopeReservation.CreateReservation(Scope, address, hardwareAddress, allowedClientTypes);
}
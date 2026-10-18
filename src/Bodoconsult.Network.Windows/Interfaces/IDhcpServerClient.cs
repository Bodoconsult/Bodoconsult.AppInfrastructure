// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.Network.Windows.Dhcp;
using Bodoconsult.Network.Windows.Dhcp.Client;

namespace Bodoconsult.Network.Windows.Interfaces;

/// <summary>
/// DHCP server client
/// </summary>
public interface IDhcpServerClient
{
    /// <summary>
    /// Address state
    /// </summary>
    DhcpServerClientAddressStates AddressState { get; }

    /// <summary>
    /// Comment
    /// </summary>
    string Comment { get; set; }

    /// <summary>
    /// DNS state
    /// </summary>
    DhcpServerClientDnsStates DnsState { get; }

    /// <summary>
    /// Hardware address
    /// </summary>
    DhcpServerHardwareAddress HardwareAddress { get; }

    /// <summary>
    /// IP address
    /// </summary>
    DhcpServerIpAddress IpAddress { get; }

    /// <summary>
    /// Lease expired date
    /// </summary>
    bool LeaseExpired { get; }

    /// <summary>
    /// Lease expires date
    /// </summary>
    DateTime LeaseExpires { get; }

    /// <summary>
    /// Lease expires date in local time
    /// </summary>
    DateTime LeaseExpiresLocal { get; }

    /// <summary>
    /// Lease expires date UTC
    /// </summary>
    DateTime LeaseExpiresUtc { get; }

    /// <summary>
    /// Lease has expiry date
    /// </summary>
    bool LeaseHasExpiry { get; }

    /// <summary>
    /// DHCP client name
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// Name protection state
    /// </summary>
    DhcpServerClientNameProtectionStates NameProtectionState { get; }

    /// <summary>
    /// Probation ends date
    /// </summary>
    DateTime ProbationEnds { get; }

    /// <summary>
    /// Capable for quarantine
    /// </summary>
    bool QuarantineCapable { get; }

    /// <summary>
    /// Quarantine statuses
    /// </summary>
    DhcpServerClientQuarantineStates QuarantineStatus { get; }

    /// <summary>
    /// The associated DHCP scope
    /// </summary>
    IDhcpServerScope Scope { get; }

    /// <summary>
    /// The associated DHCP Server
    /// </summary>
    IDhcpServer Server { get; }

    /// <summary>
    /// Subnet mask
    /// </summary>
    DhcpServerIpMask SubnetMask { get; }

    /// <summary>
    /// Client type
    /// </summary>
    DhcpServerClientTypes Type { get; }

    /// <summary>
    /// Convert into a reservation
    /// </summary>
    /// <returns>Scope reservation</returns>
    IDhcpServerScopeReservation ConvertToReservation();

    /// <summary>
    /// Delete the client
    /// </summary>
    void Delete();

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string that represents the current object.</returns>
    string ToString();
}
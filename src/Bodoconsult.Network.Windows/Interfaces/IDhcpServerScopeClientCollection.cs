// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.Network.Windows.Dhcp;
using Bodoconsult.Network.Windows.Dhcp.Client;

namespace Bodoconsult.Network.Windows.Interfaces;

/// <summary>
/// Interface for scope client collections
/// </summary>
public interface IDhcpServerScopeClientCollection : IEnumerable<IDhcpServerClient>
{
    /// <summary>
    /// The associated DHCP scope
    /// </summary>
    IDhcpServerScope Scope { get; }

    /// <summary>
    /// The associated DHCP Server
    /// </summary>
    IDhcpServer Server { get; }

    /// <summary>
    /// Add a DHCP client
    /// </summary>
    /// <param name="address">IP address</param>
    /// <param name="hardwareAddress">Hardware address</param>
    /// <returns>DHCP client instance</returns>
    IDhcpServerClient AddClient(DhcpServerIpAddress address, DhcpServerHardwareAddress hardwareAddress);

    /// <summary>
    /// Add a DHCP client
    /// </summary>
    /// <param name="address">IP address</param>
    /// <param name="hardwareAddress">Hardware address</param>
    /// <param name="name">Name</param>
    /// <returns>DHCP client instance</returns>
    IDhcpServerClient AddClient(DhcpServerIpAddress address, DhcpServerHardwareAddress hardwareAddress, string name);

    /// <summary>
    /// Add a DHCP client
    /// </summary>
    /// <param name="address">IP address</param>
    /// <param name="hardwareAddress">Hardware address</param>
    /// <param name="name">Name</param>
    /// <param name="comment">Comment</param>
    /// <returns>DHCP client instance</returns>
    IDhcpServerClient AddClient(DhcpServerIpAddress address, DhcpServerHardwareAddress hardwareAddress, string name, string comment);

    /// <summary>
    /// Add a DHCP client
    /// </summary>
    /// <param name="address">IP address</param>
    /// <param name="hardwareAddress">Hardware address</param>
    /// <param name="name">Name</param>
    /// <param name="comment">Comment</param>
    /// <param name="leaseExpires">Lease expires date</param>
    /// <returns>DHCP client instance</returns>
    IDhcpServerClient AddClient(DhcpServerIpAddress address, DhcpServerHardwareAddress hardwareAddress, string name, string comment, DateTime leaseExpires);

    /// <summary>
    /// Add a DHCP client
    /// </summary>
    /// <param name="address">IP address</param>
    /// <param name="hardwareAddress">Hardware address</param>
    /// <param name="name">Name</param>
    /// <param name="comment">Comment</param>
    /// <param name="leaseExpires">Lease expires date</param>
    /// <param name="ownerHost">Owner host</param>
    /// <returns>DHCP client instance</returns>
    IDhcpServerClient AddClient(DhcpServerIpAddress address, DhcpServerHardwareAddress hardwareAddress, string name, string comment, DateTime leaseExpires, IDhcpServerHost ownerHost);
    
    /// <summary>
    /// Add a DHCP client
    /// </summary>
    /// <param name="address">IP address</param>
    /// <param name="hardwareAddress">Hardware address</param>
    /// <param name="name">Name</param>
    /// <param name="comment">Comment</param>
    /// <param name="leaseExpires">Lease expires date</param>
    /// <param name="ownerHost">Owner host</param>
    /// <param name="clientType">Client type</param>
    /// <param name="addressState">Address state</param>
    /// <param name="quarantineStatus">Quarantine state</param>
    /// <param name="probationEnds">Probation ends date</param>
    /// <param name="quarantineCapable">Capable for quarantine?</param>
    /// <returns>DHCP client instance</returns>
    IDhcpServerClient AddClient(DhcpServerIpAddress address, DhcpServerHardwareAddress hardwareAddress, string name, string comment, DateTime leaseExpires, IDhcpServerHost ownerHost, DhcpServerClientTypes clientType, DhcpServerClientAddressStates addressState, DhcpServerClientQuarantineStates quarantineStatus, DateTime probationEnds, bool quarantineCapable);

    /// <summary>
    /// Remove a DHCP client
    /// </summary>
    /// <param name="client">DHCP client instance</param>
    void RemoveClient(IDhcpServerClient client);
}
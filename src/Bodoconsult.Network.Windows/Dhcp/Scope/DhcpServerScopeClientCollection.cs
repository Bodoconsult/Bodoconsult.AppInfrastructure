// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.Network.Windows.Dhcp.Client;
using Bodoconsult.Network.Windows.Interfaces;
using System.Collections;

namespace Bodoconsult.Network.Windows.Dhcp.Scope;

/// <summary>
/// Scope client collection
/// Scope client collection
/// </summary>
public class DhcpServerScopeClientCollection : IDhcpServerScopeClientCollection
{
    /// <summary>
    /// The associated DHCP server
    /// </summary>
    public DhcpServer Server { get; }

    /// <summary>
    /// The associated DHCP server
    /// </summary>
    IDhcpServer IDhcpServerScopeClientCollection.Server => Server;

    /// <summary>
    /// The associated DHCP scope
    /// </summary>
    public DhcpServerScope Scope { get; }

    /// <summary>
    /// The associated DHCP scope
    /// </summary>
    IDhcpServerScope IDhcpServerScopeClientCollection.Scope => Scope;

    internal DhcpServerScopeClientCollection(DhcpServerScope scope)
    {
        Server = scope.Server;
        Scope = scope;
    }

    /// <summary>Returns an enumerator that iterates through the collection.</summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    public IEnumerator<IDhcpServerClient> GetEnumerator()
        => DhcpServerClient.GetScopeClients(Scope).GetEnumerator();

    /// <summary>Returns an enumerator that iterates through the collection.</summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    /// <summary>
    /// Remove a DHCP client
    /// </summary>
    /// <param name="client">DHCP client instance</param>
    public void RemoveClient(IDhcpServerClient client)
        => client.Delete();

    /// <summary>
    /// Add a DHCP client
    /// </summary>
    /// <param name="address">IP address</param>
    /// <param name="hardwareAddress">Hardware address</param>
    /// <returns>DHCP client instance</returns>
    public IDhcpServerClient AddClient(DhcpServerIpAddress address, DhcpServerHardwareAddress hardwareAddress)
        => DhcpServerClient.CreateClient(Scope, address, hardwareAddress);

    /// <summary>
    /// Add a DHCP client
    /// </summary>
    /// <param name="address">IP address</param>
    /// <param name="hardwareAddress">Hardware address</param>
    /// <param name="name">Name</param>
    /// <returns>DHCP client instance</returns>
    public IDhcpServerClient AddClient(DhcpServerIpAddress address, DhcpServerHardwareAddress hardwareAddress, string name)
        => DhcpServerClient.CreateClient(Scope, address, hardwareAddress, name);

    /// <summary>
    /// Add a DHCP client
    /// </summary>
    /// <param name="address">IP address</param>
    /// <param name="hardwareAddress">Hardware address</param>
    /// <param name="name">Name</param>
    /// <param name="comment">Comment</param>
    /// <returns>DHCP client instance</returns>
    public IDhcpServerClient AddClient(DhcpServerIpAddress address, DhcpServerHardwareAddress hardwareAddress, string name, string comment)
        => DhcpServerClient.CreateClient(Scope, address, hardwareAddress, name, comment);

    /// <summary>
    /// Add a DHCP client
    /// </summary>
    /// <param name="address">IP address</param>
    /// <param name="hardwareAddress">Hardware address</param>
    /// <param name="name">Name</param>
    /// <param name="comment">Comment</param>
    /// <param name="leaseExpires">Lease expires date</param>
    /// <returns>DHCP client instance</returns>
    public IDhcpServerClient AddClient(DhcpServerIpAddress address, DhcpServerHardwareAddress hardwareAddress, string name, string comment, DateTime leaseExpires)
        => DhcpServerClient.CreateClient(Scope, address, hardwareAddress, name, comment, leaseExpires);

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
    public IDhcpServerClient AddClient(DhcpServerIpAddress address, DhcpServerHardwareAddress hardwareAddress, string name, string comment, DateTime leaseExpires, IDhcpServerHost ownerHost)
        => DhcpServerClient.CreateClient(Scope, address, hardwareAddress, name, comment, leaseExpires, (DhcpServerHost)ownerHost);

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
    public IDhcpServerClient AddClient(DhcpServerIpAddress address, DhcpServerHardwareAddress hardwareAddress, string name, string comment, DateTime leaseExpires, IDhcpServerHost ownerHost, DhcpServerClientTypes clientType, DhcpServerClientAddressStates addressState, DhcpServerClientQuarantineStates quarantineStatus, DateTime probationEnds, bool quarantineCapable)
        => DhcpServerClient.CreateClient(Scope, address, hardwareAddress, name, comment, leaseExpires, (DhcpServerHost)ownerHost, clientType, addressState, quarantineStatus, probationEnds, quarantineCapable);

}
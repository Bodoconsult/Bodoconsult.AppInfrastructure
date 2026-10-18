// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Network.Windows.Interfaces;
using System.Collections;

namespace Bodoconsult.Network.Windows.Dhcp.Client;

/// <summary>
///  DHCP client collection
/// </summary>
public class DhcpServerClientCollection : IDhcpServerClientCollection
{
    /// <summary>
    /// The associated DHCP server
    /// </summary>
    public DhcpServer Server { get; }

    /// <summary>
    /// The associated DHCP server
    /// </summary>
    IDhcpServer IDhcpServerClientCollection.Server => Server;

    internal DhcpServerClientCollection(DhcpServer server)
    {
        Server = server;
    }

    /// <summary>Returns an enumerator that iterates through the collection.</summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    public IEnumerator<IDhcpServerClient> GetEnumerator()
        => DhcpServerClient.GetClients(Server).GetEnumerator();

    /// <summary>Returns an enumerator that iterates through a collection.</summary>
    /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    /// <summary>
    /// Remove a DHCP client
    /// </summary>
    /// <param name="client">Client to remove</param>
    public void RemoveClient(IDhcpServerClient client)
        => client.Delete();
}
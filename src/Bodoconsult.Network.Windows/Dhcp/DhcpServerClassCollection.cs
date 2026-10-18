// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Network.Windows.Interfaces;
using System.Collections;

namespace Bodoconsult.Network.Windows.Dhcp;

/// <summary>
/// A collection of server class instances
/// </summary>
public class DhcpServerClassCollection : IDhcpServerClassCollection
{
    /// <summary>
    /// The associated DHCP Server
    /// </summary>
    public DhcpServer Server { get; }

    /// <summary>
    /// The associated DHCP Server
    /// </summary>
    IDhcpServer IDhcpServerClassCollection.Server => Server;

    internal DhcpServerClassCollection(DhcpServer server)
    {
        Server = server;
    }

    /// <summary>
    /// Queries the DHCP Server for the specified User or Vendor Class
    /// </summary>
    /// <param name="name">The name of the User or Vendor Class to retrieve</param>
    /// <returns>A <see cref="DhcpServerClass"/>.</returns>
    public IDhcpServerClass GetClass(string name)
        => DhcpServerClass.GetClass(Server, name);

    /// <summary>Returns an enumerator that iterates through the collection.</summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    public IEnumerator<IDhcpServerClass> GetEnumerator()
        => DhcpServerClass.GetClasses(Server).GetEnumerator();

    /// <summary>Returns an enumerator that iterates through the collection.</summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
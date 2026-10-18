// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Network.Windows.Interfaces;
using System.Collections;

namespace Bodoconsult.Network.Windows.Dhcp;

/// <summary>
/// Current implementation of <see cref="IDhcpServerBindingElementCollection"/> for a collection of binding elements
/// </summary>
public class DhcpServerBindingElementCollection : IDhcpServerBindingElementCollection
{
    /// <summary>
    /// The associated DHCP Server
    /// </summary>
    public DhcpServer Server { get; }

    /// <summary>
    /// The associated DHCP Server
    /// </summary>
    IDhcpServer IDhcpServerBindingElementCollection.Server => Server;

    internal DhcpServerBindingElementCollection(DhcpServer server)
    {
        Server = server;
    }

    /// <summary>Returns an enumerator that iterates through the collection.</summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    public IEnumerator<IDhcpServerBindingElement> GetEnumerator()
        => DhcpServerBindingElement.GetBindingInfo(Server).GetEnumerator();

    /// <summary>Returns an enumerator that iterates through the collection.</summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
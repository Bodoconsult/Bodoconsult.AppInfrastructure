// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Network.Windows.Interfaces;

/// <summary>
/// Interface for DHCP client collections
/// </summary>
public interface IDhcpServerClientCollection : IEnumerable<IDhcpServerClient>
{
    /// <summary>
    /// The associated DHCP server
    /// </summary>
    IDhcpServer Server { get; }

    /// <summary>
    /// Remove a DHCP client
    /// </summary>
    /// <param name="client">Client to remove</param>
    void RemoveClient(IDhcpServerClient client);
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Network.Windows.Interfaces;

/// <summary>
/// Interface for collections of server class instances
/// </summary>
public interface IDhcpServerClassCollection : IEnumerable<IDhcpServerClass>
{
    /// <summary>
    /// The associated DHCP Server
    /// </summary>
    IDhcpServer Server { get; }

    /// <summary>
    /// Queries the DHCP Server for the specified User or Vendor Class
    /// </summary>
    /// <param name="name">The name of the User or Vendor Class to retrieve</param>
    /// <returns>An <see cref="IDhcpServerClass"/> instance.</returns>
    IDhcpServerClass GetClass(string name);
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Network.Windows.Interfaces;

/// <summary>
/// Interface for collections of binding elements
/// </summary>
public interface IDhcpServerBindingElementCollection : IEnumerable<IDhcpServerBindingElement>
{
    /// <summary>
    /// The associated DHCP Server
    /// </summary>
    IDhcpServer Server { get; }
}
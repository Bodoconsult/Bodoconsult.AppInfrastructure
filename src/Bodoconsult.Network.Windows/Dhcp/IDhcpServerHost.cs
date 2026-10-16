// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Network.Windows.Dhcp;

/// <summary>
/// Interface for DHCP server hosts
/// </summary>
public interface IDhcpServerHost
{
    /// <summary>
    /// IP address
    /// </summary>
    DhcpServerIpAddress Address { get; }

    /// <summary>
    /// NetBios name
    /// </summary>
    string NetBiosName { get; }
    
    /// <summary>
    /// Server name
    /// </summary>
    string ServerName { get; }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Network.Windows.Dhcp;

namespace Bodoconsult.Network.Windows.Interfaces;

/// <summary>
/// Collection with exclude IP addresses ranges
/// </summary>
public interface IDhcpServerScopeExcludedIpRangeCollection : IEnumerable<DhcpServerIpRange>
{
    /// <summary>
    /// The associated DHCP scope
    /// </summary>
    IDhcpServerScope Scope { get; }

    /// <summary>
    /// The associated DHCP server
    /// </summary>
    IDhcpServer Server { get; }

    /// <summary>
    /// Add a range with excluded IP addresses
    /// </summary>
    /// <param name="startAddress">Start address</param>
    /// <param name="endAddress">End address</param>
    void AddExcludedIpRange(DhcpServerIpAddress startAddress, DhcpServerIpAddress endAddress);
    
    /// <summary>
    /// Add a range with excluded IP addresses
    /// </summary>
    /// <param name="address">IP address</param>
    /// <param name="mask">Mask</param>
    void AddExcludedIpRange(DhcpServerIpAddress address, DhcpServerIpMask mask);

    /// <summary>
    /// Add a range with excluded IP addresses
    /// </summary>
    /// <param name="range">Range with excluded IP addresses</param>
    void AddExcludedIpRange(DhcpServerIpRange range);

    /// <summary>
    /// Add a range with excluded IP addresses
    /// </summary>
    /// <param name="cidrRange">CIDR-Range with excluded IP addresses as string</param>
    void AddExcludedIpRange(string cidrRange);

    /// <summary>
    /// Remove a range with excluded IP addresses
    /// </summary>
    /// <param name="range">Range with excluded IP addresses</param>
    void RemoveExcludedIpRange(DhcpServerIpRange range);
}
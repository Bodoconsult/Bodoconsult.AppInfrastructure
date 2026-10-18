// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Collections;
using Bodoconsult.Network.Windows.Dhcp.Native;
using Bodoconsult.Network.Windows.Interfaces;

namespace Bodoconsult.Network.Windows.Dhcp.Scope;

/// <summary>
/// Collection with exclude IP addresses ranges
/// </summary>
public class DhcpServerScopeExcludedIpRangeCollection : IDhcpServerScopeExcludedIpRangeCollection
{
    /// <summary>
    /// The associated DHCP server
    /// </summary>
    public DhcpServer Server { get; }

    /// <summary>
    /// The associated DHCP server
    /// </summary>
    IDhcpServer IDhcpServerScopeExcludedIpRangeCollection.Server => Server;

    /// <summary>
    /// The associated DHCP scope
    /// </summary>
    public DhcpServerScope Scope { get; }

    /// <summary>
    /// The associated DHCP scope
    /// </summary>
    IDhcpServerScope IDhcpServerScopeExcludedIpRangeCollection.Scope => Scope;

    internal DhcpServerScopeExcludedIpRangeCollection(DhcpServerScope scope)
    {
        Server = scope.Server;
        Scope = scope;
    }

    /// <summary>
    /// Enumerates a list of excluded IP ranges associated with the DHCP Scope
    /// </summary>
    public IEnumerator<DhcpServerIpRange> GetEnumerator()
        => DhcpServerScope.EnumSubnetElements(Server, Scope.Address, DhcpSubnetElementType.DhcpExcludedIpRanges).GetEnumerator();

    /// <summary>
    /// Enumerates a list of excluded IP ranges associated with the DHCP Scope
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    /// <summary>
    /// Add a range with excluded IP addresses
    /// </summary>
    /// <param name="range">Range with excluded IP addresses</param>
    public void AddExcludedIpRange(DhcpServerIpRange range)
        => DhcpServerScope.AddSubnetExcludedIpRangeElement(Server, Scope.Address, range);

    /// <summary>
    /// Add a range with excluded IP addresses
    /// </summary>
    /// <param name="startAddress">Start address</param>
    /// <param name="endAddress">End address</param>
    public void AddExcludedIpRange(DhcpServerIpAddress startAddress, DhcpServerIpAddress endAddress)
        => DhcpServerScope.AddSubnetExcludedIpRangeElement(Server, Scope.Address, DhcpServerIpRange.AsExcluded(startAddress, endAddress));

    /// <summary>
    /// Add a range with excluded IP addresses
    /// </summary>
    /// <param name="address">IP address</param>
    /// <param name="mask">Mask</param>
    public void AddExcludedIpRange(DhcpServerIpAddress address, DhcpServerIpMask mask)
        => DhcpServerScope.AddSubnetExcludedIpRangeElement(Server, Scope.Address, DhcpServerIpRange.AsExcluded(address, mask));

    /// <summary>
    /// Add a range with excluded IP addresses
    /// </summary>
    /// <param name="cidrRange">CIDR-Range with excluded IP addresses as string</param>
    public void AddExcludedIpRange(string cidrRange)
        => DhcpServerScope.AddSubnetExcludedIpRangeElement(Server, Scope.Address, DhcpServerIpRange.AsExcluded(cidrRange));

    /// <summary>
    /// Remove a range with excluded IP addresses
    /// </summary>
    /// <param name="range">Range with excluded IP addresses</param>
    public void RemoveExcludedIpRange(DhcpServerIpRange range)
        => DhcpServerScope.RemoveSubnetExcludedIpRangeElement(Server, Scope.Address, range);
}
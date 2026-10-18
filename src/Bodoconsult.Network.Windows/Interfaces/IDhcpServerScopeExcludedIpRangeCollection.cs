// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.Network.Windows.Dhcp;

namespace Bodoconsult.Network.Windows.Interfaces;

public interface IDhcpServerScopeExcludedIpRangeCollection : IEnumerable<DhcpServerIpRange>
{
    IDhcpServerScope Scope { get; }
    IDhcpServer Server { get; }

    void AddExcludedIpRange(DhcpServerIpAddress startAddress, DhcpServerIpAddress endAddress);
    void AddExcludedIpRange(DhcpServerIpAddress address, DhcpServerIpMask mask);
    void AddExcludedIpRange(DhcpServerIpRange range);
    void AddExcludedIpRange(string cidrRange);
    void RemoveExcludedIpRange(DhcpServerIpRange range);
}
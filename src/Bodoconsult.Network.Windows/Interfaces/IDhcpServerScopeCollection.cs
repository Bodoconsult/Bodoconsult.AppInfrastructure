// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.Network.Windows.Dhcp;

namespace Bodoconsult.Network.Windows.Interfaces;

public interface IDhcpServerScopeCollection : IEnumerable<IDhcpServerScope>
{
    IDhcpServer Server { get; }

    IDhcpServerScope AddScope(string name, DhcpServerIpRange ipRange);
    IDhcpServerScope AddScope(string name, DhcpServerIpRange ipRange, DhcpServerIpMask mask);
    IDhcpServerScope AddScope(string name, DhcpServerIpRange ipRange, DhcpServerIpMask mask, TimeSpan timeDelayOffer, TimeSpan? leaseDuration);
    IDhcpServerScope AddScope(string name, string comment, DhcpServerIpRange ipRange);
    IDhcpServerScope AddScope(string name, string comment, DhcpServerIpRange ipRange, DhcpServerIpMask mask);
    IDhcpServerScope AddScope(string name, string comment, DhcpServerIpRange ipRange, DhcpServerIpMask mask, TimeSpan timeDelayOffer, TimeSpan? leaseDuration);
    IDhcpServerScope GetScope(DhcpServerIpAddress scopeAddress);
    void RemoveScope(IDhcpServerScope scope, bool retainClientDnsRecords = false);
}
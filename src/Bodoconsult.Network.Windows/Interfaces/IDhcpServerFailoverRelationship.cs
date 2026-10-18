// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.Network.Windows.Dhcp;
using Bodoconsult.Network.Windows.Dhcp.Failover;

namespace Bodoconsult.Network.Windows.Interfaces;

public interface IDhcpServerFailoverRelationship
{
    byte? HotStandbyAddressesReservedPercentage { get; }
    byte? LoadBalancePercentage { get; }
    TimeSpan MaximumClientLeadTime { get; }
    DhcpServerFailoverMode Mode { get; }
    string Name { get; }
    DhcpServerFailoverState PreviousState { get; }
    DhcpServerIpAddress PrimaryServerAddress { get; }
    string PrimaryServerName { get; }
    IEnumerable<IDhcpServerScope> Scopes { get; }
    DhcpServerIpAddress SecondaryServerAddress { get; }
    string SecondaryServerName { get; }

    /// <summary>
    /// The associated DHCP server
    /// </summary>
    IDhcpServer Server { get; }
    DhcpServerFailoverServerType ServerType { get; }
    string SharedSecret { get; }
    DhcpServerFailoverState State { get; }
    TimeSpan? StateSwitchoverInterval { get; }

    IDhcpServer ConnectToPartner();
    void Delete();
    void RedistributesFreeAddresses();
    void ReplicateRelationship();
}
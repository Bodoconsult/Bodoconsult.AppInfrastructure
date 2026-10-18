// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Network.Windows.Dhcp;
using Bodoconsult.Network.Windows.Dhcp.Failover;

namespace Bodoconsult.Network.Windows.Interfaces;

/// <summary>
/// DHCP server failover relationship
/// </summary>
public interface IDhcpServerFailoverRelationship
{
    /// <summary>
    /// Hot standby addresses reserved percentage
    /// </summary>
    byte? HotStandbyAddressesReservedPercentage { get; }

    /// <summary>
    /// Load balance percentage
    /// </summary>
    byte? LoadBalancePercentage { get; }

    /// <summary>
    /// Maximum client leadtime
    /// </summary>
    TimeSpan MaximumClientLeadTime { get; }

    /// <summary>
    /// Mode
    /// </summary>
    DhcpServerFailoverMode Mode { get; }

    /// <summary>
    /// Name of the relationship
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Previous state
    /// </summary>
    DhcpServerFailoverState PreviousState { get; }

    /// <summary>
    /// Primary DHCP server IP address
    /// </summary>
    DhcpServerIpAddress PrimaryServerAddress { get; }

    /// <summary>
    /// Primary DHCP server name
    /// </summary>
    string PrimaryServerName { get; }

    /// <summary>
    /// Scopes
    /// </summary>
    IEnumerable<IDhcpServerScope> Scopes { get; }

    /// <summary>
    /// Secondary DHCP server IP address
    /// </summary>
    DhcpServerIpAddress SecondaryServerAddress { get; }

    /// <summary>
    /// Secondary DHCP server name
    /// </summary>
    string SecondaryServerName { get; }

    /// <summary>
    /// The associated DHCP server
    /// </summary>
    IDhcpServer Server { get; }

    /// <summary>
    /// DHCP server type
    /// </summary>
    DhcpServerFailoverServerType ServerType { get; }

    /// <summary>
    /// Shared secret
    /// </summary>
    string SharedSecret { get; }

    /// <summary>
    /// State
    /// </summary>
    DhcpServerFailoverState State { get; }

    /// <summary>
    /// Time interval for state switchover
    /// </summary>
    TimeSpan? StateSwitchoverInterval { get; }

    /// <summary>
    /// Cponnnect to partner
    /// </summary>
    /// <returns>Partner DHCP server</returns>
    IDhcpServer ConnectToPartner();

    /// <summary>
    /// Delete relationship
    /// </summary>
    void Delete();

    /// <summary>
    /// Redistribute free addresses
    /// </summary>
    void RedistributesFreeAddresses();

    /// <summary>
    /// Replicate relationship
    /// </summary>
    void ReplicateRelationship();
}
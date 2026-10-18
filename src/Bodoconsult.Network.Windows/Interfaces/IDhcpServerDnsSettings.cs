// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Network.Windows.Interfaces;

/// <summary>
/// Defines Dynamic DNS settings
/// </summary>
public interface IDhcpServerDnsSettings
{
    /// <summary>
    /// True when Dynamic updates for DNS PTR records are disabled
    /// </summary>
    bool DisableDynamicPtrRecordUpdates { get; }

    /// <summary>
    /// True when A and PTR records are discarded when the lease is deleted
    /// </summary>
    bool DiscardRecordsWhenLeasesDeleted { get; }

    /// <summary>
    /// True when DNS dynamic updates always update DNS records 
    /// </summary>
    bool DynamicDnsUpdatedAlways { get; }

    /// <summary>
    /// True when DNS dynamic updates are enabled only when requested by the DHCP clients
    /// </summary>
    bool DynamicDnsUpdatedOnlyWhenRequested { get; }

    /// <summary>
    /// True when DNS dynamic updates are enabled
    /// </summary>
    bool DynamicDnsUpdatesEnabled { get; }

    /// <summary>
    /// True when DNS records are dynamically updated for DHCP clients that do not request updates (for example, clients running Windows NT 4.0)
    /// </summary>
    bool UpdateRecordsForDownLevelClients { get; }
}
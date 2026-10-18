// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Network.Windows.Interfaces;

/// <summary>
/// Interface for failover statistics
/// </summary>
public interface IDhcpServerScopeFailoverStatistics
{
    /// <summary>
    /// The total number of IPv4 addresses that are free and can be leased to DHCPv4 clients on an IPv4 subnet that is part of a failover relationship.
    /// </summary>
    int AddressesFree { get; }

    /// <summary>
    /// The total number of IPv4 addresses leased to DHCPv4 clients on an IPv4 subnet that is part of a failover relationship.
    /// </summary>
    int AddressesInUse { get; }

    /// <summary>
    /// The total number of IPv4 addresses that can be leased out to DHCPv4 clients on an IPv4 subnet that is part of a failover relationship.
    /// </summary>
    int AddressesTotal { get; }

    /// <summary>
    /// The total number of IPv4 addresses that are free and can be leased to DHCPv4 clients on an IPv4 subnet that is part of a failover relationship on the local DHCP server.
    /// </summary>
    int LocalAddressesFree { get; }

    /// <summary>
    /// The total number of IPv4 addresses leased to DHCPv4 clients on an IPv4 subnet that is part of a failover relationship on the local DHCP server.
    /// </summary>
    int LocalAddressesInUse { get; }

    /// <summary>
    /// The total number of IPv4 addresses that are free and can be leased to DHCPv4 clients on an IPv4 subnet that is part of a failover relationship on the partner server.
    /// </summary>
    int PartnerAddressesFree { get; }

    /// <summary>
    /// The total number of IPv4 addresses leased to DHCPv4 clients on an IPv4 subnet that is part of a failover relationship on the partner server.
    /// </summary>
    int PartnerAddressesInUse { get; }

    /// <summary>
    /// Associated DHCP scope
    /// </summary>
    IDhcpServerScope Scope { get; }

    /// <summary>
    /// The associated DHCP server
    /// </summary>
    IDhcpServer Server { get; }
}
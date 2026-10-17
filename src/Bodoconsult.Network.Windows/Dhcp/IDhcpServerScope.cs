// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Network.Windows.Dhcp;

/// <summary>
/// DHCP server scope
/// </summary>
public interface IDhcpServerScope
{
    /// <summary>
    /// Associated DHCP server IP address
    /// </summary>
    DhcpServerIpAddress Address { get; }

    /// <summary>
    /// Client collection
    /// </summary>
    IDhcpServerScopeClientCollection Clients { get; }

    /// <summary>
    /// Comment
    /// </summary>
    string Comment { get; set; }

    /// <summary>
    /// DNS settings
    /// </summary>
    IDhcpServerDnsSettings DnsSettings { get; }

    /// <summary>
    /// Excluded IP ranges
    /// </summary>
    IDhcpServerScopeExcludedIpRangeCollection ExcludedIpRanges { get; }

    /// <summary>
    /// Current IP range
    /// </summary>
    DhcpServerIpRange IpRange { get; set; }

    /// <summary>
    /// Lease duration
    /// </summary>
    TimeSpan? LeaseDuration { get; set; }

    /// <summary>
    /// IP mask
    /// </summary>
    DhcpServerIpMask Mask { get; }

    /// <summary>
    /// Scope name
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// Scope options value collection
    /// </summary>
    IDhcpServerScopeOptionValueCollection Options { get; }

    /// <summary>
    /// The primary host
    /// </summary>
    IDhcpServerHost PrimaryHost { get; }

    /// <summary>
    /// Is quarantine on
    /// </summary>
    bool QuarantineOn { get; }

    /// <summary>
    /// Reservations
    /// </summary>
    IDhcpServerScopeReservationCollection Reservations { get; }

    /// <summary>
    /// The current DHCP serer
    /// </summary>
    IDhcpServer Server { get; }

    /// <summary>
    /// The current state of the scope
    /// </summary>
    DhcpServerScopeState State { get; }

    /// <summary>
    /// Time delay offer
    /// </summary>
    TimeSpan TimeDelayOffer { get; set; }

    /// <summary>
    /// Activate the scope
    /// </summary>
    void Activate();

    /// <summary>
    /// Configures a failover relationship for this scope
    /// </summary>
    /// <param name="partnerServer">The failover partner server</param>
    /// <param name="sharedSecret">Secret used by the relationship</param>
    /// <param name="mode">Failover mode to configure</param>
    /// <returns>The created failover relationship</returns>
    IDhcpServerFailoverRelationship ConfigureFailover(IDhcpServer partnerServer, string sharedSecret, DhcpServerFailoverMode mode);

    /// <summary>
    /// Configures a failover relationship for this scope
    /// </summary>
    /// <param name="partnerServer">The failover partner server</param>
    /// <param name="sharedSecret">Secret used by the relationship</param>
    /// <param name="mode">Failover mode to configure</param>
    /// <param name="modePercentage">Percentage argument for the failover mode</param>
    /// <returns>The created failover relationship</returns>
    IDhcpServerFailoverRelationship ConfigureFailover(IDhcpServer partnerServer, string sharedSecret, DhcpServerFailoverMode mode, byte modePercentage);

    /// <summary>
    /// Configures a failover relationship for this scope
    /// </summary>
    /// <param name="partnerServer">The failover partner server</param>
    /// <param name="sharedSecret">Secret used by the relationship</param>
    /// <param name="mode">Failover mode to configure</param>
    /// <param name="modePercentage">Percentage argument for the failover mode</param>
    /// <param name="maximumClientLeadTime">Maximum client lead time</param>
    /// <param name="stateSwitchInterval">State switch interval or null to disable</param>
    /// <returns>The created failover relationship</returns>
    IDhcpServerFailoverRelationship ConfigureFailover(IDhcpServer partnerServer, string sharedSecret, DhcpServerFailoverMode mode, byte modePercentage, TimeSpan maximumClientLeadTime, TimeSpan? stateSwitchInterval);

    /// <summary>
    /// Configures a failover relationship for this scope
    /// </summary>
    /// <param name="partnerServer">The failover partner server</param>
    /// <param name="name">Name of the failover relationship</param>
    /// <param name="sharedSecret">Secret used by the relationship</param>
    /// <param name="mode">Failover mode to configure</param>
    /// <returns>The created failover relationship</returns>
    IDhcpServerFailoverRelationship ConfigureFailover(IDhcpServer partnerServer, string name, string sharedSecret, DhcpServerFailoverMode mode);

    /// <summary>
    /// Configures a failover relationship for this scope
    /// </summary>
    /// <param name="partnerServer">The failover partner server</param>
    /// <param name="name">Name of the failover relationship</param>
    /// <param name="sharedSecret">Secret used by the relationship</param>
    /// <param name="mode">Failover mode to configure</param>
    /// <param name="modePercentage">Percentage argument for the failover mode</param>
    /// <returns>The created failover relationship</returns>
    IDhcpServerFailoverRelationship ConfigureFailover(IDhcpServer partnerServer, string name, string sharedSecret, DhcpServerFailoverMode mode, byte modePercentage);

    /// <summary>
    /// Configures a failover relationship for this scope
    /// </summary>
    /// <param name="partnerServer">The failover partner server</param>
    /// <param name="name">Name of the failover relationship</param>
    /// <param name="sharedSecret">Secret used by the relationship</param>
    /// <param name="mode">Failover mode to configure</param>
    /// <param name="modePercentage">Percentage argument for the failover mode</param>
    /// <param name="maximumClientLeadTime">Maximum client lead time</param>
    /// <param name="stateSwitchInterval">State switch interval or null to disable</param>
    /// <returns>The created failover relationship</returns>
    IDhcpServerFailoverRelationship ConfigureFailover(IDhcpServer partnerServer, string name, string sharedSecret, DhcpServerFailoverMode mode, byte modePercentage, TimeSpan maximumClientLeadTime, TimeSpan? stateSwitchInterval);

    /// <summary>
    /// Configure a failover DHCP relationship with a partner server
    /// </summary>
    /// <param name="failoverRelationship">Configured failover DHCP relationship</param>
    /// <returns>A failover DHCP relationship</returns>
    void ConfigureFailover(IDhcpServerFailoverRelationship failoverRelationship);

    /// <summary>
    /// Deactivate the scope
    /// </summary>
    void Deactivate();

    /// <summary>
    /// Removes the scope from its failover relationship. This will delete the scope from the partner server.
    /// </summary>
    void DeconfigureFailover();

    /// <summary>
    /// Deletes this scope from the server
    /// </summary>
    /// <param name="retainClientDnsRecords">If true registered client DNS records are not removed. Useful in failover scenarios. Default = false</param>
    void Delete(bool retainClientDnsRecords = false);

    /// <summary>
    /// Get DHCP failover relationship
    /// </summary>
    /// <returns>DHCP failover relationship</returns>
    IDhcpServerFailoverRelationship GetFailoverRelationship();

    /// <summary>
    /// Get DHCP failover statistics
    /// </summary>
    /// <returns>DHCP failover statistics</returns>
    IDhcpServerScopeFailoverStatistics GetFailoverStatistics();

    /// <summary>
    /// Replicate a DHCP failover partner server
    /// </summary>
    void ReplicateFailoverPartner();
}
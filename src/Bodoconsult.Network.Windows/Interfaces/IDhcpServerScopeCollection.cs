// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.Network.Windows.Dhcp;

namespace Bodoconsult.Network.Windows.Interfaces;

/// <summary>
/// Interface for collections of scopes for a DHCP server
/// </summary>
public interface IDhcpServerScopeCollection : IEnumerable<IDhcpServerScope>
{
    /// <summary>
    /// The associated DHCP server
    /// </summary>
    IDhcpServer Server { get; }

    /// <summary>
    /// Creates a new DHCP Scope
    /// </summary>
    /// <param name="name">Name of the DHCP Scope</param>
    /// <param name="ipRange">IP Range to be associated with the DHCP Scope from which the scope mask is inferred</param>
    /// <returns>Newly created DHCP Scope</returns>
    IDhcpServerScope AddScope(string name, DhcpServerIpRange ipRange);

    /// <summary>
    /// Creates a new DHCP Scope
    /// </summary>
    /// <param name="name">Name of the DHCP Scope</param>
    /// <param name="ipRange">IP Range to be associated with the DHCP Scope from which the scope mask is inferred</param>
    /// <param name="mask">Subnet mask</param>
    /// <returns>Newly created DHCP Scope</returns>
    IDhcpServerScope AddScope(string name, DhcpServerIpRange ipRange, DhcpServerIpMask mask);

    /// <summary>
    /// Creates a new DHCP Scope
    /// </summary>
    /// <param name="name">Name of the DHCP Scope</param>
    /// <param name="ipRange">IP Range to be associated with the DHCP Scope</param>
    /// <param name="mask">Subnet mask to be associated with the DHCP Scope</param>
    /// <param name="timeDelayOffer">Milliseconds to wait before sending a lease offer (maximum 1 second)</param>
    /// <param name="leaseDuration">Number of seconds the lease is held for</param>
    /// <returns>Newly created DHCP Scope</returns>
    IDhcpServerScope AddScope(string name, DhcpServerIpRange ipRange, DhcpServerIpMask mask, TimeSpan timeDelayOffer, TimeSpan? leaseDuration);

    /// <summary>
    /// Creates a new DHCP Scope
    /// </summary>
    /// <param name="name">Name of the DHCP Scope</param>
    /// <param name="comment">Comment for the DHCP Scope</param>
    /// <param name="ipRange">IP Range to be associated with the DHCP Scope from which the scope mask is inferred</param>
    /// <returns>Newly created DHCP Scope</returns>
    IDhcpServerScope AddScope(string name, string comment, DhcpServerIpRange ipRange);

    /// <summary>
    /// Creates a new DHCP Scope
    /// </summary>
    /// <param name="name">Name of the DHCP Scope</param>
    /// <param name="comment">Comment for the DHCP Scope</param>
    /// <param name="ipRange">IP Range to be associated with the DHCP Scope</param>
    /// <param name="mask">Subnet mask to be associated with the DHCP Scope</param>
    /// <returns>Newly created DHCP Scope</returns>
    IDhcpServerScope AddScope(string name, string comment, DhcpServerIpRange ipRange, DhcpServerIpMask mask);

    /// <summary>
    /// Creates a new DHCP Scope
    /// </summary>
    /// <param name="name">Name of the DHCP Scope</param>
    /// <param name="comment">Comment for the DHCP Scope</param>
    /// <param name="ipRange">IP Range to be associated with the DHCP Scope</param>
    /// <param name="mask">Subnet mask to be associated with the DHCP Scope</param>
    /// <param name="timeDelayOffer">Milliseconds to wait before sending a lease offer (maximum 1 second)</param>
    /// <param name="leaseDuration">Number of seconds the lease is held for</param>
    IDhcpServerScope AddScope(string name, string comment, DhcpServerIpRange ipRange, DhcpServerIpMask mask, TimeSpan timeDelayOffer, TimeSpan? leaseDuration);

    /// <summary>
    /// Gets the DHCP scope with the associated scope address
    /// </summary>
    /// <param name="scopeAddress">IP address</param>
    /// <returns>Scope instance or null</returns>
    IDhcpServerScope GetScope(DhcpServerIpAddress scopeAddress);

    /// <summary>
    /// Deletes the specified scope
    /// </summary>
    /// <param name="scope">The scope to be deleted</param>
    /// <param name="retainClientDnsRecords">If true registered client DNS records are not removed. Useful in failover scenarios. Default = false</param>
    void RemoveScope(IDhcpServerScope scope, bool retainClientDnsRecords = false);
}
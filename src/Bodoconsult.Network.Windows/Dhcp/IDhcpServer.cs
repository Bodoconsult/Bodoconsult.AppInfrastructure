// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Network.Windows.Dhcp;

/// <summary>
/// Interface for DHCP server instances
/// </summary>
public interface IDhcpServer
{
    /// <summary>
    /// DHCP server IP address
    /// </summary>
    DhcpServerIpAddress Address { get; }

    /// <summary>
    /// DHCP server audit log
    /// </summary>
    IDhcpServerAuditLog AuditLog { get; }

    /// <summary>
    /// Binding elemets
    /// </summary>
    IDhcpServerBindingElementCollection BindingElements { get; }

    /// <summary>
    ///  Classes
    /// </summary>
    IDhcpServerClassCollection Classes { get; }

    /// <summary>
    /// DHCP clients
    /// </summary>
    IDhcpServerClientCollection Clients { get; }

    /// <summary>
    /// DHCP cinfiguration
    /// </summary>
    IDhcpServerConfiguration Configuration { get; }

    /// <summary>
    /// DNS settings
    /// </summary>
    IDhcpServerDnsSettings DnsSettings { get; }

    /// <summary>
    /// Failover relationships
    /// </summary>
    IDhcpServerFailoverRelationshipCollection FailoverRelationships { get; }

    /// <summary>
    /// Name of the DHCP server
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Server options
    /// </summary>
    IDhcpServerOptionCollection Options { get; }

    /// <summary>
    /// Scopes
    /// </summary>
    IDhcpServerScopeCollection Scopes { get; }

    /// <summary>
    /// Specific strings
    /// </summary>
    IDhcpServerSpecificStrings SpecificStrings { get; }

    /// <summary>
    /// DHCP server version
    /// </summary>
    DhcpServerVersions Version { get; }

    /// <summary>
    /// DHCP server major version
    /// </summary>
    int VersionMajor { get; }

    /// <summary>
    /// DHCP server minor version
    /// </summary>
    int VersionMinor { get; }

    /// <summary>
    /// Is the server compatible to the requested version
    /// </summary>
    /// <param name="version">Requested version</param>
    /// <returns></returns>
    bool IsCompatible(DhcpServerVersions version);
}
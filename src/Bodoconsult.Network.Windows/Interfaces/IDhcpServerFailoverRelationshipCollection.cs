// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Network.Windows.Interfaces;

/// <summary>
/// Collection of failover relationships
/// </summary>
public interface IDhcpServerFailoverRelationshipCollection : IEnumerable<IDhcpServerFailoverRelationship>
{
    /// <summary>
    /// The associated DHCP server
    /// </summary>
    IDhcpServer Server { get; }

    /// <summary>
    /// Get a failover relationship by name
    /// </summary>
    /// <param name="relationshipName">Name of the relationship</param>
    /// <returns>Failover relationship</returns>
    IDhcpServerFailoverRelationship GetRelationship(string relationshipName);

    /// <summary>
    /// Remove failover relationship
    /// </summary>
    /// <param name="relationship">Relationship to remove</param>
    void RemoveRelationship(IDhcpServerFailoverRelationship relationship);
}
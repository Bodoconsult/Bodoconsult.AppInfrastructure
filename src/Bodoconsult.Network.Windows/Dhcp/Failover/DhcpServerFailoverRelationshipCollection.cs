// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Network.Windows.Interfaces;
using System.Collections;

namespace Bodoconsult.Network.Windows.Dhcp.Failover;

/// <summary>
/// Failover relationship collection
/// </summary>
public class DhcpServerFailoverRelationshipCollection : IDhcpServerFailoverRelationshipCollection
{
    /// <summary>
    /// The associated DHCP server
    /// </summary>
    public DhcpServer Server { get; }

    /// <summary>
    /// The associated DHCP server
    /// </summary>
    IDhcpServer IDhcpServerFailoverRelationshipCollection.Server => Server;

    internal DhcpServerFailoverRelationshipCollection(DhcpServer server)
    {
        Server = server;
    }

    /// <summary>
    /// Get a failover relationship by name
    /// </summary>
    /// <param name="relationshipName">Name of the relationship</param>
    /// <returns>Failover relationship</returns>
    public IDhcpServerFailoverRelationship GetRelationship(string relationshipName)
        => DhcpServerFailoverRelationship.GetFailoverRelationship(Server, relationshipName);

    /// <summary>
    /// Remove failover relationship
    /// </summary>
    /// <param name="relationship">Relationship to remove</param>
    public void RemoveRelationship(IDhcpServerFailoverRelationship relationship)
        => relationship.Delete();

    /// <summary>Returns an enumerator that iterates through the collection.</summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    public IEnumerator<IDhcpServerFailoverRelationship> GetEnumerator()
        => DhcpServerFailoverRelationship.GetFailoverRelationships(Server).GetEnumerator();

    /// <summary>Returns an enumerator that iterates through a collection.</summary>
    /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.Network.Windows.Interfaces;
using System.Collections;

namespace Bodoconsult.Network.Windows.Dhcp.Failover;

public class DhcpServerFailoverRelationshipCollection : IDhcpServerFailoverRelationshipCollection
{
    public DhcpServer Server { get; }
    IDhcpServer IDhcpServerFailoverRelationshipCollection.Server => Server;

    internal DhcpServerFailoverRelationshipCollection(DhcpServer server)
    {
        Server = server;
    }

    public IDhcpServerFailoverRelationship GetRelationship(string relationshipName)
        => DhcpServerFailoverRelationship.GetFailoverRelationship(Server, relationshipName);

    public void RemoveRelationship(IDhcpServerFailoverRelationship relationship)
        => relationship.Delete();

    public IEnumerator<IDhcpServerFailoverRelationship> GetEnumerator()
        => DhcpServerFailoverRelationship.GetFailoverRelationships(Server).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
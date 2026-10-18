// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Network.Windows.Interfaces;

public interface IDhcpServerClientCollection : IEnumerable<IDhcpServerClient>
{
    IDhcpServer Server { get; }

    void RemoveClient(IDhcpServerClient client);
}
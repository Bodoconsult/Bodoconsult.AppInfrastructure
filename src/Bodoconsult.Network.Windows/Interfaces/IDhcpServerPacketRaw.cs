// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Network.Windows.Interfaces;

public interface IDhcpServerPacketRaw : IDhcpServerPacket
{
    /// <summary>
    /// Returns the buffer containing the raw packet bytes
    /// </summary>
    byte[] Buffer { get; }

    /// <summary>
    /// Calculates the length of the packet
    /// </summary>
    /// <returns>The length of the packet in the buffer</returns>
    int GetLength();
}
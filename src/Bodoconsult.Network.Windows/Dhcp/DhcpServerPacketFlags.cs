// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


namespace Bodoconsult.Network.Windows.Dhcp;

/// <summary>
/// Packet flags
/// </summary>
[Flags]
public enum DhcpServerPacketFlags : ushort
{
    /// <summary>
    /// Broadcast
    /// </summary>
    Broadcast = 0x8000
}
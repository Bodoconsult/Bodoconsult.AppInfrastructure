// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Network.Windows.Dhcp;

/// <summary>
/// Message types
/// </summary>
public enum DhcpServerMessageTypes : byte
{
    /// <summary>
    /// BootRequest
    /// </summary>
    BootRequest = 1,
    /// <summary>
    /// BootReply
    /// </summary>
    BootReply = 2
}
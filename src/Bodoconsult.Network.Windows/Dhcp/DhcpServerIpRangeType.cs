// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Network.Windows.Dhcp;

/// <summary>
/// IP range type enum
/// </summary>
public enum DhcpServerIpRangeType
{
    /// <summary>
    /// Reserved
    /// </summary>
    Reserved = 2,
    /// <summary>
    /// Excluded
    /// </summary>
    Excluded = 3,
    /// <summary>
    /// ScopeDhcpOnly
    /// </summary>
    ScopeDhcpOnly = 5,
    /// <summary>
    /// ScopeDhcpAndBootp
    /// </summary>
    ScopeDhcpAndBootp = 6,
    /// <summary>
    /// ScopeBootpOnly
    /// </summary>
    ScopeBootpOnly = 7
}
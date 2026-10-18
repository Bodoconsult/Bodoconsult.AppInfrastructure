// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


namespace Bodoconsult.Network.Windows.Dhcp;

/// <summary>
/// Server versions enum
/// </summary>
public enum DhcpServerVersions : long
{
    /// <summary>
    /// Windows NT 3.1
    /// </summary>
    WindowsNt310 = 0x3000A,
    /// <summary>
    /// Windows NT 3.5
    /// </summary>
    WindowsNt350 = 0x30032,
    /// <summary>
    /// Windows NT 3.51
    /// </summary>
    WindowsNt351 = 0x30033,
    /// <summary>
    /// Windows NT 4
    /// </summary>
    WindowsNt40 = 0x40000,
    /// <summary>
    /// Windows 2000
    /// </summary>
    Windows2000 = 0x50000,
    /// <summary>
    /// Windows 2003
    /// </summary>
    Windows2003 = 0x50002,
    /// <summary>
    /// Windows 2003R2
    /// </summary>
    Windows2003R2 = 0x50002,
    /// <summary>
    /// Windows 2008
    /// </summary>
    Windows2008 = 0x60000,
    /// <summary>
    /// Windows 2008R2
    /// </summary>
    Windows2008R2 = 0x60001,
    /// <summary>
    /// Windows 2012
    /// </summary>
    Windows2012 = 0x60002,
    /// <summary>
    /// Windows 2012R2
    /// </summary>
    Windows2012R2 = 0x60003,
    /// <summary>
    /// Represents Windows Server 2016 and greater
    /// </summary>
    WindowsServer = 0xA0000
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Network.Windows.Dhcp;

/// <summary>
/// Option ID
/// </summary>
public enum DhcpServerOptionIdTypes : byte
{
    /// <summary>
    /// Hex
    /// </summary>
    Hex = 0,
    /// <summary>
    /// Pad
    /// </summary>
    Pad,
    /// <summary>
    /// End
    /// </summary>
    End,
    /// <summary>
    /// IpAddress
    /// </summary>
    IpAddress,
    /// <summary>
    /// IpAddressList
    /// </summary>
    IpAddressList,
    /// <summary>
    /// Byte
    /// </summary>
    Byte,
    /// <summary>
    /// Int16
    /// </summary>
    Int16,
    /// <summary>
    /// UInt16
    /// </summary>
    UInt16,
    /// <summary>
    /// Int32
    /// </summary>
    Int32,
    /// <summary>
    /// UInt32
    /// </summary>
    UInt32,
    /// <summary>
    /// AsciiString
    /// </summary>
    AsciiString,
    /// <summary>
    /// Utf8String
    /// </summary>
    Utf8String,
    /// <summary>
    /// IpAddressAndSubnet
    /// </summary>
    IpAddressAndSubnet,
    /// <summary>
    /// IpAddressAndIpAddress
    /// </summary>
    IpAddressAndIpAddress,
    /// <summary>
    /// UInt16List
    /// </summary>
    UInt16List,
    /// <summary>
    /// DhcpMessageType
    /// </summary>
    DhcpMessageType,
    /// <summary>
    /// DhcpParameterRequestList
    /// </summary>
    DhcpParameterRequestList,
    /// <summary>
    /// ZeroLengthFlag
    /// </summary>
    ZeroLengthFlag,
    /// <summary>
    /// ClientFqdn
    /// </summary>
    ClientFqdn,
    /// <summary>
    /// DnsName
    /// </summary>
    DnsName,
    /// <summary>
    /// DnsNameList
    /// </summary>
    DnsNameList,
    /// <summary>
    /// ClientUuid
    /// </summary>
    ClientUuid,
    /// <summary>
    /// SipServers
    /// </summary>
    SipServers,
    /// <summary>
    /// StatusCode
    /// </summary>
    StatusCode,
    /// <summary>
    /// DhcpState
    /// </summary>
    DhcpState
}
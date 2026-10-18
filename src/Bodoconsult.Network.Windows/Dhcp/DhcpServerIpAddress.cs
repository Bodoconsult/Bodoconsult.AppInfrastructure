// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Bodoconsult.Network.Windows.Dhcp.Native;
using Bodoconsult.Network.Windows.Helpers;

namespace Bodoconsult.Network.Windows.Dhcp;

/// <summary>
/// The IP address of a DHCP server
/// </summary>
[Serializable]
public struct DhcpServerIpAddress : IEquatable<DhcpServerIpAddress>, IEquatable<IPAddress>
{
#pragma warning disable IDE0032 // Use auto property
    /// <summary>
    /// IP Address stored in big-endian (network) order
    /// </summary>
    private readonly uint _address;
#pragma warning restore IDE0032 // Use auto property

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="nativeAddress">Native address</param>
    public DhcpServerIpAddress(uint nativeAddress)
    {
        _address = nativeAddress;
    }

    /// <summary>
    /// Ctor providing an IP address as string
    /// </summary>
    /// <param name="address">IP address as string</param>
    public DhcpServerIpAddress(string address)
    {
        _address = BitHelper.StringToIpAddress(address);
    }

    /// <summary>
    /// Ctor providing an IP address as object
    /// </summary>
    /// <param name="address">IP address</param>
    public DhcpServerIpAddress(IPAddress address)
    {
        if (address.AddressFamily != AddressFamily.InterNetwork)
        {
            throw new ArgumentOutOfRangeException(nameof(address), "Only IPv4 addresses are supported");
        }

#pragma warning disable CS0618 // Type or member is obsolete
        var ip = (uint)address.Address;
#pragma warning restore CS0618 // Type or member is obsolete

        // DhcpServerIpAddress always stores in network order
        // IPAddress stores in host order
        _address = BitHelper.HostToNetworkOrder(ip);
    }

    /// <summary>
    /// IP Address in network order
    /// </summary>
    public uint Native => _address;

    /// <summary>
    /// Get IP address as byte array
    /// </summary>
    /// <returns>IP address as byte array</returns>
    public byte[] GetBytes()
    {
        var buffer = new byte[4];
        BitHelper.Write(buffer, 0, _address);
        return buffer;
    }

    /// <summary>
    /// Get IP address as byte array
    /// </summary>
    /// <returns>IP address as byte array</returns>
    public byte GetByte(int index)
    {
        if (index < 0 || index > 3)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        return (byte)(_address >> ((3 - index) * 8));
    }

    /// <summary>
    /// Get an empty IP address
    /// </summary>
    public static DhcpServerIpAddress Empty => new(0);

    /// <summary>
    /// Get IP address from native address
    /// </summary>
    /// <param name="nativeAddress">Native address</param>
    /// <returns>IP address</returns>
    public static DhcpServerIpAddress FromNative(uint nativeAddress) => new(nativeAddress);

    /// <summary>
    /// Get IP address from native address
    /// </summary>
    /// <param name="nativeAddress">Native address</param>
    /// <returns>IP address</returns>
    public static DhcpServerIpAddress FromNative(int nativeAddress) => new((uint)nativeAddress);

    /// <summary>
    /// Get IP address from native address
    /// </summary>
    /// <param name="pointer">Native address as pointer</param>
    /// <returns>IP address</returns>
    internal static DhcpServerIpAddress FromNative(IntPtr pointer)
    {
        if (pointer == IntPtr.Zero)
        {
            throw new ArgumentNullException(nameof(pointer));
        }

        return new DhcpServerIpAddress((uint)BitHelper.HostToNetworkOrder(Marshal.ReadInt32(pointer)));
    }
    /// <summary>
    /// Get IP address from native address
    /// </summary>
    /// <param name="address">String with IP address</param>
    /// <returns>IP address</returns>
    public static DhcpServerIpAddress FromString(string address) => new(address);

    /// <summary>Returns the fully qualified type name of this instance.</summary>
    /// <returns>The fully qualified type name.</returns>
    public override string ToString() => BitHelper.IpAddressToString(_address);

    /// <summary>Indicates whether this instance and a specified object are equal.</summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj is DhcpServerIpAddress sia)
        {
            return Equals(sia);
        }

        if (obj is IPAddress ia)
        {
            return Equals(ia);
        }

        return false;
    }

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
    public override int GetHashCode() => (int)_address;

    /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
    public bool Equals(DhcpServerIpAddress other) => _address == other._address;

    /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
    public bool Equals(IPAddress other)
    {
        if (other is null || other.AddressFamily != AddressFamily.InterNetwork)
        {
            return false;
        }

#pragma warning disable CS0618 // Type or member is obsolete
        var otherIp = (uint)other.Address;
#pragma warning restore CS0618 // Type or member is obsolete

        return _address == BitHelper.HostToNetworkOrder(otherIp);
    }

    /// <summary>
    /// Equals operator
    /// </summary>
    /// <param name="lhs">IP address 1</param>
    /// <param name="rhs">IP address 2</param>
    /// <returns>True if both IP addresses are equal</returns>
    public static bool operator ==(DhcpServerIpAddress lhs, DhcpServerIpAddress rhs) => lhs.Equals(rhs);

    /// <summary>
    /// Not equal operator
    /// </summary>
    /// <param name="lhs">IP address 1</param>
    /// <param name="rhs">IP address 2</param>
    /// <returns>True if both IP addresses are not equal</returns>
    public static bool operator !=(DhcpServerIpAddress lhs, DhcpServerIpAddress rhs) => !lhs.Equals(rhs);

    /// <summary>
    /// Equals operator
    /// </summary>
    /// <param name="lhs">IP address 1</param>
    /// <param name="rhs">IP address 2</param>
    /// <returns>True if both IP addresses are equal</returns>
    public static bool operator ==(DhcpServerIpAddress lhs, IPAddress rhs) => lhs.Equals(rhs);

    /// <summary>
    /// Not equal operator
    /// </summary>
    /// <param name="lhs">IP address 1</param>
    /// <param name="rhs">IP address 2</param>
    /// <returns>True if both IP addresses are not equal</returns>
    public static bool operator !=(DhcpServerIpAddress lhs, IPAddress rhs) => !lhs.Equals(rhs);

    internal DhcpIpAddress ToNativeAsHost() => new(BitHelper.NetworkToHostOrder(_address));

    internal DhcpIpAddress ToNativeAsNetwork() => new(_address);

    /// <summary>
    /// IP address as uint operator
    /// </summary>
    /// <param name="address">IP address</param>
    public static explicit operator uint(DhcpServerIpAddress address) => address._address;

    /// <summary>
    /// Uint as IP address operator
    /// </summary>
    /// <param name="address">Address</param>
    public static explicit operator DhcpServerIpAddress(uint address) => new(address);

    /// <summary>
    /// IP address as int operator
    /// </summary>
    /// <param name="address">IP address</param>
    public static explicit operator int(DhcpServerIpAddress address) => (int)address._address;

    /// <summary>
    /// Int as IP address operator
    /// </summary>
    /// <param name="address">Address</param>
    public static explicit operator DhcpServerIpAddress(int address) => new((uint)address);

    /// <summary>
    /// <see cref="IPAddress"/> as <see cref="DhcpServerIpAddress"/> operator
    /// </summary>
    /// <param name="address"></param>
    public static implicit operator DhcpServerIpAddress(IPAddress address) => new(address);

    /// <summary>
    /// As <see cref="IPAddress"/> operator
    /// </summary>
    /// <param name="address">IP address</param>
    public static implicit operator IPAddress(DhcpServerIpAddress address)
        // IPAddress stores in host order; DhcpServerIpAddress always stores in network order
        => new(BitHelper.NetworkToHostOrder(address._address));

    /// <summary>
    /// IP address as subnet mask operator
    /// </summary>
    /// <param name="address">IP address</param>
    public static explicit operator DhcpServerIpMask(DhcpServerIpAddress address) => new(address._address);

    /// <summary>
    /// As string operator
    /// </summary>
    /// <param name="address">IP address</param>
    public static implicit operator string(DhcpServerIpAddress address) => address.ToString();

    /// <summary>
    /// From IP address to <see cref="DhcpServerIpAddress"/> operator
    /// </summary>
    /// <param name="address">IP address as string</param>
    public static implicit operator DhcpServerIpAddress(string address) => FromString(address);

    /// <summary>
    /// Is address a bigger than b
    /// </summary>
    /// <param name="a">Address a</param>
    /// <param name="b">Address b</param>
    /// <returns>True if a is bigger than b</returns>
    public static bool operator >(DhcpServerIpAddress a, DhcpServerIpAddress b) => a._address > b._address;

    /// <summary>
    /// Is address a bigger or equal than b
    /// </summary>
    /// <param name="a">Address a</param>
    /// <param name="b">Address b</param>
    /// <returns>True if a is bigger o equal than b</returns>
    public static bool operator >=(DhcpServerIpAddress a, DhcpServerIpAddress b) => a._address >= b._address;

    /// <summary>
    /// Is address a lower than b
    /// </summary>
    /// <param name="a">Address a</param>
    /// <param name="b">Address b</param>
    /// <returns>True if a is lower than b</returns>
    public static bool operator <(DhcpServerIpAddress a, DhcpServerIpAddress b) => a._address < b._address;

    /// <summary>
    /// Is address a lower or equal than b
    /// </summary>
    /// <param name="a">Address a</param>
    /// <param name="b">Address b</param>
    /// <returns>True if a is lower or equal than b</returns>
    public static bool operator <=(DhcpServerIpAddress a, DhcpServerIpAddress b) => a._address <= b._address;
}
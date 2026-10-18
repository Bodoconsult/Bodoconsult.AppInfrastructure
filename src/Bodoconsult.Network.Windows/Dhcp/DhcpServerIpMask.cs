// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Runtime.InteropServices;
using Bodoconsult.Network.Windows.Dhcp.Native;
using Bodoconsult.Network.Windows.Helpers;

namespace Bodoconsult.Network.Windows.Dhcp;

/// <summary>
/// IP mask
/// </summary>
[Serializable]
public struct DhcpServerIpMask
{
#pragma warning disable IDE0032// Use auto property
    /// <summary>
    /// IP Mask stored in big-endian (network) order
    /// </summary>
    private readonly uint _mask;
#pragma warning restore IDE0032 // Use auto property

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="mask">IP Mask stored in big-endian (network) order</param>
    public DhcpServerIpMask(uint mask)
    {
        _mask = mask;
    }

    /// <summary>
    /// Ctor providing an IP mask as string
    /// </summary>
    /// <param name="mask">IP mask as string</param>
    public DhcpServerIpMask(string mask)
    {
        _mask = BitHelper.StringToIpAddress(mask);
    }

    /// <summary>
    /// IP Mask in network order
    /// </summary>
    public uint Native => _mask;

    /// <summary>
    /// Get an empty IP mask
    /// </summary>
    public static DhcpServerIpMask Empty => new(0);

    /// <summary>
    /// Get an IP mask from a native mask
    /// </summary>
    /// <param name="nativeMask">Native mask</param>
    /// <returns>IP mask</returns>
    public static DhcpServerIpMask FromNative(uint nativeMask) => new(nativeMask);

    /// <summary>
    /// Get an IP mask from a native mask
    /// </summary>
    /// <param name="nativeMask">Native mask</param>
    /// <returns>IP mask</returns>
    public static DhcpServerIpMask FromNative(int nativeMask) => new((uint)nativeMask);

    /// <summary>
    /// Get an IP mask from a native mask
    /// </summary>
    /// <param name="pointer">Native mask as pointer</param>
    /// <returns>IP mask</returns>
    internal static DhcpServerIpMask FromNative(IntPtr pointer)
    {
        if (pointer == IntPtr.Zero)
            throw new ArgumentNullException(nameof(pointer));

        return new DhcpServerIpMask((uint)BitHelper.HostToNetworkOrder(Marshal.ReadInt32(pointer)));
    }

    /// <summary>
    /// Get an IP mask from a string
    /// </summary>
    /// <param name="mask">Mask as string</param>
    /// <returns>IP mask</returns>
    public static DhcpServerIpMask FromString(string mask) => new(mask);

    /// <summary>
    /// Get an IP mask from significant bits 
    /// </summary>
    /// <param name="bitCount">Bit count</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static DhcpServerIpMask FromSignificantBits(int bitCount)
    {
        if (bitCount > 32 || bitCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(bitCount));
        }

        if (bitCount == 0)
        {
            return new DhcpServerIpMask(0U);
        }

        var m = unchecked((int)0x8000_0000) >> (bitCount - 1); // signed int

        return new DhcpServerIpMask((uint)m);
    }

    /// <summary>
    /// Significant bits
    /// </summary>
    public int SignificantBits => BitHelper.HighSignificantBits(_mask);

    /// <summary>
    /// Get a DHCP IP range
    /// </summary>
    /// <param name="address">Current IP address</param>
    /// <returns>IP range</returns>
    public DhcpServerIpRange GetDhcpIpRange(DhcpServerIpAddress address) => DhcpServerIpRange.AsDhcpScope(address, this);

    /// <summary>
    /// Get a DHCP IP and BOOTP range
    /// </summary>
    /// <param name="address">Current IP address</param>
    /// <returns>IP and BOOTP range</returns>
    public DhcpServerIpRange GetDhcpAndBootpIpRange(DhcpServerIpAddress address) => DhcpServerIpRange.AsDhcpAndBootpScope(address, this);

    /// <summary>
    /// Get a DHCP BOOTP range
    /// </summary>
    /// <param name="address">Current IP address</param>
    /// <returns>BOOTP range</returns>
    public DhcpServerIpRange GetBootpIpRange(DhcpServerIpAddress address) => DhcpServerIpRange.AsBootpScope(address, this);

    /// <summary>
    /// Get a DHCP IP range
    /// </summary>
    /// <param name="address">Current IP address</param>
    /// <param name="type">Requested range type</param>
    /// <returns>IP range</returns>
    internal DhcpServerIpRange GetIpRange(DhcpServerIpAddress address, DhcpServerIpRangeType type) => DhcpServerIpRange.FromMask(address, this, type);

    internal DhcpIpMask ToNativeAsHost() => new(BitHelper.NetworkToHostOrder(_mask));
    internal DhcpIpMask ToNativeAsNetwork() => new(_mask);

    /// <summary>Returns the fully qualified type name of this instance.</summary>
    /// <returns>The fully qualified type name.</returns>
    public override string ToString() => BitHelper.IpAddressToString(_mask);

    public static explicit operator DhcpServerIpAddress(DhcpServerIpMask mask) => new(mask._mask);
    public static explicit operator DhcpServerIpMask(string mask) => new(mask);
}
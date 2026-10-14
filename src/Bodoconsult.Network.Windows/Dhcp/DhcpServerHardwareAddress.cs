// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Network.Windows.Dhcp.Native;

namespace Bodoconsult.Network.Windows.Dhcp;

/// <summary>
/// DHCP server hardware address
/// </summary>
[Serializable]
public struct DhcpServerHardwareAddress : IEquatable<DhcpServerHardwareAddress>
{
    /// <summary>
    /// Maximum length of the address
    /// </summary>
    public const int MaximumLength = 16;

#pragma warning disable IDE0032 // Use auto property
    private readonly DhcpServerHardwareType _hwAddrType;
    private readonly byte _hwAddrLen;
    private readonly ulong _hwAddr1;
    private readonly ulong _hwAddr2;
#pragma warning restore IDE0032 // Use auto property

    /// <summary>
    /// Type of hardware address (see RFC 1700 Assigned Numbers - Number Hardware Type (hrd))
    /// </summary>
    public readonly DhcpServerHardwareType Type => _hwAddrType;

    /// <summary>
    /// Length of hardware address in bytes
    /// </summary>
    public readonly int Length => _hwAddrLen;

    /// <summary>
    /// Native hardware address as a byte array
    /// </summary>
    public readonly byte[] Native
    {
        get
        {
            var addr = new byte[_hwAddrLen];
            ToNative(addr, 0, _hwAddrLen);
            return addr;
        }
    }

    internal DhcpServerHardwareAddress(DhcpServerHardwareType type, byte length, ulong addr1, ulong addr2)
    {
        if (length > MaximumLength)
            throw new ArgumentOutOfRangeException(nameof(length), $"Exceeds maximum length {MaximumLength}");

        _hwAddrType = type;
        _hwAddrLen = length;
        _hwAddr1 = addr1;
        _hwAddr2 = addr2;
    }

    /// <summary>
    /// Convert to native address
    /// </summary>
    /// <param name="buffer">Buffer with the address</param>
    /// <param name="offset">Offset</param>
    /// <param name="length">Length</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public readonly void ToNative(byte[] buffer, int offset, int length)
    {
        if (length < _hwAddrLen)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        for (var i = 0; i < length; i++)
        {
            if (i < 8)
            {
                buffer[offset + i] = (byte)(_hwAddr1 >> ((7 - i) * 8));
            }
            else
            {
                buffer[offset + i] = (byte)(_hwAddr2 >> ((15 - i) * 8));
            }
        }
    }

    internal readonly DhcpBinaryDataManaged ToNativeBinaryData() => new(_hwAddr1, _hwAddr2, _hwAddrLen);
    internal readonly DhcpClientUidManaged ToNativeClientUid() => new(_hwAddr1, _hwAddr2, _hwAddrLen);

    /// <summary>
    /// From native ethernet
    /// </summary>
    /// <param name="nativeHardwareAddress">Hardware address</param>
    /// <returns>DHCP server hardware address instance</returns>
    public static DhcpServerHardwareAddress FromNativeEthernet(ulong nativeHardwareAddress)
        => new(DhcpServerHardwareType.Ethernet, 6, nativeHardwareAddress, 0UL);

    /// <summary>
    /// From native ethernet
    /// </summary>
    /// <param name="hardwareAddress">Hardware address</param>
    /// <returns>DHCP server hardware address instance</returns>
    public static DhcpServerHardwareAddress FromNativeEthernet(byte[] hardwareAddress)
        => FromNative(DhcpServerHardwareType.Ethernet, hardwareAddress);

    /// <summary>
    /// From ethernet string
    /// </summary>
    /// <param name="hardwareAddress">Hardware address</param>
    /// <returns>DHCP server hardware address instance</returns>
    public static DhcpServerHardwareAddress FromEthernetString(string hardwareAddress)
    {
        if (string.IsNullOrWhiteSpace(hardwareAddress))
        {
            throw new ArgumentNullException(nameof(hardwareAddress));
        }

        var hwAddr1 = 0UL;
        var hwAddr2 = 0UL;
        int hwLength;

        if (hardwareAddress.Length < 3 || (hardwareAddress[2] != ':' && hardwareAddress[2] != '-'))
        {
            // no separator
            hwLength = hardwareAddress.Length / 2;

            if (hardwareAddress.Length % 2 != 0 || hwLength > MaximumLength)
            {
                throw new ArgumentOutOfRangeException(nameof(hardwareAddress), "Invalid hardware address format");
            }

            for (var i = 0; i < hwLength; i++)
            {
                if (!BitHelper.TryParseByteFromHexSubstring(hardwareAddress, i * 2, 2, out var octet))
                {
                    throw new ArgumentOutOfRangeException(nameof(hardwareAddress), "Invalid hardware address format");
                }

                if (i < 8)
                {
                    hwAddr1 |= ((ulong)octet) << (7 - i) * 8;
                }
                else
                {
                    hwAddr2 |= ((ulong)octet) << (15 - i) * 8;
                }
            }
        }
        else
        {
            // separator between octets
            hwLength = (hardwareAddress.Length + 1) / 3;

            if (hardwareAddress.Length % 3 != 2 || hwLength > MaximumLength)
            {
                throw new ArgumentOutOfRangeException(nameof(hardwareAddress), "Invalid hardware address format");
            }

            for (var i = 0; i < hwLength; i++)
            {
                if (!BitHelper.TryParseByteFromHexSubstring(hardwareAddress, i * 3, 2, out var octet))
                {
                    throw new ArgumentOutOfRangeException(nameof(hardwareAddress), "Invalid hardware address format");
                }

                if (i < 8)
                {
                    hwAddr1 |= ((ulong)octet) << (7 - i) * 8;
                }
                else
                {
                    hwAddr2 |= ((ulong)octet) << (15 - i) * 8;
                }
            }
        }

        return new DhcpServerHardwareAddress(DhcpServerHardwareType.Ethernet, (byte)hwLength, hwAddr1, hwAddr2);
    }

    internal static DhcpServerHardwareAddress FromNative(DhcpServerHardwareType type, byte hardwareAddressLength, ulong hardwareAddress1, ulong hardwareAddress2)
    {
        return new DhcpServerHardwareAddress(type, hardwareAddressLength, hardwareAddress1, hardwareAddress2);
    }

    /// <summary>
    /// From native hardware address
    /// </summary>
    /// <param name="type">Type</param>
    /// <param name="hardwareAddress">Hardware address</param>
    /// <returns>DHCP server hardware address instance</returns>
    public static DhcpServerHardwareAddress FromNative(DhcpServerHardwareType type, byte[] hardwareAddress)
    {
        if (hardwareAddress.Length > MaximumLength)
        {
            throw new ArgumentOutOfRangeException(nameof(hardwareAddress));
        }

        return new DhcpServerHardwareAddress(
            type,
            (byte)hardwareAddress.Length,
            Marshal(hardwareAddress, 0),
            hardwareAddress.Length > 8 ? Marshal(hardwareAddress, 8) : 0UL);
    }

    /// <summary>
    /// From native hardware address
    /// </summary>
    /// <param name="type">Type</param>
    /// <param name="buffer">Buffer with hardware address contained</param>
    /// <param name="hardwareAddressOffset">Hardware address offset</param>
    /// <param name="hardwareAddressLength">Hardware address length</param>
    /// <returns>DHCP server hardware address instance</returns>
    public static DhcpServerHardwareAddress FromNative(DhcpServerHardwareType type, byte[] buffer, int hardwareAddressOffset, int hardwareAddressLength)
    {
        if (hardwareAddressLength > MaximumLength)
        {
            throw new ArgumentOutOfRangeException(nameof(hardwareAddressLength));
        }
        if (buffer is null)
        {
            throw new ArgumentNullException(nameof(buffer));
        }
        if (buffer.Length < hardwareAddressOffset + hardwareAddressLength)
        {
            throw new ArgumentOutOfRangeException(nameof(hardwareAddressOffset));
        }

        return new DhcpServerHardwareAddress(
            type,
            (byte)hardwareAddressLength,
            Marshal(buffer, hardwareAddressOffset, hardwareAddressLength),
            hardwareAddressLength > 8 ? Marshal(buffer, hardwareAddressOffset + 8, hardwareAddressLength - 8) : 0UL);
    }

    /// <summary>
    /// From native hardware address
    /// </summary>
    /// <param name="type">Type</param>
    /// <param name="pointer">Hardware address pointer</param>
    /// <param name="length">Length of the hardware address</param>
    /// <returns>DHCP server hardware address instance</returns>
    public static DhcpServerHardwareAddress FromNative(DhcpServerHardwareType type, IntPtr pointer, int length)
    {
        if (pointer == IntPtr.Zero)
        {
            throw new ArgumentNullException(nameof(pointer));
        }
        if (length > MaximumLength)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        var hwAddr1 = 0UL;
        var hwAddr2 = 0UL;

        if (length == 0)
        {
            return new DhcpServerHardwareAddress(type, (byte)length, hwAddr1, hwAddr2);
        }

        hwAddr1 = (ulong)BitHelper.ReadIntVar(pointer, 0, length > 7 ? 8 : length);

        if (length > 8)
        {
            hwAddr2 = (ulong)BitHelper.ReadIntVar(pointer, 8, length - 8);
        }

        return new DhcpServerHardwareAddress(type, (byte)length, hwAddr1, hwAddr2);
    }

    private static ulong Marshal(byte[] address, int offset)
    {
        if (address is null)
        {
            throw new ArgumentNullException(nameof(address));
        }
        if (offset < 0 || address.Length == 0 || offset >= address.Length)
        {
            return 0UL;
        }

        var length = address.Length < offset + 8 ? address.Length - offset : 8;

        return Marshal(address, offset, length);
    }

    private static ulong Marshal(byte[] address, int offset, int length)
    {
        if (address is null)
        {
            throw new ArgumentNullException(nameof(address));
        }
        if (offset < 0 || address.Length == 0 || offset >= address.Length)
        {
            return 0UL;
        }

        if (length > 8)
        {
            length = 8;
        }

        var result = 0UL;

        for (var i = 0; i < length; i++)
        {
            result |= (ulong)address[offset + i] << ((7 - i) * 8);
        }

        return result;
    }

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
    public override int GetHashCode()
    {
        return
            (int)(_hwAddr1 >> 32) ^
            (int)(_hwAddr1 & 0xFFFFFFFF) ^
            (int)(_hwAddr2 >> 32) ^
            ((int)_hwAddrType << 24) ^
            (_hwAddrLen << 16);
    }

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

        if (obj is DhcpServerHardwareAddress ha)
        {
            return Equals(ha);
        }

        return false;
    }

    /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
    public bool Equals(DhcpServerHardwareAddress other)
    {
        return
            _hwAddrType == other._hwAddrType &&
            _hwAddrLen == other._hwAddrLen &&
            _hwAddr1 == other._hwAddr1 &&
            _hwAddr2 == other._hwAddr2;
    }

    /// <summary>
    /// == equal operator
    /// </summary>
    /// <param name="lhs">Left side address</param>
    /// <param name="rhs">Right side address</param>
    /// <returns>True if both side are equal</returns>
    public static bool operator ==(DhcpServerHardwareAddress lhs, DhcpServerHardwareAddress rhs) => lhs.Equals(rhs);

    /// <summary>
    /// != not equal operator
    /// </summary>
    /// <param name="lhs">Left side address</param>
    /// <param name="rhs">Right side address</param>
    /// <returns>True if both sides are not equal</returns>
    public static bool operator !=(DhcpServerHardwareAddress lhs, DhcpServerHardwareAddress rhs) => !lhs.Equals(rhs);

    /// <summary>
    /// Address to string operator
    /// </summary>
    /// <param name="address">DHCP server hardware address instance</param>
    public static implicit operator string(DhcpServerHardwareAddress address) => address.ToString();

    /// <summary>
    /// Ethernet address to string operator
    /// </summary>
    /// <param name="address">DHCP server hardware address instance</param>
    public static implicit operator DhcpServerHardwareAddress(string address) => FromEthernetString(address);

    /// <summary>Returns the fully qualified type name of this instance.</summary>
    /// <returns>The fully qualified type name.</returns>
    public override string ToString() => BitHelper.ReadHexString(_hwAddr1, _hwAddr2, 0, _hwAddrLen, ':');
}
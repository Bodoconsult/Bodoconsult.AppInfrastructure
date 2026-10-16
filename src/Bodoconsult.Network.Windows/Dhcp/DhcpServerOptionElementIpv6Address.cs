// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Runtime.InteropServices;
using Bodoconsult.Network.Windows.Dhcp.Native;

namespace Bodoconsult.Network.Windows.Dhcp;

/// <summary>
/// IPv6 address DHCP server option element
/// </summary>
public class DhcpServerOptionElementIpv6Address : DhcpServerOptionElement
{
    /// <summary>
    /// Type of the option element
    /// </summary>
    public override DhcpServerOptionElementType Type => DhcpServerOptionElementType.Ipv6Address;

    /// <summary>
    /// Option value
    /// </summary>
    public override object Value => RawValue;

    /// <summary>
    /// Option value formatted
    /// </summary>
    public override string ValueFormatted => RawValue;

    /// <summary>
    /// Raw option value
    /// </summary>
    public string RawValue { get; }

    internal DhcpServerOptionElementIpv6Address(string value)
    {
        RawValue = value;
    }

    internal static DhcpServerOptionElementIpv6Address ReadNative(DhcpOptionDataElement native)
        => new(Marshal.PtrToStringUni(native.Ipv6AddressDataOption));

    internal override DhcpOptionDataElementManaged ToNative() => new((DhcpOptionDataType)Type, RawValue);

    /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
    public override bool Equals(IDhcpServerOptionElement other)
    {
        if (other is null)
            return false;

        if (other is DhcpServerOptionElementIpv6Address oe)
            return string.Equals(RawValue, oe.RawValue, StringComparison.Ordinal);

        return false;
    }

    /// <summary>Serves as the default hash function.</summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
        => RawValue.GetHashCode();
}
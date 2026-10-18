// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Network.Windows.Dhcp.Native;
using Bodoconsult.Network.Windows.Interfaces;

namespace Bodoconsult.Network.Windows.Dhcp.Options;

/// <summary>
/// IP address DHCP server option element
/// </summary>
public class DhcpServerOptionElementIpAddress : DhcpServerOptionElement
{
    /// <summary>
    /// Type of the option element
    /// </summary>
    public override DhcpServerOptionElementType Type => DhcpServerOptionElementType.IpAddress;

    /// <summary>
    /// Option value
    /// </summary>
    public override object Value => RawValue;

    /// <summary>
    /// Option value formatted
    /// </summary>
    public override string ValueFormatted => _address;

    private readonly DhcpServerIpAddress _address;

    /// <summary>
    /// Raw option value
    /// </summary>
    public uint RawValue => _address.Native;

    internal DhcpServerOptionElementIpAddress(DhcpServerIpAddress value)
    {
        _address = value;
    }
    internal DhcpServerOptionElementIpAddress(DhcpIpAddress value)
        : this(value.AsNetworkToIpAddress()) { }

    internal static DhcpServerOptionElementIpAddress ReadNative(DhcpOptionDataElement native) =>
        new(native.IpAddressOption);

    internal override DhcpOptionDataElementManaged ToNative() => new(new DhcpIpAddress(RawValue));

    /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
    public override bool Equals(IDhcpServerOptionElement other)
    {
        if (other is null)
        {
            return false;
        }

        if (other is DhcpServerOptionElementIpAddress oe)
        {
            return _address == oe._address;
        }

        return false;
    }

    /// <summary>Serves as the default hash function.</summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
        => _address.GetHashCode();
}
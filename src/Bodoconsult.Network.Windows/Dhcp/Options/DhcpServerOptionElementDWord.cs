// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Network.Windows.Dhcp.Native;
using Bodoconsult.Network.Windows.Interfaces;

namespace Bodoconsult.Network.Windows.Dhcp.Options;

/// <summary>
/// DWord DHCP server option element
/// </summary>
public class DhcpServerOptionElementDWord : DhcpServerOptionElement
{
    /// <summary>
    /// Type of the option element
    /// </summary>
    public override DhcpServerOptionElementType Type => DhcpServerOptionElementType.DWord;

    /// <summary>
    /// Option value
    /// </summary>
    public override object Value => RawValue;

    /// <summary>
    /// Option value formatted
    /// </summary>
    public override string ValueFormatted => RawValue.ToString("N0");

    /// <summary>
    /// Raw option value
    /// </summary>
    public int RawValue { get; }

    internal DhcpServerOptionElementDWord(int value)
    {
        RawValue = value;
    }

    internal static DhcpServerOptionElementDWord ReadNative(DhcpOptionDataElement native) => new(native.DWordOption);

    internal override DhcpOptionDataElementManaged ToNative() => new(RawValue);

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

        if (other is DhcpServerOptionElementDWord oe)
        {
            return RawValue == oe.RawValue;
        }

        return false;
    }

    /// <summary>Serves as the default hash function.</summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
        => RawValue;
}
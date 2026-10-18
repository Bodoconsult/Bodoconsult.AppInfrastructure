// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Network.Windows.Dhcp.Native;
using Bodoconsult.Network.Windows.Helpers;
using Bodoconsult.Network.Windows.Interfaces;

namespace Bodoconsult.Network.Windows.Dhcp.Options;

/// <summary>
/// Binary DHCP server option element
/// </summary>
public class DhcpServerOptionElementBinary : DhcpServerOptionElement
{
    private readonly DhcpServerOptionElementType _type;

    /// <summary>
    /// Type of the option element
    /// </summary>
    public override DhcpServerOptionElementType Type => _type;

    /// <summary>
    /// Option value
    /// </summary>
    public override object Value => RawValue;

    /// <summary>
    /// Option value formatted
    /// </summary>
    public override string ValueFormatted => (RawValue is null) ? null : BitHelper.ReadHexString(RawValue, ' ');

    /// <summary>
    /// Raw option value
    /// </summary>
    public byte[] RawValue { get; }

    internal DhcpServerOptionElementBinary(DhcpServerOptionElementType type, byte[] value)
    {
        _type = type;
        RawValue = value;
    }

    internal static DhcpServerOptionElementBinary ReadNative(DhcpOptionDataElement native)
        => new((DhcpServerOptionElementType)native.OptionType, native.BinaryDataOption.Data);

    internal override DhcpOptionDataElementManaged ToNative()
        => new((DhcpOptionDataType)Type, new DhcpBinaryDataManaged(RawValue));

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

        if (other is DhcpServerOptionElementBinary oe)
        {
            if (RawValue is null && oe.RawValue is null)
            {
                return true;
            }

            if (RawValue is null || oe.RawValue is null)
            {
                return false;
            }

            if (RawValue.Length == 0 && oe.RawValue.Length == 0)
            {
                return true;
            }

            return RawValue.SequenceEqual(oe.RawValue);
        }

        return false;
    }

    /// <summary>Serves as the default hash function.</summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        var result = 0;
        if (RawValue != null && RawValue.Length > 0)
        {
            foreach (var b in RawValue)
                result = (result * 397) ^ b;
        }
        return result;
    }
}
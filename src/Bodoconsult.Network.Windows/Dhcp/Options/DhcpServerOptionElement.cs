// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.Network.Windows.Dhcp.Native;
using Bodoconsult.Network.Windows.Interfaces;

namespace Bodoconsult.Network.Windows.Dhcp.Options;

/// <summary>
/// DHCP server option element
/// </summary>
public abstract class DhcpServerOptionElement : IDhcpServerOptionElement
{
    /// <summary>
    /// Type of the option element
    /// </summary>
    public abstract DhcpServerOptionElementType Type { get; }

    /// <summary>
    /// Option value
    /// </summary>
    public abstract object Value { get; }

    /// <summary>
    /// Option value formatted
    /// </summary>
    public abstract string ValueFormatted { get; }

    internal static List<DhcpServerOptionElement> CreateElement(byte value) => [new DhcpServerOptionElementByte(value)];
    internal static List<DhcpServerOptionElement> CreateElement(List<byte> values)
        => values.Select(v => (DhcpServerOptionElement)new DhcpServerOptionElementByte(v)).ToList();
    internal static List<DhcpServerOptionElement> CreateElement(short value) =>
        [new DhcpServerOptionElementWord(value)];
    internal static List<DhcpServerOptionElement> CreateElement(List<short> values)
        => values.Select(v => (DhcpServerOptionElement)new DhcpServerOptionElementWord(v)).ToList();
    internal static List<DhcpServerOptionElement> CreateElement(int value) => [new DhcpServerOptionElementDWord(value)];
    internal static List<DhcpServerOptionElement> CreateElement(List<int> values)
        => values.Select(v => (DhcpServerOptionElement)new DhcpServerOptionElementDWord(v)).ToList();
    internal static List<DhcpServerOptionElement> CreateElement(long value) =>
        [new DhcpServerOptionElementDWordDWord(value)];
    internal static List<DhcpServerOptionElement> CreateElement(List<long> values)
        => values.Select(v => (DhcpServerOptionElement)new DhcpServerOptionElementDWordDWord(v)).ToList();
    internal static List<DhcpServerOptionElement> CreateElement(DhcpServerIpAddress value) =>
        [new DhcpServerOptionElementIpAddress(value)];
    internal static List<DhcpServerOptionElement> CreateElement(List<DhcpServerIpAddress> values)
        => values.Select(v => (DhcpServerOptionElement)new DhcpServerOptionElementIpAddress(v)).ToList();
    internal static List<DhcpServerOptionElement> CreateStringElement(string value)
        => [new DhcpServerOptionElementString(value)];
    internal static List<DhcpServerOptionElement> CreateStringElement(List<string> values)
        => values.Select(v => (DhcpServerOptionElement)new DhcpServerOptionElementString(v)).ToList();
    internal static List<DhcpServerOptionElement> CreateBinaryElement(byte[] value)
        => [new DhcpServerOptionElementBinary(DhcpServerOptionElementType.BinaryData, value)];
    internal static List<DhcpServerOptionElement> CreateBinaryElement(List<byte[]> values)
        => values.Select(v => (DhcpServerOptionElement)new DhcpServerOptionElementBinary(DhcpServerOptionElementType.BinaryData, v)).ToList();
    internal static List<DhcpServerOptionElement> CreateEncapsulatedElement(byte[] value)
        => [new DhcpServerOptionElementBinary(DhcpServerOptionElementType.EncapsulatedData, value)];
    internal static List<DhcpServerOptionElement> CreateEncapsulatedElement(List<byte[]> values)
        => values.Select(v => (DhcpServerOptionElement)new DhcpServerOptionElementBinary(DhcpServerOptionElementType.EncapsulatedData, v)).ToList();
    internal static List<DhcpServerOptionElement> CreateIpv6AddressElement(string value)
        => [new DhcpServerOptionElementIpv6Address(value)];
    internal static List<DhcpServerOptionElement> CreateIpv6AddressElement(List<string> values)
        => values.Select(v => (DhcpServerOptionElement)new DhcpServerOptionElementIpv6Address(v)).ToList();

    internal static IEnumerable<DhcpServerOptionElement> ReadNativeElements(DhcpOptionData elementArray)
    {
        foreach (var element in elementArray.Elements)
            yield return ReadNative(element);
    }

    internal static DhcpOptionDataManaged WriteNative(IEnumerable<IDhcpServerOptionElement> elements)
    {
        return new DhcpOptionDataManaged(elements.Select(e => ((DhcpServerOptionElement)e).ToNative()).ToArray());
    }

    private static DhcpServerOptionElement ReadNative(DhcpOptionDataElement element)
    {
        switch (element.OptionType)
        {
            case DhcpOptionDataType.DhcpByteOption:
                return DhcpServerOptionElementByte.ReadNative(element);
            case DhcpOptionDataType.DhcpWordOption:
                return DhcpServerOptionElementWord.ReadNative(element);
            case DhcpOptionDataType.DhcpDWordOption:
                return DhcpServerOptionElementDWord.ReadNative(element);
            case DhcpOptionDataType.DhcpDWordDWordOption:
                return DhcpServerOptionElementDWordDWord.ReadNative(element);
            case DhcpOptionDataType.DhcpIpAddressOption:
                return DhcpServerOptionElementIpAddress.ReadNative(element);
            case DhcpOptionDataType.DhcpStringDataOption:
                return DhcpServerOptionElementString.ReadNative(element);
            case DhcpOptionDataType.DhcpBinaryDataOption:
            case DhcpOptionDataType.DhcpEncapsulatedDataOption:
                return DhcpServerOptionElementBinary.ReadNative(element);
            case DhcpOptionDataType.DhcpIpv6AddressOption:
                return DhcpServerOptionElementIpv6Address.ReadNative(element);
            default:
                throw new InvalidCastException($"Unknown Option Data Type: {element.OptionType}");
        }
    }

    internal abstract DhcpOptionDataElementManaged ToNative();

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => $"{Type}: {ValueFormatted}";

    /// <summary>Serves as the default hash function.</summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        // GetHashCode must be overridden by all implementing classes
        return base.GetHashCode();
    }

    /// <summary>Determines whether the specified object is equal to the current object.</summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>
    /// <see langword="true" /> if the specified object  is equal to the current object; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj is DhcpServerOptionElement oe)
        {
            return Equals(oe);
        }

        return false;
    }

    /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
    public abstract bool Equals(IDhcpServerOptionElement other);
}
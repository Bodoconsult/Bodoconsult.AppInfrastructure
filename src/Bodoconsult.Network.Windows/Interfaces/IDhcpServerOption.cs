// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Network.Windows.Dhcp;
using Bodoconsult.Network.Windows.Dhcp.Options;

namespace Bodoconsult.Network.Windows.Interfaces;

/// <summary>
/// DHCP server option
/// </summary>
public interface IDhcpServerOption
{
    /// <summary>
    /// User Class associated with this Option
    /// </summary>
    IDhcpServerClass Class { get; }

    /// <summary>
    /// Name of the User Class associated with this Option
    /// </summary>
    string ClassName { get; }

    /// <summary>
    /// Comment about this Option
    /// </summary>
    string Comment { get; }

    /// <summary>
    /// Contains the data associated with this option
    /// </summary>
    IEnumerable<IDhcpServerOptionElement> DefaultValue { get; }

    /// <summary>
    /// True if this Option has a single data item associated with it, False when an array of data items are associated with it.
    /// </summary>
    bool IsUnaryOption { get; }

    /// <summary>
    /// Is this a vendor class option
    /// </summary>
    bool IsUserClassOption { get; }

    /// <summary>
    /// Is this a user class option
    /// </summary>
    bool IsVendorClassOption { get; }

    /// <summary>
    /// Name for this Option
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Unique ID number (also called a "code") for this Option
    /// </summary>
    int OptionId { get; }

    /// <summary>
    /// The associated DHCP server
    /// </summary>
    IDhcpServer Server { get; }

    /// <summary>
    /// Option element type
    /// </summary>
    DhcpServerOptionElementType ValueType { get; }

    /// <summary>
    /// Name of the Vendor Class associated with this Option
    /// </summary>
    string VendorName { get; }

    /// <summary>
    /// Create option value from byte
    /// </summary>
    /// <param name="value">Value to convert</param>
    /// <returns>DHCP server option value</returns>
    IDhcpServerOptionValue CreateOptionBinaryValue(byte[] value);

    /// <summary>
    /// Create option value from byte
    /// </summary>
    /// <param name="values">Values to convert</param>
    /// <returns>DHCP server option value</returns>
    IDhcpServerOptionValue CreateOptionBinaryValue(IEnumerable<byte[]> values);

    /// <summary>
    /// Create option value from byte
    /// </summary>
    /// <param name="values">Values to convert</param>
    /// <returns>DHCP server option value</returns>
    IDhcpServerOptionValue CreateOptionBinaryValue(params byte[][] values);

    /// <summary>
    /// Create option value from int16
    /// </summary>
    /// <param name="value">Value to convert</param>
    /// <returns>DHCP server option value</returns>
    IDhcpServerOptionValue CreateOptionByteValue(byte value);

    /// <summary>
    /// Create option value from byte
    /// </summary>
    /// <param name="values">Values to convert</param>
    /// <returns>DHCP server option value</returns>
    IDhcpServerOptionValue CreateOptionByteValue(IEnumerable<byte> values);

    /// <summary>
    /// Create option value from byte params
    /// </summary>
    /// <param name="values">Values to convert</param>
    /// <returns>DHCP server option value</returns>
    IDhcpServerOptionValue CreateOptionByteValue(params byte[] values);

    /// <summary>
    /// Create option value from encapsulated value
    /// </summary>
    /// <param name="value">Value to convert</param>
    /// <returns>DHCP server option value</returns>
    IDhcpServerOptionValue CreateOptionEncapsulatedValue(byte[] value);

    /// <summary>
    /// Create option value from encapsulated value
    /// </summary>
    /// <param name="values">Values to convert</param>
    /// <returns>DHCP server option value</returns>
    IDhcpServerOptionValue CreateOptionEncapsulatedValue(IEnumerable<byte[]> values);

    /// <summary>
    /// Create option value from encapsulated value params
    /// </summary>
    /// <param name="values">Values to convert</param>
    /// <returns>DHCP server option value</returns>
    IDhcpServerOptionValue CreateOptionEncapsulatedValue(params byte[][] values);

    /// <summary>
    /// Create option value from int16
    /// </summary>
    /// <param name="values">Values to convert</param>
    /// <returns>DHCP server option value</returns>
    IDhcpServerOptionValue CreateOptionInt16Value(IEnumerable<short> values);

    /// <summary>
    /// Create option value from int16 params
    /// </summary>
    /// <param name="values">Values to convert</param>
    /// <returns>DHCP server option value</returns>
    IDhcpServerOptionValue CreateOptionInt16Value(params short[] values);

    /// <summary>
    /// Create option value from int16
    /// </summary>
    /// <param name="value">Value to convert</param>
    /// <returns>DHCP server option value</returns>
    IDhcpServerOptionValue CreateOptionInt16Value(short value);

    /// <summary>
    /// Create option value from int32
    /// </summary>
    /// <param name="values">Values to convert</param>
    /// <returns>DHCP server option value</returns>
    IDhcpServerOptionValue CreateOptionInt32Value(IEnumerable<int> values);

    /// <summary>
    /// Create option value from int32
    /// </summary>
    /// <param name="value">Value to convert</param>
    /// <returns>DHCP server option value</returns>
    IDhcpServerOptionValue CreateOptionInt32Value(int value);

    /// <summary>
    /// Create option value from int32 params
    /// </summary>
    /// <param name="values">Values to convert</param>
    /// <returns>DHCP server option value</returns>
    IDhcpServerOptionValue CreateOptionInt32Value(params int[] values);

    /// <summary>
    /// Create option value from int64
    /// </summary>
    /// <param name="values">Values to convert</param>
    /// <returns>DHCP server option value</returns>
    IDhcpServerOptionValue CreateOptionInt64Value(IEnumerable<long> values);

    /// <summary>
    /// Create option value from long
    /// </summary>
    /// <param name="value">Value to convert</param>
    /// <returns>DHCP server option value</returns>
    IDhcpServerOptionValue CreateOptionInt64Value(long value);

    /// <summary>
    /// Create option value from int64 params
    /// </summary>
    /// <param name="values">Values to convert</param>
    /// <returns>DHCP server option value</returns>
    IDhcpServerOptionValue CreateOptionInt64Value(params long[] values);

    /// <summary>
    /// Create option value from IP address
    /// </summary>
    /// <param name="value">Value to convert</param>
    /// <returns>DHCP server option value</returns>
    IDhcpServerOptionValue CreateOptionIpAddressValue(DhcpServerIpAddress value);

    /// <summary>
    /// Create option value from IP address
    /// </summary>
    /// <param name="values">Values to convert</param>
    /// <returns>DHCP server option value</returns>
    IDhcpServerOptionValue CreateOptionIpAddressValue(IEnumerable<DhcpServerIpAddress> values);

    /// <summary>
    /// Create option value from IP address params
    /// </summary>
    /// <param name="values">Values to convert</param>
    /// <returns>DHCP server option value</returns>
    IDhcpServerOptionValue CreateOptionIpAddressValue(params DhcpServerIpAddress[] values);

    /// <summary>
    /// Create option value from IPv6 address
    /// </summary>
    /// <param name="values">Values to convert</param>
    /// <returns>DHCP server option value</returns>
    IDhcpServerOptionValue CreateOptionIpv6AddressValue(IEnumerable<string> values);

    /// <summary>
    /// Create option value from IPv6 address params
    /// </summary>
    /// <param name="values">Values to convert</param>
    /// <returns>DHCP server option value</returns>
    IDhcpServerOptionValue CreateOptionIpv6AddressValue(params string[] values);

    /// <summary>
    /// Create option value from IPv6 address
    /// </summary>
    /// <param name="value">Value to convert</param>
    /// <returns>DHCP server option value</returns>
    IDhcpServerOptionValue CreateOptionIpv6AddressValue(string value);

    /// <summary>
    /// Create option value from string
    /// </summary>
    /// <param name="values">Values to convert</param>
    /// <returns>DHCP server option value</returns>
    IDhcpServerOptionValue CreateOptionStringValue(IEnumerable<string> values);

    /// <summary>
    /// Create option value from string params
    /// </summary>
    /// <param name="values">Values to convert</param>
    /// <returns>DHCP server option value</returns>
    IDhcpServerOptionValue CreateOptionStringValue(params string[] values);

    /// <summary>
    /// Create option value from string
    /// </summary>
    /// <param name="value">Value to convert</param>
    /// <returns>DHCP server option value</returns>
    IDhcpServerOptionValue CreateOptionStringValue(string value);
}
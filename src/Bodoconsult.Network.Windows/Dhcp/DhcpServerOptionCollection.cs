// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Collections;

namespace Bodoconsult.Network.Windows.Dhcp;

/// <summary>
/// Collection of DHCP server options
/// </summary>
public class DhcpServerOptionCollection : IDhcpServerOptionCollection
{
    /// <summary>
    /// DHCP server instance
    /// </summary>
    public DhcpServer Server { get; }

    /// <summary>
    /// DHCP server instance
    /// </summary>
    IDhcpServer IDhcpServerOptionCollection.Server => Server;

    internal DhcpServerOptionCollection(DhcpServer server)
    {
        Server = server;
    }

    /// <summary>Returns an enumerator that iterates through the collection.</summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    public IEnumerator<IDhcpServerOption> GetEnumerator()
        => DhcpServerOption.GetAllOptions(Server).GetEnumerator();

    /// <summary>Returns an enumerator that iterates through a collection.</summary>
    /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    /// <summary>
    /// Get default options
    /// </summary>
    /// <returns>Options</returns>
    public IEnumerable<IDhcpServerOption> GetDefaultOptions()
        => DhcpServerOption.EnumDefaultOptions(Server);

    /// <summary>
    /// Get user options
    /// </summary>
    /// <param name="className">Class name</param>
    /// <returns>Options</returns>
    public IEnumerable<IDhcpServerOption> GetUserOptions(string className)
        => DhcpServerOption.EnumUserOptions(Server, className);

    /// <summary>
    /// Get vendor options
    /// </summary>
    /// <param name="vendorName">Vendor name</param>
    /// <returns>Vendor options</returns>
    public IEnumerable<IDhcpServerOption> GetVendorOptions(string vendorName)
        => DhcpServerOption.EnumVendorOptions(Server, vendorName);

    /// <summary>
    /// Queries the DHCP Server for the specified OptionId from the Default options
    /// </summary>
    /// <param name="optionId">The identifier for the option to retrieve</param>
    /// <returns>A <see cref="IDhcpServerOption"/>.</returns>
    public IDhcpServerOption GetDefaultOption(int optionId)
        => DhcpServerOption.GetDefaultOption(Server, optionId);
    /// <summary>
    /// Queries the DHCP Server for the specified OptionId from the Default options
    /// </summary>
    /// <param name="optionId">The identifier for the option to retrieve</param>
    /// <returns>A <see cref="IDhcpServerOption"/>.</returns>
    public IDhcpServerOption GetDefaultOption(DhcpServerOptionIds optionId)
        => DhcpServerOption.GetDefaultOption(Server, (int)optionId);

    /// <summary>
    /// Queries the DHCP Server for the specified OptionId within a User Class
    /// </summary>
    /// <param name="className">The name of the User Class to retrieve the Option from</param>
    /// <param name="optionId">The identifier for the option to retrieve</param>
    /// <returns>A <see cref="IDhcpServerOption"/>.</returns>
    public IDhcpServerOption GetUserOption(string className, int optionId)
        => DhcpServerOption.GetUserOption(Server, className, optionId);
    /// <summary>
    /// Queries the DHCP Server for the specified OptionId within a User Class
    /// </summary>
    /// <param name="className">The name of the User Class to retrieve the Option from</param>
    /// <param name="optionId">The identifier for the option to retrieve</param>
    /// <returns>A <see cref="IDhcpServerOption"/>.</returns>
    public IDhcpServerOption GetUserOption(string className, DhcpServerOptionIds optionId)
        => DhcpServerOption.GetUserOption(Server, className, (int)optionId);

    /// <summary>
    /// Queries the DHCP Server for the specified OptionId within a Vendor Class
    /// </summary>
    /// <param name="vendorName">The name of the Vendor Class to retrieve the Option from</param>
    /// <param name="optionId">The identifier for the option to retrieve</param>
    /// <returns>A <see cref="IDhcpServerOption"/>.</returns>
    public IDhcpServerOption GetVendorOption(string vendorName, int optionId)
        => DhcpServerOption.GetVendorOption(Server, vendorName, optionId);
    /// <summary>
    /// Queries the DHCP Server for the specified OptionId within a Vendor Class
    /// </summary>
    /// <param name="vendorName">The name of the Vendor Class to retrieve the Option from</param>
    /// <param name="optionId">The identifier for the option to retrieve</param>
    /// <returns>A <see cref="IDhcpServerOption"/>.</returns>
    public IDhcpServerOption GetVendorOption(string vendorName, DhcpServerOptionIds optionId)
        => DhcpServerOption.GetVendorOption(Server, vendorName, (int)optionId);

    /// <summary>
    /// Enumerates a list of all Global Option Values, including vendor/user class options, associated with the DHCP Server
    /// </summary>
    public IEnumerable<IDhcpServerOptionValue> GetOptionValues()
        => DhcpServerOptionValue.GetAllGlobalOptionValues(Server);

    /// <summary>
    /// Enumerates a list of Global Option Values associated with the DHCP Server
    /// </summary>
    public IEnumerable<IDhcpServerOptionValue> GetDefaultOptionValues()
        => DhcpServerOptionValue.EnumGlobalDefaultOptionValues(Server);

    /// <summary>
    /// Queries the DHCP Server for the specified OptionId Value from the Default options
    /// </summary>
    /// <param name="optionId">The identifier for the option value to retrieve</param>
    /// <returns>A <see cref="IDhcpServerOptionValue"/>.</returns>
    public IDhcpServerOptionValue GetDefaultOptionValue(int optionId)
        => DhcpServerOptionValue.GetGlobalDefaultOptionValue(Server, optionId);
    /// <summary>
    /// Queries the DHCP Server for the specified OptionId Value from the Default options
    /// </summary>
    /// <param name="optionId">The identifier for the option value to retrieve</param>
    /// <returns>A <see cref="IDhcpServerOptionValue"/>.</returns>
    public IDhcpServerOptionValue GetDefaultOptionValue(DhcpServerOptionIds optionId)
        => DhcpServerOptionValue.GetGlobalDefaultOptionValue(Server, (int)optionId);

    /// <summary>
    /// Queries the DHCP Server for the specified OptionId Value within a User Class
    /// </summary>
    /// <param name="className">The name of the User Class to retrieve the Option from</param>
    /// <param name="optionId">The identifier for the option value to retrieve</param>
    /// <returns>A <see cref="IDhcpServerOptionValue"/>.</returns>
    public IDhcpServerOptionValue GetUserOptionValue(string className, int optionId)
        => DhcpServerOptionValue.GetGlobalUserOptionValue(Server, className, optionId);
    /// <summary>
    /// Queries the DHCP Server for the specified OptionId Value within a User Class
    /// </summary>
    /// <param name="className">The name of the User Class to retrieve the Option from</param>
    /// <param name="optionId">The identifier for the option value to retrieve</param>
    /// <returns>A <see cref="IDhcpServerOptionValue"/>.</returns>
    public IDhcpServerOptionValue GetUserOptionValue(string className, DhcpServerOptionIds optionId)
        => DhcpServerOptionValue.GetGlobalUserOptionValue(Server, className, (int)optionId);

    /// <summary>
    /// Get vendor option value
    /// </summary>
    /// <param name="vendorName">Vendor name</param>
    /// <param name="optionId">Option ID</param>
    /// <returns>Vendor option value</returns>
    public IDhcpServerOptionValue GetVendorOptionValue(string vendorName, int optionId)
        => DhcpServerOptionValue.GetGlobalVendorOptionValue(Server, vendorName, optionId);

    /// <summary>
    /// Get vendor option value
    /// </summary>
    /// <param name="vendorName">Vendor name</param>
    /// <param name="optionId">Option ID</param>
    /// <returns>Vendor option value</returns>
    public IDhcpServerOptionValue GetVendorOptionValue(string vendorName, DhcpServerOptionIds optionId)
        => DhcpServerOptionValue.GetGlobalVendorOptionValue(Server, vendorName, (int)optionId);
}
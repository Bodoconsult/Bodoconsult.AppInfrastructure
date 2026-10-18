// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.Network.Windows.Dhcp.Options;

namespace Bodoconsult.Network.Windows.Interfaces;

/// <summary>
/// Collection of DHCP server options
/// </summary>
public interface IDhcpServerOptionCollection : IEnumerable<IDhcpServerOption>
{
    /// <summary>
    /// DHCP server instance
    /// </summary>
    IDhcpServer Server { get; }

    /// <summary>
    /// Get default option
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <returns>Option</returns>
    IDhcpServerOption GetDefaultOption(DhcpServerOptionIds optionId);

    /// <summary>
    /// Get default option
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <returns>Option</returns>
    IDhcpServerOption GetDefaultOption(int optionId);

    /// <summary>
    /// Get default options
    /// </summary>
    /// <returns>Options</returns>
    IEnumerable<IDhcpServerOption> GetDefaultOptions();

    /// <summary>
    /// Get default option value
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <returns>Option value</returns>
    IDhcpServerOptionValue GetDefaultOptionValue(DhcpServerOptionIds optionId);

    /// <summary>
    /// Get default option value
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <returns>Option value</returns>
    IDhcpServerOptionValue GetDefaultOptionValue(int optionId);

    /// <summary>
    /// Get default option values
    /// </summary>
    /// <returns>Option values</returns>
    IEnumerable<IDhcpServerOptionValue> GetDefaultOptionValues();

    /// <summary>
    /// Get option values
    /// </summary>
    /// <returns>Options values</returns>
    IEnumerable<IDhcpServerOptionValue> GetOptionValues();

    /// <summary>
    /// Get user option
    /// </summary>
    /// <param name="className">Class name</param>
    /// <param name="optionId">Option ID</param>
    /// <returns>Option</returns>
    IDhcpServerOption GetUserOption(string className, DhcpServerOptionIds optionId);

    /// <summary>
    /// Get user option
    /// </summary>
    /// <param name="className">Class name</param>
    /// <param name="optionId">Option ID</param>
    /// <returns>Option</returns>
    IDhcpServerOption GetUserOption(string className, int optionId);

    /// <summary>
    /// Get user options
    /// </summary>
    /// <param name="className">Class name</param>
    /// <returns>Options</returns>
    IEnumerable<IDhcpServerOption> GetUserOptions(string className);

    /// <summary>
    /// Get user option value
    /// </summary>
    /// <param name="className">Class name</param>
    /// <param name="optionId">Option ID</param>
    /// <returns>Option</returns>
    IDhcpServerOptionValue GetUserOptionValue(string className, DhcpServerOptionIds optionId);

    /// <summary>
    /// Get user option value
    /// </summary>
    /// <param name="className">Class name</param>
    /// <param name="optionId">Option ID</param>
    /// <returns>Option</returns>
    IDhcpServerOptionValue GetUserOptionValue(string className, int optionId);

    /// <summary>
    /// Get vendor option
    /// </summary>
    /// <param name="vendorName">Vendor name</param>
    /// <param name="optionId">Option ID</param>
    /// <returns>Vendor option</returns>
    IDhcpServerOption GetVendorOption(string vendorName, DhcpServerOptionIds optionId);

    /// <summary>
    /// Get vendor option
    /// </summary>
    /// <param name="vendorName">Vendor name</param>
    /// <param name="optionId">Option ID</param>
    /// <returns>Vendor option</returns>
    IDhcpServerOption GetVendorOption(string vendorName, int optionId);

    /// <summary>
    /// Get vendor options
    /// </summary>
    /// <param name="vendorName">Vendor name</param>
    /// <returns>Vendor options</returns>
    IEnumerable<IDhcpServerOption> GetVendorOptions(string vendorName);

    /// <summary>
    /// Get vendor option value
    /// </summary>
    /// <param name="vendorName">Vendor name</param>
    /// <param name="optionId">Option ID</param>
    /// <returns>Vendor option value</returns>
    IDhcpServerOptionValue GetVendorOptionValue(string vendorName, DhcpServerOptionIds optionId);

    /// <summary>
    /// Get vendor option value
    /// </summary>
    /// <param name="vendorName">Vendor name</param>
    /// <param name="optionId">Option ID</param>
    /// <returns>Vendor option value</returns>
    IDhcpServerOptionValue GetVendorOptionValue(string vendorName, int optionId);
}
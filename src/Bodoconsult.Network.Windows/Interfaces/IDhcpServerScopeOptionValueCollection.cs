// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Network.Windows.Dhcp.Options;

namespace Bodoconsult.Network.Windows.Interfaces;

/// <summary>
/// A cllection of option values for DHCP scope
/// </summary>
public interface IDhcpServerScopeOptionValueCollection : IEnumerable<IDhcpServerOptionValue>
{
    /// <summary>
    /// The associated DHCP scope
    /// </summary>
    IDhcpServerScope Scope { get; }

    /// <summary>
    /// The associated DHCP server
    /// </summary>
    IDhcpServer Server { get; }

    /// <summary>
    /// Add or set an option value
    /// </summary>
    /// <param name="value">Option value</param>
    void AddOrSetOptionValue(IDhcpServerOptionValue value);

    /// <summary>
    /// Get Default Option Value associated with the DHCP Scope
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <returns>Option value</returns>
    IDhcpServerOptionValue GetDefaultOptionValue(DhcpServerOptionIds optionId);

    /// <summary>
    /// Get Default Option Value associated with the DHCP Scope
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <returns>Option value</returns>
    IDhcpServerOptionValue GetDefaultOptionValue(int optionId);

    /// <summary>
    /// Enumerates a list of Default Option Values associated with the DHCP Scope
    /// </summary>
    /// <returns>Option values</returns>
    IEnumerable<IDhcpServerOptionValue> GetDefaultOptionValues();

    /// <summary>
    /// Retrieves the Option Value associated with the Option and Scope
    /// </summary>
    /// <param name="option">The associated option to retrieve the option value for</param>
    /// <returns>A <see cref="DhcpServerOptionValue"/>.</returns>
    IDhcpServerOptionValue GetOptionValue(IDhcpServerOption option);

    /// <summary>
    /// Retrieves the Option Value associated with the Option and Scope within a User Class
    /// </summary>
    /// <param name="className">The name of the User Class to retrieve the Option from</param>
    /// <param name="optionId">The identifier for the option value to retrieve</param>
    /// <returns>A <see cref="DhcpServerOptionValue"/>.</returns>
    IDhcpServerOptionValue GetUserOptionValue(string className, DhcpServerOptionIds optionId);

    /// <summary>
    /// Retrieves the Option Value associated with the Option and Scope within a User Class
    /// </summary>
    /// <param name="className">The name of the User Class to retrieve the Option from</param>
    /// <param name="optionId">The identifier for the option value to retrieve</param>
    /// <returns>A <see cref="DhcpServerOptionValue"/>.</returns>
    IDhcpServerOptionValue GetUserOptionValue(string className, int optionId);

    /// <summary>
    /// Retrieves the Option Values associated with the Option and Scope within a User Class
    /// </summary>
    /// <param name="className">The name of the User Class to retrieve the Option from</param>
    /// <returns>An enumerable of <see cref="DhcpServerOptionValue"/>.</returns>
    IEnumerable<IDhcpServerOptionValue> GetUserOptionValues(string className);

    /// <summary>
    /// Retrieves the Option Value associated with the Option and Scope within a Vendor Class
    /// </summary>
    /// <param name="vendorName">The name of the Vendor Class to retrieve the Option from</param>
    /// <param name="optionId">The identifier for the option value to retrieve</param>
    /// <returns>A <see cref="DhcpServerOptionValue"/>.</returns>
    IDhcpServerOptionValue GetVendorOptionValue(string vendorName, DhcpServerOptionIds optionId);

    /// <summary>
    /// Retrieves the Option Value associated with the Option and Scope within a Vendor Class
    /// </summary>
    /// <param name="vendorName">The name of the Vendor Class to retrieve the Option from</param>
    /// <param name="optionId">The identifier for the option value to retrieve</param>
    /// <returns>A <see cref="DhcpServerOptionValue"/>.</returns>
    IDhcpServerOptionValue GetVendorOptionValue(string vendorName, int optionId);

    /// <summary>
    /// Get vendor option values
    /// </summary>
    /// <param name="vendorName">Vendor name</param>
    /// <returns>Option values</returns>
    IEnumerable<IDhcpServerOptionValue> GetVendorOptionValues(string vendorName);

    /// <summary>
    /// Remove an option value
    /// </summary>
    /// <param name="optionId">Option ID</param>
    void RemoveOptionValue(DhcpServerOptionIds optionId);

    /// <summary>
    /// Remove an option value
    /// </summary>
    /// <param name="value">Option value</param>
    void RemoveOptionValue(IDhcpServerOptionValue value);

    /// <summary>
    /// Remove an option value
    /// </summary>
    /// <param name="optionId">Option ID</param>
    void RemoveOptionValue(int optionId);

    /// <summary>
    /// Retrieves the Option Value associated with the Option and Scope within a User Class
    /// </summary>
    /// <param name="className">The name of the User Class to retrieve the Option from</param>
    /// <param name="optionId">The identifier for the option value to retrieve</param>
    /// <returns>A <see cref="DhcpServerOptionValue"/>.</returns>
    void RemoveUserOptionValue(string className, DhcpServerOptionIds optionId);

    /// <summary>
    /// Deletes the Option Value associated with the Option and Scope within a User Class
    /// </summary>
    /// <param name="className">The name of the User Class</param>
    /// <param name="optionId">The identifier for the option value</param>
    void RemoveUserOptionValue(string className, int optionId);

    /// <summary>
    /// Retrieves the Option Value associated with the Option and Scope within a Vendor Class
    /// </summary>
    /// <param name="vendorName">The name of the Vendor Class to retrieve the Option from</param>
    /// <param name="optionId">The identifier for the option value to retrieve</param>
    /// <returns>A <see cref="DhcpServerOptionValue"/>.</returns>
    void RemoveVendorOptionValue(string vendorName, DhcpServerOptionIds optionId);

    /// <summary>
    /// Retrieves the Option Value associated with the Option and Scope within a Vendor Class
    /// </summary>
    /// <param name="vendorName">The name of the Vendor Class to retrieve the Option from</param>
    /// <param name="optionId">The identifier for the option value to retrieve</param>
    /// <returns>A <see cref="DhcpServerOptionValue"/>.</returns>
    void RemoveVendorOptionValue(string vendorName, int optionId);

    /// <summary>
    /// Set an option value
    /// </summary>
    /// <param name="value">Option value</param>
    void SetOptionValue(IDhcpServerOptionValue value);
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.Network.Windows.Dhcp.Options;
using Bodoconsult.Network.Windows.Interfaces;
using System.Collections;

namespace Bodoconsult.Network.Windows.Dhcp.Scope;

/// <summary>
/// Collection of option values for a DHCP scope
/// </summary>
public class DhcpServerScopeOptionValueCollection : IDhcpServerScopeOptionValueCollection
{
    /// <summary>
    /// The associated DHCP server
    /// </summary>
    public DhcpServer Server { get; }

    /// <summary>
    /// The associated DHCP server
    /// </summary>
    IDhcpServer IDhcpServerScopeOptionValueCollection.Server => Server;

    /// <summary>
    /// The associated DHCP scope
    /// </summary>
    public DhcpServerScope Scope { get; }

    /// <summary>
    /// The associated DHCP scope
    /// </summary>
    IDhcpServerScope IDhcpServerScopeOptionValueCollection.Scope => Scope;

    internal DhcpServerScopeOptionValueCollection(DhcpServerScope scope)
    {
        Server = scope.Server;
        Scope = scope;
    }

    /// <summary>
    /// Enumerates a list of All Option Values, including vendor/user class option values, associated with the DHCP Scope
    /// </summary>
    public IEnumerator<IDhcpServerOptionValue> GetEnumerator()
        => DhcpServerOptionValue.GetAllScopeOptionValues(Scope).GetEnumerator();

    /// <summary>
    /// Enumerates a list of All Option Values, including vendor/user class option values, associated with the DHCP Scope
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();


    /// <summary>
    /// Enumerates a list of Default Option Values associated with the DHCP Scope
    /// </summary>
    /// <returns>Option values</returns>
    public IEnumerable<IDhcpServerOptionValue> GetDefaultOptionValues()
        => DhcpServerOptionValue.EnumScopeDefaultOptionValues(Scope);

    /// <summary>
    /// Get user option values
    /// </summary>
    /// <param name="className">Class name</param>
    /// <returns>Option values</returns>
    public IEnumerable<IDhcpServerOptionValue> GetUserOptionValues(string className)
        => DhcpServerOptionValue.EnumScopeUserOptionValues(Scope, className);

    /// <summary>
    /// Get vendor option values
    /// </summary>
    /// <param name="vendorName">Vendor name</param>
    /// <returns>Option values</returns>
    public IEnumerable<IDhcpServerOptionValue> GetVendorOptionValues(string vendorName)
        => DhcpServerOptionValue.EnumScopeVendorOptionValues(Scope, vendorName);

    /// <summary>
    /// Retrieves the Option Value associated with the Option and Scope
    /// </summary>
    /// <param name="option">The associated option to retrieve the option value for</param>
    /// <returns>A <see cref="DhcpServerOptionValue"/>.</returns>
    public IDhcpServerOptionValue GetOptionValue(IDhcpServerOption option) => ((DhcpServerOption)option).GetScopeValue(Scope);

    /// <summary>
    /// Retrieves the Option Value associated with the Option and Scope from the Default options
    /// </summary>
    /// <param name="optionId">The identifier for the option value to retrieve</param>
    /// <returns>A <see cref="DhcpServerOptionValue"/>.</returns>
    public IDhcpServerOptionValue GetDefaultOptionValue(int optionId)
        => DhcpServerOptionValue.GetScopeDefaultOptionValue(Scope, optionId);
    /// <summary>
    /// Retrieves the Option Value associated with the Option and Scope from the Default options
    /// </summary>
    /// <param name="optionId">The identifier for the option value to retrieve</param>
    /// <returns>A <see cref="DhcpServerOptionValue"/>.</returns>
    public IDhcpServerOptionValue GetDefaultOptionValue(DhcpServerOptionIds optionId)
        => DhcpServerOptionValue.GetScopeDefaultOptionValue(Scope, (int)optionId);

    /// <summary>
    /// Retrieves the Option Value associated with the Option and Scope within a User Class
    /// </summary>
    /// <param name="className">The name of the User Class to retrieve the Option from</param>
    /// <param name="optionId">The identifier for the option value to retrieve</param>
    /// <returns>A <see cref="DhcpServerOptionValue"/>.</returns>
    public IDhcpServerOptionValue GetUserOptionValue(string className, int optionId)
        => DhcpServerOptionValue.GetScopeUserOptionValue(Scope, optionId, className);

    /// <summary>
    /// Retrieves the Option Value associated with the Option and Scope within a User Class
    /// </summary>
    /// <param name="className">The name of the User Class to retrieve the Option from</param>
    /// <param name="optionId">The identifier for the option value to retrieve</param>
    /// <returns>A <see cref="DhcpServerOptionValue"/>.</returns>
    public IDhcpServerOptionValue GetUserOptionValue(string className, DhcpServerOptionIds optionId)
        => DhcpServerOptionValue.GetScopeUserOptionValue(Scope, (int)optionId, className);

    /// <summary>
    /// Retrieves the Option Value associated with the Option and Scope within a Vendor Class
    /// </summary>
    /// <param name="vendorName">The name of the Vendor Class to retrieve the Option from</param>
    /// <param name="optionId">The identifier for the option value to retrieve</param>
    /// <returns>A <see cref="DhcpServerOptionValue"/>.</returns>
    public IDhcpServerOptionValue GetVendorOptionValue(string vendorName, int optionId)
        => DhcpServerOptionValue.GetScopeVendorOptionValue(Scope, optionId, vendorName);

    /// <summary>
    /// Retrieves the Option Value associated with the Option and Scope within a Vendor Class
    /// </summary>
    /// <param name="vendorName">The name of the Vendor Class to retrieve the Option from</param>
    /// <param name="optionId">The identifier for the option value to retrieve</param>
    /// <returns>A <see cref="DhcpServerOptionValue"/>.</returns>
    public IDhcpServerOptionValue GetVendorOptionValue(string vendorName, DhcpServerOptionIds optionId)
        => DhcpServerOptionValue.GetScopeVendorOptionValue(Scope, (int)optionId, vendorName);

    /// <summary>
    /// Set an option value
    /// </summary>
    /// <param name="value">Option value</param>
    public void SetOptionValue(IDhcpServerOptionValue value)
        => DhcpServerOptionValue.SetScopeOptionValue(Scope, (DhcpServerOptionValue)value);

    /// <summary>
    /// Add or set an option value
    /// </summary>
    /// <param name="value">Option value</param>
    public void AddOrSetOptionValue(IDhcpServerOptionValue value)
        => DhcpServerOptionValue.SetScopeOptionValue(Scope, (DhcpServerOptionValue)value);

    /// <summary>
    /// Deletes the Option Value associated with the Option and Scope within a User Class
    /// </summary>
    /// <param name="className">The name of the User Class</param>
    /// <param name="optionId">The identifier for the option value</param>
    public void RemoveUserOptionValue(string className, int optionId)
        => DhcpServerOptionValue.DeleteScopeUserOptionValue(Scope, optionId, className);

    /// <summary>
    /// Retrieves the Option Value associated with the Option and Scope within a User Class
    /// </summary>
    /// <param name="className">The name of the User Class to retrieve the Option from</param>
    /// <param name="optionId">The identifier for the option value to retrieve</param>
    /// <returns>A <see cref="DhcpServerOptionValue"/>.</returns>
    public void RemoveUserOptionValue(string className, DhcpServerOptionIds optionId)
        => DhcpServerOptionValue.DeleteScopeUserOptionValue(Scope, (int)optionId, className);

    /// <summary>
    /// Retrieves the Option Value associated with the Option and Scope within a Vendor Class
    /// </summary>
    /// <param name="vendorName">The name of the Vendor Class to retrieve the Option from</param>
    /// <param name="optionId">The identifier for the option value to retrieve</param>
    /// <returns>A <see cref="DhcpServerOptionValue"/>.</returns>
    public void RemoveVendorOptionValue(string vendorName, int optionId)
        => DhcpServerOptionValue.DeleteScopeVendorOptionValue(Scope, optionId, vendorName);

    /// <summary>
    /// Retrieves the Option Value associated with the Option and Scope within a Vendor Class
    /// </summary>
    /// <param name="vendorName">The name of the Vendor Class to retrieve the Option from</param>
    /// <param name="optionId">The identifier for the option value to retrieve</param>
    /// <returns>A <see cref="DhcpServerOptionValue"/>.</returns>
    public void RemoveVendorOptionValue(string vendorName, DhcpServerOptionIds optionId)
        => DhcpServerOptionValue.DeleteScopeVendorOptionValue(Scope, (int)optionId, vendorName);
    
    /// <summary>
    /// Remove an option value
    /// </summary>
    /// <param name="optionId">Option ID</param>
    public void RemoveOptionValue(int optionId)
        => DhcpServerOptionValue.DeleteScopeOptionValue(Scope, optionId);

    /// <summary>
    /// Remove an option value
    /// </summary>
    /// <param name="optionId">Option ID</param>
    public void RemoveOptionValue(DhcpServerOptionIds optionId)
        => DhcpServerOptionValue.DeleteScopeOptionValue(Scope, (int)optionId);

    /// <summary>
    /// Remove an option value
    /// </summary>
    /// <param name="value">Option value</param>
    public void RemoveOptionValue(IDhcpServerOptionValue value)
        => DhcpServerOptionValue.DeleteScopeOptionValue(Scope, (DhcpServerOptionValue)value);

}
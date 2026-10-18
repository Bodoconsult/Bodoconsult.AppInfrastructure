// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Network.Windows.Dhcp.Options;

namespace Bodoconsult.Network.Windows.Interfaces;

/// <summary>
/// Interface for reservation related options value collections
/// </summary>
public interface IDhcpServerScopeReservationOptionValueCollection : IEnumerable<IDhcpServerOptionValue>
{
    /// <summary>
    ///  The associated reservation
    /// </summary>
    IDhcpServerScopeReservation Reservation { get; }

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
    /// Get the default option value
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <returns>Option value</returns>
    IDhcpServerOptionValue GetDefaultOptionValue(DhcpServerOptionIds optionId);

    /// <summary>
    /// Get the default option value
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <returns>Option value</returns>
    IDhcpServerOptionValue GetDefaultOptionValue(int optionId);

    /// <summary>
    /// Get the default option values
    /// </summary>
    /// <returns>Option values</returns>
    IEnumerable<IDhcpServerOptionValue> GetDefaultOptionValues();

    /// <summary>
    /// Get the default option value
    /// </summary>
    /// <param name="option">Option</param>
    /// <returns>Option value</returns>
    IDhcpServerOptionValue GetOptionValue(IDhcpServerOption option);

    /// <summary>
    /// Get the user option value
    /// </summary>
    /// <param name="className">Class name</param>
    /// <param name="optionId">Option ID</param>
    /// <returns>Option value</returns>
    IDhcpServerOptionValue GetUserOptionValue(string className, DhcpServerOptionIds optionId);

    /// <summary>
    /// Get the user option value
    /// </summary>
    /// <param name="className">Class name</param>
    /// <param name="optionId">Option ID</param>
    /// <returns>Option value</returns>
    IDhcpServerOptionValue GetUserOptionValue(string className, int optionId);

    /// <summary>
    /// Get the vendore option values
    /// </summary>
    /// <param name="vendorName">Class name</param>
    /// <returns>Option values</returns>
    IEnumerable<IDhcpServerOptionValue> GetUserOptionValues(string vendorName);

    /// <summary>
    /// Get the user option value
    /// </summary>
    /// <param name="vendorName">Class name</param>
    /// <param name="optionId">Option ID</param>
    /// <returns>Option value</returns>
    IDhcpServerOptionValue GetVendorOptionValue(string vendorName, DhcpServerOptionIds optionId);

    /// <summary>
    /// Get the user option value
    /// </summary>
    /// <param name="vendorName">Class name</param>
    /// <param name="optionId">Option ID</param>
    /// <returns>Option value</returns>
    IDhcpServerOptionValue GetVendorOptionValue(string vendorName, int optionId);

    /// <summary>
    /// Get the user option values
    /// </summary>
    /// <param name="vendorName">Class name</param>
    /// <returns>Option values</returns>
    IEnumerable<IDhcpServerOptionValue> GetVendorOptionValues(string vendorName);

    /// <summary>
    /// Remove option value
    /// </summary>
    /// <param name="optionId">Option ID</param>
    void RemoveOptionValue(DhcpServerOptionIds optionId);

    /// <summary>
    /// Remove option value
    /// </summary>
    /// <param name="value">Option value</param>
    void RemoveOptionValue(IDhcpServerOptionValue value);

    /// <summary>
    /// Remove option value
    /// </summary>
    /// <param name="optionId">Option ID</param>
    void RemoveOptionValue(int optionId);

    /// <summary>
    /// Remove user option value
    /// </summary>
    /// <param name="className">Class name</param>
    /// <param name="optionId">Option ID</param>
    void RemoveUserOptionValue(string className, DhcpServerOptionIds optionId);

    /// <summary>
    /// Remove user option value
    /// </summary>
    /// <param name="className">Class name</param>
    /// <param name="optionId">Option ID</param>
    void RemoveUserOptionValue(string className, int optionId);

    /// <summary>
    /// Remove vendor option value
    /// </summary>
    /// <param name="vendorName">Vendor name</param>
    /// <param name="optionId">Option ID</param>
    void RemoveVendorOptionValue(string vendorName, DhcpServerOptionIds optionId);

    /// <summary>
    /// Remove vendor option value
    /// </summary>
    /// <param name="vendorName">Vendor name</param>
    /// <param name="optionId">Option ID</param>
    void RemoveVendorOptionValue(string vendorName, int optionId);

    /// <summary>
    /// Set an option value
    /// </summary>
    /// <param name="value">Value to set</param>
    void SetOptionValue(IDhcpServerOptionValue value);
}
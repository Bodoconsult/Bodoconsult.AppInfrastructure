// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Network.Windows.Interfaces;

/// <summary>
/// DHCP option class
/// </summary>
public interface IDhcpServerClass
{
    /// <summary>
    /// Comment
    /// </summary>
    string Comment { get; }

    /// <summary>
    /// A byte buffer that contains specific data for the class
    /// </summary>
    byte[] Data { get; }

    /// <summary>
    /// An ASCII representation of the <see cref="Data"/> buffer.
    /// </summary>
    string DataText { get; }

    /// <summary>
    /// Enumerates a list of all Global Option Values associated with this class
    /// </summary>
    IEnumerable<IDhcpServerOptionValue> GlobalOptionValues { get; }

    /// <summary>
    /// Indicates whether or not the options are user-specific
    /// </summary>
    bool IsUserClass { get; }

    /// <summary>
    /// Indicates whether or not the options are vendor-specific
    /// </summary>
    bool IsVendorClass { get; }

    /// <summary>
    /// Name of the server class
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Enumerates a list of all Options associated with this class
    /// </summary>
    IEnumerable<IDhcpServerOption> Options { get; }

    /// <summary>
    /// The associated DHCP Server
    /// </summary>
    IDhcpServer Server { get; }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.Network.Windows.Dhcp;

namespace Bodoconsult.Network.Windows.Interfaces;

/// <summary>
/// DHCP server configuration
/// </summary>
public interface IDhcpServerConfiguration
{
    /// <summary>
    /// Specifies a set of bit flags that contain the RPC protocols supported by the DHCP server.
    /// </summary>
    DhcpServerApiProtocol ApiProtocolSupport { get; }

    /// <summary>
    /// Unicode string that specifies the absolute path and file name of the backup client lease JET database.
    /// </summary>
    TimeSpan BackupInterval { get; }

    /// <summary>
    /// Unicode string that specifies the absolute path and file name of the backup client lease JET database.
    /// </summary>
    string BackupPath { get; }

    /// <summary>
    /// Specifies the interval between backups of the client lease database.
    /// </summary>
    TimeSpan DatabaseCleanupInterval { get; }

    /// <summary>
    /// Specifies a bit flag that indicates whether or not database actions should be logged.
    /// </summary>
    bool DatabaseLoggingEnabled { get; }

    /// <summary>
    /// Unicode string that specifies the file name of the client lease JET database.
    /// </summary>
    string DatabaseName { get; }

    /// <summary>
    /// Unicode string that specifies the absolute path to DatabaseName.
    /// </summary>
    string DatabasePath { get; }

    /// <summary>
    /// The associated DHCP server
    /// </summary>
    IDhcpServer Server { get; }
}
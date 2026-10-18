// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Network.Windows.Interfaces;

/// <summary>
/// Interface for DHCP server autif logs
/// </summary>
public interface IDhcpServerAuditLog
{
    /// <summary>
    /// The directory where the audit log is stored as an absolute path within the file system.
    /// </summary>
    string AuditLogDirectory { get; }

    /// <summary>
    /// The disk check interval for attempting to write the audit log to the specified file as the number of logged DHCP server events that should occur between checks.
    /// </summary>
    int DiskCheckInterval { get; }

    /// <summary>
    /// The maximum log file size, in bytes.
    /// </summary>
    int MaxLogFilesSize { get; }

    /// <summary>
    /// The minimum required disk space, in bytes, for audit log storage.
    /// </summary>
    int MinSpaceOnDisk { get; }

    /// <summary>
    /// The associated DHCP Server
    /// </summary>
    IDhcpServer Server { get; }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for creating backup manager instances handling advanced file system backups
/// </summary>
public interface IBackupManagerFactory
{
    /// <summary>
    /// Create a <see cref="IBackupManager"/> instance
    /// </summary>
    /// <param name="backupConfig">Backup configuration</param>
    /// <returns><see cref="IBackupManager"/> instance</returns>
    IBackupManager CreateInstance(IBackupConfig backupConfig);
}
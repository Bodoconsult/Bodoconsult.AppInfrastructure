// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Windows.Backup;

/// <summary>
/// Factory for creating a <see cref="BackupManager"/> instance
/// </summary>
public class BackupManagerFactory : IBackupManagerFactory
{
    private readonly IBackupTargetFactory _backupTargetFactory;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="backupTargetFactory">Current backup target factory</param>
    public BackupManagerFactory(IBackupTargetFactory backupTargetFactory)
    {
        _backupTargetFactory = backupTargetFactory;
    }

    /// <summary>
    /// Create a <see cref="IBackupManager"/> instance
    /// </summary>
    /// <param name="backupConfig">Backup configuration</param>
    /// <returns><see cref="IBackupManager"/> instance</returns>
    public IBackupManager CreateInstance(IBackupConfig backupConfig)
    {
        return new BackupManager(backupConfig, _backupTargetFactory);
    }
}
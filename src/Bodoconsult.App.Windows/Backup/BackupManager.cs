// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Backup;
using Bodoconsult.App.Windows.Helpers;

namespace Bodoconsult.App.Windows.Backup;

/// <summary>
/// The main unit of the backup system connecting the job task and the backup system
/// </summary>
public class BackupManager : BaseBackupManager
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="backupConfig">Current backup config</param>
    /// <param name="backupTargetFactory">Current backup target factory</param>
    public BackupManager(IBackupConfig backupConfig, IBackupTargetFactory backupTargetFactory): base(backupConfig, backupTargetFactory)
    { }

    /// <summary>
    /// Activate shadowing if necessary
    /// </summary>
    public override void ActivateShadowing()
    {
        if (string.IsNullOrEmpty(BackupConfig.ShadowingSourceDrive) ||
            string.IsNullOrWhiteSpace(BackupConfig.ShadowingTargetDrive))
        {
            return;
        }

        DiskShadowHelper.ClearDrive(BackupConfig.ShadowingTargetDrive);

        DiskShadowHelper.CreateDrive(BackupConfig.ShadowingSourceDrive, BackupConfig.ShadowingTargetDrive);
    }

    /// <summary>
    /// Deactivate shadowing
    /// </summary>
    public override void DeactivateShadowing()
    {
        if (string.IsNullOrEmpty(BackupConfig.ShadowingSourceDrive) ||
            string.IsNullOrWhiteSpace(BackupConfig.ShadowingTargetDrive))
        {
            return;
        }
        DiskShadowHelper.ClearDrive(BackupConfig.ShadowingTargetDrive);
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Backup config to use with <see cref="IBackupManager"/> implementations
/// </summary>
public interface IBackupConfig
{
    /// <summary>
    /// The source drive used for disk shadowing or null. I.e.: C:
    /// </summary>
    string? ShadowingSourceDrive { get; set; }

    /// <summary>
    /// The target drive used for disk shadowing or null. I.e.: H:
    /// </summary>
    string? ShadowingTargetDrive { get; set; }

    /// <summary>
    /// Backup command to run or null if the default should be used
    /// </summary>
    string? Command { get; set; }

    /// <summary>
    /// Number of weeks the backups are kept before being deleted in weekly backup mode. Default: 5
    /// </summary>
    int Weeks { get; set; }

    /// <summary>
    /// A folder path called to get the storage media alive before starting backup
    /// </summary>
    string? GetAliveFolder { get; set; }

    /// <summary>
    /// Number of days the backups are backup after date DayDate before being deleted
    /// </summary>
    int Days { get; set; }

    /// <summary>
    /// Offset in days
    /// </summary>
    int DaysOffset { get; set; }

    /// <summary>
    /// Current list of backup targets
    /// </summary>
    List<IBackupTargetSettings> BackupTargets { get; set; }

    /// <summary>
    /// Name of the summary file to be created
    /// </summary>
    string? SummaryFile { get; set; }

    /// <summary>
    /// Current list of errors
    /// </summary>
    IList<Exception>? Errors { get; set; }

    /// <summary>
    /// Status changed event
    /// </summary>
    StatusMessageDelegate? StatusChanged { get; set; }
}
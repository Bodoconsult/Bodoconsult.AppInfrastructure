// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Backup;

/// <summary>
/// Basic backup config
/// </summary>
public class BasicBackupConfig : IBackupConfig
{
    /// <summary>
    /// The source drive used for disk shadowing or null. I.e.: C:
    /// </summary>
    public string? ShadowingSourceDrive { get; set; }

    /// <summary>
    /// The target drive used for disk shadowing or null. I.e.: H:
    /// </summary>
    public string? ShadowingTargetDrive { get; set; }

    /// <summary>
    /// Backup command to run or null if the default should be used
    /// </summary>
    public string? Command { get; set; }

    /// <summary>
    /// Number of weeks the backups are kept before being deleted in weekly backup mode. Default: 5
    /// </summary>
    public int Weeks { get; set; }

    /// <summary>
    /// A folder path called to get the storage media alive before starting backup
    /// </summary>
    public string? GetAliveFolder { get; set; }

    /// <summary>
    /// Number of days the backups are backup after date DayDate before being deleted
    /// </summary>
    public int Days { get; set; }

    /// <summary>
    /// Offset in days
    /// </summary>
    public int DaysOffset { get; set; }

    /// <summary>
    /// Current list of backup targets
    /// </summary>
    public List<IBackupTargetSettings> BackupTargets { get; set; } = [];

    /// <summary>
    /// Name of the summary file to be created
    /// </summary>
    public string? SummaryFile { get; set; }

    /// <summary>
    /// Current list of errors
    /// </summary>
    public IList<Exception>? Errors { get; set; }

    /// <summary>
    /// Status changed event
    /// </summary>
    public StatusMessageDelegate? StatusChanged { get; set; }
}
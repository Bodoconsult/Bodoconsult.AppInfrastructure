// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for backup target settings
/// </summary>
public interface IBackupTargetSettings
{
    /// <summary>
    /// Backup command if command line is used
    /// ??source??  Placeholder for source directory path
    /// ??target??  Placeholder for target directory path
    /// ??log??     Placeholder for logfile path
    /// ??xd??      Placeholder for excluded subdirectories
    /// </summary>
    string? Command { get; set; }

    /// <summary>
    /// Source folder to back up
    /// </summary>
    string? Source { get; set; }

    /// <summary>
    /// Destination folder for the backup
    /// </summary>
    string? Target { get; set; }

    /// <summary>
    /// Logfile for to use for this backup target settings
    /// </summary>
    string? Logfile { get; set; }

    /// <summary>
    /// Directories to exclude separated by blanks. Directory names containing blanks have to be set in quotation marks. Syntax: C:\ToExclude1 "C:\To Exclude2" c:\ToExclude3
    /// </summary>
    string? ExcludeDirs { get; set; }

    /// <summary>
    /// Backup mode to use for the backup target
    /// </summary>
    BackupModeEnum BackupMode { get; set; }

    /// <summary>
    /// The number of the thread the backup should run on
    /// </summary>
    int Thread { get; set; }

    /// <summary>
    /// Number of weeks or days the backups should be kept
    /// </summary>
    int Count { get; set; }

    /// <summary>
    /// Offset in days
    /// </summary>
    int DaysOffset { get; set; }

    /// <summary>
    /// Current list of errors
    /// </summary>
    public IList<Exception>? Errors { get; set; }

    /// <summary>
    /// Status changed event
    /// </summary>
    public StatusMessageDelegate? StatusChanged { get; set; }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for backup targets. A backup target is a system of backups of a source keeping backups of the source from different timepoints
/// </summary>
public interface IBackupTarget
{
    /// <summary>
    /// Clear the existing backup target from depricated backups
    /// </summary>
    void ClearBackups();

    ///// <summary>
    ///// Backup command if command line is used
    ///// ??source??  Placeholder for source directory path
    ///// ??target??  Placeholder for target directory path
    ///// ??log??     Placeholder for logfile path
    ///// ??xd??      Placeholder for excluded subdirectories
    ///// </summary>
    //string Command { get; set; }

    ///// <summary>
    ///// Excluded subdirectories
    ///// </summary>
    //string ExcludeDirectories { get; set; }

    ///// <summary>
    ///// Logfile path
    ///// </summary>
    //string LogFile { get; set; }

    ///// <summary>
    ///// Backup mode
    ///// </summary>
    //BackupModeEnum BackupMode { get; set; }

    ///// <summary>
    ///// Source directory path
    ///// </summary>
    //string Source { get; set; }

    /// <summary>
    /// Run the backup process for the backup target now
    /// </summary>
    void StartBackupProcess();

    ///// <summary>
    ///// Current list of errors
    ///// </summary>
    //IList<Exception> Errors { get; }

    ///// <summary>
    ///// Status changed event
    ///// </summary>
    //StatusMessageDelegate StatusChanged { get; }

    ///// <summary>
    ///// Target directory path
    ///// </summary>
    //string Target { get; set; }

    ///// <summary>
    ///// Number of weeks the bakups should be kept
    ///// </summary>
    //int Count { get; set; }
}
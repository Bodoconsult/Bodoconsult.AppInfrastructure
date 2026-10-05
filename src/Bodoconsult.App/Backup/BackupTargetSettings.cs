// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Backup;

/// <summary>
/// The settings to use for a backup target
/// </summary>
public class BackupTargetSettings : IBackupTargetSettings
{
    private string? _target;

    /// <summary>
    /// Backup command if command line is used
    /// ??source??  Placeholder for source directory path
    /// ??target??  Placeholder for target directory path
    /// ??log??     Placeholder for logfile path
    /// ??xd??      Placeholder for excluded subdirectories
    /// </summary>
    public string? Command { get; set; } = "robocopy ??source?? ??target??  *.* /FFT /S /E /log:??log?? /COPY:DT /B /NP /MT:48 /R:1 /W:1 /XF thumbs.db /xd ??xd?? /NoDCopy";

    /// <summary>
    /// Source folder to back up
    /// </summary>
    public string? Source { get; set; }

    /// <summary>
    /// Target directory path
    /// </summary>
    public string? Target
    {
        get => _target;
        set
        {
            _target = value;

            if (value == null)
            {
                return;
            }

            if (value.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                _target = value;
            }
            else
            {
                _target += @"\";
            }
        }
    }

    /// <summary>
    /// Logfile for to use for this backup target settings
    /// </summary>
    public string? Logfile { get; set; }

    /// <summary>
    /// Directories to exclude separated by blanks. Directory names containing blanks have to be set in quotation marks. Syntax: C:\ToExclude1 "C:\To Exclude2" c:\ToExclude3
    /// </summary>
    public string? ExcludeDirs { get; set; }

    /// <summary>
    /// Backup mode to use for the backup target
    /// </summary>
    public BackupModeEnum BackupMode { get; set; } = BackupModeEnum.Simple;

    /// <summary>
    /// The number of the thread the backup should run on
    /// </summary>
    public int Thread { get; set; }

    /// <summary>
    /// Number of weeks the bakups should be kept
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Offset in days
    /// </summary>
    public int DaysOffset { get; set; }

    /// <summary>
    /// Current list of errors
    /// </summary>
    public IList<Exception>? Errors { get; set; }

    /// <summary>
    /// Status changed event
    /// </summary>
    public StatusMessageDelegate? StatusChanged { get; set; }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Enums;
using JobExecuter.Dal.Factories;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JobExecuter.Dal.Model;

/// <summary>
/// Backup of a folder based on robocopy.exe by Microsoft.
/// </summary>
/// <remarks>Robocopy.exe is installed automatically starting with Windows 7 and Windows 2008 Server.</remarks>
/// <example>
/// <include file='..\Samples\FolderBackupJobTask.xml' path='*'/>
///  </example>
public sealed class FolderBackupJobTask : BaseJobTask
{
    /// <summary>
    /// default ctor
    /// </summary>
    public FolderBackupJobTask()
    {
        IncludeSubDirectories = true;
        ShowProgress = false;
        BackupType = FolderBackupType.PlainCopy;
    }

    /// <summary>
    /// Folder to backup
    /// </summary>
    public string SourcePath { get; set; }

    /// <summary>
    /// Target folder for the backup
    /// </summary>
    public string TargetPath { get; set; }

    /// <summary>
    /// Directories to exclude from backup
    /// Syntax: C:\ToExclude1 "C:\To Exclude2" c:\ToExclude3
    /// </summary>
    public string ExcludeDirectories { get; set; }

    /// <summary>
    /// Defines how backup works. Default is <see cref="FolderBackupType.PlainCopy"/>.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public FolderBackupType BackupType { get; set; }

    /// <summary>
    /// Show progress of backup. May reduce performance. Default: false
    /// </summary>
    public bool ShowProgress { get; set; }

    /// <summary>
    /// Include subdirectories in the folder backup. Default: true
    /// </summary>
    public bool IncludeSubDirectories { get; set; }

    /// <inheritdoc />
    public override ITaskExecuterFactory GetFactory()
    {
        return new FolderBackupExecuterFactory();
    }
}
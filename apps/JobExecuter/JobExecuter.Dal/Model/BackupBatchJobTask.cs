// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.Interfaces;
using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Factories;
using System;
using System.Collections.Generic;

namespace JobExecuter.Dal.Model;

/// <summary>
/// Task to run a backup of the file system.  
/// </summary>
/// <example>
/// <include file='..\Samples\BackupBatchJobTask.xml' path='*'/>
///  </example>
public sealed class BackupBatchJobTask : BaseJobTask, IBackupConfig
{
    /// <summary>
    /// The source drive used for disk shadowing or null. I.e.: C:
    /// </summary>
    public string ShadowingSourceDrive { get; set; }

    /// <summary>
    /// The target drive used for disk shadowing or null. I.e.: H:
    /// </summary>
    public string ShadowingTargetDrive { get; set; }

    /// <summary>
    /// Backup command to run or null if the default should be used
    /// </summary>
    public string Command { get; set; }

    /// <summary>
    /// Number of weeks the backups are kept before being deleted in weekly backup mode. Default: 5
    /// </summary>
    public int Weeks { get; set; } = 5;

    /// <summary>
    /// A folder path called to get the storage media alive before starting backup
    /// </summary>
    public string GetAliveFolder { get; set; }

    /// <summary>
    /// Number of days the backups are backup after date TagDatum before being deleted
    /// </summary>
    public int Days { get; set; } = 14;

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
    public string SummaryFile { get; set; }

    /// <summary>
    /// Current list of errors
    /// </summary>
    public IList<Exception> Errors { get; set; }

    /// <summary>
    /// Status changed event
    /// </summary>
    public StatusMessageDelegate StatusChanged { get; set; }

    /// <inheritdoc />
    public override ITaskExecuterFactory GetFactory()
    {
        return new BackupBatchExecuterFactory();
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Factories;

namespace JobExecuter.Dal.Model;

/// <summary>
/// Task to take a full backup for all SQL databases (with exception of TempDb) on a Microsoft® SqlServer starting with version 2008.
/// </summary>
/// <remarks>Uses the following SQl statement: BACKUP DATABASE [{0}] TO DISK = N'{1}' WITH  INIT, SKIP, FORMAT,  NOUNLOAD,  NAME = N'{0} full backup'</remarks>
/// <example>
/// <include file='..\Samples\SqlBackupAllDatabasesJobTask.xml' path='*'/>
///  </example>
public sealed class SqlBackupAllDatabasesJobTask : SqlBaseJobTask
{



    /// <summary>
    /// Local target folder to backup database in
    /// </summary>
    /// <remarks>SqlServer runs backup under the service account of SqlServer service. This account doesn't have network access. 
    /// Therefore a direct backup to a network location is not possible normally. 
    /// Backup to local target and then moving the backup file to a network location is possible.</remarks>
    public string LocalTarget { get; set; }

    /// <summary>
    /// Target folder for the backup on a network share (if requested).
    /// </summary>
    /// <remarks>SqlServer runs backup under the service account of SqlServer service. This account doesn't have network access. 
    /// Therefore a direct backup to a network location is not possible normally. 
    /// Backup to local target and then moving the backup file to a network location is possible.</remarks>
    public string NetworkTarget { get; set; }

    /// <summary>
    /// File name schema for the backup file. ;ust contain placeholders. 
    /// Otherwise all backups are made to the same file (and overwrite it with each backup)
    /// Must contain placeholders:
    /// ??server?? = Server name or Server and instance name
    /// ??database?? = database name
    /// Optional placeholders are:
    /// ??date?? = date in format yyyymmdd
    /// ??time?? = date and time in format yyyymmddhhnn
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// Remove local backup files after copying to network. Default: false
    /// </summary>
    public bool RemoveLocalBackup { get; set; }


    /// <inheritdoc />
    public override ITaskExecuterFactory GetFactory()
    {
        return new SqlBackupAllDatabasesExecuterFactory();
    }


}
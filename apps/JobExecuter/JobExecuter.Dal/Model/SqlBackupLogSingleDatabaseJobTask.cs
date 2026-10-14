// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Factories;

namespace JobExecuter.Dal.Model;

/// <summary>
/// Task to take a log file backup for a single SQL database on Microsoft® SqlServer starting with version 2008.
/// </summary>
/// <remarks>Uses the following SQl statement: BACKUP LOG [{0}] TO DISK = N'{1}' WITH  INIT, SKIP, FORMAT,  NOUNLOAD,  NAME = N'{0} full backup'</remarks>
/// <example>
/// <include file='..\Samples\SqlBackupLogSingleDatabaseJobTask.xml' path='*'/>
///  </example>
public sealed class SqlBackupLogSingleDatabaseJobTask : SqlBaseJobTask
{


    /// <summary>
    /// Name of the database to backup
    /// </summary>
    public string DatabaseName { get; set; }

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
    /// File name for the backup file
    /// May contain placeholders:
    /// ??server?? = Server name or Server and instance name
    /// ??database?? = database name
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
        return new SqlBackupLogSingleDatabaseExecuterFactory();
    }

}
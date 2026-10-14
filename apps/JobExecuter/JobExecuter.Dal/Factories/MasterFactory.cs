// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Model;

namespace JobExecuter.Dal.Factories;

/// <summary>
/// Create the factory needed for a certain <see cref="IJobTask"/> object
/// </summary>
internal class MasterFactory
{
    /// <summary>
    /// Create the factory needed for a certain <see cref="IJobTask"/> object
    /// </summary>
    public static ITaskExecuterFactory NewFactory(Type type)
    {
        if (type == typeof(DummyJobTask)) return new DummyExecuterFactory();
        if (type == typeof(DummyErrorJobTask)) return new DummyErrorExecuterFactory();
        if (type == typeof(SqlBackupSingleDatabaseJobTask)) return new SqlBackupSingleDatabaseExecuterFactory();
        if (type == typeof(SqlBackupAllDatabasesJobTask)) return new SqlBackupAllDatabasesExecuterFactory();
        if (type == typeof(FolderCleanJobTask)) return new FolderCleanExecuterFactory();
        if (type == typeof(SqlBatchJobTask)) return new SqlBatchExecuterFactory();
        if (type == typeof(DiskSizeJobTask)) return new DiskSizeExecuterFactory();
        if (type == typeof(FolderBackupJobTask)) return new FolderBackupExecuterFactory();
        if (type == typeof(BackupBatchJobTask)) return new BackupBatchExecuterFactory();
        if (type == typeof(CmdBatchJobTask)) return new CmdBatchExecuterFactory();
        if (type == typeof(PingCheckJobTask)) return new PingCheckExecuterFactory();
        if (type == typeof(RunAppJobTask)) return new RunAppExecuterFactory();
        if (type == typeof(PsBatchJobTask)) return new PsBatchExecuterFactory();
        if (type == typeof(TaskSchedulerDocuJobTask)) return new TaskSchedulerDocuExecuterFactory();
        if (type == typeof(JobDocuJobTask)) return new JobDocuExecuterFactory();
        if (type == typeof(DocumentationJobTask)) return new DocumentationExecuterFactory();
        if (type == typeof(SqlDocuSingleDatabaseJobTask)) return new SqlDocuSingleDatabaseExecuterFactory();
        if (type == typeof(SqlDocuAllDatabasesJobTask)) return new SqlDocuAllDatabasesExecuterFactory();
        if (type == typeof(SqlComplianceSingleDatabaseJobTask)) return new SqlComplianceSingleDatabaseExecuterFactory();
        if (type == typeof(SqlComplianceAllDatabasesJobTask)) return new SqlComplianceAllDatabasesExecuterFactory();
        if (type == typeof(FolderComplianceJobTask)) return new FolderComplianceExecuterFactory();
        if (type == typeof(SqlMaintenanceJobTask)) return new SqlMaintenanceExecuterFactory();
        if (type == typeof(ActiveDirectoryJobTask)) return new ActiveDirectoryExecuterFactory();

            
        return null;
    }
}
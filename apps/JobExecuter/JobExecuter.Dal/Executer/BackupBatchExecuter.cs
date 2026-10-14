// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using JobExecuter.Base;
using JobExecuter.Dal.Model;
using System;
using Bodoconsult.App.Windows.Backup;
using Bodoconsult.App.Windows.ShellTools;
using JobExecuter.Base.Interfaces;

namespace JobExecuter.Dal.Executer;

/// <summary>
/// Backup batch executer for running advanced file system backups
/// </summary>
internal class BackupBatchExecuter : BaseExecuter
{
    private readonly BackupBatchJobTask _jobTask;

    /// <summary>
    /// ctor to initialize task with job data
    /// </summary>
    /// <param name="jobTask">job task to run</param>
    public BackupBatchExecuter(IJobTask jobTask) : base(jobTask)
    {
        _jobTask = (BackupBatchJobTask)jobTask;
    }

    /// <summary>
    /// Run job task
    /// </summary>
    /// <returns>true in case of success</returns>
    public override bool Run()
    {
        var result = true;

        try
        {
            var shellManager = new WinShellProcessManager();

            var factory = new RobocopyBackupTargetFactory(shellManager);

            _jobTask.StatusChanged = StatusChanged;
            _jobTask.Errors = Errors;

            var bm = new BackupManager(_jobTask, factory);

            bm.ActivateShadowing();
            bm.CheckStorageMedia();
            bm.StartBackup();
            bm.CreateSummaryFile();
            bm.DeactivateShadowing();

            //SetStatus("\r\nCmd batch {0} done!!", _jobTask.Filename);
        }
        catch (Exception ex)
        {
            result = false;
            var msg = $"BackupBatch:Error:{ex.Message}";
            SetStatus(msg);
            Errors.Add(new Exception(msg));
        }

        return result;
    }
}
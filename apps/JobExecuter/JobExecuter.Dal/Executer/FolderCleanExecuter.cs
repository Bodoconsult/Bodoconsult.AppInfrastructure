// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.IO;
using System.Linq;
using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Model;

namespace JobExecuter.Dal.Executer;

/// <inheritdoc />
/// <summary>
/// Dummy executer for testing purposes
/// </summary>
internal class FolderCleanExecuter : BaseExecuter
{

    private readonly FolderCleanJobTask _jobTask;


    /// <summary>
    /// Run job task
    /// </summary>
    /// <returns>true in case of success</returns>
    public override bool Run()
    {

        var date = DateTime.Now.AddDays(-_jobTask.Days);

        var files = new DirectoryInfo(_jobTask.FolderPath).EnumerateFiles().Where(x => x.CreationTime < date).ToList();

        var result = true;

        foreach (var f in files)
        {
            try
            {
                f.Delete();
            }
            catch (Exception ex)
            {
                result = false;
                var msg = $"Error cleaning folder '{_jobTask.FolderPath}'!";
                Errors.Add(new Exception(msg, ex));
                SetStatus(msg);
            }
        }

        return result;
    }

    /// <summary>
    /// ctor to initialize task with job data
    /// </summary>
    /// <param name="jobTask">job task to run</param>
    public FolderCleanExecuter(IJobTask jobTask)
        : base(jobTask)
    {
        _jobTask = (FolderCleanJobTask)jobTask;
    }
}
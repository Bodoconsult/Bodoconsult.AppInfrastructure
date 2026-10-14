// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.IO;
using Bodoconsult.Text.Helpers;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Helpers;
using JobExecuter.Dal.Model;

namespace JobExecuter.Dal.Executer;

/// <inheritdoc />
/// <summary>
/// Creates documentation of current job
/// </summary>
/// <example>
/// <include file="..\Samples\JobDocuJobTask.xml" path="*" />
///  </example>
internal class JobDocuExecuter : DocuBaseExecuter
{
    private readonly JobDocuJobTask _jobTask;

    /// <summary>
    /// Run job task
    /// </summary>
    /// <returns>true in case of success</returns>
    public override bool Run()
    {

        string jobJsonFileName = null;

        try
        {
            if (string.IsNullOrEmpty(_jobTask.JobJsonFileName) && _jobTask.CurrentJob == null)
            {
                return false;
            }

            jobJsonFileName = _jobTask.JobJsonFileName ?? _jobTask.CurrentJob.JobJsonFileName;


            if (jobJsonFileName is null || !File.Exists(jobJsonFileName ?? string.Empty))
            {
                var msg = $"File {jobJsonFileName} does not exist!";
                Result.AddParagraph(msg);
                SetStatus(msg);
                return true;
            }

            if (_jobTask.CurrentJob is null)
            {
                const string msg = "_jobTask.CurrentJob is null!";
                Result.AddParagraph(msg);
                SetStatus(msg);
                return true;
            }

            var job = _jobTask.CurrentJob; // ?? JsonHelper.LoadJsonFile<Job>(jobJsonFileName);

            // ReSharper disable once AssignNullToNotNullAttribute
            var readme = Path.Combine(new FileInfo(jobJsonFileName).DirectoryName, "readme.txt");
            if (File.Exists(readme))
            {
                Result.AppendUtf8PlainTextFile(readme);
            }

            Result.AddHeader1("Job documentation for job '{0}'", string.Empty, job.Name);
            Result.AddParagraph(_jobTask.JobJsonFileName);
            Result.AddParagraph(job.Description);
            Result.AddParagraph(string.Empty, job.ErrorMailReceiver);
            Result.AddParagraph(job.Description);

            foreach (var task in job.JobTasks)
            {
                Result.AddHeader2("JobTask '{0}' ({1})", string.Empty, task.Name, task.GetType().Name);
                Result.AddParagraph(task.Description);

                Result.AddParagraph("JSON of JobTask '{0}':", task.Name);
                Result.AddCode(JsonHelper.SerializeObject(task));
            }

            Save("Job documentation");

            return true;
        }
        catch (Exception e)
        {
            var msg = $"JobDocu: {jobJsonFileName}: {e}";
            Errors.Add(new Exception(msg));
            SetStatus(msg);
            return false;
        }
    }

    /// <summary>
    /// ctor to initialize task with job data
    /// </summary>
    /// <param name="jobTask">job task to run</param>
    public JobDocuExecuter(IJobTask jobTask)
        : base(jobTask)
    {
        _jobTask = (JobDocuJobTask)jobTask;
    }
}
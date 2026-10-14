// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.Generic;
using JobExecuter.Base.Interfaces;
using Newtonsoft.Json;

namespace JobExecuter.Dal.Model;

/// <summary>
/// Defines the basic information required for a job to be executed
/// </summary>
public class Job : IJob
{
    /// <summary>
    /// default ctor
    /// </summary>
    public Job()
    {
        JobTasks = new List<IJobTask>();
    }

    /// <summary>
    /// Name of the job
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Description of the job used for documentation
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Send an error mail to mail addresses. Mail addresses separated by semicolon
    /// </summary>
    public string ErrorMailReceiver { get; set; }

    /// <summary>
    /// Send an summary mail to mail addresses. Mail addresses separated by semicolon
    /// </summary>
    public string SummaryMailReceiver { get; set; }

    /// <summary>
    /// All tasks to be done in the job
    /// </summary>
    public IList<IJobTask> JobTasks { get; set; }

    /// <summary>
    /// Timeout in seconds the job will be terminated after start
    /// </summary>
    public int Timeout { get; set; }

    /// <summary>
    /// Write the error messages to the Windows eventlog
    /// </summary>
    public bool WriteToEventLog { get; set; }

    /// <summary>
    /// Full path of the CJSON file with JobExecuter job to document
    /// </summary>
    [JsonIgnore]
    public string JobJsonFileName { get; set; }
}
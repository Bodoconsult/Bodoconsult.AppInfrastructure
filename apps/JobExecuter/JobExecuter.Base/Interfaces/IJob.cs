// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace JobExecuter.Base.Interfaces;

/// <summary>
/// Defines the basic information required for a job to be executed
/// </summary>
public interface IJob
{
    /// <summary>
    /// Name of the job
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// Description of the job used for documentation
    /// </summary>
    string Description { get; set; }

    /// <summary>
    /// Mail addresses separarted with semicolon to send an error mail to
    /// </summary>
    string ErrorMailReceiver { get; set; }

    /// <summary>
    /// Mail addresses separarted with semicolon to send an summary mail to
    /// </summary>
    string SummaryMailReceiver { get; set; }

    /// <summary>
    /// All tasks to be done in the job
    /// </summary>
    IList<IJobTask> JobTasks { get; set; }

    /// <summary>
    /// Timeout in seconds the job will be terminated after start
    /// </summary>
    int Timeout { get; set; }

    /// <summary>
    /// Write the error messages to the Windows eventlog
    /// </summary>
    bool WriteToEventLog { get; set; }

    /// <summary>
    /// Full path of the CJSON file with JobExecuter job to document
    /// </summary>
    [JsonIgnore]
    public string JobJsonFileName { get; set; }
}
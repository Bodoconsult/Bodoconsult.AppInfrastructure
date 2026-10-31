// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.Generic;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Interfaces;
using JobExecuter.Dal.Model;

namespace JobExecuter.Dal;

/// <summary>
/// Interface for jonhandling implementations
/// </summary>
public interface IJobHandler
{
    /// <summary>
    /// All <see cref="IJobMailer"/> objects to send error messages and summary to
    /// </summary>
    IList<IJobMailer> Mailers { get; set; }

    /// <summary>
    /// All error items that happened in the current job
    /// </summary>
    IList<ErrorItem> ErrorItems { get; set; }

    /// <summary>
    /// Current job to handle
    /// </summary>
    IJob CurrentJob { get; set; }

    /// <summary>
    /// Run a single <see cref="IJobTask"/>
    /// </summary>
    /// <param name="jobTask"></param>
    /// <returns>true on success</returns>
    bool RunTask(IJobTask jobTask);

    /// <summary>
    /// Save current job as JSON to a file 
    /// </summary>
    /// <param name="fileName">Path for the JSON file to save</param>
    /// <returns>true on success</returns>
    bool SaveJob(string fileName);

    /// <summary>
    /// Load current job from JSON file 
    /// </summary>
    /// <param name="job">Job to run</param>
    /// <param name="jsonJobFile">Filename of the CJSON file</param>
    /// <returns>true on success</returns>
    bool LoadJob(IJob job, string jsonJobFile);

    /// <summary>
    /// Run the current job
    /// </summary>
    /// <returns>true if no job task caused an error</returns>
    bool RunJob();

    /// <summary>
    /// Only for testing purposes: set JobJson file name (without loading the file)
    /// </summary>
    /// <param name="jobJson"></param>
    void SetJobJsonFileName(string jobJson);
}
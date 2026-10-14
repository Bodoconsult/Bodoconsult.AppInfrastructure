// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using JobExecuter.Base.Interfaces;

namespace JobExecuter.Dal.Model;

/// <summary>
/// Task to be done in a <see cref="IJob"/>
/// </summary>
public class JobBaseTask : IJobTask
{
    /// <summary>
    /// default ctor
    /// </summary>
    public JobBaseTask()
    {
        Timeout = 30;
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
    /// Timeout in seconds the job will be terminated after start
    /// </summary>
    public int Timeout { get; set; }

    /// <summary>
    /// Get the factory for the current task
    /// </summary>
    /// <returns>A factory for tasks</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public virtual ITaskExecuterFactory GetFactory()
    {
        throw new System.NotSupportedException();
    }
}
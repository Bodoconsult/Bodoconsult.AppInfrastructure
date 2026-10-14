// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace JobExecuter.Base.Interfaces;

/// <summary>
/// Task to be done in a <see cref="IJob"/>
/// </summary>
public interface IJobTask
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
    /// Timeout in seconds the job will be terminated after start
    /// </summary>
    int Timeout { get; set; }

    /// <summary>
    /// Get the factory for the task's executer class
    /// </summary>
    /// <returns></returns>
    ITaskExecuterFactory GetFactory();
}
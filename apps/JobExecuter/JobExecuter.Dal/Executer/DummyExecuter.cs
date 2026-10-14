// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Threading;
using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Model;

namespace JobExecuter.Dal.Executer;

/// <inheritdoc />
/// <summary>
/// Dummy executer for testing purposes
/// </summary>
internal class DummyExecuter: BaseExecuter
{
    private readonly DummyJobTask _jobTask;

    /// <summary>
    /// Run job task
    /// </summary>
    /// <returns>true in case of success</returns>
    public override bool Run()
    {
        SetStatus(_jobTask.Message);

        Thread.Sleep(3050);

        if (_jobTask.WaitTime > 0)
        {
            Thread.Sleep(_jobTask.WaitTime*1000);

        }

        return true;
    }

    /// <summary>
    /// ctor to initialize task with job data
    /// </summary>
    /// <param name="jobTask">job task to run</param>
    public DummyExecuter(IJobTask jobTask) : base(jobTask)
    {
        _jobTask = (DummyJobTask) jobTask;
    }
}
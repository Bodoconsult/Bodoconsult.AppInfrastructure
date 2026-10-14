// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Threading;
using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Model;

namespace JobExecuter.Dal.Executer;

/// <inheritdoc />
/// <summary>
/// Dummy executer for error testing purposes
/// </summary>
internal class DummyErrorExecuter : BaseExecuter
{

    private readonly DummyErrorJobTask _jobTask;


    /// <summary>
    /// Run job task
    /// </summary>
    /// <returns>true in case of success</returns>
    public override bool Run()
    {
        SetStatus(_jobTask.Message);

        //throw new Exception("DummyError: fake error");

        Thread.Sleep(3050);

        var msg = "DummyErrorExecuter:Run: Should fail";
        Errors.Add(new Exception(msg));

        return false;
    }

    /// <summary>
    /// ctor to initialize task with job data
    /// </summary>
    /// <param name="jobTask">job task to run</param>
    public DummyErrorExecuter(IJobTask jobTask)
        : base(jobTask)
    {
        _jobTask = (DummyErrorJobTask)jobTask;
    }
}
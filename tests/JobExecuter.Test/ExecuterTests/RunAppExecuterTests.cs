// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

// ReSharper disable InconsistentNaming

using JobExecuter.Dal;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Dal.Model;
using NUnit.Framework;

namespace JobExecuter.Test.ExecuterTests;

[TestFixture]
public class RunAppExecuterTests
{
    //C:\Daten\Projekte\Customer\BodoPrivate\BodoPrivateUpdater\BodoPrivateUpdater\bin\Debug

    [Test]
    public void TestRunAppJobTask_Basic()
    {

        var job = new Job
        {
            Name = "Sample for running a app without parmeters",
            Description =
                "",
            Timeout = 120
        };

        var ra = new RunAppJobTask
        {
            Name = "Sample for running a app without parmeters",
            Filename = "robocopy.exe"
        };

        job.JobTasks.Add(ra);

        var jh = new JobHandler();
        jh.Mailers.Add(new FakeJobMailer());

        jh.CurrentJob = job;

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
    }


    [Test]
    public void TestRunAppJobTask_With()
    {

        var job = new Job
        {
            Name = "Sample for running a app with parameters",
            Description ="",
            Timeout = 12
        };

        var ra = new RunAppJobTask
        {
            Name = "Sample for running a app with parameters",
            Filename = "robocopy.exe",
            Arguments = "/?"
        };

        job.JobTasks.Add(ra);

        var jh = new JobHandler();
        jh.Mailers.Add(new FakeJobMailer());

        jh.CurrentJob = job;

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
    }

}
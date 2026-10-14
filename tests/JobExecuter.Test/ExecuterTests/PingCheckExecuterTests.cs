// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.


// ReSharper disable InconsistentNaming

using JobExecuter.Dal;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Dal.Model;
using NUnit.Framework;

namespace JobExecuter.Test.ExecuterTests;

[TestFixture]
public class PingCheckExecuterTests
{
    [Test]
    public void TestPingCheckJobTask_NoError()
    {

        var job = new Job
        {
            Name = "Sample for running a cmd batch file",
            Description =
                "",
            Timeout = 120
        };

        var pc = new PingCheckJobTask
        {
            Name = "Sample task for ping check",
            IpAdresses = "192.168.10.123;192.168.10.122;192.168.10.1;8.8.8.8",
            Timeout = 60
        };

        job.JobTasks.Add(pc);

        var jh = new JobHandler();
        jh.Mailers.Add(new FakeJobMailer());

        jh.CurrentJob = job;

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
    }


    [Test]
    public void TestPingCheckJobTask_OneError()
    {

        var job = new Job
        {
            Name = "Sample for running a cmd batch file",
            Description =
                "",
            Timeout = 120
        };

        var pc = new PingCheckJobTask
        {
            Name = "Sample task for ping check",
            IpAdresses = "192.168.10.123;192.168.10.122;192.168.10.255;8.8.8.8",
            Timeout = 60
        };

        job.JobTasks.Add(pc);

        var jh = new JobHandler();
        jh.Mailers.Add(new FakeJobMailer());

        jh.CurrentJob = job;

        var result = jh.RunJob();

        Assert.That(!result);
        Assert.That(jh.ErrorItems.Count == 1);
    }

}
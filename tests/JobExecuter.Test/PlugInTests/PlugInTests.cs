// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.IO;
using JobExecuter.Dal;
using JobExecuter.Dal.Helpers;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Dal.Model;
using JobExecuter.GpoPlugIn.Model;
using JobExecuter.Test.Helpers;
using NUnit.Framework;

namespace JobExecuter.Test.PlugInTests;

[TestFixture]
public class PlugInTests
{
    [Test]
    public void TestLoadPlugInAndRunTask()
    {

        var temp = Path.Combine(TestDataHelper.LocalPath, "GpoPlugIn");

        var cJson = Path.Combine(temp, "job.cjson");

        var job = new Job
        {
            Name = "Sample job with all kinds of job tasks",
            Description = "This job shows typical settings for all kinds of job taks",
            Timeout = 600,
            ErrorMailReceiver = "errorreceiver1@test.xxx;errorreceiver2@test.xxx",
            SummaryMailReceiver = "summaryreceiver1@test.xxx;summaryreceiver2@test.xxx",
            WriteToEventLog = true
        };

        var gpotask = new GpoJobTask
        {
            Domain = "BCG-AD.DE",
            TargetDirectory = temp,
            Timeout = 600,
        };

        job.JobTasks.Add(gpotask);

        //Assembly.LoadFrom(@"D:\Daten\Projekte\OwnProjects\JobExecuter\JobExecuter.GpoPlugIn\bin\Debug\JobExecuter.GpoPlugIn.dll");

        var jh = new JobHandler
        {
            CurrentJob = job
        };

        jh.SaveJob(cJson);

        PlugInHelper.LoadPlugIns();

        jh = new JobHandler();
        jh.Mailers.Add(new FakeJobMailer());
        jh.LoadJob(job, cJson);
        jh.SaveJob(cJson.Replace(".cjson", ".json"));

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
    }
}
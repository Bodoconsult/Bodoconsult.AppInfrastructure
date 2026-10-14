// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.IO;
using System.Linq;
using JobExecuter.Dal;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Dal.Model;
using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace JobExecuter.Test.ExecuterTests;

[TestFixture]
public class TaskSchedulerDocuExecuterTests
{

    private const string TargetPath = @"d:\tmp\TaskScheduler";


    [Test]
    public void TestTaskSchedulerDocu_NoFilter()
    {
        if (Directory.Exists(TargetPath))
        {
            Directory.Delete(TargetPath, true);
        }

        var job = new Job
        {
            Name = "Sample for getting task scheduler data",
            Description = "",
            Timeout = 120
        };


        var ts = new TaskSchedulerDocuJobTask
        {
            Name = "Getting task scheduler data from local computer",
            XmlTargetPath = TargetPath,
            HtmlTargetFileName = Path.Combine(TargetPath, "docu1.html"),
            JsonTargetFileName = Path.Combine(TargetPath, "docu1.json")
        };

        job.JobTasks.Add(ts);

        var jh = new JobHandler();
        jh.Mailers.Add(new FakeJobMailer());

        jh.CurrentJob = job;

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
        Assert.That(new DirectoryInfo(TargetPath).EnumerateFiles().Any());

    }


    [Test]
    public void TestTaskSchedulerDocu_WithFilter()
    {
        if (Directory.Exists(TargetPath))
        {
            Directory.Delete(TargetPath, true);
        }

        var job = new Job
        {
            Name = "Sample for getting task scheduler data",
            Description = "",
            Timeout = 120
        };


        var ts = new TaskSchedulerDocuJobTask
        {
            Name = "Getting task scheduler data from local computer with use of filter expressions",
            XmlTargetPath = TargetPath,
            Filter = "Adobe;Microsoft",
            HtmlTargetFileName = Path.Combine(TargetPath, "docu.html"),
            JsonTargetFileName = Path.Combine(TargetPath, "docu.json")
        };

        job.JobTasks.Add(ts);

        var jh = new JobHandler();
        jh.Mailers.Add(new FakeJobMailer());

        jh.CurrentJob = job;

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
        Assert.That(new DirectoryInfo(TargetPath).EnumerateFiles().Any());

    }
}
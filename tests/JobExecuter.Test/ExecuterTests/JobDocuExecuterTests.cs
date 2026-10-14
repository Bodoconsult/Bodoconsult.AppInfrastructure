// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.IO;
using JobExecuter.Dal;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Dal.Model;
using JobExecuter.Test.Helpers;
using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace JobExecuter.Test.ExecuterTests;

[TestFixture]
public class JobDocuExecuterTests
{
    private readonly string TargetPath = TestDataHelper.LocalPath;

    public JobDocuExecuterTests()
    {
        TestDataHelper.CreateDirectory(TargetPath);
    }

    [Test]
    public void TestTaskJobDocu_NoPaths()
    {
        var jobJson = Path.Combine(TargetPath, "job.cjson");

        var job = new Job
        {
            Name = "Sample for job documentation",
            Description = "",
            Timeout = 120,
        };

        var ts = new TaskSchedulerDocuJobTask
        {
            Name = "Getting task scheduler data from local computer",
            XmlTargetPath = TargetPath,
            HtmlTargetFileName = Path.Combine(TargetPath, "docu1.html"),
            JsonTargetFileName = Path.Combine(TargetPath, "docu1.json")
        };

        job.JobTasks.Add(ts);


        var jd = new JobDocuJobTask
        {
            Name = "Document the current JobExecuter job",
            Description =
                "If JobJsonFileName and JsonTargetFileName left empty, JobExecuter will insert current job file path and as docu file name the current job file path with the suffix _docu.json",
        };
        job.JobTasks.Add(jd);

        var jh = new JobHandler();
        jh.Mailers.Add(new FakeJobMailer());

        jh.SetJobJsonFileName(jobJson);
        jh.CurrentJob = job;
        jh.SaveJob(jobJson);

        jh.CurrentJob = null;

        jh.LoadJob(job, jobJson);

        var result = jh.RunJob();

        ArgumentNullException.ThrowIfNull(jh.CurrentJob);
        var docuJob = (JobDocuJobTask)jh.CurrentJob.JobTasks[1];

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
        Assert.That(File.Exists(docuJob.JsonTargetFileName));
    }

    [Test]
    public void TestTaskJobDocu_Plain()
    {
        if (!Directory.Exists(TargetPath))
        {
            Directory.CreateDirectory(TargetPath);
        }

        var jobJson = Path.Combine(TargetPath, "job.cjson");

        var job = new Job
        {
            Name = "Sample for job documentation",
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

        var jd = new JobDocuJobTask
        {
            Name = "Document the current JobExecuter job",
            Description = "If JobJsonFileName and DocuJsonFileName left empty, JobExecuter will insert current job file path and as docu file name the current job file path with the suffix _docu.json",
            JsonTargetFileName = Path.Combine(TargetPath, "job_docu.json"),
            JobJsonFileName = jobJson
        };
        job.JobTasks.Add(jd);

        var jh = new JobHandler();
        jh.Mailers.Add(new FakeJobMailer());
        jh.SetJobJsonFileName(jobJson);
        jh.CurrentJob = job;
        jh.SaveJob(jd.JobJsonFileName);

        jh.CurrentJob = null;

        jh.LoadJob(job,jd.JobJsonFileName);

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
        Assert.That(File.Exists(jd.JsonTargetFileName));
    }

    [Test]
    public void TestTaskJobDocu_FullDocumentation()
    {
        var jobJson = Path.Combine(TargetPath, "job.cjson");

        var job = new Job
        {
            Name = "Sample for job documentation",
            Description = "",
            Timeout = 120
        };


        var json1 = Path.Combine(TargetPath, "docu1.json");

        var ts = new TaskSchedulerDocuJobTask
        {
            Name = "Getting task scheduler data from local computer",
            XmlTargetPath = TargetPath,
            HtmlTargetFileName = Path.Combine(TargetPath, "docu1.html"),
            JsonTargetFileName = json1,
            Filter = "Adobe;Microsoft"
        };

        job.JobTasks.Add(ts);


        var jd = new JobDocuJobTask
        {
            Name = "Document the current JobExecuter job",
            Description = "If JobJsonFileName and DocuJsonFileName left empty, JobExecuter will insert current job file path and as docu file name the current job file path with the suffix _docu.json",
            JsonTargetFileName = Path.Combine(TargetPath, "job_docu.json"),
            JobJsonFileName = jobJson,
        };
        job.JobTasks.Add(jd);

        var docFile = Path.Combine(TargetPath, "documentation.htm");
        if (File.Exists(docFile)) File.Delete(docFile);

        var doc = new DocumentationJobTask {Title = "Gesamtdokumentation"};
        doc.HtmlTargetFiles.Add(docFile);

        doc.JsonFiles.Add(json1);
        doc.JsonFiles.Add(jd.JsonTargetFileName);

        job.JobTasks.Add(doc);

        var jh = new JobHandler();
        jh.Mailers.Add(new FakeJobMailer());
        jh.SetJobJsonFileName(jobJson);
        jh.CurrentJob = job;
        jh.SaveJob(jd.JobJsonFileName);

        jh.CurrentJob = null;

        jh.LoadJob(job, jd.JobJsonFileName);

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
        Assert.That(File.Exists(jd.JsonTargetFileName));
        Assert.That(File.Exists(docFile));
    }
}
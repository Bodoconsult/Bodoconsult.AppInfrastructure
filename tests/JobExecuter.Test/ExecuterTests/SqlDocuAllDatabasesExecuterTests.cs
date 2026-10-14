// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.IO;
using Bodoconsult.Test.Helpers;
using JobExecuter.Dal;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Dal.Model;
using JobExecuter.Test.Helpers;
using NUnit.Framework;

namespace JobExecuter.Test.ExecuterTests;

[TestFixture]
public class SqlDocuAllDatabasesExecuterTests
{
    private readonly string _localPath = TestDataHelper.LocalPath;
    private const string FileName = "db_doku_gesamt";

    [Test]
    public void TestDocu()
    {
        var job = new Job
        {
            Name = "Sample job for SqlServer documentation of a all databases with exception of databases Master, Msdb, Model and TempDb",
            Description = "This job first saves SQL databases to a local path " +
                          "and then moves the backup file to a network location.",
            Timeout = 120
        };

        var docuFile = Path.Combine(_localPath, $"{FileName}.htm");
        var jsonFile = Path.Combine(_localPath, $"{FileName}.json");

        var sd = new SqlDocuAllDatabasesJobTask
        {
            Name = "Create a documentation for all databases on a SqlServer with exception of databases Master, Msdb, Model and TempDb",
            Server = ".\\sqlexpress",
            HtmlTargetFileName = docuFile,
            JsonTargetFileName = jsonFile,
            Timeout = 60
        };

        job.JobTasks.Add(sd);

        if (File.Exists(docuFile)) File.Delete(docuFile);
        if (File.Exists(jsonFile)) File.Delete(jsonFile);

        var jh = new JobHandler
        {
            CurrentJob = job
        };

        jh.Mailers.Add(new FakeJobMailer());

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
        Assert.That(File.Exists(docuFile));

        TestHelper.OpenFile(docuFile);
    }
}
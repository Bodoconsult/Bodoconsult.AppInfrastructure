// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.IO;
using Bodoconsult.Test.Helpers;
using JobExecuter.Dal;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Dal.Model;
using JobExecuter.Test.Helpers;
using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace JobExecuter.Test.ExecuterTests;

[TestFixture]
public class SqlDocuSingleDatabaseExecuterTests
{
    private readonly string localPath = TestDataHelper.LocalPath;
    private const string fileName = "db_doku";

    [Test]
    public void TestDocu()
    {
        var job = new Job
        {
            Name = "Sample job for SqlServer backup of a single database",
            Description = "This job first saves a SQL database to a local path " +
                          "and then moves the backup file to a network location.",
            Timeout = 120
        };

        var docuFile = Path.Combine(localPath, $"{fileName}.htm");
        var jsonFile = Path.Combine(localPath, $"{fileName}.json");

        var sd = new SqlDocuSingleDatabaseJobTask
        {
            Name ="Create a documentation for a single database",
            Server = "192.168.10.125",
            DatabaseName = "BodoFileTransfer",
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
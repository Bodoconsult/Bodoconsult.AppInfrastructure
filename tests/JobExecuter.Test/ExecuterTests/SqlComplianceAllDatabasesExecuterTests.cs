// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Test.Helpers;
using JobExecuter.Dal;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Dal.Model;
using NUnit.Framework;

namespace JobExecuter.Test.ExecuterTests;

[TestFixture]
public class SqlComplianceAllDatabasesExecuterTests
{
    [Test]
    public void TestRunJob()
    {

        var job = new Job
        {
            Name = "Sample job for SqlServer compliance documentation of a all databases with exception of databases Master, Msdb, Model and TempDb",
            Description = "This job first saves SQL databases to a local path " +
                          "and then moves the backup file to a network location.",
            Timeout = 120
        };

        var sma = new SqlComplianceAllDatabasesJobTask
        {
            Name = "Create and send an email with user access information for all databases on a SqlServer with exception of databases Master, Msdb, Model and TempDb",
            Server = "192.168.10.121",
            MailTo = "robert.leisner@bodoconsult.de",
            HtmlTargetFileName = "d:\\tmp\\JobExecuter\\Compliance3.htm",
            PdfTargetFileName = "d:\\tmp\\JobExecuter\\Compliance3.pdf",
            Timeout = 60
        };

        job.JobTasks.Add(sma);

        var jh = new JobHandler
        {
            CurrentJob = job
        };

        jh.Mailers.Add(new FakeJobMailer());
        //jh.Mailers.Add(s);

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);


        TestHelper.OpenFile(sma.PdfTargetFileName);
    }
}
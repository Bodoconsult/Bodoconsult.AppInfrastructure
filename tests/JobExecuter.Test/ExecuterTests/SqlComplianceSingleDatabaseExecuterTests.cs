// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Test.Helpers;
using JobExecuter.Dal;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Dal.Model;
using NUnit.Framework;

namespace JobExecuter.Test.ExecuterTests;

[TestFixture]
public class SqlComplianceSingleDatabaseExecuterTests
{
    [Test]
    public void TestRunJob()
    {



        var job = new Job
        {
            Name = "Sample job for SqlServer backup of a single database",
            Description = "This job first saves a SQL database to a local path " +
                          "and then moves the backup file to a network location.",
            Timeout = 120
        };


        var sms = new SqlComplianceSingleDatabaseJobTask
        {
            Name = "Create and send an email with user access information for a single database",
            Server = ".\\sqlexpress",
            DatabaseName = "MediaDB",
            MailTo = "robert.leisner@bodoconsult.de",
            HtmlTargetFileName = "d:\\tmp\\JobExecuter\\Compliance1.htm",
            PdfTargetFileName = "d:\\tmp\\JobExecuter\\Compliance1.pdf",
            Timeout = 120
        };

        job.JobTasks.Add(sms);

        var jh = new JobHandler
        {
            CurrentJob = job
        };

        jh.Mailers.Add(new FakeJobMailer());
        //jh.Mailers.Add(s);

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);

        TestHelper.OpenFile(sms.PdfTargetFileName);
    }
}
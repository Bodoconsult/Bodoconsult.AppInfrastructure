// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using JobExecuter.Dal;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Dal.Model;
using JobExecuter.Test.Helpers;
using NUnit.Framework;

namespace JobExecuter.Test.ExecuterTests;

[TestFixture]
public class SqlMaintenanceExecuterTests
{
    [Test]
    public void TestSendMail()
    {
        var job = new Job
        {
            Name = "Create a compliance report for file system folders",
            Description = "Create a report with the current access rights on a filesystem folder.",
            Timeout = 600
        };


        var smjt = new SqlMaintenanceJobTask
        {
            Name = "Runs a maintenance job on a SqlServer instance",
            MailTo = "robert.leisner@bodoconsult.de",
            HtmlTargetFileName = "d:\\tmp\\JobExecuter\\Maintenance.htm",
            Timeout = 600,
            Server = "192.168.10.125"
        };


        job.JobTasks.Add(smjt);

        var jh = new JobHandler
        {
            CurrentJob = job
        };

        var s = TestDataHelper.GetSmtpMailer();

        jh.Mailers.Add(new FakeJobMailer());
        //jh.Mailers.Add(s);

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
    }
}
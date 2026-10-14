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
public class ActiveDirectoryExecuterTests
{
    [Test]
    public void TestActiveDirectory()
    {

        var job = new Job
        {
            Name = "Report Active Directory",
            Description =
                "",
            Timeout = 120
        };

        var ad = new ActiveDirectoryJobTask
        {
            Name = "Report Active Directory",
            Description =
                "Report the current domains Active Directory structure with organisation units (OU), users, groups and computers.",
            HtmlTargetFileName = Path.Combine(TestDataHelper.LocalPath, "ActiveDirectory.htm")
        };

        if (File.Exists(ad.HtmlTargetFileName))
        {
            File.Delete(ad.HtmlTargetFileName);
        }

        job.JobTasks.Add(ad);

        var jh = new JobHandler();
        jh.Mailers.Add(new FakeJobMailer());

        jh.CurrentJob = job;

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);


        TestHelper.OpenFile(ad.HtmlTargetFileName);
    }

}
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
public class FolderComplianceExecuterTests
{
    [Test]
    public void TestRunJob()
    {
        var job = new Job
        {
            Name = "Create a compliance report for file system folders",
            Description = "Create a report with the current access rights on a filesystem folder.",
            Timeout = 120
        };

        var fc = new FolderComplianceJobTask
        {
            Name = "Create a compliance report for file system folders",
            MailTo = "robert.leisner@bodoconsult.de",
                

            HtmlTargetFileName = Path.Combine(TestDataHelper.LocalPath, "Compliance2.htm"),
            Timeout = 60
        };

        fc.FolderPaths.Add(new ComplianceFolder
        {
            Path=@"D:\daten\projekte",
            ShowPermissionsOfPredefinedAccounts = true,
            IncludeSubFolders = true
        });

        job.JobTasks.Add(fc);

        var jh = new JobHandler
        {
            CurrentJob = job
        };

        jh.Mailers.Add(new FakeJobMailer());
        //jh.Mailers.Add(s);

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);

        TestHelper.OpenFile(fc.HtmlTargetFileName);
    }


}
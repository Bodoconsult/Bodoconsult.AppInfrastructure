// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Helpers;
using JobExecuter.Dal;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Dal.Model;
using NUnit.Framework;
using System.IO;

namespace JobExecuter.Test.ExecuterTests;

[TestFixture]
public class DhcpExecuterTests
{
    [Test]
    public void TestDhcp()
    {

        var job = new Job
        {
            Name = "Report Dhcp",
            Description =
                "",
            Timeout = 120
        };

        var dhcp = new DhcpJobTask
        {
            Name = "Report Dhcp infrastructure",
            Description =
                "Report the current domains DHCP infrastructure",
            HtmlTargetFileName = @"D:\tmp\JobExecuter\DHCP.htm"
        };


        if (File.Exists(dhcp.HtmlTargetFileName)) File.Delete(dhcp.HtmlTargetFileName);

        job.JobTasks.Add(dhcp);

        var jh = new JobHandler();
        jh.Mailers.Add(new FakeJobMailer());

        jh.CurrentJob = job;

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);

        FileSystemHelper.RunInDebugMode(dhcp.HtmlTargetFileName);
    }

}
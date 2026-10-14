// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.IO;
using Bodoconsult.Test.Helpers;
using JobExecuter.Dal;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Dal.Model;
using NUnit.Framework;

namespace JobExecuter.Test.ExecuterTests;

[TestFixture]
public class DnsExecuterTests
{
    [Test]
    public void TestDns()
    {

        var job = new Job
        {
            Name = "Report Dns",
            Description = "",
            Timeout = 120
        };

        var dns = new DnsJobTask
        {
            Name = "Report a DNS server's structure",
            Description =
                "Report DNS server's structure",

            HtmlTargetFileName = @"D:\tmp\JobExecuter\DNS.htm",
            Server = "192.168.10.123"
        };


        if (File.Exists(dns.HtmlTargetFileName)) File.Delete(dns.HtmlTargetFileName);

        job.JobTasks.Add(dns);

        var jh = new JobHandler();
        jh.Mailers.Add(new FakeJobMailer());

        jh.CurrentJob = job;

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);


        TestHelper.OpenFile(dns.HtmlTargetFileName);
    }

}
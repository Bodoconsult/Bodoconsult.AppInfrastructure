// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Helpers;
using JobExecuter.Dal;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Dal.Model;
using JobExecuter.Test.Helpers;
using NUnit.Framework;
using System.IO;

namespace JobExecuter.Test.ExecuterTests;

[TestFixture]
public class DnsExecuterTests
{
    private readonly string _localPath = TestDataHelper.LocalPath;

    /// <summary>
    /// Test requires admin permissions on the DNS server
    /// </summary>
    [Explicit]
    [Test]
    public void TestDns()
    {
        var fileName = Path.Combine(_localPath, "DNS.htm");

        FileSystemHelper.DeleteFile(fileName);

        var job = new Job
        {
            Name = "Report Dns",
            Description = "",
            Timeout = 120
        };

        var dns = new DnsJobTask
        {
            Name = "Report a DNS server's structure",
            Description = "Report DNS server's structure",
            HtmlTargetFileName = fileName,
            Server = "192.168.10.123"
        };

        job.JobTasks.Add(dns);

        var jh = new JobHandler();
        jh.Mailers.Add(new FakeJobMailer());

        jh.CurrentJob = job;

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);

        FileSystemHelper.RunInDebugMode(dns.HtmlTargetFileName);
    }
}
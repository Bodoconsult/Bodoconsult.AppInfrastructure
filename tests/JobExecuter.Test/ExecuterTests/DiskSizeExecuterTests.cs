// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.


// ReSharper disable InconsistentNaming

using JobExecuter.Dal;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Dal.Model;
using NUnit.Framework;

namespace JobExecuter.Test.ExecuterTests;

[TestFixture]
public class DiskSizeExecuterTests
{
    [Test]
    public void TestDiskSizeJobTask_Basic()
    {

        var job = new Job
        {
            Name = "Sample for running a cmd batch file",
            Description = "",
            Timeout = 120
        };

        var ds = new DiskSizeJobTask();
        ds.Drives.Add(new Drive{ Name = "C:", MinimumPercentageOfFreeSpace = 0.05});
        ds.Drives.Add(new Drive{ Name = "D:", MinimumPercentageOfFreeSpace = 0.4});
        ds.Drives.Add(new Drive{ Name = @"\\192.168.10.121\data$", MinimumPercentageOfFreeSpace = 0.3});
        ds.Drives.Add(new Drive { Name = "C:", MinimumPercentageOfFreeSpace = 0.99, IpAddress ="192.168.10.121"});
        //ds.Drives.Add(new Drive { Name = "C:", MinimumPercentageOfFreeSpace = 0.05, IpAddress = "192.168.10.53" });
            
        job.JobTasks.Add(ds);

        var jh = new JobHandler();
        jh.Mailers.Add(new FakeJobMailer());

        jh.CurrentJob = job;

        var result = jh.RunJob();

        Assert.That(!result);
        Assert.That(jh.ErrorItems.Count > 0);
    }

}
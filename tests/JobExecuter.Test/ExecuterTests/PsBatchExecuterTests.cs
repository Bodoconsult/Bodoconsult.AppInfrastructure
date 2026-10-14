// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.IO;
using System.Text;
using JobExecuter.Dal;
using JobExecuter.Dal.Enums;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Dal.Model;
using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace JobExecuter.Test.ExecuterTests;

[TestFixture]
public class PsBatchExecuterTests
{ 
    private const string SourcePath = @"d:\tmp\FolderBackup1";
    private const string FileName = "test.ps1";

    [Test]
    public void TestPsBatchJobTask_V01_NoError()
    {

        // create batch file
        var psFileName = Path.Combine(SourcePath, FileName);
        if (File.Exists(psFileName)) File.Delete(psFileName);
        File.WriteAllText(psFileName,
            "Get-Process\r\n" +
            "Get-Command –Name *IP*\r\n" +
            "Get-Process | Where-Object {$_.Name –eq \"iexplore\"}\r\n", Encoding.GetEncoding("ISO-8859-1"));

        var job = new Job
        {
            Name = "Sample for running a PowerShell batch file",
            Description = "",
            Timeout = 120
        };


        var ps = new PsBatchJobTask
        {
            Name = "Running a PowerShell batch file with PowerShell V01",
            Filename = psFileName,
            PowerShellVersion =  PowerShellVersion.V01
        };

        job.JobTasks.Add(ps);

        var jh = new JobHandler();
        jh.Mailers.Add(new FakeJobMailer());

        jh.CurrentJob = job;

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
        Assert.That(File.Exists(psFileName));
    }

    [Test]
    public void TestPsBatchJobTask_V03_NoError()
    {

        // create batch file
        var psFileName = Path.Combine(SourcePath, FileName);
        if (File.Exists(psFileName)) File.Delete(psFileName);
        File.WriteAllText(psFileName,
            "Get-Process\r\n" +
            "Get-Command –Name *IP*\r\n" +
            "Get-Process | Where-Object {$_.Name –eq \"iexplore\"}\r\n", Encoding.GetEncoding("ISO-8859-1"));

        var job = new Job
        {
            Name = "Sample for running a PowerShell batch file with PowerShell V03",
            Description = "",
            Timeout = 120
        };


        var ps = new PsBatchJobTask
        {
            Name = "Running a PowerShell batch file",
            Filename = psFileName,
            PowerShellVersion = PowerShellVersion.V03
        };

        job.JobTasks.Add(ps);

        var jh = new JobHandler();
        jh.Mailers.Add(new FakeJobMailer());

        jh.CurrentJob = job;

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
        Assert.That(File.Exists(psFileName));
    }
}
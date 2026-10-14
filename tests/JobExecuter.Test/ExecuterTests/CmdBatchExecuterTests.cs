// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.IO;
using JobExecuter.Dal;
using JobExecuter.Dal.Enums;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Dal.Model;
using JobExecuter.Test.Helpers;
using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace JobExecuter.Test.ExecuterTests;

[TestFixture]
public class CmdBatchExecuterTests
{

    private readonly string SourcePath = Path.Combine(TestDataHelper.LocalPath, "FolderBackup1");
    private readonly string TargetPath = Path.Combine(TestDataHelper.LocalPath, "FolderBackup2");

    private const string FileName = "FileToBackup.txt";
    private const string FileName2 = "test.bat";

    [Test]
    public void TestCmdBatchJobTask_WholeFile()
    {

        if (!Directory.Exists(SourcePath)) Directory.CreateDirectory(SourcePath);
        if (!Directory.Exists(TargetPath)) Directory.CreateDirectory(TargetPath);

        var fileName2 = Path.Combine(SourcePath, FileName);

        if (File.Exists(fileName2)) File.Delete(fileName2);
        File.WriteAllText(fileName2, "Testdatei");

        fileName2 = Path.Combine(TargetPath, FileName);
        if (File.Exists(fileName2)) File.Delete(fileName2);

        // create batch file
        var fileName = Path.Combine(SourcePath, FileName2);
        if (File.Exists(fileName)) File.Delete(fileName);
        File.WriteAllText(fileName,
            $"REm dfgsfd\r\ncopy {SourcePath.Replace("\\", "\\\\")}\\*.* {TargetPath.Replace("\\", "\\\\")}\\*.* /y\r\nREM fghdfg\r\ncopy {TargetPath.Replace("\\", "\\\\")}\\*.* {SourcePath.Replace("\\", "\\\\")}\\*.* /y\r\nREM fghfsd\r\n");

        var job = new Job
        {
            Name = "Sample for running a cmd batch file",
            Description =
                "",
            Timeout = 120
        };


        var cb = new CmdBatchJobTask
        {
            RunType = CmdBatchRunType.WholeFile,
            Filename = fileName,
            Timeout = 110
        };

        job.JobTasks.Add(cb);

        var jh = new JobHandler();
        jh.Mailers.Add(new FakeJobMailer());
            
        jh.CurrentJob = job;

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
        Assert.That(File.Exists(fileName));
        Assert.That(File.Exists(fileName2));
    }

    [Test]
    public void TestCmdBatchJobTask_LineByLine()
    {

        if (!Directory.Exists(SourcePath)) Directory.CreateDirectory(SourcePath);
        if (!Directory.Exists(TargetPath)) Directory.CreateDirectory(TargetPath);

        var fileName2 = Path.Combine(SourcePath, FileName);

        if (File.Exists(fileName2)) File.Delete(fileName2);
        File.WriteAllText(fileName2, "Testdatei");

        fileName2 = Path.Combine(TargetPath, FileName);
        if (File.Exists(fileName2)) File.Delete(fileName2);

        // create batch file
        var fileName = Path.Combine(SourcePath, FileName2);
        if (File.Exists(fileName)) File.Delete(fileName);

        File.WriteAllText(fileName,
            $"REm dfgsfd\r\ncopy {SourcePath.Replace("\\", "\\\\")}\\*.* {TargetPath.Replace("\\", "\\\\")}\\*.* /y\r\nREM fghdfg\r\ncopy {TargetPath.Replace("\\", "\\\\")}\\*.* {SourcePath.Replace("\\", "\\\\")}\\*.* /y\r\nREM fghdfg\r\ncopy d:\\tmp\\FolderBackup99\\*.* d:\\tmp\\FolderBackup98\\*.* /y\r\nREM fghfsd\r\n");

        //File.WriteAllText(fileName, "REm dfgsfd\r\ncopy d:\\tmp\\FolderBackup1\\*.* d:\\tmp\\FolderBackup2\\*.* /y\r\n" +
        //            "REM fghdfg\r\ncopy d:\\tmp\\FolderBackup2\\*.* d:\\tmp\\FolderBackup1\\*.* /y\r\n" +
        //            "REM fghdfg\r\ncopy d:\\tmp\\FolderBackup99\\*.* d:\\tmp\\FolderBackup98\\*.* /y\r\n" +
        //            "REM fghfsd\r\n");

        var job = new Job
        {
            Name = "Sample for running a cmd batch file",
            Description =
                "",
            Timeout = 120
        };


        var cb = new CmdBatchJobTask
        {
            Name = "Run a batch (*.bat or *.cmd) with cmd.exe",
            RunType = CmdBatchRunType.LineByLine,
            Filename = fileName,
            Timeout = 110
        };

        job.JobTasks.Add(cb);

        var jh = new JobHandler();
        jh.Mailers.Add(new FakeJobMailer());
            
        jh.CurrentJob = job;

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 1);
        Assert.That(File.Exists(fileName));
        Assert.That(File.Exists(fileName2));
    }


    [Test]
    public void TestCmdBatchJobTask_KillTask()
    {

        if (!Directory.Exists(SourcePath)) Directory.CreateDirectory(SourcePath);
        if (!Directory.Exists(TargetPath)) Directory.CreateDirectory(TargetPath);

        // create batch file
        var fileName = Path.Combine(SourcePath, FileName2);
        if (File.Exists(fileName)) File.Delete(fileName);
        File.WriteAllText(fileName, "notepad.exe");

        var job = new Job
        {
            Name = "Sample for running a cmd batch file",
            Description ="",
            Timeout = 120
        };


        var cb = new CmdBatchJobTask
        {
            RunType = CmdBatchRunType.WholeFile,
            Filename = fileName,
            Timeout = 10
        };

        job.JobTasks.Add(cb);

        var jh = new JobHandler();
        jh.Mailers.Add(new FakeJobMailer());
            
        jh.CurrentJob = job;

        var result = jh.RunJob();

        Assert.That(!result);
        Assert.That(jh.ErrorItems.Count == 1);
    }

    [Test]
    public void TestCmdBatchJobTask_BatchString()
    {

        if (!Directory.Exists(SourcePath)) Directory.CreateDirectory(SourcePath);
        if (!Directory.Exists(TargetPath)) Directory.CreateDirectory(TargetPath);

        var fileName2 = Path.Combine(SourcePath, FileName);

        if (File.Exists(fileName2)) File.Delete(fileName2);
        File.WriteAllText(fileName2, "Testdatei");

        fileName2 = Path.Combine(TargetPath, FileName);
        if (File.Exists(fileName2)) File.Delete(fileName2);

        var job = new Job
        {
            Name = "Sample for running a cmd batch file",
            Description =
                "",
            Timeout = 120
        };


        var cb = new CmdBatchJobTask
        {
            Name="Run a batch commands from a string",
            RunType = CmdBatchRunType.LineByLine,
            Timeout = 110,
            BatchFileContent =
                $"REm dfgsfd\r\ncopy {SourcePath.Replace("\\", "\\\\")}\\*.* {TargetPath.Replace("\\", "\\\\")}\\*.* /y\r\nREM fghdfg\r\ncopy {TargetPath.Replace("\\", "\\\\")}\\*.* {SourcePath.Replace("\\", "\\\\")}\\*.* /y\r\nREM fghfsd\r\n"
        };

        job.JobTasks.Add(cb);

        var jh = new JobHandler();
        jh.Mailers.Add(new FakeJobMailer());

        jh.CurrentJob = job;

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
        Assert.That(File.Exists(fileName2));
    }

}
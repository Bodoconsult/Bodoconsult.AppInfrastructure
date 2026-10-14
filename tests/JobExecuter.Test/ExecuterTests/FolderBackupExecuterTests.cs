// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.IO;
using JobExecuter.Dal;
using JobExecuter.Dal.Enums;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Dal.Model;
using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace JobExecuter.Test.ExecuterTests;

[TestFixture]
public class FolderBackupExecuterTests
{
    private const string SourcePath = @"d:\tmp\FolderBackup1";
    private const string TargetPath = @"d:\tmp\FolderBackup2";

    private const string FileName = "FileToBackup.txt";
    private const string FileName2 = "FileToBackup2.txt";

    [Test]
    public void TestFolderBackupJobTask_CopyOnlyNewerFiles()
    {

        if (!Directory.Exists(SourcePath)) Directory.CreateDirectory(SourcePath);
        if (Directory.Exists(TargetPath)) Directory.Delete(TargetPath, true);

        var fileName = Path.Combine(SourcePath, FileName);

        if (File.Exists(fileName)) File.Delete(fileName);
        File.WriteAllText(fileName, "Testdatei");

        fileName = Path.Combine(TargetPath, FileName);

        if (File.Exists(fileName)) File.Delete(fileName);


        var job = new Job
        {
            Name = "Sample for backup a folder",
            Description = "This backup uses RoboCopy.exe to backup a folder to another folder. Newer files on TargetPath will not be overwritten.",
            Timeout = 120
        };


        var fb = new FolderBackupJobTask
        {
            Name="Backup a folder to a target",
            SourcePath = SourcePath,
            TargetPath = TargetPath,
            BackupType = FolderBackupType.CopyOnlyNewerFiles
        };

        job.JobTasks.Add(fb);

        var jh = new JobHandler();
        jh.Mailers.Add(new FakeJobMailer());
            
        jh.CurrentJob = job;

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
        Assert.That(File.Exists(fileName));
    }

    [Test]
    public void TestFolderBackupJobTask_PlainCopy()
    {

        if (!Directory.Exists(SourcePath)) Directory.CreateDirectory(SourcePath);
        if (Directory.Exists(TargetPath)) Directory.Delete(TargetPath, true);

        var fileName = Path.Combine(SourcePath, FileName);

        if (File.Exists(fileName)) File.Delete(fileName);
        File.WriteAllText(fileName, "Testdatei");

        fileName = Path.Combine(TargetPath, FileName);

        if (File.Exists(fileName)) File.Delete(fileName);


        var job = new Job
        {
            Name = "Sample for backup a folder",
            Description = "This backup uses RoboCopy.exe to backup a folder to another folder. Newer files on TargetPath will not be overwritten.",
            Timeout = 120
        };


        var fb = new FolderBackupJobTask
        {
            SourcePath = SourcePath,
            TargetPath = TargetPath,
            BackupType = FolderBackupType.PlainCopy
        };

        job.JobTasks.Add(fb);

        var jh = new JobHandler();
        jh.Mailers.Add(new FakeJobMailer());
            
        jh.CurrentJob = job;

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
        Assert.That(File.Exists(fileName));
    }

    [Test]
    public void TestFolderBackupJobTask_Mirror()
    {

        if (!Directory.Exists(SourcePath)) Directory.CreateDirectory(SourcePath);
        if (!Directory.Exists(TargetPath)) Directory.CreateDirectory(TargetPath);

        var fileName = Path.Combine(SourcePath, FileName);

        if (File.Exists(fileName)) File.Delete(fileName);
        File.WriteAllText(fileName, "Testdatei");

        var fileName2 = Path.Combine(TargetPath, FileName2);

        if (File.Exists(fileName2)) File.Delete(FileName2);
        File.WriteAllText(fileName2, "Testdatei");

        fileName = Path.Combine(TargetPath, FileName);
        if (File.Exists(fileName)) File.Delete(fileName);

        fileName2 = Path.Combine(SourcePath, FileName2);
        if (File.Exists(fileName2)) File.Delete(fileName2);

        var job = new Job
        {
            Name = "Sample for backup a folder",
            Description = "This backup uses RoboCopy.exe to backup a folder to another folder. Newer files on TargetPath will not be overwritten.",
            Timeout = 120
        };


        var fb = new FolderBackupJobTask
        {
            SourcePath = SourcePath,
            TargetPath = TargetPath,
            BackupType = FolderBackupType.Mirror
        };

        job.JobTasks.Add(fb);

        var jh = new JobHandler();
        jh.Mailers.Add(new FakeJobMailer());
            
        jh.CurrentJob = job;

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
        Assert.That(File.Exists(fileName));
        Assert.That(!File.Exists(fileName2));
    }


    [Test]
    public void TestFolderBackupJobTask_CopyOnlyNewerFiles_RealWorld()
    {

        const string sourcePath = @"\\192.168.10.121\data$\Bodoprivate\002_Geschäftsleitung\13. Bewertungen";
        const string targetPath = @"\\192.168.10.121\data$\Privat\003_Finanzen\BodoPrivate";


        var job = new Job
        {
            Name = "Sample for backup a folder",
            Description = "This backup uses RoboCopy.exe to backup a folder to another folder. Newer files on TargetPath will not be overwritten.",
            Timeout = 120
        };


        var fb = new FolderBackupJobTask
        {
            Name = "Backup a folder to a target",
            SourcePath = sourcePath,
            TargetPath = targetPath,
            BackupType = FolderBackupType.CopyOnlyNewerFiles
        };

        job.JobTasks.Add(fb);

        var jh = new JobHandler();
        jh.Mailers.Add(new FakeJobMailer());

        jh.CurrentJob = job;

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
    }

}
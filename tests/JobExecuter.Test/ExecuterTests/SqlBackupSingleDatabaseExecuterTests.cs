// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.IO;
using JobExecuter.Dal;
using JobExecuter.Dal.Helpers;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Dal.Model;
using JobExecuter.Test.Helpers;
using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace JobExecuter.Test.ExecuterTests;

[TestFixture]
public class SqlBackupSingleDatabaseExecuterTests
{
    private readonly string localPath = TestDataHelper.LocalPath;
    private readonly string targetPath = TestDataHelper.LocalPath;
    private const string fileName = "testbackup.bak";
    private const string fileName2 = "??server??_??database??_??date??_??time??.bak";

    [Test]
    public void TestBackup_OnlyLocal()
    {
        var job = new Job
        {
            Name = "Sample job for SqlServer backup of a single database",
            Description = "This job first saves a SQL database to a local path " +
                          "and then moves the backup file to a network location.",
            Timeout = 120
        };

        var jt = new SqlBackupSingleDatabaseJobTask
        {
            Server = ".\\sqlexpress",
            DatabaseName = "MediaDb",
            LocalTarget = localPath,
            FileName = fileName,
            Timeout = 60
        };

        job.JobTasks.Add(jt);

        var localFile = NamingHelper.ClearPath(Path.Combine(localPath, fileName), jt.Server, jt.DatabaseName);

        if (File.Exists(localFile)) File.Delete(localFile);

        var jh = new JobHandler
        {
            CurrentJob = job
        };
            
        jh.Mailers.Add(new FakeJobMailer());

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
        Assert.That(File.Exists(localFile));
    }

    [Test]
    public void TestBackup_ToNetwork()
    {
        var job = new Job
        {
            Name = "Sample job for SqlServer backup of a single database",
            Description = "This job first saves a SQL database to a local path " +
                          "and then moves the backup file to a network location.",
            Timeout = 120
        };

        var jt = new SqlBackupSingleDatabaseJobTask
        {
            Server = ".\\sqlexpress",
            DatabaseName = "MediaDb",
            LocalTarget = localPath,
            NetworkTarget = targetPath,
            FileName = fileName,
            Timeout = 60
        };

        job.JobTasks.Add(jt);

        var localFile = NamingHelper.ClearPath(Path.Combine(localPath, fileName), jt.Server, jt.DatabaseName);
        var targetFile = NamingHelper.ClearPath(Path.Combine(targetPath, fileName), jt.Server, jt.DatabaseName);

        if (File.Exists(localFile)) File.Delete(localFile);
        if (File.Exists(targetFile)) File.Delete(targetFile);

        var jh = new JobHandler
        {
            CurrentJob = job
        };
            
        jh.Mailers.Add(new FakeJobMailer());

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
        Assert.That(File.Exists(targetFile));
    }

    [Test]
    public void TestBackup_OnlyLocalWithPlaceholders()
    {
        var job = new Job
        {
            Name = "Sample job for SqlServer backup of a single database",
            Description = "This job first saves a SQL database to a local path " +
                          "and then moves the backup file to a network location.",
            Timeout = 120
        };

        var jt = new SqlBackupSingleDatabaseJobTask
        {
            Server = ".\\sqlexpress",
            DatabaseName = "MediaDb",
            LocalTarget = localPath,
            FileName = fileName2,
            Timeout = 60
        };

        job.JobTasks.Add(jt);

        var localFile = NamingHelper.ClearPath(Path.Combine(localPath, fileName2), jt.Server, jt.DatabaseName);

        if (File.Exists(localFile)) File.Delete(localFile);

        var jh = new JobHandler
        {
            CurrentJob = job
        };
            
        jh.Mailers.Add(new FakeJobMailer());

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
        Assert.That(File.Exists(localFile));
    }

    [Test]
    public void TestBackup_ToNetworkWithPlaceholders()
    {
        var job = new Job
        {
            Name = "Sample job for SqlServer backup of a single database",
            Description = "This job first saves a SQL database to a local path " +
                          "and then moves the backup file to a network location.",
            Timeout = 120
        };

        var jt = new SqlBackupSingleDatabaseJobTask
        {
            Server = ".\\sqlexpress",
            DatabaseName = "MediaDb",
            LocalTarget = localPath,
            NetworkTarget = targetPath,
            FileName = fileName2,
            Timeout = 60
        };

        job.JobTasks.Add(jt);

        var localFile = NamingHelper.ClearPath(Path.Combine(localPath, fileName2), jt.Server, jt.DatabaseName);
        var targetFile = NamingHelper.ClearPath(Path.Combine(targetPath, fileName2), jt.Server, jt.DatabaseName);

        if (File.Exists(localFile)) File.Delete(localFile);
        if (File.Exists(targetFile)) File.Delete(targetFile);

        var jh = new JobHandler
        {
            CurrentJob = job
        };
            
        jh.Mailers.Add(new FakeJobMailer());

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
        Assert.That(File.Exists(targetFile));
    }

    //ToDo: test with placeholders in folder path


}
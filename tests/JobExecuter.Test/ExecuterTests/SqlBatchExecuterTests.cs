// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.IO;
using Bodoconsult.App.Helpers;
using JobExecuter.Dal;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Dal.Model;
using JobExecuter.Test.App;
using JobExecuter.Test.Helpers;
using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace JobExecuter.Test.ExecuterTests;

[TestFixture]
public class SqlBatchExecuterTests
{
    private readonly string fileName = $@"{TestDataHelper.LocalPath}\testsql.sql";

    [Test]
    public void BatchRun_NoErrors_Successful()
    {

        const string sql = "Select * from dbo.CommandLog";

        FileSystemHelper.DeleteFile(fileName);

        File.WriteAllText(fileName, sql);

        var job = new Job
        {
            Name = "Sample job for SqlServer batch job",
            Description = "This jobs runs a sample batch file with dummy stored proc",
            Timeout = 120
        };


        var jt = new SqlBatchJobTask
        {
            Server = Globals.Instance.SqlServerForTests,
            DatabaseName = "master",
            FileName = fileName,
            Timeout = 60
        };

        job.JobTasks.Add(jt);


        var jh = new JobHandler
        {
            CurrentJob = job
        };
            
        jh.Mailers.Add(new FakeJobMailer());

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
    }


    [Test]
    public void BatchRun_WithError_Fails()
    {

        const string sql = "exec spTest 1\r\nGO\r\nexec spTest999 2\r\nGO\r\nexec spTest 3";

        FileSystemHelper.DeleteFile(fileName);

        File.WriteAllText(fileName, sql);

        var job = new Job
        {
            Name = "Sample job for SqlServer batch job",
            Description = "This jobs runs a sample batch file with dummy stored proc",
            Timeout = 120
        };


        var jt = new SqlBatchJobTask
        {
            Server = Globals.Instance.SqlServerForTests,
            DatabaseName = "master",
            FileName = fileName,
            Timeout = 60
        };

        job.JobTasks.Add(jt);


        var jh = new JobHandler
        {
            CurrentJob = job
        };
            
        jh.Mailers.Add(new FakeJobMailer());

        var result = jh.RunJob();

        Assert.That(result, Is.False);
        Assert.That(jh.ErrorItems.Count, Is.EqualTo(3));
    }

    [Test]
    public void BatchRun_WithErrorAndEmptyEntries_Successful()
    {

        const string sql = "exec spTest 1\r\ngO\r\n\r\nGO\r\nexec spTest999 2\r\nGO\r\n\r\nGo\r\nexec spTest 3";

        FileSystemHelper.DeleteFile(fileName);

        File.WriteAllText(fileName, sql);

        var job = new Job
        {
            Name = "Sample job for SqlServer batch job",
            Description = "This jobs runs a sample batch file with dummy stored proc",
            Timeout = 120
        };


        var jt = new SqlBatchJobTask
        {
            Name="Run a SQL batch file",
            Server = Globals.Instance.SqlServerForTests,
            DatabaseName = "master",
            FileName = fileName,
            Timeout = 60
        };

        job.JobTasks.Add(jt);


        var jh = new JobHandler
        {
            CurrentJob = job
        };
            
        jh.Mailers.Add(new FakeJobMailer());

        var result = jh.RunJob();

        Assert.That(result, Is.False);
        Assert.That(jh.ErrorItems.Count, Is.EqualTo(3));
    }

    [Test]
    public void BatchRun_FromBatchContentNoErrors_Fails()
    {
        const string sql = "exec dbo.spTest 1\r\nGO\r\nexec dbo.spTest 2\r\nGO\r\nexec dbo.spTest 3";

        var job = new Job
        {
            Name = "Sample job for SqlServer batch job",
            Description = "This jobs runs a sample batch content with dummy stored proc",
            Timeout = 120
        };


        var jt = new SqlBatchJobTask
        {
            Server = Globals.Instance.SqlServerForTests,
            DatabaseName = "master",
            BatchContent = sql,
            Timeout = 60
        };

        job.JobTasks.Add(jt);


        var jh = new JobHandler
        {
            CurrentJob = job
        };

        jh.Mailers.Add(new FakeJobMailer());

        var result = jh.RunJob();

        Assert.That(result, Is.False);
        Assert.That(jh.ErrorItems.Count, Is.EqualTo(3));
    }
}
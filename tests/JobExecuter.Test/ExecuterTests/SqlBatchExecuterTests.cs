// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.IO;
using JobExecuter.Dal;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Dal.Model;
using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace JobExecuter.Test.ExecuterTests;

[TestFixture]
public class SqlBatchExecuterTests
{


        
    private const string fileName = @"d:\tmp\testsql.sql";

    [Test]
    public void TestBatchRun_NoErrors()
    {

        const string sql = "exec spTest 1\r\nGO\r\nexec spTest 2\r\nGO\r\nexec spTest 3";

        if (File.Exists(fileName)) File.Delete(fileName);

        File.WriteAllText(fileName, sql);

        var job = new Job
        {
            Name = "Sample job for SqlServer batch job",
            Description = "This jobs runs a sample batch file with dummy stored proc",
            Timeout = 120
        };


        var jt = new SqlBatchJobTask
        {
            Server = ".\\sqlexpress",
            DatabaseName = "MediaDb",
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
    public void TestBatchRun_WithError()
    {

        const string sql = "exec spTest 1\r\nGO\r\nexec spTest999 2\r\nGO\r\nexec spTest 3";

        if (File.Exists(fileName)) File.Delete(fileName);

        File.WriteAllText(fileName, sql);

        var job = new Job
        {
            Name = "Sample job for SqlServer batch job",
            Description = "This jobs runs a sample batch file with dummy stored proc",
            Timeout = 120
        };


        var jt = new SqlBatchJobTask
        {
            Server = ".\\sqlexpress",
            DatabaseName = "MediaDb",
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

        Assert.That(!result);
        Assert.That(jh.ErrorItems.Count == 1);

    }

    [Test]
    public void TestBatchRun_WithErrorAndEmptyEntries()
    {

        const string sql = "exec spTest 1\r\ngO\r\n\r\nGO\r\nexec spTest999 2\r\nGO\r\n\r\nGo\r\nexec spTest 3";

        if (File.Exists(fileName)) File.Delete(fileName);

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
            Server = ".\\sqlexpress",
            DatabaseName = "MediaDb",
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

        Assert.That(!result);
        Assert.That(jh.ErrorItems.Count == 1);

    }

    [Test]
    public void TestBatchRun_FromBatchContentNoErrors()
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
            Server = ".\\sqlexpress",
            DatabaseName = "MediaDb",
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

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);

    }

}
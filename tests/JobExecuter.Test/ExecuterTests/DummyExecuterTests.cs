// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.IO;
using JobExecuter.Dal;
using JobExecuter.Dal.Model;
using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace JobExecuter.Test.ExecuterTests;

[TestFixture]
public class DummyExecuterTests
{
    [Test]
    public void TestDummyJobTask_Basic()
    {

        var jt = new DummyJobTask
        {
            Message = "Dummy message!"
        };

        var jh = new JobHandler();
        var result = jh.RunTask(jt);

        Assert.That(result);
    }


    [Test]
    public void TestDummyJobTask_TimeOut()
    {

        var jt = new DummyJobTask
        {
            Message = "Dummy message!",
            WaitTime = 5,
            Timeout = 2
        };

        var jh = new JobHandler();
            
        var result = jh.RunTask(jt);

        Assert.That(!result);
    }


    [Test]
    public void TestDummyJobTask_SaveJson()
    {

        const string file = @"d:\tmp\jobExecuter.json";

        if (File.Exists(file)) File.Delete(file);

        var job = new Job
        {
            Name = "Testjob",
            Description = "bla bla bla blubb",
            Timeout = 65
        };


        var jt = new DummyJobTask
        {
            Message = "Dummy message 1!",
            WaitTime = 5,
            Timeout = 2
        };

        job.JobTasks.Add(jt);

        jt = new DummyJobTask
        {
            Message = "Dummy message 2!",
            WaitTime = 5,
            Timeout = 2
        };

        job.JobTasks.Add(jt);

        var jh = new JobHandler
        {
            CurrentJob = job
        };
            
        var result = jh.SaveJob(file);

        Assert.That(result);
        Assert.That(File.Exists(file));
    }


    [Test]
    public void TestDummyJobTask_LoadJson()
    {

        const string file = @"d:\tmp\jobExecuter.json";

        if (File.Exists(file)) File.Delete(file);

        var job = new Job
        {
            Name = "Testjob",
            Description = "bla bla bla blubb",
            Timeout = 65
        };


        var jt = new DummyJobTask
        {
            Message = "Dummy message 1!",
            WaitTime = 5,
            Timeout = 2
        };

        job.JobTasks.Add(jt);

        jt = new DummyJobTask
        {
            Message = "Dummy message 2!",
            WaitTime = 5,
            Timeout = 2
        };

        job.JobTasks.Add(jt);

        var jh = new JobHandler
        {
            CurrentJob = job
        };
            

        jh.SaveJob(file);

        jh.CurrentJob = null;

        var result = jh.LoadJob(job, file);

        Assert.That(result);
        Assert.That(File.Exists(file));
        Assert.That(jh.CurrentJob != null);
        Assert.That(jh.CurrentJob.JobTasks.Count==2);
    }


}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.


// ReSharper disable InconsistentNaming

using JobExecuter.Dal;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Dal.Model;
using JobExecuter.Test.Helpers;
using NUnit.Framework;

namespace JobExecuter.Test.ExecuterTests;

[TestFixture]
public class DummyErrorExecuterTests
{
    [Test]
    public void TestDummyJobTask_Basic()
    {

        var jt = new DummyErrorJobTask
        {
            Message = "Error message!"
        };

        var jh = new JobHandler();
            
        var result = jh.RunTask(jt);

        Assert.That(!result);
        Assert.That(jh.ErrorItems.Count == 1);
    }

    [Test]
    public void TestDummyJobTask_RunJobWithError()
    {

        var job = new Job
        {
            Name = "Testjob",
            Description = "bla bla bla blubb",
            Timeout = 65,
            ErrorMailReceiver =  "robert.leisner@bodoconsult.de",
            SummaryMailReceiver = "robert.leisner@bodoconsult.de"
        };


        var jt = new DummyJobTask
        {
            Message = "Dummy message 1!",
            WaitTime = 5,
            Timeout = 2
        };

        job.JobTasks.Add(jt);


        var je = new DummyErrorJobTask
        {
            Message = "Error message 1!",
            Timeout = 2,
            ErrorMailReceiver = "robert.leisner@bodoconsult.de"
        };

        job.JobTasks.Add(je);


        jt = new DummyJobTask
        {
            Message = "Dummy message 2!",
            WaitTime = 5,
            Timeout = 200
        };


        job.JobTasks.Add(jt);

        var jh = new JobHandler
        {
            CurrentJob = job
        };
            
        jh.Mailers.Add(new FakeJobMailer());
        var result = jh.RunJob();

        Assert.That(!result);
        Assert.That(jh.ErrorItems.Count>0);
    }


    [Test]
    public void TestDummyJobTask_RunJobWithErrorAndTimeout()
    {

        var job = new Job
        {
            Name = "Testjob",
            Description = "bla bla bla blubb",
            Timeout = 15,
            ErrorMailReceiver = "robert.leisner@bodoconsult.de",
            SummaryMailReceiver = "robert.leisner@bodoconsult.de"
        };


        var jt = new DummyJobTask
        {
            Name= "Task1",
            Message = "Dummy message 1 successful!",
            WaitTime = 0,
            Timeout = 2
        };

        job.JobTasks.Add(jt);


        var je = new DummyErrorJobTask
        {
            Name = "Task2",
            Message = "Error message 1!",
            Timeout = 15,
            ErrorMailReceiver = "robert.leisner@bodoconsult.de"
        };

        job.JobTasks.Add(je);


        jt = new DummyJobTask
        {
            Name = "Task3",
            Message = "Dummy message timeout!",
            WaitTime = 4,
            Timeout = 3
        };


        job.JobTasks.Add(jt);

        jt = new DummyJobTask
        {
            Name = "Task4",
            Message = "Dummy message job timeout!",
            WaitTime = 10,
            Timeout = 15
        };


        job.JobTasks.Add(jt);

        var jh = new JobHandler
        {
            CurrentJob = job
        };
            
        jh.Mailers.Add(new FakeJobMailer());
        var result = jh.RunJob();

        Assert.That(!result);
        Assert.That(jh.ErrorItems.Count > 0);
    }


    [Test]
    public void TestDummyJobTask_RunJobWithErrorAndTimeout_2Mailers()
    {

        var job = new Job
        {
            Name = "Testjob",
            Description = "bla bla bla blubb",
            Timeout = 12,
            ErrorMailReceiver = "robert.leisner@bodoconsult.de",
            SummaryMailReceiver = "robert.leisner@bodoconsult.de"
        };


        var jt = new DummyJobTask
        {
            Name = "Task1",
            Message = "Dummy message 1!",
            WaitTime = 5,
            Timeout = 2
        };

        job.JobTasks.Add(jt);


        var je = new DummyErrorJobTask
        {
            Name = "Task2",
            Message = "Error message 1!",
            Timeout = 2,
            ErrorMailReceiver = "robert.leisner@bodoconsult.de"
        };

        job.JobTasks.Add(je);


        jt = new DummyJobTask
        {
            Name = "Task3",
            Message = "Dummy message timeout!",
            WaitTime = 20,
            Timeout = 15
        };


        job.JobTasks.Add(jt);

        var jh = new JobHandler
        {
            CurrentJob = job
        };
            
        jh.Mailers.Add(new FakeJobMailer());

        jh.Mailers.Add(TestDataHelper.GetSmtpMailer());


        var result = jh.RunJob();

        Assert.That(!result);
        Assert.That(jh.ErrorItems.Count > 0);
    }


}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.IO;
using JobExecuter.Dal.Helpers;
using JobExecuter.Dal.Model;
using JobExecuter.GpoPlugIn.Model;
using NUnit.Framework;

namespace JobExecuter.GpoPlugIn.Test;

[TestFixture]
public class GpoJobTaskExecuterTests
{
    [Test]
    public void TestRun()
    {

        var gpotask = new GpoJobTask
        {
            Domain = "BCG-AD.DE",
            TargetDirectory = Path.Combine(@"D:\temp\JobExecuterCore", "GpoPlugIn")
        };

        var factory = gpotask.GetFactory();

        var executer = factory.New(gpotask);

        executer.SaveAsHtml += FileHelper.SaveAsHtml;


        var result = executer.Run();

        Assert.That(executer!=null);
        Assert.That(result);
    }


    [Test]
    public void TestSaveAsCjson()
    {

        var job = new Job
        {
            Name = "Report GPO",
            Description =
                "",
            Timeout = 120
        };

        var gpotask = new GpoJobTask
        {
            Domain = "BCG-AD.DE",
            TargetDirectory = @"D:\temp\JobExecuterCore\GpoPlugIn"
        };



        job.JobTasks.Add(gpotask);


        JsonHelper.SerializeAsFile(@"D:\temp\JobExecuterCore\GpoPlugIn\job.cjson", job);
    }
}
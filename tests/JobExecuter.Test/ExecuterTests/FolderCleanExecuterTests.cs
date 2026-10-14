// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

// ReSharper disable InconsistentNaming

using JobExecuter.Dal;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Dal.Model;
using JobExecuter.Test.Helpers;
using NUnit.Framework;

namespace JobExecuter.Test.ExecuterTests;

[TestFixture]
public class FolderCleanExecuterTests
{
    [Test]
    public void TestFolderCleanJobTask_Basic()
    {
        var jt = new FolderCleanJobTask
        {
            FolderPath = TestDataHelper.LocalPath,
            Days=7

        };

        var jh = new JobHandler();
            
        jh.Mailers.Add(new FakeJobMailer());
        var result = jh.RunTask(jt);

        Assert.That(result);
    }


}
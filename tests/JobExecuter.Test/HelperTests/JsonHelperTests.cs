// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.IO;
using JobExecuter.Dal.Helpers;
using JobExecuter.Dal.Model;
using JobExecuter.Test.Helpers;
using NUnit.Framework;

namespace JobExecuter.Test.HelperTests;

[TestFixture]
public class JsonHelperTests
{
    [Test]
    public void TestCrypt()
    {

        var inputJsonFile = Path.Combine(TestDataHelper.TestDataPath, "job.json");
        var outputJsonFile = Path.Combine(TestDataHelper.TestDataPath, "job.cjson");


        // Arrange
        if (File.Exists(outputJsonFile)) File.Delete(outputJsonFile);

        var job = JsonHelper.LoadJsonFile<Job>(inputJsonFile);

        // Act
        JsonHelper.SerializeAsFile(outputJsonFile, job);

        // Assert
        Assert.That(File.Exists(outputJsonFile));
    }


    [Test]
    public void TestCryptAndDecrypt()
    {


        var inputJsonFile = Path.Combine(TestDataHelper.TestDataPath, "job.json");
        var outputJsonFile = Path.Combine(TestDataHelper.TestDataPath, "job.cjson");


        // Arrange
        if (File.Exists(outputJsonFile)) File.Delete(outputJsonFile);

        var job = JsonHelper.LoadJsonFile<Job>(inputJsonFile);
        JsonHelper.SerializeAsFile(outputJsonFile, job);

        // Act
        var job1 = JsonHelper.LoadJsonFile<Job>(outputJsonFile);

        // Assert
        Assert.That(File.Exists(outputJsonFile));
        Assert.That(job1 != null);
    }


}
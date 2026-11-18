// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.IO;
using System.Runtime.Versioning;
using Bodoconsult.Drawing.Test.Helpers;
using NUnit.Framework;

namespace Bodoconsult.Drawing.Test;

[SupportedOSPlatform("windows")]
public class OpenGraphHelperTests
{


    //[SetUp]
    //public void Setup()
    //{

    //}


    //[TearDown]
    //public void Cleanup()
    //{
    //}


    [Test]
    public void TestSave()
    {
        // Arrange
        var target = Path.Combine(TestHelper.TargetFolder, OpenGraphHelper.OpenGraphFileName);

        if (File.Exists(target))
        {
            File.Delete(target);
        }
        Assert.That(!File.Exists(target));

        OpenGraphHelper.SourceFile = Path.Combine(TestHelper.GetTestDataFolder, "OpenGraphBasis.png");

        // Act
        OpenGraphHelper.Save("Test", "Test description", target);

        // Assert
        Assert.That(File.Exists(target));

    }
}
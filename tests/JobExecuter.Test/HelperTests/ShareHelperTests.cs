// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using JobExecuter.Dal.Helpers;
using NUnit.Framework;

namespace JobExecuter.Test.HelperTests;

[TestFixture]
public class ShareHelperTests
{
    [Test]
    public void TestPathToUnc()
    {
        var result = ShareHelper.PathToUnc(@"D:\Daten\Projekte");

        Debug.Print(result);

        Assert.That(!string.IsNullOrEmpty(result));
    }

    [Test]
    public void TestPathToUncWithError()
    {
        var result = ShareHelper.PathToUnc(@"A:\");

        Debug.Print(result);

        Assert.That(string.IsNullOrEmpty(result));
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using System.Net;
using JobExecuter.Dal.Helpers;
using NUnit.Framework;

namespace JobExecuter.Test.HelperTests;

[TestFixture]
public class HelpHelperTests
{
    [Test]
    public void TestGetHelp()
    {
        var result = HelpHelper.GetHelp();
        Debug.Print(result);
        Assert.That(!string.IsNullOrEmpty(result));
    }

    [Test]
    public void TestNetworkCredentials()
    {
        var c = new NetworkCredential("test", "test");
        Debug.Print(c.Password);
        Assert.That(c.Password == "test");
    }
}
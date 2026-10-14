// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using NUnit.Framework;

namespace JobExecuter.Test.ExecuterTests;

[TestFixture]
public class JobExecuterTests
{
 
    [Test]
    public void TestGetParameterS()
    {
        var erg = Program.Main(["/s"]);
        Assert.That(erg==0);

    }
}
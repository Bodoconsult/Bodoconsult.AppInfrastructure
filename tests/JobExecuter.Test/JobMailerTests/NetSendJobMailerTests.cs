// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Text.Model;
using JobExecuter.Dal.JobMailer;
using NUnit.Framework;

namespace JobExecuter.Test.JobMailerTests;

[TestFixture]
public class NetSendJobMailerTests
{
    [Test]
    public void TestNetSendMailer_Basic()
    {
        var sr = new StructuredText();
        sr.AddHeader1("Eine Testmail an mich!");

        var s = new NetSendJobMailer();

        var result = s.SendMail("rleisner", "Testmail", sr, false);

        Assert.That(result);
    }
}
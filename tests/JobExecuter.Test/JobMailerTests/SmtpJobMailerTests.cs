// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Text.Model;
using JobExecuter.Test.Helpers;
using NUnit.Framework;

// ReSharper disable once InconsistentNaming

namespace JobExecuter.Test.JobMailerTests;

[TestFixture]
internal class SmtpJobMailerTests
{
    [Test]
    [Explicit]
    public void TestSmtpMailer_Basic()
    {

        var sr = new StructuredText();
        sr.AddHeader1("Eine 'Testmail' an mich!");

        var s= TestDataHelper.GetSmtpMailer();
        var result = s.SendMail("robert.leisner@bodoconsult.de", "Testmail", sr, false);

        Assert.That(result);
    }
}
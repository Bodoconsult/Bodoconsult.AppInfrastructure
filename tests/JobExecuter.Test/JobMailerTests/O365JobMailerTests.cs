// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.Text.Model;
using JobExecuter.Test.Helpers;
using NUnit.Framework;

namespace JobExecuter.Test.JobMailerTests;

[TestFixture]
internal class O365JobMailerTests
{
    [Test]
    [Explicit]
    public void SendMail_validAccount_MailSent()
    {

        var sr = new StructuredText();
        sr.AddHeader1("Eine 'Testmail' an mich!");

        var s = TestDataHelper.GetO365Mailer();
        var result = s.SendMail("robert.leisner@bodoconsult.de", "Testmail", sr, false);

        Assert.That(result);
    }
}
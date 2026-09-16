// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System;
using System.IO;
using Bodoconsult.Web.Mail.Converters;
using Bodoconsult.Web.Mail.Mailers;
using Bodoconsult.Web.Mail.Models;
using Bodoconsult.Web.Mail.Test.App;
using Bodoconsult.Web.Mail.Test.Helpers;
using NUnit.Framework;

namespace Bodoconsult.Web.Mail.Test.Mailers;

[TestFixture]
public class O365MailerTests
{
    //private string _baseUrl = Path.Combine(TestHelper.TestDataPath, @"TestData\HtmlLocalData\");
    private readonly string _docUrl = Path.Combine(TestHelper.TestDataPath, @"HtmlLocalData\Sample.txt");

    [Test]
    public void SendMail_ValidParameters_MailSent()
    {
        var account = TestHelper.GetTestO365Account();

        var smtp = new O365Mailer(Globals.Instance.Logger);
        smtp.LoadMailAccount(account);
        smtp.Init();
        smtp.Logon();

        smtp.SendMail(TestHelper.GetTestReceiver(), "Testmail", "dgdgdgdgdgs sfsgdgs sshshsh");

        Assert.That(true);
    }

    [Test]
    public void SendMail_ValidMailMessage_MailSent()
    {
        var account = TestHelper.GetTestO365Account();

        // New message as HTML mail
        var msg = new MailItem
        {
            From = account.MailAddressSender,
            To = TestHelper.GetTestReceiver(),
            // Subject and mail body
            Subject = "Bodoconsult.Web.Mail: test mail",
            Body = "<p>ajHA SADad asd AS Ddad</p>"
        };

        // Send the mail
        var smtp = new O365Mailer(Globals.Instance.Logger);
        smtp.LoadMailAccount(account);
        smtp.Init();
        smtp.Logon();

        smtp.SendMail(msg);

        Assert.That(true);
    }

    [Test]
    public void SendMail_ValidMassMailItem_MailSent()
    {
        // Arrange
        var c = new HtmlToMailConverter { DocUrl = _docUrl };
        c.LoadDocument();
        c.FindImages();
        c.ProcessContent();

        var account = TestHelper.GetTestO365Account();

        var smtp = new O365Mailer(Globals.Instance.Logger);
        smtp.LoadMailAccount(account);
        smtp.Init();
        smtp.Logon();

        var mmi = new MassMailItem
        {
            From = account.MailAddressSender,
            Subject = $"Testmail {DateTime.Now:s}",
            Body = c.Content
        };

        mmi.To.Add(new MailReceiver { EmailAddress = "test@bodoconsult.de" });
        mmi.To.Add(new MailReceiver { EmailAddress = "test@bodoconsult.de" });
        mmi.To.Add(new MailReceiver { EmailAddress = "test@bodoconsult.de" });

        mmi.Images.AddRange(c.Images);

        // Act and assert
        Assert.DoesNotThrow(() =>
        {
            smtp.SendMails(mmi);
        });
    }
}
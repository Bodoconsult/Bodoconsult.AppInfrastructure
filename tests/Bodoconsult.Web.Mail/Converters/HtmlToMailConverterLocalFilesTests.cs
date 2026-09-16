// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Bodoconsult.Web.Mail.Converters;
using Bodoconsult.Web.Mail.Mailers;
using Bodoconsult.Web.Mail.Test.App;
using Bodoconsult.Web.Mail.Test.Helpers;
using Microsoft.Graph.Models;
using MimeKit;
using NUnit.Framework;

namespace Bodoconsult.Web.Mail.Test.Converters;

[TestFixture]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class HtmlToMailConverterLocalFilesTests
{
    private readonly string _baseUrl = Path.Combine(TestHelper.TestDataPath, "HtmlLocalData");
    private readonly string _docUrl = Path.Combine(TestHelper.TestDataPath, @"HtmlLocalData\Sample.txt");

    [Test]
    public void TestLoadDocUrlAndCheckbaseUrlAndLocalFile()
    {
        // Arrange
        // ReSharper disable once UseObjectOrCollectionInitializer
        var c = new HtmlToMailConverter();

        // Act
        c.DocUrl = _docUrl;

        // Assert
        Assert.That(c.BaseUrl, Is.EqualTo(_baseUrl));
        Assert.That(c.LocalFile);
    }

    [Test]
    public void TestLoadDocument()
    {
        // Arrange
        var c = new HtmlToMailConverter
        {
            DocUrl = _docUrl
        };

        // Act
        c.LoadDocument();

        // Assert
        Assert.That(!string.IsNullOrEmpty(c.Content));
        Assert.That(c.LocalFile);
    }

    [Test]
    public void TestFindImages()
    {
        // Arrange
        var c = new HtmlToMailConverter
        {
            DocUrl = _docUrl
        };
        c.LoadDocument();

        // Act
        c.FindImages();

        // Assert
        Assert.That(!string.IsNullOrEmpty(c.Content));
        Assert.That(c.LocalFile);
        Assert.That(c.Images.Count > 0);
        Assert.That(c.Images[0].Url == $@"{_baseUrl}\logo.jpg");
    }

    [Test]
    public void TestProcessContent()
    {
        // Arrange
        var c = new HtmlToMailConverter
        {
            DocUrl = _docUrl
        };
        c.LoadDocument();
        c.FindImages();

        // Act
        c.ProcessContent();


        // Assert
        Assert.That(!string.IsNullOrEmpty(c.Content));
        Assert.That(c.LocalFile);
        Assert.That(c.Images.Count > 0);
        Assert.That(c.Images[0].Url == $@"{_baseUrl}\logo.jpg");
        ArgumentNullException.ThrowIfNull(c.Content);
        Assert.That(!c.Content.Contains(".jpg"));
        Assert.That(c.Content.Contains("cid:"));
    }

    [Test]
    public void SaveToMail_ValidMessage_SentViaSmtp()
    {
        // Arrange
        var msg = new MimeMessage();

        msg.From.Add(new MailboxAddress("robert.leisner@mail.bodoconsult.de", "robert.leisner@mail.bodoconsult.de"));
        msg.To.Add(new MailboxAddress("test@bodoconsult.de", "test@bodoconsult.de")); ;
        msg.Subject = $"Testmail {DateTime.Now:s}";

        var c = new HtmlToMailConverter { DocUrl = _docUrl };
        c.LoadDocument();
        c.FindImages();
        c.ProcessContent();

        var account = TestHelper.GetTestMailAccount();

        // Act
        c.SaveToMail(ref msg);

        var smtp = new SmtpMailer(Globals.Instance.Logger);
        smtp.LoadMailAccount(account);

        smtp.Init();
        smtp.SendMail(msg);
        smtp.Dispose();

        // Assert
        Assert.That(!string.IsNullOrEmpty(c.Content));
        Assert.That(c.LocalFile);
        Assert.That(c.Images.Count > 0);
        Assert.That(c.Images[0].Url == $@"{_baseUrl}\logo.jpg");
        ArgumentNullException.ThrowIfNull(c.Content);
        Assert.That(!c.Content.Contains(".jpg"));
        Assert.That(c.Content.Contains("cid:"));
    }

    [Test]
    public void SaveToMail_ValidMessage_SentViaO365()
    {
        // Arrange
        const string to = "test@bodoconsult.de";

        var msg = new Message();

        var receips = to.Split([';']).Select(receiver => new Recipient { EmailAddress = new EmailAddress { Address = receiver } }).ToList();

        msg.ToRecipients = receips;
        msg.Subject = $"Testmail {DateTime.Now:s}";

        var c = new HtmlToMailConverter { DocUrl = _docUrl };
        c.LoadDocument();
        c.FindImages();
        //c.GetLinkedRessources();
        c.ProcessContent();

        var account = TestHelper.GetTestO365Account();

        // Act
        c.SaveToMail(ref msg);


        var smtp = new O365Mailer(Globals.Instance.Logger);
        smtp.LoadMailAccount(account);

        smtp.Logon();
        smtp.SendMail(msg);
        //smtp.Dispose();

        // Assert
        Assert.That(!string.IsNullOrEmpty(c.Content));
        Assert.That(c.LocalFile);
        Assert.That(c.Images.Count > 0);
        Assert.That(c.Images[0].Url == $@"{_baseUrl}\logo.jpg");
        //Assert.That(c.LinkedResources.Count > 0);
        //Assert.That(!c.Content.Contains(".jpg"));
        //Assert.That(c.Content.Contains("cid:"));
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.IO;
using BodoFileTransfer.Business.Model;
using BodoFileTransfer.Business.Services;
using BodoFileTransfer.Test.App;
using BodoFileTransfer.Test.Fakes;
using BodoFileTransfer.Test.Helper;
using NUnit.Framework;

namespace BodoFileTransfer.Test;

[TestFixture]
internal class O365MailServiceTests
{
    [Test]
    public void TestSendMail()
    {
        // Arrange 
        var accountHandler = new FakeAccountHandler();

        var md = new AccountMailData
        {
            O365SenderAccount = TestHelper.GetTestO365Account()
        };

        accountHandler.AccountMailData = md;

        var o = new O365MailService(accountHandler, Globals.Instance);

        const string to = "robert.leisner@bodoconsult.de";
        const string subject = "Test";
        const string body = "<h1>Hallo</h1><p>Das ist ein Text.</p>";
        var fileName = Path.Combine(TestHelper.TestData, "Test.pdf");

        // Act and assert
        Assert.DoesNotThrow(() =>
        {
            o.SendMail(to, subject, body, fileName, false, null);
        });
    }
}
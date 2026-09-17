// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.IO;
using BodoFileTransferCore.Business.Interfaces;
using BodoFileTransferCore.Business.Model;
using BodoFileTransferCore.Business.Services;
using BodoFileTransferCore.Business.Test.Fakes;
using BodoFileTransferCore.Business.Test.Helper;
using NUnit.Framework;

namespace BodoFileTransferCore.Business.Test;

[TestFixture]
internal class UnitTestO365MailService
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

        var o = new O365MailService(accountHandler);

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
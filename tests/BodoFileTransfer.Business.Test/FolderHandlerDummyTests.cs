// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Logging;
using BodoFileTransfer.Business.FolderHandling;
using BodoFileTransfer.Business.Test.App;
using BodoFileTransfer.Business.Test.Helper;
using NUnit.Framework;

namespace BodoFileTransfer.Business.Test;

[TestFixture]
public class FolderHandlerDummyTests
{
    [Test]
    public void TestProcessAccounts()
    {
        // Arrange
        var config = TestHelper.GetDefaultConfig();

        var d = TestHelper.GetDummyDataHandler();

        var logger = AppLoggerExtensions.GetFakeAppLoggerProxy();

        var fh = new FolderHandler(d, config, Globals.Instance);

        // Act
        var result = fh.ProcessAccounts();


        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void TestProcessImapAccounts()
    {
        // Arrange
        var config = TestHelper.GetDefaultConfig();

        var d = TestHelper.GetDummyDataHandler();

        var logger = AppLoggerExtensions.GetFakeAppLoggerProxy();

        var fh = new FolderHandler(d, config, Globals.Instance);

        // Act
        var result = fh.ProcessImapAccounts();


        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void TestCompleteWorkFlow()
    {
        // Arrange
        var config = TestHelper.GetDefaultConfig();

        var d = TestHelper.GetDummyDataHandler();

        var logger = AppLoggerExtensions.GetFakeAppLoggerProxy();

        var fh = new FolderHandler(d, config, Globals.Instance);

        // Act
        var result = fh.ProcessImapAccounts();
        var result1 = fh.ProcessAccounts();

        // Assert
        Assert.That(result, Is.False);
        Assert.That(result1, Is.False);
    }

    [Test]
    public void TestSendSmtpMails()
    {
        // Arrange
        var config = TestHelper.GetDefaultConfig();

        var d = TestHelper.GetDummyDataHandler();

        var logger = AppLoggerExtensions.GetFakeAppLoggerProxy();

        var fh = new FolderHandler(d, config, Globals.Instance);

        var result = fh.ProcessImapAccounts();
        var result1 = fh.ProcessAccounts();

        Assert.That(result, Is.False);
        Assert.That(result1, Is.False);

        // Act
        var sendResult = fh.SendOutboundMails();


        // Assert
        Assert.That(sendResult);
    }
}
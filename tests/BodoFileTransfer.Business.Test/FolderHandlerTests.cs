// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using BodoFileTransfer.Business.DataHandling;
using BodoFileTransfer.Business.FolderHandling;
using BodoFileTransfer.Business.Test.App;
using BodoFileTransfer.Business.Test.Helper;
using NUnit.Framework;

namespace BodoFileTransfer.Business.Test;

[TestFixture]
public class FolderHandlerTests
{
    [Test]
    public void TestProcessAccounts()
    {
        // Arrange
        var config = TestHelper.GetDefaultConfig();

        TestHelper.GetDummyDataHandler();

        var d = new DataHandler(Globals.Instance);

        var fh = new FolderHandler(d, config, Globals.Instance);

        // Act
        var result = fh.ProcessAccounts();


        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void TestSendSmtpMails()
    {
        // Arrange
        var config = TestHelper.GetDefaultConfig();

        TestHelper.GetDummyDataHandler();

        var d = new DataHandler(Globals.Instance);

        var fh = new FolderHandler(d, config, Globals.Instance);

        // Act
        var result = fh.ProcessAccounts();
        var result1 = fh.SendOutboundMails();

        // Assert
        Assert.That(result, Is.False);
    }
}
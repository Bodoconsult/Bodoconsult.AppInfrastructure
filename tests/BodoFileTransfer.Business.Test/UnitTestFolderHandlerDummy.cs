using BodoFileTransferCore.Business.Logging;
using BodoFileTransferCore.Business.Test.Helper;
using NUnit.Framework;

namespace BodoFileTransferCore.Business.Test;

[TestFixture]
public class UnitTestFolderHandlerDummy
{
    [Test]
    public void TestProcessAccounts()
    {
        // Arrange
        var config = TestHelper.GetDefaultConfig();

        var d = TestHelper.GetDummyDataHandler();

        var logger = AppLoggerExtensions.GetFakeAppLoggerProxy();

        var fh = new FolderHandler(d, config, logger);

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

        var fh = new FolderHandler(d, config, logger);

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

        var fh = new FolderHandler(d, config, logger);

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

        var fh = new FolderHandler(d, config, logger);

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
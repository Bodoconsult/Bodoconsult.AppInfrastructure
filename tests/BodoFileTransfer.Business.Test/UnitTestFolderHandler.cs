using BodoFileTransferCore.Business.App;
using BodoFileTransferCore.Business.DataHandling;
using BodoFileTransferCore.Business.Logging;
using BodoFileTransferCore.Business.Test.Helper;
using NUnit.Framework;

namespace BodoFileTransferCore.Business.Test;

[TestFixture]
public class UnitTestFolderHandler
{
    [Test]
    public void TestProcessAccounts()
    {
        // Arrange
        var config = TestHelper.GetDefaultConfig();

        TestHelper.GetDummyDataHandler();

        var d = new DataHandler(GlobalValues.CurrentAppSettings);

        var logger = AppLoggerExtensions.GetFakeAppLoggerProxy();

        var fh = new FolderHandler(d, config, logger);

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

        var d = new DataHandler(GlobalValues.CurrentAppSettings);

        var logger = AppLoggerExtensions.GetFakeAppLoggerProxy();

        var fh = new FolderHandler(d, config, logger);

        // Act
        var result = fh.ProcessAccounts();
        var result1 = fh.SendOutboundMails();

        // Assert
        Assert.That(result, Is.False);
    }
}
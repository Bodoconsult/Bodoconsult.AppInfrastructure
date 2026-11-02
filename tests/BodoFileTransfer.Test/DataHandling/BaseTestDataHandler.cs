// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bodoconsult.App.Logging;
using BodoFileTransfer.Business.Enums;
using BodoFileTransfer.Business.FolderHandling;
using BodoFileTransfer.Business.Interfaces;
using BodoFileTransfer.Business.Model;
using BodoFileTransfer.Test.App;
using BodoFileTransfer.Test.Helper;
using NUnit.Framework;

namespace BodoFileTransfer.Test.DataHandling;

/// <summary>
/// Base test class for <see cref="IDataHandler"/> tests
/// </summary>
public abstract class BaseTestDataHandler
{
    protected IDataHandler DataHandler;
    protected string TestFilePath = Path.Combine(Path.GetTempPath(),  @"test.pdf");

    [Test]
    public void TestGetAllAccounts()
    {
        // Arrange

        // Act
        var result = DataHandler.GetAllAccounts();

        // Assert
        Assert.That(result.Count > 0, Is.Not.Null);
    }

    [Test]
    public void GetAllImapAccounts_ValidSetup_ReturnsAllCounts()
    {
        // Arrange
        IList<ImapAccount> accounts = null;

        // Act
        Assert.DoesNotThrow(() =>
        {
            accounts = DataHandler.GetAllImapAccounts();
        });

        // Assert
        Assert.That(accounts, Is.Not.Null);
    }

    [Test]
    public void GetAllO365Accounts_ValidSetup_ReturnsAllCounts()
    {
        // Arrange
        IList<O365Account> accounts = null;

        // Act
        Assert.DoesNotThrow(() =>
        {
            accounts = DataHandler.GetAllO365Accounts();
        });

        // Assert
        Assert.That(accounts, Is.Not.Null);
    }

    [Test]
    public void TestGetAllO365SenderAccounts()
    {
        // Arrange
        IList<O365SenderAccount> accounts = null;

        // Act
        Assert.DoesNotThrow(() =>
        {
            accounts = DataHandler.GetAllO365SenderAccounts();
        });

        // Assert
        Assert.That(accounts, Is.Not.Null);
    }

    //[Test]
    //public void TestTransferSmtpMails()
    //{
    //    // Arrange


    //    // Act
    //    var result = DataHandler.TransferSmtpMails();

    //    // Assert
    //    Assert.That(!result);

    //}

    [Test]
    public void TestProcessImapAccounts()
    {
        // Arrange
        var config = TestHelper.GetDefaultConfig();

        var fh = new FolderHandler(DataHandler, config, Globals.Instance);

        // Act
        var result = fh.ProcessImapAccounts();
        
        // Assert
        Assert.That(!result, Is.Not.Null);
    }

    [Test]
    public void TestCompleteWorkFlow()
    {
        // Arrange
        var config = TestHelper.GetDefaultConfig();

        var logger = AppLoggerExtensions.GetFakeAppLoggerProxy();

        var fh = new FolderHandler(DataHandler, config, Globals.Instance);

        // Act
        var result = fh.ProcessImapAccounts();
        var result1 = fh.ProcessAccounts();

        // Assert
        Assert.That(!result, Is.Not.Null);
        Assert.That(!result1, Is.Not.Null);
    }

    [Test]
    public void TestGetFilesForAnAccount()
    {
        // Arrange
        const int accountId = 1;

        var doc = GetDocFile();

        Assert.DoesNotThrow(() =>
        {
            DataHandler.AddFile(doc);
        });

        // Act
        var result = DataHandler.GetFilesForAnAccount(accountId);

        // Assert
        Assert.That(result.Count > 0, Is.Not.Null);

    }

    [Test]
    public void TestGetFilesArchiveForAnAccount()
    {
        // Arrange
        const int accountId = 1;

        var doc = GetDocFile();

        Assert.DoesNotThrow(() =>
        {
            DataHandler.AddFileToArchive(doc);
        });

        // Act
        var result = DataHandler.GetFilesArchiveForAnAccount(accountId);

        // Assert
        Assert.That(result.Count > 0, Is.Not.Null);
    }

    [Test]
    public void TestAddTrace()
    {
        // Arrange
        var trace = new TraceEntry
        {
            Message = "Hallo",
            MessageCode = TraceMessageCode.ApplicationEvent,
            FileId = Guid.NewGuid()
        };

        // Act and assert
        Assert.DoesNotThrow(() =>
        {
            DataHandler.AddTrace(trace);
        });
    }


    [Test]
    public void TestGetTracesForFile()
    {
        // Arrange
        var trace = new TraceEntry
        {
            Message = "Hallo",
            MessageCode = TraceMessageCode.ApplicationEvent,
            FileId = Guid.NewGuid()
        };

        Assert.DoesNotThrow(() =>
        {
            DataHandler.AddTrace(trace);
        });

        // Act
        var result = DataHandler.GetTracesForFile(trace.FileId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Any(), Is.Not.False);
    }


    [Test]
    public void TestAddFile()
    {
        // Arrange
        var count = DataHandler.GetFilesCount();

        var doc = GetDocFile();

        // Act
        //Assert.DoesNotThrow(() =>
        //{
        DataHandler.AddFile(doc);
        //});


        // Assert
        var resultCount = DataHandler.GetFilesCount();
        Assert.That(count + 1, Is.EqualTo(resultCount));

    }

    private DocumentFile GetDocFile()
    {
        return new()
        {
            AccountId = 1,
            Keyword = "Test",
            Path = TestFilePath,
            Title = "Testbeleg"
        };
    }

    [Test]
    public void TestAddFileToArchive()
    {
        // Arrange
        var count = DataHandler.GetFilesArchiveCount();

        var doc = GetDocFile();

        // Act
        Assert.DoesNotThrow(() =>
        {
            DataHandler.AddFileToArchive(doc);
        });

        // Assert
        var resultCount = DataHandler.GetFilesArchiveCount();
        Assert.That(count + 1, Is.EqualTo(resultCount));
    }

    [Test]
    public void TestDeleteFile()
    {
        // Arrange
        var doc = GetDocFile();

        var count = DataHandler.GetFilesCount();

        Assert.DoesNotThrow(() =>
        {
            DataHandler.AddFile(doc);
        });

        var resultCount = DataHandler.GetFilesCount();
        Assert.That(count + 1, Is.EqualTo(resultCount));

        // Act
        DataHandler.DeleteFile(doc.Id);

        // Assert
        resultCount = DataHandler.GetFilesCount();
        Assert.That(count, Is.EqualTo(resultCount));
    }

    [Test]
    public void TestDeleteFileArchive()
    {
        // Arrange
        var count = DataHandler.GetFilesArchiveCount();

        var doc = GetDocFile();

        Assert.DoesNotThrow(() =>
        {
            DataHandler.AddFileToArchive(doc);
        });

        var resultCount = DataHandler.GetFilesArchiveCount();
        Assert.That(count + 1, Is.EqualTo(resultCount));

        // Act
        DataHandler.DeleteFileArchive(doc.Id);


        // Assert
        resultCount = DataHandler.GetFilesArchiveCount();
        Assert.That(count, Is.EqualTo(resultCount));
    }

    [Test]
    public void TestGetFile()
    {
        // Arrange
        var doc = GetDocFile();

        Assert.DoesNotThrow(() =>
        {
            DataHandler.AddFile(doc);
        });


        // Act
        var result = DataHandler.GetFile(doc.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(doc.Id, Is.EqualTo( result.Id));

    }

    [Test]
    public void TestGetFileArchive()
    {
        // Arrange
        var doc = GetDocFile();

        Assert.DoesNotThrow(() =>
        {
            DataHandler.AddFileToArchive(doc);
        });

        // Act
        var result = DataHandler.GetFileArchive(doc.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(doc.Id, Is.EqualTo(result.Id));
    }

    [Test]
    public void TestGetAccountMailData()
    {
        // Arrange
        const int accountId = 1;

        // Act

        var result = DataHandler.GetAccountMailData(accountId);

        // Assert
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void TestGetMailTransferAccounts()
    {
        // Arrange

        // Act
        var result = DataHandler.GetMailTransferAccounts();

        // Assert
        Assert.That(result, Is.Not.Null);
    }
}
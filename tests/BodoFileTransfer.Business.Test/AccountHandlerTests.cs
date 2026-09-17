// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.IO;
using System.Linq;
using BodoFileTransfer.Business.Interfaces;
using BodoFileTransfer.Business.Model;
using BodoFileTransfer.Business.Test.App;
using BodoFileTransfer.Business.Test.Helper;
using NUnit.Framework;

namespace BodoFileTransfer.Business.Test;

[TestFixture]
public class AccountHandlerTests
{
    private IDataHandler _dataHandler;

    private const int AccountId = 1;

    private IAccountHandler _accountHandler;

    [SetUp]
    public void Setup()
    {

        var config = TestHelper.GetDefaultConfig();

        var dummyHandler = TestHelper.GetDummyDataHandler();

        var file = new DocumentFile
        {
            AccountId = 1,
            Keyword = "Beleg",
            Path = @"D:\temp\test.pdf",
            Title = "Steuerbescheinigung 2021"
        };

        dummyHandler.Files.Add(file);

        _dataHandler = dummyHandler;

        _accountHandler = new AccountHandling.AccountHandler(AccountId, _dataHandler, config, Globals.Instance);


    }

    [Test]
    public void TestCtor()
    {
        // Arrange 

        // Act  

        // Assert
        Assert.That(AccountId, Is.EqualTo( _accountHandler.Account.Id));

    }


    [Test]
    public void TestGetAccountMailData()
    {
        // Arrange 

        // Act  
        var result = _accountHandler.GetAccountMailData();

        // Assert
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void TestGetFiles()
    {
        // Arrange 

        // Act  
        var result = _accountHandler.GetFiles();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Any());
    }

    [Test]
    public void TestGetFilesByDate()
    {
        // Arrange 
        var db = DateTime.Now;
        var dv = db.AddYears(-1);

        // Act  
        var result = _accountHandler.GetFilesByDate(dv, db);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Any());
    }


    [Test]
    public void TestGetFilesArchiveByDate()
    {
        // Arrange 
        var db = DateTime.Now;
        var dv = db.AddYears(-1);

        // Act  
        var result = _accountHandler.GetFilesArchiveByDate(dv, db);

        // Assert
        Assert.That(result, Is.Not.Null);
        //Assert.That(result.Any());
    }


    [Test]
    public void TestGetFilesArchive()
    {
        // Arrange 

        // Act  
        var result = _accountHandler.GetFilesArchive();

        // Assert
        Assert.That(result, Is.Not.Null);
    }


    [Test]
    public void TestAddFile()
    {
        // Arrange
        var doc = TestHelper.GetNewDocument();

        // Act
        Assert.DoesNotThrow(() =>
        {
            _accountHandler.AddFile(doc);
        });


        // Assert
        var result = _accountHandler.GetFile(doc.Id);
        Assert.That(result, Is.Not.Null);
    }


    [Test]
    public void TestAddFileFromInbox()
    {
        // Arrange
        const int counter = 99;

        var id = Guid.Empty;

            
        const string fileName = @"d:\temp\test.pdf";

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
            Assert.That(!File.Exists(fileName));
        }
            
        File.WriteAllText(fileName, "Hallo");
        Assert.That(File.Exists(fileName));

        var fi = new FileInfo(fileName);

        var targetFileName = _accountHandler.GetNewFileName(fi, counter);

        if (File.Exists(targetFileName))
        {
            File.Delete(targetFileName);
            Assert.That(!File.Exists(targetFileName));
        }


        // Act
        Assert.DoesNotThrow(() =>
        {
            id = _accountHandler.AddFileFromInbox(fi, counter);
        });


        // Assert
        var result = _accountHandler.GetFile(id);
        Assert.That(result, Is.Not.Null);
    }


    [Test]
    public void TestAddFileArchive()
    {
        // Arrange
        var doc = TestHelper.GetNewDocument();

        // Act
        Assert.DoesNotThrow(() =>
        {
            _accountHandler.AddFileArchive(doc);
        });


        // Assert
        var result = _accountHandler.GetFileArchive(doc.Id);
        Assert.That(result, Is.Not.Null);
    }



    [Test]
    public void TestReSendFile()
    {
        // Arrange
        var doc = TestHelper.GetNewDocument();

        Assert.DoesNotThrow(() =>
        {
            _accountHandler.AddFileArchive(doc);
        });

        var result = _accountHandler.GetFileArchive(doc.Id);
        Assert.That(result, Is.Not.Null);

        // Act
        Assert.DoesNotThrow(() =>
        {
            _accountHandler.ReSendFile(doc);
        });

        // Assert
        result = _accountHandler.GetFileArchive(doc.Id);
        Assert.That(result, Is.Null);

        result = _accountHandler.GetFile(doc.Id);
        Assert.That(result, Is.Not.Null);
    }
}
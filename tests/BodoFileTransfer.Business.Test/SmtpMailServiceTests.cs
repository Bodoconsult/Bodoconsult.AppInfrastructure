// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Linq;
using BodoFileTransfer.Business.Interfaces;
using BodoFileTransfer.Business.Model;
using BodoFileTransfer.Business.Services;
using BodoFileTransfer.Business.Test.App;
using BodoFileTransfer.Business.Test.Helper;
using NUnit.Framework;

namespace BodoFileTransfer.Business.Test;

[TestFixture]
public class SmtpMailServiceTests
{
    private IDataHandler _dataHandler;

    private ISmtpMailService _mailservice;

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


        _mailservice = new SmtpMailService(_accountHandler);

    }


    [Test]
    public void TestCtor()
    {
        // Arrange

        // Act
            
        // Assert
        Assert.That(AccountId, Is.EqualTo( _mailservice.AccountHandler.Account.Id));
        Assert.That(_mailservice.DocumentFiles, Is.Null);
    }


    [Test]
    public void TestGetDocumentFiles()
    {
        // Arrange

        // Act
        _mailservice.GetDocumentFiles();

        // Assert
        Assert.That(_mailservice.DocumentFiles, Is.Not.Null);
        Assert.That(_mailservice.DocumentFiles.Any());
    }


    [Test]
    public void TestGetSubject()
    {
        // Arrange
        _mailservice.GetDocumentFiles();
        Assert.That(_mailservice.DocumentFiles, Is.Not.Null);
        Assert.That(_mailservice.DocumentFiles.Any());

        // Act
        _mailservice.CreateSubject();

        // Assert
        Assert.That(!string.IsNullOrEmpty(_mailservice.Subject));
    }

    [Test]
    public void TestCreateMails()
    {
        // Arrange
        _mailservice.GetDocumentFiles();
        Assert.That(_mailservice.DocumentFiles, Is.Not.Null);
        Assert.That(_mailservice.DocumentFiles.Any());

        _mailservice.CreateSubject();
        Assert.That(!string.IsNullOrEmpty(_mailservice.Subject));

        // Act
        _mailservice.CreateMails();

        // Assert
        Assert.That(_mailservice.Mails, Is.Not.Null);
        Assert.That(_mailservice.Mails.Any());
    }


    [Test]
    public void TestSendMails()
    {
        // Arrange
        _mailservice.GetDocumentFiles();
        Assert.That(_mailservice.DocumentFiles, Is.Not.Null);
        Assert.That(_mailservice.DocumentFiles.Any());

        _mailservice.CreateSubject();
        Assert.That(!string.IsNullOrEmpty(_mailservice.Subject));

        _mailservice.CreateMails();
        Assert.That(_mailservice.Mails, Is.Not.Null);
        Assert.That(_mailservice.Mails.Any());

        // Act
        Assert.DoesNotThrow(() =>
        {
            _mailservice.SendMails();
        });

        // Assert

    }
}
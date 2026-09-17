// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Linq;
using BodoFileTransfer.Business.AccountHandling;
using BodoFileTransfer.Business.Interfaces;
using BodoFileTransfer.Business.Model;
using BodoFileTransfer.Business.Test.App;
using BodoFileTransfer.Business.Test.Helper;
using NUnit.Framework;

namespace BodoFileTransfer.Business.Test;

[TestFixture]
public class AccountManagementHandlerTests
{
    private IDataHandler _dataHandler;

    private const int AccountId = 1;

    private IAccountManagementHandler _accountManagementHandler;

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

        _accountManagementHandler = new AccountManagementHandler(_dataHandler, config, Globals.Instance);


    }

    [Test]
    public void TestLoadAccounts()
    {
        // Arrange 

        // Act  
        _accountManagementHandler.LoadAccounts();

        // Assert
        Assert.That(_accountManagementHandler.Accounts, Is.Not.Null);
        Assert.That(_accountManagementHandler.AccountHandlers, Is.Not.Null);
        Assert.That(_accountManagementHandler.Accounts.Any());
        Assert.That(_accountManagementHandler.AccountHandlers.Any());

    }


    [Test]
    public void TestSelectAccount()
    {
        // Arrange 
        _accountManagementHandler.LoadAccounts();
        Assert.That(_accountManagementHandler.Accounts.Any());
        Assert.That(_accountManagementHandler.AccountHandlers.Any());

        var account = _accountManagementHandler.Accounts[0];

        // Act  
        _accountManagementHandler.SelectAccount(account);

        // Assert
        Assert.That(_accountManagementHandler.Accounts, Is.Not.Null);
        Assert.That(_accountManagementHandler.AccountHandlers, Is.Not.Null);
        Assert.That(_accountManagementHandler.Accounts.Any());
        Assert.That(_accountManagementHandler.AccountHandlers.Any());

        Assert.That(account.Id, Is.EqualTo( _accountManagementHandler.CurrentAccount.Id));
        Assert.That(account.Id, Is.EqualTo( _accountManagementHandler.CurrentAccountHandler.Account.Id));
    }
}
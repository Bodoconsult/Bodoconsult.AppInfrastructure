// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.Generic;
using System.Linq;
using Bodoconsult.App.Abstractions.Interfaces;
using BodoFileTransfer.Business.Interfaces;
using BodoFileTransfer.Business.Model;

namespace BodoFileTransfer.Business.AccountHandling;

/// <summary>
/// Manages accounts for user interface
/// </summary>
public class AccountManagementHandler : IAccountManagementHandler
{

    private readonly IDataHandler _dataHandler;

    private readonly FolderHandlerConfig _config;

    private readonly IAppGlobals _appGlobals;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="dataHandler">Data handler to use</param>
    /// <param name="config">Current folder handler config</param>
    /// <param name="appGlobals">Current app globals</param>
    public AccountManagementHandler(IDataHandler dataHandler, FolderHandlerConfig config, IAppGlobals appGlobals)
    {
        _dataHandler = dataHandler;
        _config=config;
        _appGlobals = appGlobals;
    }


    /// <summary>
    /// Load all accounts
    /// </summary>
    public void LoadAccounts()
    {
        // ToDo: only accounts for a user
        var accounts = _dataHandler.GetAllAccounts();

        if (!accounts.Any())
        {
            return;
        }

        foreach (var account in accounts)
        {
            Accounts.Add(account);
            var ah = new AccountHandler(account, _dataHandler, _config, _appGlobals);
            AccountHandlers.Add(ah);
        }

        SelectAccount(accounts[0]);
    }

    /// <summary>
    /// Current account
    /// </summary>
    public Account CurrentAccount { get; private set; }

    /// <summary>
    /// Current account handler
    /// </summary>
    public IAccountHandler CurrentAccountHandler { get; private set; }

    /// <summary>
    /// All loaded accounts
    /// </summary>
    public IList<Account> Accounts { get; } = new List<Account>();

    /// <summary>
    /// All loaded account handler
    /// </summary>
    public IList<IAccountHandler> AccountHandlers { get; } = new List<IAccountHandler>();

    /// <summary>
    /// Select an account
    /// </summary>
    /// <param name="account">Account to select</param>
    public void SelectAccount(Account account)
    {
        CurrentAccount = account;
        CurrentAccountHandler = AccountHandlers.First(x => x.Account.Id == account.Id);
    }
}
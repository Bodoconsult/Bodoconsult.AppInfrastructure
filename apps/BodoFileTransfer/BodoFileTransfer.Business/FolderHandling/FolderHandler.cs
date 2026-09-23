// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using BodoFileTransfer.Business.Interfaces;
using BodoFileTransfer.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using BodoFileTransfer.Business.AccountHandling;

namespace BodoFileTransfer.Business.FolderHandling;

/// <summary>
/// Handles the folders of the accounts (if supplied)
/// </summary>
public class FolderHandler : IFolderHandler
{
    private readonly IDataHandler _dataHandler;
    private readonly IAppLoggerProxy _appLogger;
    private readonly IAppGlobals _appGlobals;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="dataHandler">Database layer</param>
    /// <param name="config">Current config settings</param>
    /// <param name="appGlobals">Current app globals</param>
    public FolderHandler(IDataHandler dataHandler, IFolderHandlerConfig config, IAppGlobals appGlobals)
    {
        _dataHandler = dataHandler;
        _appLogger = appGlobals.Logger;
        _appGlobals = appGlobals;
        Config = config;
    }

    /// <summary>
    /// All loaded account handlers
    /// </summary>
    public IList<IAccountHandler> AccountHandlers { get; } = new List<IAccountHandler>();

    /// <summary>
    /// Current config
    /// </summary>
    public IFolderHandlerConfig Config { get; private set; }

    /// <summary>
    /// Load all accounts
    /// </summary>
    public void LoadAccounts()
    {
        var data = _dataHandler.GetAllAccounts();
        foreach (var account in data)
        {
            var ah = new AccountHandler(account, _dataHandler, Config, _appGlobals);
            AccountHandlers.Add(ah);
        }
    }

    /// <summary>
    /// Get all folder for accounts and process them
    /// </summary>
    /// <returns>true if error has happened</returns>
    public bool ProcessAccounts()
    {
        foreach (var ah in AccountHandlers)
        {
            Status($"Process account ID{ah.Account.Id} ...");
            ah.ProcessInbox();
            Status($"Process account ID{ah.Account.Id} done!");
        }

        return false;
    }

    /// <summary>
    /// Process all O365 accounts
    /// </summary>
    /// <returns>true if one or more errors happened</returns>
    public bool ProcessO365Accounts()
    {
        Status("Process O365 accounts...");

        var result = false;
        var data = _dataHandler.GetAllO365Accounts();
        foreach (var account in data)
        {
            try
            {
                Status($"Process O365 account ID{account.Id} ...");

                account.SentDateFilter = Config.SentDateFilter;

                ProcessO365Account(account);
                Status($"Process O365 account ID{account.Id} done!");
            }
            catch (Exception e)
            {
                Error($"Error processing O365 account ID{account.Id}: {e}");
                result = true;
            }
        }

        return result;
    }

    private void ProcessO365Account(O365Account account)
    {
        var exts = account.FileExtensionFilter;

        if (exts.EndsWith(";", StringComparison.OrdinalIgnoreCase))
        {
            exts += ";";
        }

        account.FileExtensionFilter = exts;
        account.SignatureFileExtensions = Config.SignatureFileExtensions;

        var oh = new O365MailHandler(account, _appLogger);
        oh.Login();
        oh.GetAllEmailAttachments();
    }

    /// <summary>
    /// Process all IMAP accounts
    /// </summary>
    /// <returns>true if one or more errors happened</returns>
    public bool ProcessImapAccounts()
    {
        Status("Process IMAP accounts...");

        var result = false;
        var data = _dataHandler.GetAllImapAccounts();
        foreach (var account in data)
        {

            try
            {
                Status($"Process IMAP account ID{account.Id} ...");

                account.SentDateFilter = Config.SentDateFilter;

                ProcessImapAccount(account);
                Status($"Process IMAP account ID{account.Id} done!");
            }
            catch (Exception e)
            {
                Error($"Error processing IMAP account ID{account.Id}: {e}");
                result = true;
            }
        }

        return result;
    }

    /// <summary>
    /// Process an IMAP account
    /// </summary>
    private void ProcessImapAccount(ImapAccount account)
    {
        var exts = account.FileExtensionFilter;

        if (exts.EndsWith(";", StringComparison.OrdinalIgnoreCase))
        {
            exts += ";";
        }

        account.FileExtensionFilter = exts;
        account.SignatureFileExtensions = Config.SignatureFileExtensions;

        //var mh = new ImapMailHandler(account)
        //{
        //    StatusMessage = Status,
        //    FileHandling = account.FileHandling,
        //};
        //mh.CheckMails();
        //mh.Dispose();
    }


    /// <summary>
    /// Send files via email for all not manually sending acounts
    /// </summary>
    public bool SendOutboundMails()
    {
        Status("Transfer files to SMTP transport...");

        var result = true;

        foreach (var ah in AccountHandlers)
        {
            if (ah.Account.ManualTransfer)
            {
                continue;
            }

            if (!ah.SendSmtpMailsAccount())
            {
                result = false;
            }
        }

        Status("Transfer files to SMTP transport done!");
        return result;
    }


    /// <summary>
    /// Send files via email for an account manually
    /// </summary>
    public bool ManuallySendOutboundMails(int accountId)
    {
        Status("Transfer files to email transport...");

        var ah = AccountHandlers.FirstOrDefault(x => x.Account.Id == accountId);

        if (ah == null)
        {
            return false;
        }

        var result = ah.SendSmtpMailsAccount();
        Status("Transfer files to email transport done!");

        return result;
    }

    private void Status(string msg)
    {
        _appLogger.LogInformation(msg);
        _appGlobals.StatusMessageDelegate?.Invoke(msg);
    }

    private void Error(string msg)
    {
        _appLogger.LogError(msg);
        _appGlobals.StatusMessageDelegate?.Invoke(msg);
    }
}
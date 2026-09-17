// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Collections.Generic;

// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace BodoFileTransfer.Business.Interfaces;

public interface IFolderHandler
{
    /// <summary>
    /// All loaded account handlers
    /// </summary>
    IList<IAccountHandler> AccountHandlers { get; }

    /// <summary>
    /// Current config
    /// </summary>
    IFolderHandlerConfig Config { get; }

    /// <summary>
    /// Load all accounts
    /// </summary>
    void LoadAccounts();

    /// <summary>
    /// Get all folder for accounts and process them
    /// </summary>
    /// <returns>true if error has happened</returns>
    bool ProcessAccounts();

    /// <summary>
    /// Process all IMAP accounts
    /// </summary>
    /// <returns>true if one or more errors happened</returns>
    bool ProcessImapAccounts();

    /// <summary>
    /// Process all O365 accounts
    /// </summary>
    /// <returns>true if one or more errors happened</returns>
    bool ProcessO365Accounts();


    /// <summary>
    /// Send files via email for all not manually sending acounts
    /// </summary>
    bool SendOutboundMails();

    /// <summary>
    /// Send files via email for an account manually
    /// </summary>
    bool ManuallySendOutboundMails(int accountId);
}
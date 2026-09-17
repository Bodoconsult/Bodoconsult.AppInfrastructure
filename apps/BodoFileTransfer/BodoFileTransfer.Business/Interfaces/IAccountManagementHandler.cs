// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.Generic;
using BodoFileTransfer.Business.Model;

namespace BodoFileTransfer.Business.Interfaces;

/// <summary>
/// Interface for account management handler classes
/// </summary>
public interface IAccountManagementHandler
{

    /// <summary>
    /// Load all accounts
    /// </summary>
    void LoadAccounts();


    /// <summary>
    /// Current account
    /// </summary>
    Account CurrentAccount { get;  }


    /// <summary>
    /// Current account handler
    /// </summary>
    IAccountHandler CurrentAccountHandler { get; }

    /// <summary>
    /// All loaded accounts
    /// </summary>
    IList<Account> Accounts { get;  }

    /// <summary>
    /// All loaded account handler
    /// </summary>
    IList<IAccountHandler> AccountHandlers { get;  }


    /// <summary>
    /// Select an account
    /// </summary>
    /// <param name="account">Account to select</param>
    void SelectAccount(Account account);
}
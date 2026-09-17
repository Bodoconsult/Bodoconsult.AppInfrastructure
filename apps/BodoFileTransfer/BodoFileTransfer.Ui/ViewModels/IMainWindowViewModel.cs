// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Collections.Generic;
using BodoFileTransfer.Business.Model;

namespace BodoFileTransferCore.Ui.ViewModels;

/// <summary>
/// Interface for MainWindow viewmodel implementations
/// </summary>
public interface IMainWindowViewModel
{
    /// <summary>
    /// All accounts for the user
    /// </summary>

    IList<Account> Accounts { get;  }


    /// <summary>
    /// Current account to view
    /// </summary>
    Account CurrentAccount { get; set; }



    /// <summary>
    /// Load all accounts
    /// </summary>
    void LoadAccounts();



    /// <summary>
    /// Activate an account and load its data
    /// </summary>
    /// <param name="account">Current account</param>
    void LoadAccountData(Account account);



    /// <summary>
    /// All files in the inbox
    /// </summary>
    IList<DocumentFile> InboxFiles { get;  }


    /// <summary>
    /// Al files in the archive
    /// </summary>
    IList<DocumentFile> ArchiveFiles { get;  }

}
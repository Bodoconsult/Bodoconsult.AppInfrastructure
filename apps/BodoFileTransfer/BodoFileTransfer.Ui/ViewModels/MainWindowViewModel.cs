// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BodoFileTransfer.Business.Interfaces;
using BodoFileTransfer.Business.Model;

namespace BodoFileTransfer.Ui.ViewModels;

/// <summary>
/// Current MainWindow viewmodel implementation
/// </summary>
public class MainWindowViewModel : IMainWindowViewModel, INotifyPropertyChanged
{
    private IList<Account> _accounts = new List<Account>();
    private Account _currentAccount;

    private readonly IDataHandler _dataHandler;
    private IList<DocumentFile> _inboxFiles;
    private IList<DocumentFile> _archiveFiles;

    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="dataHandler">Current data access handler</param>
    public MainWindowViewModel(IDataHandler dataHandler)
    {
        _dataHandler = dataHandler;

        LoadAccounts();

    }


    // This method is called by the Set accessor of each property.
    // The CallerMemberName attribute that is applied to the optional propertyName
    // parameter causes the property name of the caller to be substituted as an argument.
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    /// <summary>
    /// All accounts for the user
    /// </summary>
    public IList<Account> Accounts
    {
        get => _accounts;
        private set
        {
            _accounts = value;
            NotifyPropertyChanged(nameof(Accounts));
        }
    }

    /// <summary>
    /// Current account to view
    /// </summary>
    public Account CurrentAccount
    {
        get => _currentAccount;
        set
        {
            _currentAccount = value;
            NotifyPropertyChanged();
        }
    }

    /// <summary>
    /// Load all accounts
    /// </summary>
    public void LoadAccounts()
    {
        Accounts = _dataHandler.GetAllAccounts();
    }


    /// <summary>
    /// Activate an account and load its data
    /// </summary>
    /// <param name="account">Current account</param>
    public void LoadAccountData(Account account)
    {
        // Set current account
        CurrentAccount = account;

        // Load inbox files
        InboxFiles = _dataHandler.GetFilesForAnAccount(account.Id);

        // Load archive files
        ArchiveFiles = _dataHandler.GetFilesArchiveForAnAccount(account.Id);
    }

    /// <summary>
    /// All files in the inbox
    /// </summary>
    public IList<DocumentFile> InboxFiles
    {
        get => _inboxFiles;
        private set
        {
            _inboxFiles = value;
            NotifyPropertyChanged();
        }
    }

    /// <summary>
    /// Al files in the archive
    /// </summary>
    public IList<DocumentFile> ArchiveFiles
    {
        get => _archiveFiles;
        private set
        {
            _archiveFiles = value;
            NotifyPropertyChanged();
        }
    }
}
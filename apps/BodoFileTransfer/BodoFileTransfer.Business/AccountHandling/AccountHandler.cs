// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Bodoconsult.App.Abstractions.Interfaces;
using BodoFileTransfer.Business.Enums;
using BodoFileTransfer.Business.Interfaces;
using BodoFileTransfer.Business.Model;
using BodoFileTransfer.Business.Services;

namespace BodoFileTransfer.Business.AccountHandling;

/// <summary>
/// Manages document data for an account
/// </summary>
public class AccountHandler : IAccountHandler
{
    private string _currentArchivePath;
    private readonly IAppGlobals _globals;

    private readonly IDataHandler _dataHandler;

    private readonly IFolderHandlerConfig _config;

    /// <summary>
    /// Ctor with account ID
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="dataHandler"></param>
    /// <param name="config"></param>
    /// <param name="globals">Current app globals</param>
    /// <exception cref="ArgumentNullException"></exception>
    public AccountHandler(int accountId, IDataHandler dataHandler, IFolderHandlerConfig config, IAppGlobals globals)
    {
        _dataHandler = dataHandler;
        if (_dataHandler == null)
        {
            throw new ArgumentNullException(nameof(dataHandler));
        }

        // ToDo: replace get all
        var account = _dataHandler.GetAllAccounts().FirstOrDefault(x => x.Id == accountId);
        Account = account;

        _config = config;
        _globals = globals;

        BaseCtor();

    }

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="account">Current account</param>
    /// <param name="dataHandler">Current data handler</param>
    /// <param name="config">Current config</param>
    /// <param name="globals">Current app globals</param>
    public AccountHandler(Account account, IDataHandler dataHandler, IFolderHandlerConfig config, IAppGlobals globals)
    {
        Account = account;
        _dataHandler = dataHandler;
        _config = config;
        _globals = globals;

        BaseCtor();
    }

    /// <summary>
    /// Base jobs for all ctors
    /// </summary>
    private void BaseCtor()
    {

        AccountMailData = GetAccountMailData();

        // Office 365 sender account?
        if (AccountMailData.Office365AccountId > 0)
        {
            AccountMailData.O365SenderAccount = _dataHandler.GetAllO365SenderAccounts().FirstOrDefault(x => x.Id == AccountMailData.Office365AccountId);
        }

        if (!Directory.Exists(Account.InboxPath))
        {
            Directory.CreateDirectory(Account.InboxPath);
        }

        if (!Directory.Exists(Account.ArchivePath))
        {
            Directory.CreateDirectory(Account.ArchivePath);
        }

        _currentArchivePath = Path.Combine(Account.ArchivePath, DateTime.Now.ToString("yyyyMM"));

        if (!Directory.Exists(_currentArchivePath))
        {
            Directory.CreateDirectory(_currentArchivePath);
        }
    }

    /// <summary>
    /// Current account
    /// </summary>
    public Account Account { get; }

    /// <summary>
    /// Account mail data for sending mails to external receiver
    /// </summary>
    public AccountMailData AccountMailData { get; set; }

    /// <summary>
    /// Process the inbox folde rof the account
    /// </summary>
    public void ProcessInbox()
    {
        var counter = Account.DocCounter + 1;

        foreach (var file in new DirectoryInfo(Account.InboxPath).GetFiles())
        {
            if (AddFileFromInbox(file, counter) != Guid.Empty)
            {
                counter++;
                _dataHandler.UpdateDocCounter(Account.Id);
            };
        }
    }

    /// <summary>
    /// Add a file from the inbox of the account
    /// </summary>
    /// <param name="file">Current file</param>
    /// <param name="counter"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public Guid AddFileFromInbox(FileInfo file, int counter)
    {
        string fileName;

        // Check if signature and move it to archive
        if (_config.SignatureFileExtensions.Contains(file.Extension))
        {
            fileName = Path.Combine(_currentArchivePath, file.Name);

            try
            {
                if (File.Exists(fileName))
                {
                    return Guid.Empty;
                }

                file.MoveTo(fileName);
            }
            catch (Exception e)
            {
                throw new Exception($"Error moving file {fileName}: {e.Message}");
            }

            return Guid.Empty;
        }

        // Proceed with normal files
        fileName = GetNewFileName(file, counter);
        Debug.Print(fileName);

        try
        {
            //#if DEBUG
            //                    if (File.Exists(fileName)) File.Delete(fileName);
            //#endif
            file.MoveTo(fileName);

            var doc = new DocumentFile
            {
                Path = fileName,
                Title = $"Beleg{counter.ToString(_config.NumberSchema)}",
                Keyword = "Beleg",
                AccountId = Account.Id
            };

            AddFile(doc);

            return doc.Id;

        }
        catch (Exception e)
        {
            _dataHandler.SendMail($"ProcessInbox acount ID{Account.Id}", $"{e.Message} {file.FullName}");

            return Guid.Empty;
        }

    }

    public string GetNewFileName(FileInfo file, int counter)
    {
        var fileName = $"{Account.Keyword}_{counter.ToString(_config.NumberSchema)}_{file.Name.Replace(file.Extension, "")}{file.Extension}";

        fileName = Path.Combine(_currentArchivePath, fileName);

        return fileName;
    }

    /// <summary>
    /// Get all documents for an account by date
    /// </summary>
    /// <param name="from">Date from</param>
    /// <param name="until">Date until</param>
    /// <returns>List of documents</returns>
    public IList<DocumentFile> GetFilesByDate(DateTime @from, DateTime until)
    {
        return _dataHandler.GetFilesForAnAccountByDate(Account.Id, from, until);
    }

    /// <summary>
    /// Get all documents for an account by date
    /// </summary>
    /// <param name="from">Date from</param>
    /// <param name="until">Date until</param>
    /// <returns>List of documents</returns>
    public IList<DocumentFile> GetFilesArchiveByDate(DateTime @from, DateTime until)
    {
        return _dataHandler.GetFilesArchiveForAnAccountByDate(Account.Id, from, until);
    }

    /// <summary>
    /// Delete a file from table T_Files
    /// </summary>
    /// <param name="fileUid">UID of the file</param>
    public void DeleteFile(Guid fileUid)
    {
        _dataHandler.DeleteFile(fileUid);
    }

    /// <summary>
    /// Add a file
    /// </summary>
    /// <param name="doc">Current document</param>
    public void AddFile(DocumentFile doc)
    {
        _dataHandler.AddFile(doc);

        AddTrace(doc.Id, TraceMessageCode.FileRegistered, $"File registered as {doc.Title} with keyword '{doc.Keyword}");
    }

    /// <summary>
    /// Add a file to the archive
    /// </summary>
    /// <param name="doc">Current document to add to the archive</param>
    public void AddFileArchive(DocumentFile doc)
    {
        _dataHandler.AddFileToArchive(doc);

        _dataHandler.DeleteFile(doc.Id);

        AddTrace(doc.Id, TraceMessageCode.FileArchiveRegistered, $"File registered for archive as {doc.Title} with keyword '{doc.Keyword}");

    }

    /// <summary>
    /// Resending a file from the archive
    /// </summary>
    /// <param name="doc">Current document to add to the archive</param>
    public void ReSendFile(DocumentFile doc)
    {

        _dataHandler.AddFile(doc);

        _dataHandler.DeleteFileArchive(doc.Id);

        AddTrace(doc.Id, TraceMessageCode.FileReSendRegistered, $"File prepared to be sent again");

    }

    /// <summary>
    /// Send files via SMTP for the account
    /// </summary>
    /// <returns>true if mails were sent else false</returns>
    public bool SendSmtpMailsAccount()
    {

        ISmtpMailService mailservice;

        switch (AccountMailData.MailType)
        {
            case SendEmailsType.None:
                return false;
            case SendEmailsType.Smtp:
                mailservice = new SmtpMailService(this);
                break;
            case SendEmailsType.Office365:
                mailservice = new O365MailService(this, _globals);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        mailservice.GetDocumentFiles();

        if (!mailservice.DocumentFiles.Any())
        {
            return true;
        }

        mailservice.CreateSubject();
        mailservice.CreateMails();
        mailservice.SendMails();
        return true;
    }

    /// <summary>
    /// Get the SMTP mail account data
    /// </summary>
    /// <returns>SMTP mail account data</returns>
    public AccountMailData GetAccountMailData()
    {
        return _dataHandler.GetAccountMailData(Account.Id);
    }

    /// <summary>
    /// Get all files
    /// </summary>
    /// <returns>List with documents</returns>
    public IList<DocumentFile> GetFiles()
    {
        return _dataHandler.GetFilesForAnAccount(Account.Id);
    }

    public IList<DocumentFile> GetFilesForDelivery()
    {
        return _dataHandler.GetFilesForAnAccountForDelivery(Account.Id);
    }

    /// <summary>
    /// Get all files from archive
    /// </summary>
    /// <returns>List with documents</returns>
    public IList<DocumentFile> GetFilesArchive()
    {
        return _dataHandler.GetFilesArchiveForAnAccount(Account.Id);
    }

    /// <summary>
    /// Add a trace log entry
    /// </summary>
    /// <param name="entry">Trace log entry</param>
    public void AddTrace(TraceEntry entry)
    {
        _dataHandler.AddTrace(entry);
    }


    /// <summary>
    /// Add a trace log entry
    /// </summary>
    /// <param name="uid">File ID to log</param>
    /// <param name="messageCode">Message code to log</param>
    /// <param name="message">Message to log</param>
    public void AddTrace(Guid uid, TraceMessageCode messageCode, string message)
    {

        var trace = new TraceEntry
        {
            FileId = uid,
            MessageCode = messageCode,
            Message = message
        };

        _dataHandler.AddTrace(trace);

    }


    /// <summary>
    /// Send a email to the error mail receiver
    /// </summary>
    /// <param name="mailAddress">Mail to address</param>
    /// <param name="subject">Subject for the mail to send</param>
    /// <param name="body">Message for the mail to send</param>
    /// <param name="attachments">Attachment paths for the mail to send</param>
    /// <param name="zip">ZIP the attachments</param>
    /// <param name="zipPassword">Password for the ZIP file with the attachments</param>
    public void SendMail(string mailAddress, string subject, string body, string attachments, bool zip = false, string zipPassword = null)
    {
        _dataHandler.SendMail(mailAddress, subject, body, attachments, zip, zipPassword);
    }

    /// <summary>
    /// Get a single document file
    /// </summary>
    /// <param name="id">Document ID</param>
    /// <returns>Document entity</returns>
    public DocumentFile GetFile(Guid id)
    {
        return _dataHandler.GetFile(id);
    }

    /// <summary>
    /// Get a single document file from the archive
    /// </summary>
    /// <param name="id">Document ID</param>
    /// <returns>Document entity</returns>
    public DocumentFile GetFileArchive(Guid id)
    {
        return _dataHandler.GetFileArchive(id);
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BodoFileTransfer.Business.Interfaces;
using BodoFileTransfer.Business.Model;

namespace BodoFileTransfer.Business.DataHandling;

/// <inheritdoc />
/// <summary>
/// Dummy data handler provies test data
/// </summary>
public class DummyDataHandler : IDataHandler
{

    public IList<Account> Accounts { get; }

    public IList<ImapAccount> ImapAccounts { get; }

    public IList<TraceEntry> TraceEntries { get; }

    public IList<DocumentFile> Files { get; }

    public IList<DocumentFile> FilesArchive { get; }


    private readonly string _errorMailAddressee;

    public DummyDataHandler(string errorMailAddressee)
    {
        _errorMailAddressee = errorMailAddressee;

        // ReSharper disable once UseObjectOrCollectionInitializer
        Accounts = new List<Account>();
        ImapAccounts = new List<ImapAccount>();
        TraceEntries = new List<TraceEntry>();
        Files = new List<DocumentFile>();
        FilesArchive = new List<DocumentFile>();
    }


    /// <summary>
    /// Gets all necessary accounts with their data
    /// </summary>
    /// <returns></returns>
    public IList<Account> GetAllAccounts()
    {
        return Accounts;
    }


    /// <summary>
    /// Adds 1 to the account's document counter
    /// </summary>
    /// <param name="accountId"></param>
    public void UpdateDocCounter(int accountId)
    {
        // Do nothing in Dummy
    }


    /// <summary>
    /// Send a email to the error mail receiver
    /// </summary>
    /// <param name="subject">Subject for the mail to send</param>
    /// <param name="message">Message for the mail to send</param>
    public void SendMail(string subject, string message)
    {
        Debug.Print($"Mail to {_errorMailAddressee}\r\n{subject}\r\n{message}");
    }

    /// <summary>
    /// Send a email to the error mail receiver
    /// </summary>
    /// <param name="mailTo">Mail to address</param>
    /// <param name="subject">Subject for the mail to send</param>
    /// <param name="message">Message for the mail to send</param>
    /// <param name="attachments">Attachment paths for the mail to send</param>
    /// <param name="zip">ZIP the attachments</param>
    /// <param name="zipPassword">Password for the ZIP file with the attachments</param>

    public void SendMail(string mailTo, string subject, string message, string attachments, bool zip = false, string zipPassword = null)
    {
        // Do nothing
    }


    /// <summary>
    /// Add a file to the files table for transfer
    /// </summary>
    public void AddFile(DocumentFile file)
    {
        Files.Add(file);
    }

    /// <summary>
    /// Add a file to the files archive table for archiving
    /// </summary>
    public void AddFileToArchive(DocumentFile file)
    {
        FilesArchive.Add(file);
    }


    /// <summary>
    /// Delete a file from the files table
    /// </summary>
    public void DeleteFile(Guid fileId)
    {
        var doc = Files.FirstOrDefault(x => x.Id == fileId);

        if (doc == null)
        {
            return;
        }

        Files.Remove(doc);
    }

    //bool IDataHandler.TransferSmtpMails()
    //{
    //    Debug.Print("Transfer to SMTP done!");
    //    return false;
    //}

    ///// <summary>
    ///// Transfer SmtpMails for one account
    ///// </summary>
    ///// <param name="accountId">ID of the account</param>
    ///// <returns></returns>
    //public bool TransferSmtpMailForAccount(int accountId)
    //{
    //    Debug.Print("Transfer to SMTP done!");
    //    return false;
    //}


    /// <summary>
    /// Get all IMAP accounts to process
    /// </summary>
    /// <returns></returns>
    public IList<ImapAccount> GetAllImapAccounts()
    {
        return ImapAccounts;
    }


    /// <summary>
    /// Check in the database if a file with same name has been stored already
    /// </summary>
    /// <param name="originalFileName">file name to search (without directory path)</param>
    /// <param name="accountId">ID of the BFT account</param>
    /// <returns>true if file already exists</returns>
    public bool CheckIfFileExists(string originalFileName, int accountId)
    {
        return Files.Any(x => x.AccountId == accountId && x.Path == originalFileName);
    }

    /// <summary>
    /// Add a trace entry
    /// </summary>
    /// <param name="entry">Trace log entry</param>
    public void AddTrace(TraceEntry entry)
    {
        TraceEntries.Add(entry);
    }

    /// <summary>
    /// Get document files for an account
    /// </summary>
    /// <param name="accountId">Acccount ID</param>
    /// <returns>List with document files</returns>
    public IList<DocumentFile> GetFilesForAnAccount(int accountId)
    {
        return Files.Where(x => x.AccountId == accountId).ToList();
    }

    /// <summary>
    /// Get all traces for a file
    /// </summary>
    /// <param name="fileId">Current file ID</param>
    /// <returns>List of all traces stored for the file</returns>
    public IList<TraceEntry> GetTracesForFile(Guid fileId)
    {
        return TraceEntries.Where(x => x.FileId == fileId).ToList();
    }

    /// <summary>
    /// Get the number of document files
    /// </summary>
    /// <returns>number of document files</returns>
    public int GetFilesCount()
    {
        return Files.Count;
    }

    /// <summary>
    /// Get the number of document files in the archive
    /// </summary>
    /// <returns>number of document files in the archive</returns>
    public int GetFilesArchiveCount()
    {
        return FilesArchive.Count;
    }

    /// <summary>
    /// Delete a file from archive
    /// </summary>
    /// <param name="fileId">ID of the document entity to delete</param>
    public void DeleteFileArchive(Guid fileId)
    {
        var doc = FilesArchive.FirstOrDefault(x => x.Id == fileId);

        if (doc == null)
        {
            return;
        }

        FilesArchive.Remove(doc);
    }

    public void DeleteFile(string fileName)
    {
        var doc = Files.FirstOrDefault(x => x.Path == fileName);

        if (doc != null)
        {
            Files.Remove(doc);
        }

        doc = FilesArchive.FirstOrDefault(x => x.Path == fileName);

        if (doc == null)
        {
            return;
        }

        FilesArchive.Remove(doc);
    }

    /// <summary>
    /// Get a single document file
    /// </summary>
    /// <param name="fileId">ID of the document entity</param>
    /// <returns>Document entity</returns>
    public DocumentFile GetFile(Guid fileId)
    {
        return Files.FirstOrDefault(x => x.Id == fileId);
    }

    /// <summary>
    /// Get a single document file from archive
    /// </summary>
    /// <param name="fileId">ID of the document entity</param>
    /// <returns>Document entity</returns>
    public DocumentFile GetFileArchive(Guid fileId)
    {
        return FilesArchive.FirstOrDefault(x => x.Id == fileId);
    }

    /// <summary>
    /// Get the required mail data for an account
    /// </summary>
    /// <param name="accountId">Account ID</param>
    /// <returns>Mail data</returns>
    public AccountMailData GetAccountMailData(int accountId)
    {
        return new()
        {
            AccountName = "Bodoconsult",
            Keyword = "M55048",
            MailAddress = "Info@bodoconsult.de",
            Password = "password"
        };
    }

    /// <summary>
    /// Get all accounts requiring a mail transfer
    /// </summary>
    /// <returns>List with account IDs</returns>
    public IList<int> GetMailTransferAccounts()
    {
        return Files.GroupBy(x => x.AccountId).Select(x => x.Key).ToList();
    }

    /// <summary>
    /// Get all files from archive
    /// </summary>
    /// <param name="accountId">AccountId</param>
    /// <returns>List of documents</returns>
    public IList<DocumentFile> GetFilesArchiveForAnAccount(int accountId)
    {
        return FilesArchive.Where(x => x.AccountId == accountId).ToList();
    }

    /// <summary>
    /// Get all documents for an account by date
    /// </summary>
    /// <param name="accountId">Account ID</param>
    /// <param name="from">Date from</param>
    /// <param name="until">Date until</param>
    /// <returns>List of documents</returns>
    public IList<DocumentFile> GetFilesForAnAccountByDate(int accountId, DateTime @from, DateTime until)
    {
        return Files.Where(x => x.AccountId == accountId &&
                                x.DateCreated >= from &&
                                x.DateCreated <= until).ToList();
    }

    public IList<DocumentFile> GetFilesArchiveForAnAccountByDate(int accountId, DateTime @from, DateTime until)
    {
        return FilesArchive.Where(x => x.AccountId == accountId &&
                                       x.DateCreated >= from &&
                                       x.DateCreated <= until).ToList();
    }

    /// <summary>
    /// Get all O365 accounts from database
    /// </summary>
    /// <returns>List with all O365 accounts</returns>
    public IList<O365Account> GetAllO365Accounts()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get all O365 accounts from database used to sent mails
    /// </summary>
    /// <returns>List with all O365 accounts used to sent mails</returns>
    public IList<O365SenderAccount> GetAllO365SenderAccounts()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get all files for delivery
    /// </summary>
    /// <returns>List with documents</returns>
    public IList<DocumentFile> GetFilesForAnAccountForDelivery(int accountId)
    {
        throw new NotImplementedException();
    }
}
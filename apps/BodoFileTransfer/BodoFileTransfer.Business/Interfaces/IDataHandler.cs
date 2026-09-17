// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Collections.Generic;
using BodoFileTransfer.Business.Model;

namespace BodoFileTransfer.Business.Interfaces;

/// <summary>
/// Interface for raw data handling
/// </summary>
public interface IDataHandler
{
    /// <summary>
    /// Gets all necessary accounts with their data
    /// </summary>
    /// <returns></returns>
    IList<Account> GetAllAccounts();

    /// <summary>
    /// Adds 1 to the account's document counter
    /// </summary>
    /// <param name="accountId"></param>
    void UpdateDocCounter(int accountId);


    /// <summary>
    /// Send a email to the error mail receiver
    /// </summary>
    /// <param name="subject">Subject for the mail to send</param>
    /// <param name="message">Message for the mail to send</param>
    void SendMail(string subject, string message);

    /// <summary>
    /// Send a email to the error mail receiver
    /// </summary>
    /// <param name="mailTo">Mail to address</param>
    /// <param name="subject">Subject for the mail to send</param>
    /// <param name="message">Message for the mail to send</param>
    /// <param name="attachments">Attachment paths for the mail to send</param>
    /// <param name="zip">ZIP the attachments</param>
    /// <param name="zipPassword">Password for the ZIP file with the attachments</param>

    void SendMail(string mailTo, string subject, string message, string attachments, bool zip = false, string zipPassword = null);



    /// <summary>
    /// Add a file to the files table for transfer
    /// </summary>
    void AddFile(DocumentFile file);


    /// <summary>
    /// Add a file to the files archive table for archiving
    /// </summary>
    void AddFileToArchive(DocumentFile file);



    /// <summary>
    /// Delete a file from the files table
    /// </summary>
    void DeleteFile(Guid fileId);



    ///// <summary>
    ///// Transfer files to send via SMTP
    ///// </summary>
    ///// <returns>true if error</returns>
    //bool TransferSmtpMails();




    /// <summary>
    /// Get all IMAP accounts to process
    /// </summary>
    /// <returns></returns>
    IList<ImapAccount> GetAllImapAccounts();

    /// <summary>
    /// Check in the database if a file with same name has been stored already
    /// </summary>
    /// <param name="originalFileName">file name to search (without directory path)</param>
    /// <param name="accountId">ID of the BFT account</param>
    /// <returns>true if file already exists</returns>
    bool CheckIfFileExists(string originalFileName, int accountId);


    /// <summary>
    /// Add a trace entry
    /// </summary>
    /// <param name="entry">Trace log entry</param>
    void AddTrace(TraceEntry entry);


    /// <summary>
    /// Get document files for an account
    /// </summary>
    /// <param name="accountId">Acccount ID</param>
    /// <returns>List with document files</returns>
    IList<DocumentFile> GetFilesForAnAccount(int accountId);

    /// <summary>
    /// Get all traces for a file
    /// </summary>
    /// <param name="fileId">Current file ID</param>
    /// <returns>List of all traces stored for the file</returns>
    IList<TraceEntry> GetTracesForFile(Guid fileId);


    /// <summary>
    /// Get the number of document files
    /// </summary>
    /// <returns>number of document files</returns>
    int GetFilesCount();

    /// <summary>
    /// Get the number of document files in the archive
    /// </summary>
    /// <returns>number of document files in the archive</returns>
    int GetFilesArchiveCount();

    /// <summary>
    /// Delete a file from archive
    /// </summary>
    /// <param name="fileId">ID of the document entity to delete</param>
    void DeleteFileArchive(Guid fileId);

    /// <summary>
    /// Delete a file by its name
    /// </summary>
    /// <param name="fileName">Full filename</param>
    void DeleteFile(string fileName);

    /// <summary>
    /// Get a single document file
    /// </summary>
    /// <param name="fileId">ID of the document entity</param>
    /// <returns>Document entity</returns>
    DocumentFile GetFile(Guid fileId);

    /// <summary>
    /// Get a single document file from archive
    /// </summary>
    /// <param name="fileId">ID of the document entity</param>
    /// <returns>Document entity</returns>
    DocumentFile GetFileArchive(Guid fileId);


    /// <summary>
    /// Get the required mail data for an account
    /// </summary>
    /// <param name="accountId">Account ID</param>
    /// <returns>Mail data</returns>
    AccountMailData GetAccountMailData(int accountId);


    /// <summary>
    /// Get all accounts requiring a mail transfer
    /// </summary>
    /// <returns>List with account IDs</returns>
    IList<int> GetMailTransferAccounts();

    /// <summary>
    /// Get all files from archive
    /// </summary>
    /// <param name="accountId">AccountId</param>
    /// <returns>List of documents</returns>
    IList<DocumentFile> GetFilesArchiveForAnAccount(int accountId);

    /// <summary>
    /// Get all documents for an account by date
    /// </summary>
    /// <param name="accountId">Account ID</param>
    /// <param name="from">Date from</param>
    /// <param name="until">Date until</param>
    /// <returns>List of documents</returns>
    IList<DocumentFile> GetFilesForAnAccountByDate(int accountId, DateTime @from, DateTime until);

    /// <summary>
    /// Get all documents for an account by date
    /// </summary>
    /// <param name="accountId">Account ID</param>
    /// <param name="from">Date from</param>
    /// <param name="until">Date until</param>
    /// <returns>List of documents</returns>
    IList<DocumentFile> GetFilesArchiveForAnAccountByDate(int accountId, DateTime @from, DateTime until);

    /// <summary>
    /// Get all O365 accounts from database
    /// </summary>
    /// <returns>List with all O365 accounts</returns>
    IList<O365Account> GetAllO365Accounts();

    /// <summary>
    /// Get all O365 accounts from database used to sent mails
    /// </summary>
    /// <returns>List with all O365 accounts used to sent mails</returns>
    IList<O365SenderAccount> GetAllO365SenderAccounts();

    /// <summary>
    /// Get all files for delivery
    /// </summary>
    /// <returns>List with documents</returns>
    IList<DocumentFile> GetFilesForAnAccountForDelivery(int accountId);
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using BodoFileTransfer.Business.Enums;
using BodoFileTransfer.Business.Model;

namespace BodoFileTransfer.Business.Interfaces;

/// <summary>
/// Interface for handling accounts
/// </summary>
public interface IAccountHandler
{
    /// <summary>
    /// Current account
    /// </summary>
    Account Account { get; }

    /// <summary>
    /// Account mail data for sending mails to external receiver
    /// </summary>
    AccountMailData AccountMailData { get; }





    /// <summary>
    /// Process the inbox folde rof the account
    /// </summary>
    void ProcessInbox();

    /// <summary>
    /// Add a file from the inbox of the account
    /// </summary>
    /// <param name="file">Current file</param>
    /// <param name="counter"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    Guid AddFileFromInbox(FileInfo file, int counter);

    /// <summary>
    /// Add a file
    /// </summary>
    /// <param name="doc">Current document</param>
    void AddFile(DocumentFile doc);

    /// <summary>
    /// Add a file to the archive
    /// </summary>
    /// <param name="doc">Current document to add to the archive</param>
    void AddFileArchive(DocumentFile doc);

    /// <summary>
    /// Resending a file from the archive
    /// </summary>
    /// <param name="doc">Current document to add to the archive</param>
    void ReSendFile(DocumentFile doc);

    /// <summary>
    /// Send files via SMTP for the account
    /// </summary>
    /// <returns>true if mails were sent else false</returns>
    bool SendSmtpMailsAccount();

    /// <summary>
    /// Get the SMTP mail account data
    /// </summary>
    /// <returns>SMTP mail account data</returns>
    AccountMailData GetAccountMailData();

    /// <summary>
    /// Get all files
    /// </summary>
    /// <returns>List with documents</returns>
    IList<DocumentFile> GetFiles();

    /// <summary>
    /// Get all files for delivery
    /// </summary>
    /// <returns>List with documents</returns>
    IList<DocumentFile> GetFilesForDelivery();


    /// <summary>
    /// Get all files from archive
    /// </summary>
    /// <returns>List with documents</returns>
    IList<DocumentFile> GetFilesArchive();

    /// <summary>
    /// Add a trace log entry
    /// </summary>
    /// <param name="entry">Trace log entry</param>
    void AddTrace(TraceEntry entry);

    /// <summary>
    /// Add a trace log entry
    /// </summary>
    /// <param name="uid">File ID to log</param>
    /// <param name="messageCode">Message code to log</param>
    /// <param name="message">Message to log</param>
    void AddTrace(Guid uid, TraceMessageCode messageCode, string message);

    /// <summary>
    /// Send an email to the error mail receiver
    /// </summary>
    /// <param name="mailAddress">Mail to address</param>
    /// <param name="subject">Subject for the mail to send</param>
    /// <param name="body">Message for the mail to send</param>
    /// <param name="attachments">Attachment paths for the mail to send</param>
    /// <param name="zip">ZIP the attachments</param>
    /// <param name="zipPassword">Password for the ZIP file with the attachments</param>

    void SendMail(string mailAddress, string subject, string body, string attachments, bool zip = false, string zipPassword = null);

    /// <summary>
    /// Get a single document file
    /// </summary>
    /// <param name="id">Document ID</param>
    /// <returns>Document entity</returns>
    DocumentFile GetFile(Guid id);


    /// <summary>
    /// Get a single document file from the archive
    /// </summary>
    /// <param name="id">Document ID</param>
    /// <returns>Document entity</returns>
    DocumentFile GetFileArchive(Guid id);


    /// <summary>
    /// Get a new filename
    /// </summary>
    /// <param name="file">Current file info</param>
    /// <param name="counter">Current counter</param>
    /// <returns>Name of the new file</returns>
    string GetNewFileName(FileInfo file, int counter);

    /// <summary>
    /// Get all documents for an account by date
    /// </summary>
    /// <param name="from">Date from</param>
    /// <param name="until">Date until</param>
    /// <returns>List of documents</returns>
    IList<DocumentFile> GetFilesByDate(DateTime from, DateTime until);

    /// <summary>
    /// Get all documents for an account by date
    /// </summary>
    /// <param name="from">Date from</param>
    /// <param name="until">Date until</param>
    /// <returns>List of documents</returns>
    IList<DocumentFile> GetFilesArchiveByDate(DateTime from, DateTime until);

    /// <summary>
    /// Delete a file from table T_Files
    /// </summary>
    /// <param name="fileUid">UID of the file</param>
    void DeleteFile(Guid fileUid);
}
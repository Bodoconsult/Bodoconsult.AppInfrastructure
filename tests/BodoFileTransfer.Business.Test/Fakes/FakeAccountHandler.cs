// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using BodoFileTransferCore.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using BodoFileTransferCore.Business.Enums;
using BodoFileTransferCore.Business.Model;

namespace BodoFileTransferCore.Business.Test.Fakes;

internal class FakeAccountHandler: IAccountHandler
{
    /// <summary>
    /// Current account
    /// </summary>
    public Account Account { get; set; }

    /// <summary>
    /// Account mail data for sending mails to external receiver
    /// </summary>
    public AccountMailData AccountMailData { get; set; }

    /// <summary>
    /// Process the inbox folde rof the account
    /// </summary>
    public void ProcessInbox()
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    /// <summary>
    /// Add a file
    /// </summary>
    /// <param name="doc">Current document</param>
    public void AddFile(DocumentFile doc)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Add a file to the archive
    /// </summary>
    /// <param name="doc">Current document to add to the archive</param>
    public void AddFileArchive(DocumentFile doc)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Resending a file from the archive
    /// </summary>
    /// <param name="doc">Current document to add to the archive</param>
    public void ReSendFile(DocumentFile doc)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Send files via SMTP for the account
    /// </summary>
    /// <returns>true if mails were sent else false</returns>
    public bool SendSmtpMailsAccount()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get the SMTP mail account data
    /// </summary>
    /// <returns>SMTP mail account data</returns>
    public AccountMailData GetAccountMailData()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get all files
    /// </summary>
    /// <returns>List with documents</returns>
    public IList<DocumentFile> GetFiles()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get all files for delivery
    /// </summary>
    /// <returns>List with documents</returns>
    public IList<DocumentFile> GetFilesForDelivery()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get all files from archive
    /// </summary>
    /// <returns>List with documents</returns>
    public IList<DocumentFile> GetFilesArchive()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Add a trace log entry
    /// </summary>
    /// <param name="entry">Trace log entry</param>
    public void AddTrace(TraceEntry entry)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Add a trace log entry
    /// </summary>
    /// <param name="uid">File ID to log</param>
    /// <param name="messageCode">Message code to log</param>
    /// <param name="message">Message to log</param>
    public void AddTrace(Guid uid, TraceMessageCode messageCode, string message)
    {
        throw new NotImplementedException();
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
    public void SendMail(string mailAddress, string subject, string body, string attachments, bool zip = false,
        string zipPassword = null)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get a single document file
    /// </summary>
    /// <param name="id">Document ID</param>
    /// <returns>Document entity</returns>
    public DocumentFile GetFile(Guid id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get a single document file from the archive
    /// </summary>
    /// <param name="id">Document ID</param>
    /// <returns>Document entity</returns>
    public DocumentFile GetFileArchive(Guid id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get a new filename
    /// </summary>
    /// <param name="file">Current file info</param>
    /// <param name="counter">Current counter</param>
    /// <returns>Name of the new file</returns>
    public string GetNewFileName(FileInfo file, int counter)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get all documents for an account by date
    /// </summary>
    /// <param name="from">Date from</param>
    /// <param name="until">Date until</param>
    /// <returns>List of documents</returns>
    public IList<DocumentFile> GetFilesByDate(DateTime from, DateTime until)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get all documents for an account by date
    /// </summary>
    /// <param name="from">Date from</param>
    /// <param name="until">Date until</param>
    /// <returns>List of documents</returns>
    public IList<DocumentFile> GetFilesArchiveByDate(DateTime from, DateTime until)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Delete a file from table T_Files
    /// </summary>
    /// <param name="fileUid">UID of the file</param>
    public void DeleteFile(Guid fileUid)
    {
        throw new NotImplementedException();
    }
}
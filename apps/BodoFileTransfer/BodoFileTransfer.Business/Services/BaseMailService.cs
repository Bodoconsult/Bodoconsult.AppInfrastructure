// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BodoFileTransfer.Business.Enums;
using BodoFileTransfer.Business.Interfaces;
using BodoFileTransfer.Business.Model;

namespace BodoFileTransfer.Business.Services;

/// <summary>
/// Current implementation of an <see cref="ISmtpMailService"/>
/// </summary>
public class BaseMailService : ISmtpMailService
{

    /// <summary>
    /// The maximum number of attachments to be added to the email
    /// </summary>
    public int NumberOfAttachments { get; set; } = 20;

    /// <summary>
    /// Thetotal size of ll attachments in MB
    /// </summary>
    public int SizeOfAttachments { get; set; } = 10;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="accountHandler">Database handler</param>
    public BaseMailService(IAccountHandler accountHandler)
    {
        AccountHandler = accountHandler;
    }

    /// <summary>
    /// Current account handler
    /// </summary>
    public IAccountHandler AccountHandler { get; }


    /// <summary>
    /// Current document files to send
    /// </summary>
    public IList<DocumentFile> DocumentFiles { get;  } = new List<DocumentFile>();

    /// <summary>
    /// Send a email to the error mail receiver
    /// </summary>
    /// <param name="mailAddress">Mail to address</param>
    /// <param name="subject">Subject for the mail to send</param>
    /// <param name="body">Message for the mail to send</param>
    /// <param name="attachments">Attachment paths for the mail to send</param>
    /// <param name="zip">ZIP the attachments</param>
    /// <param name="zipPassword">Password for the ZIP file with the attachments</param>
    public virtual void SendMail(string mailAddress, string subject, string body, string attachments, bool zip = false,
        string zipPassword = null)
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// Mail subject
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Number of emails to send
    /// </summary>
    public int NumberOfMails { get; private set; }


    /// <summary>
    /// Current mails to send
    /// </summary>
    public IList<MailData> Mails { get; } = new List<MailData>();



    /// <summary>
    /// Get all document files to send
    /// </summary>
    public virtual void GetDocumentFiles()
    {
        var data = AccountHandler.GetFiles();

        // Check if the files in the input folder still exist. Remove item it if not
        foreach (var file in data)
        {
            if (File.Exists(file.Path))
            {
                DocumentFiles.Add(file);
                continue;
            }

            AccountHandler.DeleteFile(file.Id);
        }

        // Get the total number of mails necessary now
        NumberOfMails = (int)Math.Ceiling(DocumentFiles.Count / (double)NumberOfAttachments);
    }

    /// <summary>
    /// Create the subject for the mail
    /// </summary>
    public void CreateSubject()
    {
        Subject = $"Datenlieferung {AccountHandler.AccountMailData.Keyword} {AccountHandler.AccountMailData.AccountName}";
    }

    /// <summary>
    /// Send mails to receivers
    /// </summary>
    public virtual void SendMails()
    {
        foreach (var mailData in Mails)
        {
            try
            {
                SendMail(AccountHandler.AccountMailData.MailAddress, mailData.Subject, mailData.Body, mailData.Attachments, true, AccountHandler.AccountMailData.Password);

                foreach (var doc in mailData.DocumentFiles)
                {
                    AccountHandler.AddFileArchive(doc);

                    var te = new TraceEntry
                    {
                        FileId = doc.Id,
                        Message = $"Mail sent to {AccountHandler.AccountMailData.MailAddress}",
                        MessageCode = TraceMessageCode.FileSent
                    };

                    AccountHandler.AddTrace(te);
                }
            }
            catch (Exception e)
            {
                var te = new TraceEntry
                {
                    FileId = Guid.Empty,
                    Message = $"Error during mail sending process: Account {AccountHandler.Account.Id}; MailItem: {Mails.IndexOf(mailData)}: {e.Message}",
                    MessageCode = TraceMessageCode.FileSentError
                };

                AccountHandler.AddTrace(te);
            }

        }

    }


    /// <summary>
    /// Create all mails for an account
    /// </summary>
    public void CreateMails()
    {

        if (NumberOfMails < 1)
        {
            return;
        }

        for (var i = 0; i < NumberOfMails; i++)
        {
            CreatePagedMail(i);
        }

    }

    /// <summary>
    /// Get a mail for a certain data page
    /// </summary>
    /// <param name="pageNumber">Current data page number</param>
    private void CreatePagedMail(int pageNumber)
    {

        var start = pageNumber * NumberOfAttachments;
        var endNumber = (pageNumber + 1) * NumberOfAttachments;

        if (endNumber >= DocumentFiles.Count)
        {
            endNumber = DocumentFiles.Count;
        }

        var body = new StringBuilder();
        var files = new StringBuilder();

        var md = new MailData
        {
            Subject = $"{Subject} Teil {pageNumber+1}/{NumberOfMails}"
        };

        body.AppendLine($"<h1>{md.Subject}</h1>");


        for (var i = start; i < endNumber; i++)
        {

            var doc = DocumentFiles[i];

            var fi = new FileInfo(doc.Path);

            body.AppendLine($"<p>{doc.Keyword}: {doc.Title} ({fi.Name})</p>");

            files.Append($"{doc.Path};");
            md.DocumentFiles.Add(doc);
        }

        md.Body = body.ToString();
        md.Attachments = files.ToString();

        Mails.Add(md);
    }



}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Diagnostics;
using System.IO;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Web.Mail.Mailers;
using Microsoft.Graph.Models;

namespace BodoFileTransfer.Business.Model;

/// <summary>
/// Office 365 received mails handling
/// </summary>
public class O365MailHandler
{
    private O365Mailer _mailer;
    private readonly IAppLoggerProxy _logger;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="account"></param>
    /// <param name="logger"></param>
    public O365MailHandler(O365Account account, IAppLoggerProxy logger)
    {
        Account = account;
        _logger = logger;
    }

    /// <summary>
    /// Current O365 Graph API config
    /// </summary>
    public O365Account Account { get; }

    /// <summary>
    /// Login to O365 Graph API
    /// </summary>
    /// <returns>Awaitable task</returns>
    public void Login()
    {
        _mailer = new O365Mailer(_logger);
        _mailer.LoadMailAccount(Account);
        _mailer.Init();
        _mailer.Logon();
    }

    public void GetAllEmailAttachments()
    {
        try
        {
            //hasAttachments eq true and received
            var filter = $"receivedDateTime ge {Account.SentDateFilter:yyyy-MM-dd}";

            var mails = _mailer.GetMessagesWithAttachments(filter, Account.UserName).GetAwaiter().GetResult();

            //.Select(m => new
            //          {
            //              // Only request specific properties
            //              m.From,
            //              m.ReceivedDateTime,
            //              m.Subject,
            //              m.HasAttachments,
            //              m.Attachments
            //          })
            //// Search
            //.Filter(filter)
            //.Expand("attachments")
            //.Top(100)
            //// Sort by received time, newest first
            //.OrderBy("ReceivedDateTime DESC")
            //.GetAsync().GetAwaiter().GetResult();

            foreach (var mail in mails)
            {
                if (mail.HasAttachments != true)
                {
                    continue;
                }

                // Subject filtering mail subject
                if (!Account.CheckSubject(mail.Subject))
                {
                    Debug.Print(mail.Subject);
                    continue;
                }

                if (mail.Attachments == null)
                {
                    continue;
                }

                foreach (var attachment in mail.Attachments)
                {
                    if (attachment is not FileAttachment attach)
                    {
                        continue;
                    }

                    // Subject filtering attachment name
                    if (!Account.CheckSubject(attach.Name))
                    {
                        continue;
                    }

                    Debug.Print(attach.Name);

                    var ext = new FileInfo(attach.Name!).Extension.ToLower();

                    if (!string.IsNullOrEmpty(Account.FileExtensionFilter) &&
                        !Account.FileExtensionFilter.Contains(ext, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    //Status($"  -> {attach.Name}");

                    var fi = new FileInfo(attach.Name);

                    var fileName = $"{fi.Name.Replace(fi.Extension, "")}_{mail.ReceivedDateTime:yyyyMMddhhmmss}{fi.Extension}";

                    var content = attach.ContentBytes;
                    Account.FileHandling(fileName, content);

                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
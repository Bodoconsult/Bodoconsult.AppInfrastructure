// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.IO;
using System.Linq;
using System.Text;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Web.Mail.Mailers;
using BodoFileTransfer.Business.Interfaces;
using Microsoft.Graph.Models;

namespace BodoFileTransfer.Business.Services;

/// <summary>
/// Current implementation of an <see cref="ISmtpMailService"/>
/// </summary>
public class O365MailService : BaseMailService
{
    private readonly string _htmlTemplate;
    private readonly IAppLoggerProxy _logger;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="accountHandler">Database handler</param>
    /// <param name="globals">Current app globals</param>
    public O365MailService(IAccountHandler accountHandler, IAppGlobals globals) : base(accountHandler)
    {
        var path = globals.AppStartParameter.AppPath;
        ArgumentNullException.ThrowIfNull(path);
        path = Path.Combine(path, "Templates", "HtmlMail.txt");

        _htmlTemplate = Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(path));
        _logger = globals.Logger;
    }

    /// <summary>
    /// Send an email to an error mail receiver
    /// </summary>
    /// <param name="mailAddress">Mail to address</param>
    /// <param name="subject">Subject for the mail to send</param>
    /// <param name="body">Message for the mail to send</param>
    /// <param name="attachments">Attachment paths for the mail to send</param>
    /// <param name="zip">ZIP the attachments</param>
    /// <param name="zipPassword">Password for the ZIP file with the attachments</param>
    public override void SendMail(string mailAddress, string subject, string body, string attachments, bool zip = false,
        string zipPassword = null)
    {

        //return;

        var content = _htmlTemplate.Replace("{0}", body);

        var msg = new Message();

        var receips = mailAddress.Split([';']).Select(receiver => new Recipient { EmailAddress = new EmailAddress { Address = receiver } }).ToList();

        msg.ToRecipients = receips;
        msg.Subject = subject;
        msg.Body = new ItemBody
        {
            Content = content,
            ContentType = BodyType.Html
        };

        msg.HasAttachments = true;
        msg.Attachments = [];

        foreach (var image in attachments.Split([';']))
        {
            if (string.IsNullOrEmpty(image))
            {
                continue;
            }

            var att = new FileAttachment
            {
                ContentBytes = File.ReadAllBytes(image),
                ContentType = "application/pdf",
                Name = new FileInfo(image).Name
            };

            msg.Attachments.Add(att);
        }

        var o365 = new O365Mailer(_logger);
        o365.LoadMailAccount(AccountHandler.AccountMailData.O365SenderAccount);
        o365.Init();
        o365.Logon();

        // ToDo:
        o365.SendMail(msg);
    }
}
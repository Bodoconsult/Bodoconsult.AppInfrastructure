// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Zip;
using Bodoconsult.Web.Mail.Helpers;
using Bodoconsult.Web.Mail.Interfaces;
using Bodoconsult.Web.Mail.Models;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Bodoconsult.Web.Mail.Mailers;

/// <summary>
/// Send an email via SMTP unsecured or via SSL secured
/// </summary>
public sealed class SmtpMailer : BaseMailer
{
    private SmtpClient _smtpClient;
    private readonly string _htmlMailTemplate;

    private SmtpMailAccount _currentMailAccount;

    /// <summary>
    /// Default SMTP mailer
    /// </summary>
    /// <param name="logger">Current logger</param>
    public SmtpMailer(IAppLoggerProxy logger) : base(logger)
    {
        _htmlMailTemplate = MailHelper.GetTemplate("HtmlMail");
    }

    /// <summary>
    /// Initialize SMTP client before sending a mail
    /// </summary>
    public override void Init()
    {
        _smtpClient = new SmtpClient();
        _smtpClient.Connect(_currentMailAccount.SmtpServer, _currentMailAccount.SmtpPort);

        // Note: only needed if the SMTP server requires authentication
        _smtpClient.Authenticate(_currentMailAccount.SmtpAccountName, _currentMailAccount.SmtpPassword);
    }

    /// <summary>
    /// Send an email based on an HTML body string
    /// </summary>
    /// <param name="to">Mail receiver</param>
    /// <param name="subject">Subject of the mail</param>
    /// <param name="body">HTML encoded text to send as mail</param>
    public override void SendMail(string to, string subject, string body)
    {
        try
        {
            var msg = new MimeMessage();

            msg.From.Add(new MailboxAddress(_currentMailAccount.SmtpAccountName, _currentMailAccount.SmtpAccountName));
            msg.Subject = subject;
            msg.Body = new TextPart("html")
            {
                Text = body,
            };
            msg.To.Add(new MailboxAddress(to, to));

            _smtpClient.Send(msg);
        }
        catch (Exception ex)
        {
            throw new Exception("Smtp mailing error", ex);
        }
    }

    //private static string StripHtml(string input)
    //{
    //    return Regex.Replace(input, "<.*?>", string.Empty);
    //}

    /// <summary>
    /// Send mail based on a <see cref="MailMessage"/> object
    /// </summary>
    /// <param name="message"></param>
    public void SendMail(MimeMessage message)
    {
        try
        {
            _smtpClient.Send(message);
        }
        catch (Exception ex)
        {
            throw new Exception("Smtp mailing error", ex);
        }
    }

    /// <summary>
    /// Send a mail item
    /// </summary>
    /// <param name="mailItem">Mail item to send</param>
    /// <returns>True on error else false</returns>
    public override bool SendMail(MailItem mailItem)
    {
        //return  string.IsNullOrEmpty(logoPath) ? SendMailPlain(to, subject, body, signatureTemplate, attachments) : SendMailLogo(to, subject, body, logoPath, signatureTemplate, attachments);

        //try
        //{
        var msg = new MimeMessage();
        msg.From.Add(new MailboxAddress(_currentMailAccount.SmtpAccountName, _currentMailAccount.SmtpAccountName));
        msg.Subject = mailItem.Subject;


        var receivers = mailItem.To.Split(';', StringSplitOptions.RemoveEmptyEntries);

        foreach (var receiver in receivers)
        {
            msg.To.Add(new MailboxAddress(receiver, receiver));
        }

        string body;

        var builder = new BodyBuilder();

        if (!string.IsNullOrEmpty(mailItem.Logo))
        {

            // Plain HTML ink
            if (mailItem.Logo.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                body = MailHelper.FormatBody(mailItem.Body, mailItem.Subject, mailItem.SignatureTemplate, mailItem.Logo, _htmlMailTemplate);

            }
            // Local file as logo
            else
            {
                var inlineLogo = builder.LinkedResources.Add(mailItem.Logo);

                body = MailHelper.FormatBody(mailItem.Body, mailItem.Subject, mailItem.SignatureTemplate, $"cid:{inlineLogo.ContentId}", _htmlMailTemplate);

            }
        }
        else
        {
            body = MailHelper.FormatBody(mailItem.Body, mailItem.Subject, mailItem.SignatureTemplate, string.Empty, _htmlMailTemplate);
        }

        if (!string.IsNullOrEmpty(mailItem.Attachments))
        {
            var files = mailItem.Attachments.Split([';'], StringSplitOptions.RemoveEmptyEntries);

            if (mailItem.Zip)
            {
                var zipFileStream = new MemoryStream();

                // ToDo: Password handling missing
                var zh = new ZipHandler(files);
                zh.GenerateZip(zipFileStream);

                var attachment = new MimePart("image", "gif")
                {
                    Content = new MimeContent(zipFileStream),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = "data.zip"
                };

                builder.Attachments.Add(attachment);
            }
            else
            {
                foreach (var file in files.Where(file => !string.IsNullOrEmpty(file)))
                {
                    builder.Attachments.Add(file);
                }
            }
        }

        builder.HtmlBody = body;

        try
        {
            SendMail(msg);
            return false;
        }
        catch (Exception ex)
        {
            Logger.LogError("Sending message failed", ex);
            return true;
        }
        //}
        //catch (Exception e)
        //{
        //    _logger.Error("SendMailException", e);
        //    return true;
        //}
    }

    /// <summary>
    /// Send mail to all mail addresses registered in <see cref="MassMailItem"/>
    /// </summary>
    /// <param name="massMailItem">Mass mail item</param>
    public override void SendMails(MassMailItem massMailItem)
    {
        // 1. Send emails to receivers with no salutation
        if (massMailItem.To.Any(x => string.IsNullOrEmpty(x.Salutation)))
        {
            SendWithNoSalutation(massMailItem);
        }

        // 2. Send emails to receivers with salutation
        SendWithSalutation(massMailItem);
    }

    private void SendWithSalutation(MassMailItem massMailItem)
    {
        foreach (var mailReciever1 in massMailItem.To.Where(x => !string.IsNullOrEmpty(x.Salutation)))
        {
            var msg = new MimeMessage
            {
                Subject = massMailItem.Subject,
            };

            msg.From.Add(new MailboxAddress(massMailItem.From, massMailItem.From));

            msg.Bcc.Add(new MailboxAddress(mailReciever1.EmailAddress, mailReciever1.EmailAddress) );

            var builder = new BodyBuilder
            {
                HtmlBody = massMailItem.Body.Replace("??address??", mailReciever1.Salutation, StringComparison.OrdinalIgnoreCase)
            };

            // Add inline images
            AddImages(massMailItem.Images, builder);

            // Add attachments
            AddAttachments(massMailItem, builder);

            // Add body
            msg.Body = builder.ToMessageBody();

            // Send the mail
            SendMail(msg);
        }
    }

    private static void AddAttachments(MassMailItem massMailItem, BodyBuilder builder)
    {
        foreach (var file in massMailItem.Attachments.Where(file => !string.IsNullOrEmpty(file)))
        {
            builder.Attachments.Add(file);
        }
    }

    private static void AddImages(IList<ImageMetaData> images, BodyBuilder builder)
    {
        foreach (var image in images)
        {
            var imagelink = builder.LinkedResources.Add(image.Url);
            imagelink.ContentId = image.ContentId;
        }
    }

    private void SendWithNoSalutation(MassMailItem massMailItem)
    {
        // build the message to send
        var msg = new MimeMessage();
        msg.From.Add(new MailboxAddress(massMailItem.From, massMailItem.From));  
        foreach (var mailReciever in massMailItem.To.Where(x => string.IsNullOrEmpty(x.Salutation)))
        {
            msg.Bcc.Add(new MailboxAddress(mailReciever.EmailAddress, mailReciever.EmailAddress));
        }

        msg.Subject = massMailItem.Subject;

        var builder = new BodyBuilder
        {
            HtmlBody = massMailItem.Body.Replace("??address??", massMailItem.DefaultSalutation, StringComparison.OrdinalIgnoreCase)
        };

        // Add inline images
        AddImages(massMailItem.Images, builder);

        // Add attachments
        AddAttachments(massMailItem, builder);

        // Add body
        msg.Body = builder.ToMessageBody();

        // Send the mail
        SendMail(msg);
    }

    /// <summary>
    /// Load mail account data from a JSON string
    /// </summary>
    /// <param name="json">JSON string with fail account data</param>
    public override void LoadMailAccount(string json)
    {
        var ad = JsonHelper.LoadJsonFromString<SmtpMailAccount>(json);
        CurrentMailAccount = ad;
        _currentMailAccount = ad;
    }

    /// <summary>
    /// Load mail account
    /// </summary>
    /// <param name="mailAccount">Mail account instance</param>
    public override void LoadMailAccount(IMailAccount mailAccount)
    {
        CurrentMailAccount = mailAccount;

        if (mailAccount is not SmtpMailAccount o365)
        {
            throw new ArgumentException("mailAccount is not SmtpMailAccount");
        }

        _currentMailAccount = o365;
    }

    /// <summary>
    /// Dispose smtp client
    /// </summary>
    public override void Dispose()
    {
        try
        {
            _smtpClient.Dispose();
        }
        catch
        {
            // ignored
        }
    }
}
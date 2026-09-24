// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Zip;
using Bodoconsult.Web.Mail.Helpers;
using Bodoconsult.Web.Mail.Interfaces;
using Bodoconsult.Web.Mail.Models;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.Security;
using Microsoft.Graph.Users.Item.SendMail;
using Microsoft.Kiota.Abstractions.Authentication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bodoconsult.App.Helpers;
using Attachment = Microsoft.Graph.Models.Attachment;

namespace Bodoconsult.Web.Mail.Mailers;

/// <summary>
/// Implementation of an Office 365 Graph based mailer
/// </summary>
public class O365Mailer: BaseMailer
{
    // Even if this is a console application here, a daemon application is a confidential client application
    private GraphServiceClient _app;
    private IO365MailAccount _mailAccount;
    private readonly string _htmlMailTemplate;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="logger">Current logger</param>
    public O365Mailer(IAppLoggerProxy logger) : base(logger)
    {
        _htmlMailTemplate = MailHelper.GetTemplate("HtmlMail");
    }

    /// <summary>
    /// Login to O365 Graph API
    /// </summary>
    /// <returns>Awaitable task</returns>
    public override void Logon()
    {
        var tenantId = _mailAccount.Tenant;

        var authenticationProvider = new BaseBearerTokenAuthenticationProvider(new TokenProvider(_mailAccount.ClientId, _mailAccount.ClientSecret, tenantId));

        _app = new GraphServiceClient(authenticationProvider);
    }

    /// <summary>
    /// Send an email over an O365 account
    /// </summary>
    /// <param name="to">Mail receiver separated by semmicolon</param>
    /// <param name="subject">Mail subject</param>
    /// <param name="content">Mail content with full HTML markup for a webpage</param>
    public override void SendMail(string to, string subject, string content)
    {
        // Define a simple e-mail message.
        var message = new Message
        {
            Subject = subject,
            Body = new ItemBody
            {
                ContentType = BodyType.Html,
                Content = content
            },
        };

        var receips = to.Split([';'] ).Select(receiver => new Recipient { EmailAddress = new EmailAddress { Address = receiver } }).ToList();

        message.ToRecipients = receips;

        // Send mail as the given user. 
        SendMail(message);
    }

    /// <summary>
    /// Send a mail item
    /// </summary>
    /// <param name="mailItem">Mail item to send</param>
    /// <returns>True on error else false</returns>
    public override bool SendMail(MailItem mailItem)
    {
        //SendMail("robert.leisner@bodoconsult.de", "Test", "Blubb");
        //return false;

        string content;

        var message = new Message
        {
            Subject = mailItem.Subject,
            //From = GetRecipient(mailItem.From)
        };

        var receips = mailItem.To.Split([';']).Select(GetRecipient).ToList();
        message.ToRecipients = receips;

        if (!string.IsNullOrEmpty(mailItem.Logo))
        {
            content = AddLogoToContent(mailItem, message, _htmlMailTemplate);
        }
        else
        {
            content = MailHelper.FormatBody(mailItem.Body, mailItem.Subject, mailItem.SignatureTemplate, string.Empty, _htmlMailTemplate);
        }

        message.Body = new ItemBody
        {
            ContentType = BodyType.Html,
            Content = content
        };

        if (!string.IsNullOrEmpty(mailItem.Attachments))
        {
            AddAttachments(mailItem, message);
        }

        try
        {
            SendMail(message);
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

    private static string AddLogoToContent(MailItem mailItem, Message message, string htmlMailTemplate)
    {
        string content;

        // Plain HTML ink
        if (mailItem.Logo.StartsWith("http", StringComparison.OrdinalIgnoreCase))
        {
            content = MailHelper.FormatBody(mailItem.Body, mailItem.Subject, mailItem.SignatureTemplate, mailItem.Logo, htmlMailTemplate);
        }
        // Local file as logo
        else
        {
            var attachment = new FileAttachment
            {
                ContentType = MimeTypeHelper.GetMimeTypeFromFilePath(mailItem.Logo),
                ContentBytes = System.IO.File.ReadAllBytes(mailItem.Logo),
                ContentId = "i0",
                IsInline = true,
                Name = "Logo"
            };

            message.Attachments = [attachment];

            content = MailHelper.FormatBody(mailItem.Body, mailItem.Subject, mailItem.SignatureTemplate, $"cid:{attachment.ContentId}", htmlMailTemplate);
        }

        return content;
    }

    private static Recipient GetRecipient(string address)
    {
        return new Recipient
        {
            EmailAddress = new EmailAddress
            {
                Address = address
            }
        };
    }

    private static void AddAttachments(MailItem mailItem, Message message)
    {
        var files = mailItem.Attachments.Split([';'], StringSplitOptions.RemoveEmptyEntries);

        message.Attachments ??= new List<Attachment>(files.Length);

        if (mailItem.Zip)
        {
            var zipFileStream = new MemoryStream();

            // Password handling missing
            var zh = new ZipHandler(files);
            zh.GenerateZip(zipFileStream);

            var attachment = new FileAttachment
            {
                ContentType = "application/zip",
                ContentId = "i1",
                Name = "data.zip"
            };

            zipFileStream.ReadExactly(attachment.ContentBytes);

            message.Attachments.Add(attachment);
        }
        else
        {
            var i = 1;
            foreach (var file in files.Where(file => !string.IsNullOrEmpty(file)))
            {
                var attachment = new FileAttachment
                {
                    ContentType = MimeTypeHelper.GetMimeTypeFromFilePath(file),
                    ContentId = $"i{i}",
                    ContentBytes = System.IO.File.ReadAllBytes(file),
                    Name = Path.GetFileName(file)
                };

                message.Attachments.Add(attachment);
                i++;
            }
        }
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
            var msg = new Message
            {
                Subject = massMailItem.Subject,
                From = GetRecipient(massMailItem.From),
                BccRecipients =
                [
                    GetRecipient(mailReciever1.EmailAddress)
                ],
                Body = new ItemBody
                {
                    ContentType = BodyType.Html,
                    Content = massMailItem.Body.Replace("??address??", mailReciever1.Salutation, StringComparison.OrdinalIgnoreCase)
                }
            };


            // Add inline images
            AddImages(massMailItem.Images, msg);

            // Add attachments
            AddAttachments(massMailItem, msg);

            // Send the mail
            SendMail(msg);
        }
    }

    private void AddAttachments(MassMailItem massMailItem, Message message)
    {
        message.Attachments ??= new();

        var i = 0;
        foreach (var file in massMailItem.Attachments.Where(file => !string.IsNullOrEmpty(file)))
        {
            var attachment = new FileAttachment
            {
                ContentType = MimeTypeHelper.GetMimeTypeFromFilePath(file),
                ContentId = $"i{i}",
                ContentBytes = System.IO.File.ReadAllBytes(file),
                Name = Path.GetFileName(file)
            };

            message.Attachments.Add(attachment);
            i++;
        }
    }

    private static void AddImages(IList<ImageMetaData> images, Message message)
    {
        message.Attachments ??= new();

        foreach (var image in images.Where(file => !string.IsNullOrEmpty(file.Url)))
        {
            var attachment = new FileAttachment
            {
                ContentType = MimeTypeHelper.GetMimeTypeFromFilePath(image.Url),
                ContentId = image.ContentId,
                ContentBytes = System.IO.File.ReadAllBytes(image.Url),
                IsInline = true,
                Name = Path.GetFileName(image.Url)
            };

            message.Attachments.Add(attachment);
        }
    }

    private void SendWithNoSalutation(MassMailItem massMailItem)
    {
        // build the message to send
        var msg = new Message
        {
            Subject = massMailItem.Subject,
            From = GetRecipient(massMailItem.From),
            BccRecipients = []
        };
        foreach (var mailReciever in massMailItem.To.Where(x => string.IsNullOrEmpty(x.Salutation)))
        {
            msg.BccRecipients.Add(GetRecipient(mailReciever.EmailAddress));
        }

        msg.Body = new ItemBody
        {
            ContentType = BodyType.Html,
            Content = massMailItem.Body.Replace("??address??", massMailItem.DefaultSalutation, StringComparison.OrdinalIgnoreCase)
        };

        // Add inline images
        AddImages(massMailItem.Images,msg);

        // Add attachments
        AddAttachments(massMailItem, msg);

        // Send the mail
        SendMail(msg);
    }

    /// <summary>
    /// Load mail account data from a JSON string with encrypted values
    /// </summary>
    /// <param name="json">JSON string with mail account data</param>
    public override void LoadMailAccount(string json)
    {
        var ad = JsonHelper.LoadJsonFromString<O365MailAccount>(json);

        var ma = new O365MailAccount
        {
            UserName = PasswordHandler.Decrypt(ad.UserName),
            Tenant = PasswordHandler.Decrypt(ad.Tenant),
            ClientId = PasswordHandler.Decrypt(ad.ClientId),
            ClientSecret = PasswordHandler.Decrypt(ad.ClientSecret),
            Instance = PasswordHandler.Decrypt(ad.Instance),
            Scope = PasswordHandler.Decrypt(ad.Scope)
        };

        CurrentMailAccount = ma;
        _mailAccount = ma;
    }

    /// <summary>
    /// Load mail account
    /// </summary>
    /// <param name="mailAccount">Mail account instance</param>
    public override void LoadMailAccount(IMailAccount mailAccount)
    {
        CurrentMailAccount = mailAccount;

        if (mailAccount is not IO365MailAccount o365)
        {
            throw new ArgumentException("mailAccount is not IO365MailAccount");
        }

        _mailAccount = o365;
    }

    /// <summary>
    /// Send a mail message via O365
    /// </summary>
    /// <param name="message">Message to be sent</param>
    public void SendMail(Message message)
    {
        // Send mail as the given user. 
        _app.Users[_mailAccount.UserName].SendMail.PostAsync(new SendMailPostRequestBody
        {
            Message = message,
        }).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Get messages with attachments
    /// </summary>
    /// <param name="filter">Filter to apply</param>
    /// <param name="accountName">Account name to get the mails from</param>
    /// <returns>List with mail messages</returns>
    public async Task<List<Message>> GetMessagesWithAttachments(string filter, string accountName)
    {
        var result = new List<Message>();

        var messages = await _app.Users[accountName]

            // Only messages from Inbox folder
            .MailFolders["Inbox"]
            .Messages
            .GetAsync((requestConfiguration) =>
            {
                requestConfiguration.QueryParameters.Filter = filter;
                requestConfiguration.QueryParameters.Expand = ["attachments"];
                requestConfiguration.QueryParameters.Top = 100;
                requestConfiguration.QueryParameters.Orderby = ["ReceivedDateTime DESC"];
                requestConfiguration.QueryParameters.Select = ["From", "ReceivedDateTime", "Subject", "HasAttachments", "Attachments"];
            });

        while (messages?.Value != null)
        {
            result.AddRange(messages.Value);

            // If OdataNextLink has a value, there is another page
            if (!string.IsNullOrEmpty(messages.OdataNextLink))
            {
                // Pass the OdataNextLink to the WithUrl method
                // to request the next page
                messages = await _app.Me.Messages
                    .WithUrl(messages.OdataNextLink)
                    .GetAsync();
            }
            else
            {
                // No more results, exit loop
                break;
            }
        }

        return result;
    }
}
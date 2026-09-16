// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using Bodoconsult.Web.Mail.Converters;
using Bodoconsult.Web.Mail.Interfaces;
using Bodoconsult.Web.Mail.Models;

namespace Bodoconsult.Web.Mail.Mailers;

/// <summary>
/// Mass mail handler to create an HTML based mass email
/// </summary>
public sealed class MassMailHandler : IMassMailHandler
{
    private readonly IMailer _mailer;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="mailer">Current mailer instance</param>
    public MassMailHandler(IMailer mailer)
    {
        _mailer = mailer;
    }

    /// <summary>
    /// List of all MailReceivers
    /// </summary>
    public List<MailReceiver> MailReceivers { get; } = new();

    /// <summary>
    /// List with file paths to be attached to the mail
    /// </summary>
    public List<string> Attachments { get; } = new();

    /// <summary>
    /// Subject for the mass mail
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Default salutation
    /// </summary>
    public string DefaultSalutation { get; set; } = "Sehr geehrte Damen und Herren";

    /// <summary>
    /// Contains the converted mail text as master
    /// </summary>
    public HtmlToMailConverter MasterMailText { get; set; }

    /// <summary>
    /// Load an HTML file as mail text
    /// </summary>
    /// <param name="docUrl">Path to the HTML template document</param>
    public void LoadHtmlMailText(string docUrl)
    {
        if (string.IsNullOrEmpty(docUrl))
        {
            ArgumentNullException.ThrowIfNull(docUrl);
        }

        MasterMailText = new HtmlToMailConverter { DocUrl = docUrl };
        MasterMailText.LoadDocument();
        MasterMailText.FindImages();
        MasterMailText.ProcessContent();
    }

    /// <summary>
    /// Send the emails out
    /// </summary>
    public void SendMails()
    {
        if (MailReceivers == null || MailReceivers.Count == 0)
        {
            return;
        }

        var mmi = new MassMailItem
        {
            From = _mailer.CurrentMailAccount.MailAddressSender,
            Subject = Subject,
            Body = MasterMailText.Content,
            DefaultSalutation = DefaultSalutation,
        };

        mmi.Attachments.AddRange(Attachments);

        foreach (var receiver in MailReceivers)
        {
            mmi.To.Add(receiver);
        }

        _mailer.SendMails(mmi);
    }
}
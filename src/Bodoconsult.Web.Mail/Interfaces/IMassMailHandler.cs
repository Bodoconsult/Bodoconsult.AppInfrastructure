// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System.Collections.Generic;
using Bodoconsult.Web.Mail.Converters;
using Bodoconsult.Web.Mail.Models;

namespace Bodoconsult.Web.Mail.Interfaces;

/// <summary>
/// Interface for mass mailing handlers sending an email to multiple receivers
/// </summary>
public interface IMassMailHandler
{
    /// <summary>
    /// List of all MailReceivers
    /// </summary>
    List<MailReceiver> MailReceivers { get; }

    /// <summary>
    /// List with file paths to be attached to the mail
    /// </summary>
    List<string> Attachments { get; }

    /// <summary>
    /// Subject for the mass mail
    /// </summary>
    string Subject { get; set; }

    /// <summary>
    /// Default salutation
    /// </summary>
    string DefaultSalutation { get; set; }

    /// <summary>
    /// Contains the converted mail text as master
    /// </summary>
    HtmlToMailConverter MasterMailText { get; set; }

    /// <summary>
    /// Load an HTML file as mail text
    /// </summary>
    /// <param name="docUrl">Path to the HTML template document</param>
    void LoadHtmlMailText(string docUrl);

    /// <summary>
    /// Send the emails out
    /// </summary>
    void SendMails();
}
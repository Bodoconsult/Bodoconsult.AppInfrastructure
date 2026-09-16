// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Web.Mail.Models;

namespace Bodoconsult.Web.Mail.Interfaces;

/// <summary>
/// Interface for mailer implementations sending the mails via SMTP, ...
/// </summary>
public interface IMailer : IDisposable
{
    /// <summary>
    /// Current logger instance
    /// </summary>
    IAppLoggerProxy Logger { get; }

    /// <summary>
    /// Current mail account
    /// </summary>
    IMailAccount CurrentMailAccount { get; }

    /// <summary>
    /// Init the mail account
    /// </summary>
    void Init();

    /// <summary>
    /// Logon to mailserver
    /// </summary>
    void Logon();

    /// <summary>
    /// Send an email based on an HTML body string
    /// </summary>
    /// <param name="to">Mail receiver</param>
    /// <param name="subject">Subject of the mail</param>
    /// <param name="body">HTML encoded text to send as mail</param>
    public void SendMail(string to, string subject, string body);

    /// <summary>
    /// Send a mail item
    /// </summary>
    /// <param name="mailItem">Mail item to send</param>
    /// <returns>True on success else false</returns>
    bool SendMail(MailItem mailItem);

    /// <summary>
    /// Send mail to all mail addresses registered in <see cref="MassMailItem"/>
    /// </summary>
    /// <param name="massMailItem">Mass mail item</param>
    void SendMails(MassMailItem massMailItem);

    /// <summary>
    /// Load mail account data from a JSON string
    /// </summary>
    /// <param name="json">JSON string with fail account data</param>
    void LoadMailAccount(string json);

    /// <summary>
    /// Load mail account
    /// </summary>
    /// <param name="mailAccount">Mail account instance</param>
    void LoadMailAccount(IMailAccount  mailAccount);
}
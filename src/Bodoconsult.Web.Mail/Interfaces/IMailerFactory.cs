// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Web.Mail.Models;
using System;

namespace Bodoconsult.Web.Mail.Interfaces;

/// <summary>
/// Interface for mailer factory implementations sending the mails via SMTP, ...
/// </summary>
public interface IMailerFactory : IDisposable
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
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using BodoFileTransfer.Business.Interfaces;

namespace BodoFileTransfer.Business.Services;

/// <summary>
/// Current implementation of an <see cref="ISmtpMailService"/> using BodoWebMailer
/// </summary>
public class SmtpMailService : BaseMailService
{


    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="accountHandler">Database handler</param>
    public SmtpMailService(IAccountHandler accountHandler) : base(accountHandler)
    { }

    /// <summary>
    /// Send a email to the error mail receiver
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
        AccountHandler.SendMail(mailAddress, subject, body, attachments, zip, zipPassword);
    }
}
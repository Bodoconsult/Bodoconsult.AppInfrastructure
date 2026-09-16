// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Web.Mail.Interfaces;
using Bodoconsult.Web.Mail.Models;

namespace Bodoconsult.Web.Mail.Mailers;

/// <summary>
/// Fake sending a mail
/// </summary>
public sealed class FakeMailer : BaseMailer
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="logger">Current logger</param>
    public FakeMailer(IAppLoggerProxy logger) : base(logger)
    { }

    /// <summary>
    /// Init the mail account
    /// </summary>
    public override void Init()
    {
        // Do nothing
    }

    /// <summary>
    /// Logon to mailserver
    /// </summary>
    public override void Logon()
    {
        // Do nothing
    }

    /// <summary>
    /// Send an email based on an HTML body string
    /// </summary>
    /// <param name="to">Mail receiver</param>
    /// <param name="subject">Subject of the mail</param>
    /// <param name="body">HTML encoded text to send as mail</param>
    public override void SendMail(string to, string subject, string body)
    {
        // Do nothing
    }

    /// <summary>
    /// Send a mail item
    /// </summary>
    /// <param name="mailItem">Mail item to send</param>
    /// <returns>True on error else false</returns>
    public override bool SendMail(MailItem mailItem)
    {
        // Do nothing
        return false;
    }

    /// <summary>
    /// Send mail to all mail addresses registered in <see cref="MassMailItem"/>
    /// </summary>
    /// <param name="massMailItem">Mass mail item</param>
    public override void SendMails(MassMailItem massMailItem)
    {
        // Do nothing
    }

    /// <summary>
    /// Load mail account data from a JSON string with encrypted values
    /// </summary>
    /// <param name="json">JSON string with mail account data</param>
    public override void LoadMailAccount(string json)
    {
        // Do nothing
    }

    /// <summary>
    /// Load mail account
    /// </summary>
    /// <param name="mailAccount">Mail account instance</param>
    public override void LoadMailAccount(IMailAccount mailAccount)
    {
        // Do nothing
    }
}
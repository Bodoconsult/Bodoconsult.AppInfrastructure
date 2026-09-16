// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Web.Mail.Interfaces;
using Bodoconsult.Web.Mail.Models;

namespace Bodoconsult.Web.Mail.Mailers;

/// <summary>
/// Base class for mailer instances
/// </summary>
public abstract class BaseMailer: IMailer
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="logger">Current logger</param>
    protected BaseMailer(IAppLoggerProxy logger)
    {
        Logger = logger;
    }

    /// <summary>
    /// Current logger
    /// </summary>
    public IAppLoggerProxy Logger { get; }

    /// <summary>
    /// Current mail account to use
    /// </summary>
    public IMailAccount CurrentMailAccount { get; set; }

    /// <summary>
    /// Dispose all needed objects
    /// </summary>
    public virtual void Dispose()
    {
        // Do nothing
    }

    /// <summary>
    /// Init the mail account
    /// </summary>
    public virtual void Init()
    {
        // Do nothing
    }

    /// <summary>
    /// Logon to mailserver
    /// </summary>
    public virtual void Logon()
    {
        // Do nothing
    }

    /// <summary>
    /// Send an email based on an HTML body string
    /// </summary>
    /// <param name="to">Mail receiver</param>
    /// <param name="subject">Subject of the mail</param>
    /// <param name="body">HTML encoded text to send as mail</param>
    public virtual void SendMail(string to, string subject, string body)
    {
        throw new NotSupportedException("Override in derived classes");
    }

    /// <summary>
    /// Send a mail item
    /// </summary>
    /// <param name="mailItem">Mail item to send</param>
    /// <returns>True on errorelse false</returns>
    public virtual bool SendMail(MailItem mailItem)
    {
        throw new NotSupportedException("Override in derived classes");
    }

    /// <summary>
    /// Send mail to all mail addresses registered in <see cref="MassMailItem"/>
    /// </summary>
    /// <param name="massMailItem">Mass mail item</param>
    public virtual void SendMails(MassMailItem massMailItem)
    {
        throw new NotSupportedException("Override in derived classes");
    }

    /// <summary>
    /// Load mail account data from a JSON string with encrypted values
    /// </summary>
    /// <param name="json">JSON string with mail account data</param>
    public virtual void LoadMailAccount(string json)
    {
        throw new NotSupportedException("Override in derived classes");
    }

    /// <summary>
    /// Load mail account
    /// </summary>
    /// <param name="mailAccount">Mail account instance</param>
    public virtual void LoadMailAccount(IMailAccount mailAccount)
    {
        throw new NotSupportedException("Override in derived classes");
    }
}
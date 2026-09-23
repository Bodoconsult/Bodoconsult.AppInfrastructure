// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.Web.Mail.Interfaces;

/// <summary>
/// Interface for O365 based email accounts
/// </summary>
public interface IO365MailAccount : IMailAccount
{
    /// <summary>
    /// Current O365 instance
    /// </summary>
    string Instance { get; set; }

    /// <summary>
    /// Current tenant
    /// </summary>
    string Tenant { get; set; }

    /// <summary>
    /// Current client ID
    /// </summary>
    string ClientId { get; set; }

    /// <summary>
    /// Current client secret
    /// </summary>
    string ClientSecret { get; set; }

    /// <summary>
    /// User account name: Email address or for Office 365 shared mailboxes email addresse slash mailbox name (i.e. test@test/rechnung)
    /// </summary>
    string UserName { get; set; }

    /// <summary>
    /// The current O365 scope to use
    /// </summary>
    string Scope { get; set; }
}
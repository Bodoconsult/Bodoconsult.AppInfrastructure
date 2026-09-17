// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using BodoFileTransfer.Business.Interfaces;

namespace BodoFileTransfer.Business.Model;

/// <summary>
/// Represents an Office 365 mail account
/// </summary>
/// <remarks>The Office 365 app account requires Azure app permission Mail.Read</remarks>
public class O365Account : BaseMailAccount
{

    public O365Account(IDataHandler dataHandler)
    {
        DataHandler = dataHandler;

    }

    /// <summary>
    /// Current O365 instance
    /// </summary>
    public string Instance { get; set; }

    /// <summary>
    /// Current tenant
    /// </summary>
    public string Tenant { get; set; }

    /// <summary>
    /// Current client ID
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// Current client secret
    /// </summary>
    public string ClientSecret { get; set; }

    /// <summary>
    /// User account name: Email address or for Office365 shared mailboxes email addresse slash mailbox name (i.e. test@test/rechnung)
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// The current O365 scope to use
    /// </summary>
    public string Scope { get; set; }

}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Web.Mail.Models;

namespace BodoFileTransfer.Business.Model;

/// <summary>
/// O365 accunt used to send account data to a external receiver
/// </summary>
public class O365SenderAccount: O365MailAccount
{
    /// <summary>
    /// The ID of the account in the database
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The maximum number of attachments to be added to the email
    /// </summary>
    public int NumberOfAttachments { get; set; } = 50;

    /// <summary>
    /// Thetotal size of ll attachments in MB
    /// </summary>
    public int SizeOfAttachments { get; set; } = 20;

    /// <summary>
    /// The cleartext name of the account
    /// </summary>
    public string Name { get; set; }
}
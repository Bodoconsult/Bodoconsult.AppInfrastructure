// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using BodoFileTransfer.Business.Enums;

namespace BodoFileTransfer.Business.Model;

public class AccountMailData
{
    /// <summary>
    /// Account name
    /// </summary>
    public string AccountName { get; set; }

    /// <summary>
    /// Password for zipped mail attachements
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Keyword for mail 
    /// </summary>
    public string Keyword { get; set; }

    /// <summary>
    /// Mail address to send mail to
    /// </summary>
    public string MailAddress { get; set; }

    /// <summary>
    /// Type of email transport to use for sending emails to external receivers
    /// </summary>
    public SendEmailsType MailType { get; set; } = SendEmailsType.Smtp;

    /// <summary>
    /// The ID of the Office 365 account to use or 0 if not applicable
    /// </summary>
    public int Office365AccountId { get; set; }


    /// <summary>
    /// The account stored <see cref="Office365AccountId"/>
    /// </summary>
    public O365SenderAccount O365SenderAccount { get; set; }
}
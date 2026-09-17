// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System;
using BodoFileTransfer.Business.Interfaces;

namespace BodoFileTransfer.Business.Model;

/// <summary>
/// Represents an IMAP email account connected to an <see cref="T:Account" />
/// </summary>
public class ImapAccount: BaseMailAccount
{

    public ImapAccount(IDataHandler dataHandler)
    {
        DataHandler = dataHandler;
        Port = 993;
        ServerDateFormat = "{0:dd-MMM-yyyy}";
        SentDateFilter = DateTime.Now.AddMonths(-3);
        UseSsl = true;
    }

    /// <summary>
    /// Url of the IMAP server
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// Port to use for IMAP server. Default 993;
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// User account name: Email address or for Office365 shared mailboxes email addresse slash mailbox name (i.e. test@test/rechnung)
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Password for the user account
    /// </summary>
    public string Password { get; set; }


    /// <summary>
    /// Use secure SSL for IMAP connection. Default true.
    /// </summary>
    public bool UseSsl { get; set; }


    /// <summary>
    /// The server's date format: normally {0:dd-MMM-yyyy} in Germany. Google may use 'X-GM-RAW ""AFTER:{0:yyyy-MM-dd}""'
    /// </summary>
    public string ServerDateFormat { get; set; }


        

}
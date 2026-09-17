using System.Collections.Generic;
using BodoFileTransfer.Business.Interfaces;
using BodoFileTransfer.Business.Model;

namespace BodoFileTransfer.Business.Services;

/// <summary>
/// Interface for SMTP mail of document files to email receiver
/// </summary>
public interface ISmtpMailService
{

    /// <summary>
    /// The maximum number of attachments to be added to the email
    /// </summary>
    int NumberOfAttachments { get; set; } 

    /// <summary>
    /// Thetotal size of ll attachments in MB
    /// </summary>
    int SizeOfAttachments { get; set; }


    /// <summary>
    /// Current account handler
    /// </summary>
    IAccountHandler AccountHandler { get; }


    /// <summary>
    /// Current document files to send
    /// </summary>
    IList<DocumentFile> DocumentFiles { get;  }



    /// <summary>
    /// Get all document files to send
    /// </summary>
    void GetDocumentFiles();



    /// <summary>
    /// Create all mails for an account
    /// </summary>
    void CreateMails();


    /// <summary>
    /// Create the subject for the mail
    /// </summary>
    void CreateSubject();


    /// <summary>
    /// Send mails to receivers
    /// </summary>
    void SendMails();

    /// <summary>
    /// Send a email to the error mail receiver
    /// </summary>
    /// <param name="mailAddress">Mail to address</param>
    /// <param name="subject">Subject for the mail to send</param>
    /// <param name="body">Message for the mail to send</param>
    /// <param name="attachments">Attachment paths for the mail to send</param>
    /// <param name="zip">ZIP the attachments</param>
    /// <param name="zipPassword">Password for the ZIP file with the attachments</param>

    void SendMail(string mailAddress, string subject, string body, string attachments, bool zip = false, string zipPassword = null);


    /// <summary>
    /// Mail subject
    /// </summary>
    string Subject { get;  }

    /// <summary>
    /// Current mails to send
    /// </summary>
    IList<MailData> Mails { get; }


}
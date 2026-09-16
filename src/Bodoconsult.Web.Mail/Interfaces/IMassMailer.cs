//// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

//using System.Collections.Generic;
//using Bodoconsult.Web.Mail.Models;

//namespace Bodoconsult.Web.Mail.Interfaces;

///// <summary>
///// Enhancement of <see cref="IMailer"/> for mass mailer implementations
///// </summary>
//public interface IMassMailer : IMailer
//{
//    /// <summary>
//    /// HTML-Body für eMails
//    /// </summary>
//    string Body { get; set; }

//    /// <summary>
//    /// Subject for the mail
//    /// </summary>
//    string Subject { get; set; }

//    /// <summary>
//    /// Sending mail address
//    /// </summary>
//    string From { get; set; }

//    /// <summary>
//    /// Default salutation tu use: default is "Sehr geehrte Damen und Herren"
//    /// </summary>
//    string DefaultSalutation { get; set; }

//    /// <summary>
//    /// All mail addresses the mail will be sent to
//    /// </summary>
//    IList<MailReceiver> To { get; set; }

//    /// <summary>
//    /// Contains all found images in the document
//    /// </summary>
//    IList<ImageMetaData> Images { get; set; }

//    /// <summary>
//    /// Send mail to all mail addresses registered in <see cref="To"/>
//    /// </summary>
//    void SendMails();
//}
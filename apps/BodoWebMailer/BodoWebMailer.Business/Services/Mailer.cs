//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

//using Bodoconsult.App.Abstractions.Interfaces;
//using Bodoconsult.App.Zip;
//using Bodoconsult.Web.Mail.Model;
//using BodoWebMailer.Business.Helpers;
//using BodoWebMailer.Business.Interfaces;
//using BodoWebMailer.Business.Models;
//using System;
//using System.IO;
//using System.Linq;
//using System.Net.Mail;
//using System.Net.Mime;
//using System.Reflection;
//using System.Text;
//using Bodoconsult.Web.Mail.Interfaces;

//namespace BodoWebMailer.Business.Services;

///// <summary>
///// SMTP implementation of <see cref="IMailer"/>
///// </summary>
//public sealed class SmtpMailer : IMailer
//{
//    private Bodoconsult.Web.Mail.Mailers.SmtpMailer _smtpClient;
//    private string _htmlMailTemplate;

//    private readonly IAppLoggerProxy _appLogger;

//    public SmtpMailer(IAppLoggerProxy _appLogger, SmtpMailAccount mailAccount)
//    {
//        _appLogger = _appLogger;
//        CurrentMailAccount = mailAccount;
//    }

//    /// <summary>
//    /// Current mail account
//    /// </summary>
//    public SmtpMailAccount CurrentMailAccount { get; }

//    /// <summary>
//    /// Init the mail account
//    /// </summary>
//    public void Init()
//    {

//        //Debug.Print(Bodoconsult.Console.PasswordHandler.Decrypt(SmtpPassword));

//        var acc = new SmtpMailAccount
//        {
//            SmtpAccountName = CurrentMailAccount.SmtpAccountName,
//            SmtpPassword = PasswordHandler.Decrypt(CurrentMailAccount.SmtpPassword),
//            MailAddressSender = CurrentMailAccount.MailAddressSender,
//            SmtpServer = CurrentMailAccount.SmtpServer,
//            UseSecureConnection = CurrentMailAccount.UseSecureConnection
//        };

//        _htmlMailTemplate = GetTemplate("HtmlMail");
//        _smtpClient = new Bodoconsult.Web.Mail.SmtpMailer(acc);
//        _smtpClient.Init();
//    }

//    /// <summary>
//    /// Logon to mailserver
//    /// </summary>
//    public void Logon()
//    {

//    }

//    //public bool SendMailPlain(string to, string subject, string body, string signatureTemplate, string attachments)
//    //{

//    //    //_smtpClient.SmtpAccount = SmtpAccount;
//    //    //_smtpClient.SmtpPassword = SmtpPassword;
//    //    //_smtpClient.SmtpServer = SmtpServer;
//    //    //_smtpClient.UseSecureConnection = UseSecureConnection;


//    //    if (string.IsNullOrEmpty(From)) From = SmtpAccount;

//    //    var msg = new MailMessage { From = new MailAddress(From) };
//    //    msg.To.Add(to.Replace(";",","));
//    //    msg.Subject = subject;
//    //    msg.IsBodyHtml = true;
//    //    msg.Body = FormatBody(body, subject, signatureTemplate, "");
//    //    msg.BodyEncoding = Encoding.UTF8;





//    //    if (!string.IsNullOrEmpty(attachments))
//    //    {
//    //        var files = attachments.Split(';');

//    //        foreach (var file in files.Where(file => !string.IsNullOrEmpty(file)))
//    //        {
//    //            msg.Attachments.Add(new Attachment(file));
//    //        }
//    //    }


//    //    try
//    //    {

//    //        _smtpClient.SendMail(msg);

//    //        return false;
//    //    }
//    //    catch (Exception ex)
//    //    {
//    //        _logger.Error(msg, ex);
//    //        return true;
//    //    }

//    //}

//    /// <summary>
//    /// Send a mail item
//    /// </summary>
//    /// <param name="mailItem">Mail item to send</param>
//    /// <returns>True on success else false</returns>
//    public bool SendMail(MailItem mailItem)
//    {
//        //return  string.IsNullOrEmpty(logoPath) ? SendMailPlain(to, subject, body, signatureTemplate, attachments) : SendMailLogo(to, subject, body, logoPath, signatureTemplate, attachments);

//        //try
//        //{
//        if (string.IsNullOrEmpty(CurrentMailAccount.MailAddressSender))
//        {
//            CurrentMailAccount.MailAddressSender = CurrentMailAccount.SmtpAccountName;
//        }

//        var msg = new MailMessage
//        {
//            From = new MailAddress(CurrentMailAccount.MailAddressSender),
//            Subject = mailItem.Subject,
//            IsBodyHtml = true
//        };

//        msg.To.Add(mailItem.To.Replace(";", ","));
//        msg.Subject = mailItem.Subject;
//        msg.IsBodyHtml = true;


//        if (!string.IsNullOrEmpty(mailItem.Logo))
//        {

//            // Plain HTML ink
//            if (mailItem.Logo.StartsWith("http", StringComparison.OrdinalIgnoreCase))
//            {
//                msg.Body = FormatBody(mailItem.Body, mailItem.Subject, mailItem.SignatureTemplate, mailItem.Logo, _htmlMailTemplate);

//            }
//            // Local file as logo
//            else
//            {
//                var inlineLogo = new LinkedResource(mailItem.Logo) { ContentId = Guid.NewGuid().ToString() };

//                var body = FormatBody(mailItem.Body, mailItem.Subject, mailItem.SignatureTemplate, $"cid:{inlineLogo.ContentId}", _htmlMailTemplate);
//                msg.BodyEncoding = Encoding.UTF8;

//                var view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
//                view.LinkedResources.Add(inlineLogo);
//                msg.AlternateViews.Add(view);
//            }
//        }
//        else
//        {
//            msg.Body = FormatBody(mailItem.Body, mailItem.Subject, mailItem.SignatureTemplate, string.Empty, _htmlMailTemplate);
//        }


//        if (!string.IsNullOrEmpty(mailItem.Attachments))
//        {
//            var files = mailItem.Attachments.Split([';'], StringSplitOptions.RemoveEmptyEntries);

//            if (mailItem.Zip)
//            {
//                var zipFileStream = new MemoryStream();

//                // Password handling missing
//                var zh = new ZipHandler(files);
//                zh.GenerateZip(zipFileStream);

//                var att = new Attachment(zipFileStream, new ContentType("application/zip"))
//                {
//                    Name = "data.zip"
//                };

//                msg.Attachments.Add(att);
//            }
//            else
//            {
//                foreach (var file in files.Where(file => !string.IsNullOrEmpty(file)))
//                {
//                    msg.Attachments.Add(new Attachment(file));
//                }
//            }
//        }


//        try
//        {
//            _smtpClient.SendMail(msg);
//            return false;
//        }
//        catch (Exception ex)
//        {
//            _appLogger.LogError("Sending message failed", ex);
//            return true;
//        }
//        //}
//        //catch (Exception e)
//        //{
//        //    _logger.Error("SendMailException", e);
//        //    return true;
//        //}
//    }

//    /// <summary>
//    /// Format the mail body
//    /// </summary>
//    /// <param name="body">Body text</param>
//    /// <param name="subject">Subject</param>
//    /// <param name="signatureTemplate">Signature template</param>
//    /// <param name="logoUrl">URL of the logo to use or null</param>
//    /// <param name="htmlMailTemplate">HTML mail template</param>
//    /// <returns></returns>
//    public static string FormatBody(string body, string subject, string signatureTemplate, string logoUrl, string htmlMailTemplate)
//    {
//        ////Do not use string.Format due to { and } in body
//        //body = body.Replace("{0}", contentId);

//        if (body.Contains("<html", StringComparison.OrdinalIgnoreCase) && body.Contains("</html>", StringComparison.OrdinalIgnoreCase))
//        {
//            return body;
//        }

//        var signature = string.IsNullOrEmpty(signatureTemplate) ? "" : GetTemplate(signatureTemplate).Replace("{0}", logoUrl);

//        //Do not use string.Format due to { and } in body
//        var html = htmlMailTemplate.Replace("{0}", body + signature).Replace("{1}", subject);

//        return html;
//    }

//    /// <summary>
//    /// Get template file as string
//    /// </summary>
//    /// <param name="templateName">Full path to template file</param>
//    /// <returns>template content</returns>
//    private static string GetTemplate(string templateName)
//    {
//        try
//        {
//            // ReSharper disable once AssignNullToNotNullAttribute

//            var dir = Assembly.GetExecutingAssembly().Location;

//            if (string.IsNullOrEmpty(dir))
//            {
//                return "";
//            }

//            var directoryName = new FileInfo(dir).DirectoryName;
//            if (directoryName == null)
//            {
//                return "";
//            }

//            if (!templateName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
//            {
//                templateName = $"{templateName}.txt";
//            }

//            templateName = Path.Combine(directoryName, $"Templates\\{templateName}");

//            var fsIn = new FileStream(templateName, FileMode.Open, FileAccess.Read, FileShare.Read);
//            var sr = new StreamReader(fsIn);
//            var s = sr.ReadToEnd();
//            sr.Dispose();
//            fsIn.Close();
//            return s;
//        }
//        catch
//        {
//            return "";
//        }
//    }

//    /// <summary>
//    /// Dispose mail client
//    /// </summary>
//    public void Dispose()
//    {
//        _smtpClient.Dispose();
//    }
//}
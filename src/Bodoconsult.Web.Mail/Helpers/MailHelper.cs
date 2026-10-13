// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.DataProtection;
using Bodoconsult.App.Helpers;
using System;
using System.IO;
using System.Reflection;

namespace Bodoconsult.Web.Mail.Helpers;

/// <summary>
/// Mail helper class
/// </summary>
public static class MailHelper
{
    /// <summary>
    /// Default ctor
    /// </summary>
    static MailHelper()
    {
        var location = Assembly.GetExecutingAssembly().Location;

        if (string.IsNullOrEmpty(location ))
        {
            return;
        }

        var directoryName = new FileInfo(location).DirectoryName;
        if (directoryName == null)
        {
            return;
        }

        TemplateFolderPath = directoryName;
    }

    /// <summary>
    /// Template folder path
    /// </summary>
    public static string TemplateFolderPath { get; }

    /// <summary>
    /// Format the mail body
    /// </summary>
    /// <param name="body">Body text</param>
    /// <param name="subject">Subject</param>
    /// <param name="signatureTemplate">Signature template</param>
    /// <param name="logoUrl">URL of the logo to use or null</param>
    /// <param name="htmlMailTemplate">HTML mail template</param>
    /// <returns></returns>
    public static string FormatBody(string body, string subject, string signatureTemplate, string logoUrl, string htmlMailTemplate)
    {
        ////Do not use string.Format due to { and } in body
        //body = body.Replace("{0}", contentId);

        if (body.Contains("<html", StringComparison.OrdinalIgnoreCase) && body.Contains("</html>", StringComparison.OrdinalIgnoreCase))
        {
            return body;
        }

        var signature = string.IsNullOrEmpty(signatureTemplate) ? "" : GetTemplate(signatureTemplate).Replace("{0}", logoUrl);

        //Do not use string.Format due to { and } in body
        var html = htmlMailTemplate.Replace("{0}", body + signature).Replace("{1}", subject);

        return html;
    }

    /// <summary>
    /// Get template file as string
    /// </summary>
    /// <param name="templateName">Full path to template file</param>
    /// <returns>template content</returns>
    public static string GetTemplate(string templateName)
    {
        try
        {
            // ReSharper disable once AssignNullToNotNullAttribute

            if (!templateName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                templateName = $"{templateName}.txt";
            }

            templateName = Path.Combine(TemplateFolderPath, $"Templates\\{templateName}");

            var fsIn = new FileStream(templateName, FileMode.Open, FileAccess.Read, FileShare.Read);
            var sr = new StreamReader(fsIn);
            var s = sr.ReadToEnd();
            sr.Dispose();
            fsIn.Close();
            return s;
        }
        catch
        {
            return "";
        }
    }

    /// <summary>
    /// Get a decrypted mail account 
    /// </summary>
    /// <param name="account">Account with crypted properties</param>
    /// <returns>Account with decrypted properties</returns>
    public static SmtpMailAccount GetDecryptedSmtpMailAccount(SmtpMailAccount account)
    {
        var newSmtp = new SmtpMailAccount
        {
            MailAddressSender = PasswordHandler.Decrypt(account.MailAddressSender ?? string.Empty),
            SmtpAccountName = PasswordHandler.Decrypt(account.SmtpAccountName ?? string.Empty),
            SmtpPassword = PasswordHandler.Decrypt(account.SmtpPassword ?? string.Empty),
            SmtpServer = PasswordHandler.Decrypt(account.SmtpServer ?? string.Empty),
            SmtpPort = account.SmtpPort,
            UseSecureConnection = account.UseSecureConnection
        };
        return newSmtp;
    }

    /// <summary>
    /// Get a decrypted mail account 
    /// </summary>
    /// <param name="account">Account with crypted properties</param>
    /// <returns>Account with decrypted properties</returns>
    public static O365MailAccount GetDecryptedO365MailAccount(O365MailAccount account)
    {
        var ma = new O365MailAccount
        {
            UserName = PasswordHandler.Decrypt(account.UserName ?? string.Empty),
            Tenant = PasswordHandler.Decrypt(account.Tenant ?? string.Empty),
            ClientId = PasswordHandler.Decrypt(account.ClientId ?? string.Empty),
            ClientSecret = PasswordHandler.Decrypt(account.ClientSecret ?? string.Empty),
            Instance = PasswordHandler.Decrypt(account.Instance ?? string.Empty),
            Scope = PasswordHandler.Decrypt(account.Scope ?? string.Empty),
            MailAddressSender = PasswordHandler.Decrypt(account.MailAddressSender ?? string.Empty)
        };

        return ma;
    }

    /// <summary>
    /// Get an encrypted mail account 
    /// </summary>
    /// <param name="account">Account with decrypted properties</param>
    /// <returns>Account with encrypted properties</returns>
    public static SmtpMailAccount GetEncryptedSmtpMailAccount(SmtpMailAccount account)
    {
        var newSmtp = new SmtpMailAccount
        {
            MailAddressSender = PasswordHandler.Encrypt(account.MailAddressSender ?? string.Empty),
            SmtpAccountName = PasswordHandler.Encrypt(account.SmtpAccountName ?? string.Empty),
            SmtpPassword = PasswordHandler.Encrypt(account.SmtpPassword ?? string.Empty),
            SmtpServer = PasswordHandler.Encrypt(account.SmtpServer ?? string.Empty),
            SmtpPort = account.SmtpPort,
            UseSecureConnection = account.UseSecureConnection
        };
        return newSmtp;
    }

    /// <summary>
    /// Get an encrypted mail account 
    /// </summary>
    /// <param name="account">Account with decrypted properties</param>
    /// <returns>Account with encrypted properties</returns>
    public static O365MailAccount GetEncryptedO365MailAccount(O365MailAccount account)
    {
        var ma = new O365MailAccount
        {
            UserName = PasswordHandler.Encrypt(account.UserName ?? string.Empty),
            Tenant = PasswordHandler.Encrypt(account.Tenant ?? string.Empty),
            ClientId = PasswordHandler.Encrypt(account.ClientId ?? string.Empty),
            ClientSecret = PasswordHandler.Encrypt(account.ClientSecret ?? string.Empty),
            Instance = PasswordHandler.Encrypt(account.Instance ?? string.Empty),
            Scope = PasswordHandler.Encrypt(account.Scope ?? string.Empty),
            MailAddressSender = PasswordHandler.Encrypt(account.MailAddressSender ?? string.Empty)
        };

        return ma;
    }

    /// <summary>
    /// Get an uncrypted <see cref="IMailAccount"/> instance from JSON
    /// </summary>
    /// <param name="json">JSON with the mail account data with crypted properties</param>
    /// <param name="useSmtp">Use SMTP? If false, O365 is used. Default: false</param>
    /// <returns>Mail account data with decrypted properties</returns>
    public static IMailAccount GetDecryptedAccountFromJson(string json, bool useSmtp)
    {
        if (useSmtp)
        {
            var smtp = SimpleJsonHelper.LoadJsonString<SmtpMailAccount>(json);
            var newSmtp = GetDecryptedSmtpMailAccount(smtp);
            return newSmtp;
        }

        var o365 = SimpleJsonHelper.LoadJsonString<O365MailAccount>(json);
        var newO365 = GetDecryptedO365MailAccount(o365);
        return newO365;
    }

    /// <summary>
    /// Get an encrypted <see cref="IMailAccount"/> instance from JSON
    /// </summary>
    /// <param name="json">JSON with the mail account data with crypted properties</param>
    /// <param name="useSmtp">Use SMTP? If false, O365 is used. Default: false</param>
    /// <returns>Mail account data with decrypted properties</returns>
    public static IMailAccount GetEncryptedAccountFromJson(string json, bool useSmtp)
    {
        if (useSmtp)
        {
            var smtp = SimpleJsonHelper.LoadJsonString<SmtpMailAccount>(json);
            var newSmtp = GetEncryptedSmtpMailAccount(smtp);
            return newSmtp;
        }

        var o365 = SimpleJsonHelper.LoadJsonString<O365MailAccount>(json);
        var newO365 = GetEncryptedO365MailAccount(o365);
        return newO365;
    }
}
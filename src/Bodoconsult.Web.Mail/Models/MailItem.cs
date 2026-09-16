// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Web.Mail.Models;

/// <summary>
/// Represents data for an email to send
/// </summary>
public sealed class MailItem
{
    /// <summary>
    /// GUID of the mail item
    /// </summary>
    public string MailGuid;

    /// <summary>
    /// From
    /// </summary>
    public string From;

    /// <summary>
    /// To
    /// </summary>
    public string To;

    /// <summary>
    /// Subject
    /// </summary>
    public string Subject;

    /// <summary>
    /// HTML body
    /// </summary>
    public string Body;

    /// <summary>
    /// Logo path
    /// </summary>
    public string Logo;

    /// <summary>
    /// Attachment paths
    /// </summary>
    public string Attachments;

    /// <summary>
    /// Template for a signature for the mail body
    /// </summary>
    public string SignatureTemplate;

    /// <summary>
    /// Archive the mail
    /// </summary>
    public bool Archive;

    /// <summary>
    /// SQL queries
    /// </summary>
    public string Queries;

    /// <summary>
    /// Zip the attachments
    /// </summary>
    public bool Zip;

    /// <summary>
    /// Use this password for protecting the ZIP file with attachments
    /// </summary>
    public string ZipPassword;
}
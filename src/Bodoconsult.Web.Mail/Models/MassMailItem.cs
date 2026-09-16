// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System.Collections.Generic;

namespace Bodoconsult.Web.Mail.Models;

/// <summary>
/// Mass mail item
/// </summary>
public sealed class MassMailItem
{
    /// <summary>
    /// Body
    /// </summary>
    public string Body { get; set; }

    /// <summary>
    /// Subject
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// From
    /// </summary>
    public string From { get; set; }

    /// <summary>
    /// Default salutation
    /// </summary>
    public string DefaultSalutation { get; set; } = "Sehr geehrte Damen und Herren";

    /// <summary>
    /// Mail receivers
    /// </summary>
    public List<MailReceiver> To { get; } = new();

    /// <summary>
    /// Images found in the body
    /// </summary>
    public List<ImageMetaData> Images { get; } = new();

    /// <summary>
    /// List with file paths to be attached to the mail
    /// </summary>
    public List<string> Attachments { get; } = new();
}
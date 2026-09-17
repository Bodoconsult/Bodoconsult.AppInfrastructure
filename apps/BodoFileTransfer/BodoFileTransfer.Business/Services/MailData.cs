using System.Collections.Generic;
using BodoFileTransfer.Business.Model;

namespace BodoFileTransfer.Business.Services;

/// <summary>
/// Holds mail component data
/// </summary>
public class MailData
{

    /// <summary>
    /// Current subject of the mail
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Current HTML body of the mail
    /// </summary>
    public string Body { get; set; }

    /// <summary>
    /// File attachments as semicolon separated list
    /// </summary>
    public string Attachments { get; set; }

    /// <summary>
    /// Document files handled in this mail
    /// </summary>
    public List<DocumentFile> DocumentFiles { get; } = new();

}
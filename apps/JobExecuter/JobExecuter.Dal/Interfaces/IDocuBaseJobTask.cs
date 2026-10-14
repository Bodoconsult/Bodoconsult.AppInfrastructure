// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Text.Model;

namespace JobExecuter.Dal.Interfaces;

/// <summary>
/// Basic interface for job tasks creating reports as mail, HTML or JSON
/// </summary>
public interface IDocuBaseJobTask
{
    /// <summary>
    /// Export documentation to HTML
    /// </summary>
    string HtmlTargetFileName { get; set; }


    /// <summary>
    /// Export documentation to Pdf
    /// </summary>
    string PdfTargetFileName { get; set; }

    /// <summary>
    /// Export documentation as <see cref="StructuredText"/> object to JSON.
    /// If empty, the file will be placed in app subfolder docu with the name "job_docu.json" 
    /// </summary>
    string JsonTargetFileName { get; set; }

    /// <summary>
    /// Mail receiver(s) separated by semicolon. Intended to use for mail compliance report to compliance process owner
    /// </summary>
    string MailTo { get; set; }

    /// <summary>
    /// Title for documentation
    /// </summary>
    string Title { get; set; }
}
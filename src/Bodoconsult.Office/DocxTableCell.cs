// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using DocumentFormat.OpenXml;

namespace Bodoconsult.Office;

/// <summary>
/// Table cell in a DOCX table
/// </summary>
public class DocxTableCell
{
    /// <summary>
    /// Paragraph items (with one or more runs)
    /// </summary>
    public List<List<OpenXmlElement>> Items { get;  }= [];

    /// <summary>
    /// Style ID to use for the cell paragraphs
    /// </summary>
    public string? StyleId { get; set; }
}
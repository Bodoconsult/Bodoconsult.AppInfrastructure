// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using DocumentFormat.OpenXml;

namespace Bodoconsult.Office;

/// <summary>
/// Defintion list row in a DOCX table
/// </summary>
public class DocxDefinitionListRow
{
    /// <summary>
    /// Defintion list term
    /// </summary>
    public List<OpenXmlElement> Term { get; } = [];

    /// <summary>
    /// Paragraph items (with one or more runs)
    /// </summary>
    public List<List<OpenXmlElement>> Items { get; } = [];

    /// <summary>
    /// Style ID to use for term
    /// </summary>
    public string? TermStyleId { get; set; }

    /// <summary>
    /// Style ID to use for items
    /// </summary>
    public string? ItemsStyleId { get; set; }
}
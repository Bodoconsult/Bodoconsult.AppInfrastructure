// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.Generic;

namespace Bodoconsult.Pdf.PdfSharp;

/// <summary>
/// A term item for a definition list (HTML dt)
/// </summary>
public class PdfDefinitionListTerm
{
    /// <summary>
    /// The term value to show of this definition list item
    /// </summary>
    public string Term { get; set; }

    /// <summary>
    /// The item values to show for this definition list item
    /// </summary>
    public List<string> Items { get; set; } = [];

}
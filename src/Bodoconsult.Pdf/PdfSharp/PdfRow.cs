// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.Generic;
using MigraDoc.DocumentObjectModel;

namespace Bodoconsult.Pdf.PdfSharp;

/// <summary>
/// A table row
/// </summary>
public class PdfRow
{
    /// <summary>
    /// Cells in the row
    /// </summary>
    public List<PdfCell> Cells { get;  } = new();

    /// <summary>
    /// Current shading color. Use only for non-default values. If not set, the default values for the table are choosen
    /// </summary>
    public Color? ShadingColor { get; set; }
}
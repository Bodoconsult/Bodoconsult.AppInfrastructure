// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using MigraDoc.DocumentObjectModel;
using System.Collections.Generic;
using Bodoconsult.App.Abstractions.Typography;

namespace Bodoconsult.Pdf.PdfSharp;

/// <summary>
/// A table in Pdf
/// </summary>
public class PdfTable
{
    /// <summary>
    /// Caption for the table
    /// </summary>
    public string Caption { get; set; }

    /// <summary>
    /// The name of a bookmark tag to set
    /// </summary>
    public string Tag { get; set; }

    /// <summary>
    /// Columns of the table
    /// </summary>
    public List<PdfColumn> Columns { get;  } = new();

    /// <summary>
    /// Data rows of the table
    /// </summary>
    public List<PdfRow> Rows { get;  } = new();

    /// <summary>
    /// The name of the table style
    /// </summary>
    public string TableStyleName { get; set; } = "NormalTable";

    /// <summary>
    /// Legend to be written below the table
    /// </summary>
    public string Legend { get; set; }

    /// <summary>
    /// Heading for the table to be presented before the table
    /// </summary>
    public string Heading { get; set; }

    /// <summary>
    /// Style name for the heading
    /// </summary>
    public string HeadingStyleName { get; set; }

    /// <summary>
    /// Additional info for the table to be presented before the table
    /// </summary>
    public string AdditionalInfos { get; set; }

    /// <summary>
    /// Style name for the additonal info
    /// </summary>
    public string AdditionalInfosStyleName { get; set; }

    /// <summary>
    /// The style to use for the table
    /// </summary>
    public ITypoTableStyle TableStyle { get; set; } = new PdfDefaultTableStyle();

    ///// <summary>
    ///// The width to use for the table in cm or 0. Default 0
    ///// </summary>
    //public double Width;

    ///// <summary>
    ///// Fit the table to the given width. If the width is zero the available page (or text column) width is taken. Default: false
    ///// </summary>
    //public bool FitToWidth { get; set; }
}
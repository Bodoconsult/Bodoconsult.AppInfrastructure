// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.Pdf.PdfSharp;

/// <summary>
/// A column in a PDF table
/// </summary>
public class PdfColumn
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="columnName">Column name</param>
    public PdfColumn(string columnName)
    {
        ColumnName = columnName;
    }

    /// <summary>
    /// Column name
    /// </summary>
    public string ColumnName { get;  }

    /// <summary>
    /// Text alignment of the column
    /// </summary>
    public PdfTextAlignment TextAlignment { get; set; } = PdfTextAlignment.Left;

    /// <summary>
    /// Maximum length of the content in chars
    /// </summary>
    public int MaxLength { get; set; }

    /// <summary>
    /// Calculated width of the column in cm
    /// </summary>
    public double Width { get; set; }
}
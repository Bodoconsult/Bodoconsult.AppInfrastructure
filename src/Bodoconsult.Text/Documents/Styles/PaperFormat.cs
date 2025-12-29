// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Globalization;
using System.Text;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Text.Interfaces;

namespace Bodoconsult.Text.Documents;

/// <summary>
/// Paper format
/// </summary>
public class PaperFormat : TypoPaperFormat, IPropertyAsAttributeElement
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public PaperFormat()
    {
        PaperFormatName = "A4";
        Size = new Size(21, 29.7);
    }

    /// <summary>
    /// Ctor providing paper format name, width and height
    /// </summary>
    /// <param name="pageFormat">Pageformat as string like A4,21.0,29.7</param>
    public PaperFormat(string pageFormat)
    {
        var values = pageFormat.Split(',');

        if (values.Length != 3)
        {
            throw new Exception("Input string for pageformat has invalid format. Should be: A4,21.0,29.7");
        }

        PaperFormatName = values[0];
        Size = new Size(Convert.ToDouble(values[1]), Convert.ToDouble(values[2]));
    }

    /// <summary>
    /// Ctor providing paper format name, width and height
    /// </summary>
    /// <param name="paperFormatName">Name of the paper format like A4</param>
    /// <param name="width">Width of the paper format in cm</param>
    /// <param name="height">Height of the paper format in cm</param>
    public PaperFormat(string paperFormatName, double width, double height)
    {
        PaperFormatName =paperFormatName;
        Size = new Size(width, height);
    }

    /// <summary>
    /// Ctor providing paper format name, width and height
    /// </summary>
    /// <param name="typoPaperFormat">Typo paper format</param>
    public PaperFormat(TypoPaperFormat typoPaperFormat)
    {
        PaperFormatName = typoPaperFormat.PaperFormatName;
        Size = new Size(typoPaperFormat.Size.Width, typoPaperFormat.Size.Height);
    }

    /// <summary>
    /// Current indenttation for LDML creation
    /// </summary>
    public string Indentation { get; set; }

    /// <summary>
    /// Parent element
    /// </summary>
    public DocumentElement Parent { get; set; }

    /// <summary>
    /// Add the current element to a document defined in LDML (Logical document markup language)
    /// </summary>
    /// <param name="document">StringBuilder instance to create the LDML in</param>
    /// <param name="indent">Current indent</param>
    public void ToLdmlString(StringBuilder document, string indent)
    {
        // do nothing
    }

    /// <summary>
    /// Get the element data as formatted property value for an LDML attribute
    /// </summary>
    public string ToPropertyValue()
    {
        return $"{PaperFormatName},{Size.Width.ToString("0.0000", CultureInfo.InvariantCulture)},{Size.Height.ToString("0.0000", CultureInfo.InvariantCulture)}";
    }
}
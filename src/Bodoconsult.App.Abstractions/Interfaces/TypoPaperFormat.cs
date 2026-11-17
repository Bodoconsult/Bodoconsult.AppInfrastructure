// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Paper format information like name and size. Papersize is given always in portrait mode (meaning paper width smaller or equal to paper height). Set <see cref="Landscape"/> to true for getting a landscape paper
/// </summary>
[DebuggerDisplay("FormatName = {PaperFormatName} Size = {Size.Width} cm x {Size.Height} cm")]
public class TypoPaperFormat
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public TypoPaperFormat()
    {
        PaperFormatName = "A4";
        Size = new(21, 29.7);
    }

    /// <summary>
    /// A name for the papersize like A4. Default A4
    /// </summary>
    public string PaperFormatName { get; set; }

    /// <summary>
    /// Size of the paper. Papersize is given always in portrait mode (meaning paper width smaller or equal to paper height). Set <see cref="Landscape"/> to true for getting a landscape paper. Default: width 21 cm height 29,7 cm (=A4 portrait)
    /// </summary>
    public TypoSize Size { get; set; }

    /// <summary>
    /// Paper orientation is landscape. Default: false
    /// </summary>
    public bool Landscape { get; set; }
}
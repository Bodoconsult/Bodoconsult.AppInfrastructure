// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Helpers;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Text.Interfaces;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Bodoconsult.Text.Documents;

/// <summary>
/// Defines a size of an element on the paper with width and height in cm
/// </summary>
[DebuggerDisplay("Width = {Width} Height = {Height}")]
public class Size : TypoSize, IPropertyAsAttributeElement
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="width">Width in cm</param>
    /// <param name="height">Height in cm</param>
    public Size(double width, double height) : base(width, height)
    {
    }

    /// <summary>
    /// Ctor to built from base class <see cref="TypoSize"/>
    /// </summary>
    /// <param name="size">Base class instance</param>
    public Size(TypoSize size) : base(size.Width, size.Height)
    { }

    /// <summary>
    /// Current indentation for LDML creation
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
        // Do nothing
    }

    /// <summary>
    /// Get the element data as formatted property value for an LDML attribute
    /// </summary>
    public string ToPropertyValue()
    {
        if (Width < TypeHelper.ToleranceValueComparisonsDouble && Height < TypeHelper.ToleranceValueComparisonsDouble)
        {
            return null;
        }

        return $"{Width.ToString("0.0000", CultureInfo.InvariantCulture)},{Height.ToString("0.0000", CultureInfo.InvariantCulture)}";
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Text;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Text.Interfaces;

namespace Bodoconsult.Text.Documents;

/// <summary>
/// Base class for brushes
/// </summary>
public abstract class Brush : TypoBrush, IPropertyAsBlockElement
{
    /// <summary>
    /// Default ctor
    /// </summary>
    protected Brush()
    {
        TypoColor = Styleset.DefaultColor;
    }

    /// <summary>
    /// Get the element data as formatted property value as a LDML block
    /// </summary>
    public virtual string ToPropertyValue()
    {
        throw new NotSupportedException("Create an override for method ToPropertyValue()");
    }

    /// <summary>
    /// Current indenttation for LDML creation
    /// </summary>
    [DoNotSerialize]
    public string Indentation { get; set; } = "    ";

    /// <summary>
    /// Parent element
    /// </summary>
    [DoNotSerialize]
    public DocumentElement Parent { get; set; }

    /// <summary>
    /// Add the current element to a document defined in LDML (Logical document markup language)
    /// </summary>
    /// <param name="document">StringBuilder instance to create the LDML in</param>
    /// <param name="indent">Current indent</param>
    public virtual void ToLdmlString(StringBuilder document, string indent)
    {
        throw new NotSupportedException("Create an override for method ToLdmlString()");
    }
}
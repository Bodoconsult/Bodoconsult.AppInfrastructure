// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Helpers;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Text.Interfaces;
using System;
using System.Globalization;
using System.Text;

namespace Bodoconsult.Text.Documents;

/// <summary>
/// Thickness definition for margins and 
/// </summary>
public class Thickness : TypoThickness, IPropertyAsAttributeElement
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public Thickness()
    { }

    /// <summary>
    /// Ctor with thickness values provided for left, top, right and bottom
    /// </summary>
    /// <param name="left">Left thickness in pt</param>
    /// <param name="top">Top thickness in pt</param>
    /// <param name="right">Right thickness in cm</param>
    /// <param name="bottom">Bottom thickness in pt</param>
    public Thickness(double left, double top, double right, double bottom)
    {
        Left = left;
        Right = right;
        Top = top;
        Bottom = bottom;
    }

    /// <summary>
    /// Ctor with a uniform value provided for left, top, right and bottom
    /// </summary>
    /// <param name="uniformValue"></param>
    public Thickness(double uniformValue)
    {
        Left = uniformValue;
        Right = uniformValue;
        Top = uniformValue;
        Bottom = uniformValue;
    }

    /// <summary>
    /// Ctor providing a LDML property string with 4 numbers separated by comma like 0,6,0,0
    /// </summary>
    /// <param name="values">LDML property string</param>
    public Thickness(string values)
    {
        var data = values.Split(',');

        Left = Convert.ToDouble(data[0]);
        Top = Convert.ToDouble(data[1]);
        Right = Convert.ToDouble(data[2]);
        Bottom = Convert.ToDouble(data[3]);
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
    public void ToLdmlString(StringBuilder document, string indent)
    {
        // Do nothing
        
        
        //document.Append(this.ToHtml());
    }

    /// <summary>
    /// Get the element data as formatted property value for an LDML attribute
    /// </summary>
    public string ToPropertyValue()
    {
        if (Left < TypeHelper.ToleranceValueComparisonsDouble && Top < TypeHelper.ToleranceValueComparisonsDouble &&
            Right < TypeHelper.ToleranceValueComparisonsDouble && Bottom < TypeHelper.ToleranceValueComparisonsDouble)
        {
            return null;
        }

        return $"{Left.ToString("0.0000", CultureInfo.InvariantCulture)},{Top.ToString("0.0000", CultureInfo.InvariantCulture)},{Right.ToString("0.0000", CultureInfo.InvariantCulture)},{Bottom.ToString("0.0000", CultureInfo.InvariantCulture)}";
    }
}
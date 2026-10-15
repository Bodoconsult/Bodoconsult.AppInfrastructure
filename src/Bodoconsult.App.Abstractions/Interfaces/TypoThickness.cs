// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Thickness definition for margins and paddings
/// </summary>
[DebuggerDisplay("L = {Left}cm T = {Top}cm R = {Right}cm B = {Bottom}cm")]
public class TypoThickness
{
    /// <summary>
    /// Line width 1pt (in cm)
    /// </summary>
    public const double LineWidth1Pt = 0.0352775;

    /// <summary>
    /// Default ctor
    /// </summary>
    public TypoThickness()
    { }

    /// <summary>
    /// Ctor with thickness values provided for left, top, right and bottom
    /// </summary>
    /// <param name="left">Left thickness in cm</param>
    /// <param name="top">Top thickness in cm</param>
    /// <param name="right">Right thickness in cm</param>
    /// <param name="bottom">Bottom thickness in cm</param>
    public TypoThickness(double left, double top, double right, double bottom)
    {
        Left = left;
        Right = right;
        Top = top;
        Bottom = bottom;
    }

    /// <summary>
    /// Ctor with a uniform value provided for left, top, right and bottom
    /// </summary>
    /// <param name="uniformValue">A value to set a uniform thickness</param>
    public TypoThickness(double uniformValue)
    {
        Left = uniformValue;
        Right = uniformValue;
        Top = uniformValue;
        Bottom = uniformValue;
    }

    /// <summary>
    /// Ctor providing a string with 4 numbers separated by comma like 0,6,0,0
    /// </summary>
    /// <param name="values">String with 4 numbers separated by comma</param>
    public TypoThickness(string values)
    {
        var data = values.Split(',');

        Left = Convert.ToDouble(data[0]);
        Top = Convert.ToDouble(data[1]);
        Right = Convert.ToDouble(data[2]);
        Bottom = Convert.ToDouble(data[3]);
    }

    /// <summary>
    /// Left thickness in cm
    /// </summary>
    public double Left { get; set; }

    /// <summary>
    /// Top thickness in cm
    /// </summary>
    public double Top { get; set; } 

    /// <summary>
    /// Right thickness in cm
    /// </summary>
    public double Right { get; set; }

    /// <summary>
    /// Bottom thickness in cm
    /// </summary>
    public double Bottom { get; set; }
}
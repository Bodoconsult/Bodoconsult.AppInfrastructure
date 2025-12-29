// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Defines a size of an element on the paper with width and height in cm
/// </summary>
[DebuggerDisplay("Width = {Width} Height = {Height}")]
public class TypoSize
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="width">Width in cm</param>
    /// <param name="height">Height in cm</param>
    public TypoSize(double width, double height)
    {
        Width = width; 
        Height = height;
    }

    /// <summary>
    /// Width in cm
    /// </summary>
    public double Width { get; }

    /// <summary>
    /// Height in cm
    /// </summary>
    public double Height { get; }
}
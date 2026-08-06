// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Base class for brushes
/// </summary>
public abstract class TypoBrush
{
    /// <summary>
    /// Color to use if only one color is possible to render
    /// </summary>
    public TypoColor TypoColor { get; set; } = TypoColors.Black;
}
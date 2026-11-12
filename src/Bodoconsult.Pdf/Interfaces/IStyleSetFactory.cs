// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.Pdf.Interfaces;

/// <summary>
/// Interface for creating <see cref="IStyleSet"/> instances
/// </summary>
public interface IStyleSetFactory
{
    /// <summary>
    /// Generate a stylesheet instance
    /// </summary>
    /// <param name="landscape">Orientation is landscape?</param>
    IStyleSet CreateInstance(bool landscape);
}
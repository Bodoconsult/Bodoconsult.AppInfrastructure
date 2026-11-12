// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Pdf.Interfaces;
using Bodoconsult.Pdf.Stylesets;

namespace Bodoconsult.Pdf.Factories;

/// <summary>
/// Factory to create a <see cref="DefaultStyleSet"/> instance
/// </summary>
public class DefaultStyleSetFactory : IStyleSetFactory
{
    /// <summary>
    /// Generate a stylesheet instance
    /// </summary>
    /// <param name="landscape">Orientation is landscape?</param>
    public IStyleSet CreateInstance(bool landscape)
    {
        var s = new DefaultStyleSet();
        s.CreatePageSetup(landscape);
        s.InitializeStyles();
        return s;
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Pdf.Interfaces;
using BodoWebReporting.Dal.Styles;

namespace BodoWebReporting.Dal.Factories;

/// <summary>
/// Factory to create a <see cref="BodoWebReportingStyleSet"/> instance
/// </summary>
public class BodoWebReportingStyleSetFactory : IStyleSetFactory
{
    /// <summary>
    /// Generate a stylesheet instance
    /// </summary>
    /// <param name="landscape">Orientation is landscape?</param>
    public IStyleSet CreateInstance(bool landscape)
    {
        var s = new BodoWebReportingStyleSet();
        s.CreatePageSetup(landscape);
        s.InitializeStyles();
        return s;
    }
}
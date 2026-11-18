// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.Office.Test.Models;

/// <summary>
/// Default page style A4 landscape with 3 text columns
/// </summary>
public class ThreeColumnA4LandscapePageStyle : BaseTypoPageStyle
{
    public ThreeColumnA4LandscapePageStyle()
    {
        TypoPaperFormat.Landscape = true;
        NumberOfColumns = 3;
        ColumnGap = 1;

        SetMargins();
    }
}
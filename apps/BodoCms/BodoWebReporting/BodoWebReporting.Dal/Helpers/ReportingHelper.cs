// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Abstractions.Typography;

namespace BodoWebReporting.Dal.Helpers;

/// <summary>
/// Class with helper methods for reporting
/// </summary>
public static class ReportingHelper
{
    /// <summary>
    /// Get a elegant default typography
    /// </summary>
    /// <returns></returns>
    public static ITypography DefaultTypography()
    {
        var typography = new ElegantTypographyPageHeader("Calibri", "Calibri", "Calibri")
        {
            GridColumnCount = 12,
            GridColumnWidth = 1.8,
            ChartStyle =
            {
                Width = 750,
                Height = 464,
                FontSize = 10,
                BackGradientStyle = GradientStyle.TopBottom,
                BackgroundColor = TypoColors.Transparent
            },
            MarginLeftFactor = 1.5,
            MarginRightFactor = 1.5,
            MarginTopFactor = 1.5,
            MarginBottomFactor = 1.5,
            TypoPaperFormat =
            {
                Landscape = true
            }
        };

        //var ps = new TypoPaperFormat()
        //    { PaperFormatName = "A3", Size = new TypoSize(29.7, 21)}

        typography.SetMargins();

        return typography;
    }
}
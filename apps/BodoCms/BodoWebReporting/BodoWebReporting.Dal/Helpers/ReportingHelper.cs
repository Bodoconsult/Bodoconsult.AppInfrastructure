// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Reflection;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Abstractions.Typography;
using BodoWebReporting.Dal.Model;

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
            ChartStyle =
            {
                Width = 750,
                Height = 464,
                FontSize = 10,
                BackGradientStyle = GradientStyle.TopBottom,
                BackgroundColor = TypoColors.Transparent
            }
        };

        return typography;
    }
}
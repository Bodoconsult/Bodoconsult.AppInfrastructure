// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Abstractions.Typography;

namespace Bodoconsult.Charting.Test;

public static class GlobalValues
{
    /// <summary>
    /// Get a elegant default typography
    /// </summary>
    /// <returns></returns>
    public static ITypography DefaultTypography()
    {
        var typography = new ElegantTypographyPageHeader("Arial", "Arial", "Arial")
        {
            ChartStyle =
            {
                Width = 750,
                Height = 464,
                FontSize = 10,
                BackGradientStyle = GradientStyle.TopBottom,
            }
        };

        return typography;
    }

}
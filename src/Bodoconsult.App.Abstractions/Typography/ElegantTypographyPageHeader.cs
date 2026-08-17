// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Abstractions.Typography;

/// <summary>
/// Provides an elegant typograph based on sans serif fonts at default. See non-default constructor to change fonts
/// </summary>
public class ElegantTypographyPageHeader : TypographyBase
{
    /// <summary>
    /// Default ctor. Sets font names to Aptos
    /// </summary>
    public ElegantTypographyPageHeader()
    {
        BaseConstructor("Aptos", "Aptos", "Aptos");
    }

    /// <summary>
    /// Ctor to set font names
    /// </summary>
    /// <param name="primaryFontname">Font name for text</param>
    /// <param name="secondaryFontName">Font name for headings</param>
    /// <param name="thirdFontName">Font name for titles</param>
    public ElegantTypographyPageHeader(string primaryFontname, string secondaryFontName, string thirdFontName)
    {
        BaseConstructor(primaryFontname, secondaryFontName, thirdFontName);
    }

    private void BaseConstructor(string primaryFontname, string secondaryFontName, string thirdFontName)
    {
        FontName = primaryFontname;
        FontSize = 11;
        SmallFontSize = FontSize - 2;
        ExtraSmallFontSize = SmallFontSize - 2;

        HeadingFontName = secondaryFontName;
        HeadingFontSize5 = FontSize;
        HeadingFontSize4 = HeadingFontSize5;
        HeadingFontSize3 = HeadingFontSize4;
        HeadingFontSize2 = HeadingFontSize3;
        HeadingFontSize1 = HeadingFontSize2 + 2;

        LineHeight = 0.5;
        GridColumnDividerWidth = 0.5;
        GridColumnWidth = 2.0;
        GridColumnCount = 6;
        DotsPerInch = 300;
        LogoWidth = 2 * GridColumnWidth + GridColumnDividerWidth;

        TitleFontName = thirdFontName;
        SubTitleFontName = thirdFontName;

        TitleFontSize = HeadingFontSize1 + 4;
        SubTitleFontSize = HeadingFontSize1 + 2;


        MarginLeftFactor = 2.5;
        MarginRightFactor = 2.5;
        MarginTopFactor = 3;
        MarginBottomFactor = 2;

        SetMargins();

        PageFooterHeight = 0.5;
        PageHeaderHeight = 1.5;
        PageHeaderMargin = 1.5;
        PageFooterMargin = GridColumnDividerWidth;


        ChartStyle = new ChartStyle
        {
            FontName = FontName,
            TitleFontName = HeadingFontName,
            FontSize = (float)FontSize,
            Width = GetPixelWidth(6),
            Height = GetPixelHeight(6),
            PaperColor = TypoColors.White
        };

        TableBodyBackground = TypoColors.White;
        TableHeaderBackground = TypoColors.White;
        TableBodyUnborderedBackground = TypoColors.Transparent;
        TableHeaderUnborderedBackground = TypoColors.Transparent;
        TableCornerRadius = 0.3;
        TableBorderWidth = 0.05;
        TableBorderColor = TypoColor.FromArgb(178, 204, 255);

    }
}
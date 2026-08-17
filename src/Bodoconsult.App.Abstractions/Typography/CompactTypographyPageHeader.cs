// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Abstractions.Typography;

/// <summary>
/// Provides a compact typograph based on sans serif fonts at default. Use non-default constructor to change fonts
/// </summary>
public class CompactTypographyPageHeader : TypographyBase
{
    /// <summary>
    /// Default ctor. Sets font names to Aptos
    /// </summary>
    public CompactTypographyPageHeader()
    {
        BaseConstructor("Aptos", "Aptos", "Aptos");
    }

    /// <summary>
    /// Ctor to set font names
    /// </summary>
    /// <param name="primaryFontname">Font name for text</param>
    /// <param name="secondaryFontName">Font name for headings</param>
    /// <param name="thirdFontName">Font name for titles</param>
    public CompactTypographyPageHeader(string primaryFontname, string secondaryFontName, string thirdFontName)
    {
        BaseConstructor(primaryFontname, secondaryFontName, thirdFontName);
    }

    private void BaseConstructor(string primaryFontname, string secondaryFontName, string thirdFontName)
    {
        FontName = primaryFontname;
        FontSize = 10;
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
        GridColumnWidth = 2.5;
        GridColumnCount = 6;
        DotsPerInch = 300;
        LogoWidth = 1 * GridColumnWidth + GridColumnDividerWidth;

        TitleFontName = thirdFontName;
        SubTitleFontName = thirdFontName;

        TitleFontSize = HeadingFontSize1 + 4;
        SubTitleFontSize = HeadingFontSize1 + 2;


        MarginLeftFactor = 1.5;
        MarginRightFactor = 1;
        MarginTopFactor = 2;
        MarginBottomFactor = 1.5;

        SetMargins();

        PageFooterHeight = 0.5;
        PageHeaderHeight = 1;
        PageHeaderMargin = 1;
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

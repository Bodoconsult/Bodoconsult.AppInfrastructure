Typography
================================

Bodoconsult.App.Abstractions.TypographyBase is a base class implementing ITypography containing properties and methods around typographic settings for paper reports and / or charts. 
The itention of this library is to store a complete typographic set of properties outside the app in an adjustable JSON file and to read it from there if needed. 

The library contains three predefined typographic sets for common use cases in the classes called DefaultTypographyPageHeader, CompactTypographyPageHeader and ElegantTypographyPageHeader. 
The default paper format is DIN A4 portrait.

DefaultTypographyPageHeader is a typographic style set for a normal paper report. It is a compromiss between use of space and elegance. 

CompactTypographyPageHeader is a typographic style set for compact paper report intended to get as much information as possible on a given paper format.

ElegantTypographyPageHeader is a typograhic style set preferring elegance over use of space. It needs more pages of paper per given information to print.

# How to use the library

The source code contain NUnit test classes, the following source code is extracted from. The samples below show the most helpful use cases for the library.

## Basic usage

``` csharp
var t = new ElegantTypographyPageHeader("Cambria", "Cambria", "Cambria")
            {
                LogoPath = @"C:\bodoconsult\Logos\logo.jpg"
            };
            
t.SetMargins();
```
			
## Customizing a typographic set

``` csharp
var t = new ElegantTypographyPageHeader("Cambria", "Cambria", "Cambria")
            {
                LogoPath = @"C:\bodoconsult\Logos\logo.jpg",
                FontSize = 12,
            };
            
t.SetMargins();
```

## Creating your user-defined typographic set

The following example shows the implementation of a user-defined typographic set. 

``` csharp
/// <summary>
/// Provides your user-defined typograph based on sans serif fonts at default. See non-default constructor to change fonts
/// </summary>
public class UserDefinedTypographyPageHeader : TypographyBase
{
    /// <summary>
    /// Default ctor. Sets font names to Calibri
    /// </summary>
    public UserDefinedTypographyPageHeader()
    {
        BaseConstructor("Calibri", "Calibri", "Calibri");
    }

    /// <summary>
    /// Ctor to set font names
    /// </summary>
    /// <param name="primaryFontname">Font name for text</param>
    /// <param name="secondaryFontName">Font name for headings</param>
    /// <param name="thirdFontName">Font name for titles</param>
    public UserDefinedTypographyPageHeader(string primaryFontname, string secondaryFontName, string thirdFontName)
    {
        BaseConstructor(primaryFontname, secondaryFontName, thirdFontName);
    }

    private void BaseConstructor(string primaryFontname, string secondaryFontName, string thirdFontName)
    {
        // Default page DIN A4 
        PageHeight = 29.4;
        PageWidth = 21.0;

        FontName = primaryFontname;
        FontSize = 11;
        SmallFontSize = FontSize - 2;
        ExtraSmallFontSize = SmallFontSize - 2;

        LogoPath = @"C:\bodoconsult\Logos\logo.jpg";

        HeadingFontName = secondaryFontName;
        HeadingFontSize5 = FontSize;
        HeadingFontSize4 = HeadingFontSize5;
        HeadingFontSize3 = HeadingFontSize4;
        HeadingFontSize2 = HeadingFontSize3;
        HeadingFontSize1 = HeadingFontSize2 + 2;

        LineHeight = 0.5;
        ColumnDividerWidth = 0.5;
        ColumnWidth = 2.0;
        ColumnCount = 6;
        DotsPerInch = 300;
        LogoWidth = 2 * ColumnWidth + ColumnDividerWidth;

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
        PageFooterMargin = ColumnDividerWidth;


        ChartStyle = new ChartStyle
        {
            FontName = FontName,
            TitleFontName = HeadingFontName,
            FontSize = (float)FontSize,
            Width = GetPixelWidth(6),
            Height = GetPixelHeight(6),
            PaperColor = Color.White
        };

        TableBodyBackground = Color.White;
        TableHeaderBackground = Color.White;
        TableBodyUnborderedBackground = Color.Transparent;
        TableHeaderUnborderedBackground = Color.Transparent;
        TableCornerRadius = 0.3;
        TableBorderWidth = 0.05;
        TableBorderColor = Color.FromArgb(178, 204, 255);

    }
}
```
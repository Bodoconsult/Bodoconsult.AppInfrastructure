// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using MigraDoc.DocumentObjectModel;

namespace Bodoconsult.Pdf.Stylesets;

/// <summary>
/// Defines a default style set.Use subclassing for creating advanced stylesets
/// </summary>
public class TypographyBasedStyleSet : DefaultStyleSet
{
    private readonly ITypography _typography;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="typography">Typography to use</param>
    public TypographyBasedStyleSet(ITypography typography)
    {
        _typography = typography;
        DefaultFontSize = _typography.FontSize;
        DefaultVerticalMargin = _typography.FontSize / 4;
    }

    /// <summary>
    /// Create the page setup. This method should set <see cref="DefaultStyleSet.TypeAreaWidth"/>!
    /// </summary>
    /// <returns>Page setup</returns>
    public override PageSetup CreatePageSetup()
    {
        var ps = new PageSetup
        {
            Orientation = _typography.TypoPaperFormat.Size.Width <_typography.TypoPaperFormat.Size.Height ?  Orientation.Portrait: Orientation.Landscape,
            PageFormat = GetPageFormat(_typography.TypoPaperFormat.PaperFormatName),
            PageWidth = Unit.FromCentimeter(_typography.TypoPaperFormat.Size.Width),
            PageHeight = Unit.FromCentimeter(_typography.TypoPaperFormat.Size.Height),
            LeftMargin = Unit.FromCentimeter(_typography.TypoMargins.Left),
            TopMargin = Unit.FromCentimeter(_typography.TypoMargins.Top),
            RightMargin = Unit.FromCentimeter(_typography.TypoMargins.Right),
            BottomMargin = Unit.FromCentimeter(_typography.TypoMargins.Bottom)
        };
        PageSetup = ps;
        return ps;
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.DefinitionListItem"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateDefinitionListItemStyle()
    {
        return new Style("DefinitionListItem", "Normal")
        {
            Font =
            {
                Name = _typography.FontName,
                Size =  DefaultFontSize,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceBefore = DefaultVerticalMargin,
                SpaceAfter = DefaultVerticalMargin,
                PageBreakBefore = false,
                Alignment = ParagraphAlignment.Left,
            }
        };
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.DefinitionListTerm"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateDefinitionListTermStyle()
    {
        return new Style("DefinitionListTerm", "Normal")
        {
            Font =
            {
                Name = _typography.FontName,
                Size =  DefaultFontSize,
                Color = Colors.Black,
                Italic = true
            },
            ParagraphFormat =
            {
                SpaceBefore = DefaultVerticalMargin,
                SpaceAfter = DefaultVerticalMargin,
                PageBreakBefore = false,
                Alignment = ParagraphAlignment.Left,
            }
        };
    }
    /// <summary>
    /// Create <see cref="DefaultStyleSet.Bullet1"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateBullet1Style()
    {
        var style = new Style("Bullet1", "Normal")
        {
            Font =
            {
                Name = _typography.FontName,
                Size = DefaultFontSize,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                Alignment = ParagraphAlignment.Left,
                SpaceBefore = DefaultVerticalMargin,
                SpaceAfter = DefaultVerticalMargin
            }
        };
        style.ParagraphFormat.AddTabStop("26.7cm", TabAlignment.Right);
        style.ParagraphFormat.LeftIndent = Unit.FromCentimeter(1);
        var li = new ListInfo { ListType = ListType.BulletList1 };
        style.ParagraphFormat.ListInfo = li;
        return style;
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Code"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateCodeStyle()
    {
        var style = new Style("Code", "Normal")
        {
            Font =
            {
                Name = "Courier New",
                Size = DefaultFontSize- 2,
                Color = Colors.Black,
                Italic = false,
                Bold = false
            },
            ParagraphFormat =
            {
                Alignment = ParagraphAlignment.Left,
                SpaceBefore = 2 * DefaultVerticalMargin,
                SpaceAfter = 2 * DefaultVerticalMargin
            }
        };
        style.ParagraphFormat.TabStops.ClearAll();
        style.ParagraphFormat.TabStops.AddTabStop("0.5cm");
        style.ParagraphFormat.TabStops.AddTabStop("1cm");
        style.ParagraphFormat.TabStops.AddTabStop("1.5cm");
        style.ParagraphFormat.TabStops.AddTabStop("2cm");
        style.ParagraphFormat.TabStops.AddTabStop("2.5cm");
        style.ParagraphFormat.TabStops.AddTabStop("3cm");
        style.ParagraphFormat.TabStops.AddTabStop("3.5cm");
        style.ParagraphFormat.TabStops.AddTabStop("4cm");
        style.ParagraphFormat.TabStops.AddTabStop("4.5cm");
        style.ParagraphFormat.TabStops.AddTabStop("5cm");
        //style.ParagraphFormat.TabStops.AddTabStop("6cm");
        //style.ParagraphFormat.TabStops.AddTabStop("7cm");
        //style.ParagraphFormat.TabStops.AddTabStop("8cm");
        //style.ParagraphFormat.TabStops.AddTabStop("9cm");
        //style.ParagraphFormat.TabStops.AddTabStop("10cm");
        //style.ParagraphFormat.TabStops.AddTabStop("11cm");
        //style.ParagraphFormat.TabStops.AddTabStop("12cm");
        //style.ParagraphFormat.TabStops.AddTabStop("13cm");
        //style.ParagraphFormat.TabStops.AddTabStop("14cm");
        style.ParagraphFormat.TabStops.AddTabStop("26.7cm", TabAlignment.Right);
        return style;
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Footer"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateFooterStyle()
    {
        var style = new Style("Footer", "Normal")
        {
            Font =
            {
                Name = "Arial Narrow",
                Size = _typography.SmallFontSize,
                Color = Colors.Black
            },
            ParagraphFormat =
            {

                Alignment = ParagraphAlignment.Left,
                SpaceAfter = 0,
                SpaceBefore = 0
            }
        };
        style.ParagraphFormat.Borders.Top.Visible = true;
        style.ParagraphFormat.Borders.Top.Width = 0.5;
        style.ParagraphFormat.Borders.Top.Color = Colors.Black;
        style.ParagraphFormat.AddTabStop(TypeAreaWidth, TabAlignment.Right);
        return style;
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.ChartCell"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateChartCellStyle()
    {
        return new Style("ChartCell", "Normal")
        {
            ParagraphFormat = { Alignment = ParagraphAlignment.Center }
        };
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Equation"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateEquationStyle()
    {
        return new Style("Equation", "Normal")
        {
            Font =
            {
                Name = _typography.FontName,
                Size = DefaultFontSize,
                Color = Colors.Black,
            },
            ParagraphFormat =
            {
                SpaceAfter = 3 * DefaultVerticalMargin,
                Alignment = ParagraphAlignment.Center
            }
        };
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Figure"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateFigureStyle()
    {
        return new Style("Figure", "Normal")
        {
            Font =
            {
                Name = _typography.FontName,
                Size = DefaultFontSize,
                Color = Colors.Black,
            },
            ParagraphFormat =
            {
                SpaceAfter = 3 * DefaultVerticalMargin,
                Alignment = ParagraphAlignment.Center
            }
        };
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.TableLegend"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateTableLegendStyle()
    {
        return new Style("TableLegend", "Normal")
        {
            Font =
            {
                Name = _typography.FontName,
                Size = DefaultFontSize,
                Color = Colors.Black,
            },
            ParagraphFormat =
            {
                SpaceAfter = 3 * DefaultVerticalMargin,
                Alignment = ParagraphAlignment.Center
            }
        };
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Table"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateTableStyle()
    {
        return new Style("Table", "Normal")
        {
            Font =
            {
                Name = "Arial Narrow",
                Size = DefaultFontSize- 2,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceBefore = 3 * DefaultVerticalMargin,
                SpaceAfter = 3 * DefaultVerticalMargin,
                Alignment = ParagraphAlignment.Center
            }
        };
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Error"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateErrorStyle()
    {
        var style = new Style("Error", "Normal")
        {
            Font =
            {
                Name = _typography.FontName,
                Size = DefaultFontSize,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceBefore = 2 * DefaultVerticalMargin,
                SpaceAfter = DefaultVerticalMargin,
                Alignment = ParagraphAlignment.Left
            }
        };
        AddBorder(style, Colors.Red, 1);
        return style;
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Warning"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateWarningStyle()
    {
        var style = new Style("Warning", "Normal")
        {
            Font =
            {
                Name = _typography.FontName,
                Size = DefaultFontSize,
                Color = Colors.Black,
            },
            ParagraphFormat =
            {
                SpaceBefore = 2 * DefaultVerticalMargin,
                SpaceAfter = DefaultVerticalMargin,
                Alignment = ParagraphAlignment.Left
            }
        };
        AddBorder(style, Colors.Yellow, 1);
        return style;
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Info"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateInfoStyle()
    {
        var style = new Style("Info", "Normal")
        {
            Font =
            {
                Name = _typography.FontName,
                Size = DefaultFontSize,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceBefore = 2 * DefaultVerticalMargin,
                SpaceAfter = DefaultVerticalMargin,
                Alignment = ParagraphAlignment.Left
            }
        };
        AddBorder(style, Colors.Black, 1);
        return style;
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.CitationSource"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateCitationSourceStyle()
    {
        return new Style("CitationSource", "Normal")
        {
            Font =
            {
                Name = _typography.FontName,
                Size = DefaultFontSize,
                Color = Colors.Black,
                Italic = true
            },
            ParagraphFormat =
            {
                SpaceAfter = DefaultVerticalMargin,
                Alignment = ParagraphAlignment.Center
            }
        };
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Citation"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateCitationStyle()
    {
        return new Style("Citation", "Normal")
        {
            Font =
            {
                Name = _typography.FontName,
                Size = DefaultFontSize,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceBefore = 2 * DefaultVerticalMargin,
                SpaceAfter = 0,
                Alignment = ParagraphAlignment.Center
            }
        };
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Details"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateDetailsStyle()
    {
        return new Style("Details", "Normal")
        {
            Font =
            {
                Name = "Arial Narrow",
                Size = DefaultFontSize,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceAfter = 3 * DefaultVerticalMargin,
                Alignment = ParagraphAlignment.Center
            }
        };
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Header"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateHeaderStyle()
    {
        var style = new Style("Header", "Normal")
        {
            Font =
            {
                Name = "Arial Narrow",
                Size = _typography.SmallFontSize,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                Alignment = ParagraphAlignment.Right,
                SpaceAfter = 0,
                SpaceBefore = 0
            }
        };
        style.ParagraphFormat.Borders.Bottom.Width = 1;
        style.ParagraphFormat.Borders.Bottom.Color = Colors.Black;
        style.ParagraphFormat.AddTabStop(TypeAreaWidth, TabAlignment.Right);
        return style;
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Tot"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateTotStyle()
    {
        var style = new Style("Tot", "Normal")
        {
            Font =
            {
                Name = _typography.FontName,
                Size = DefaultFontSize,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceAfter = DefaultVerticalMargin,
                SpaceBefore = 0,
                Alignment = ParagraphAlignment.Left
            }
        };
        style.ParagraphFormat.TabStops.AddTabStop(TypeAreaWidth, TabAlignment.Right);
        return style;
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.TotHeading"/> style
    /// </summary>
    /// <returns>Style</returns>

    protected override Style CreateTotHeadingStyle()
    {
        var style = new Style("TotHeading", "Normal")
        {
            Font =
            {
                Name = _typography.HeadingFontName,
                Size = _typography.HeadingFontSize1,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                PageBreakBefore = true,
                KeepTogether = true,
                KeepWithNext = true,
                SpaceAfter = 3 * DefaultVerticalMargin,
                SpaceBefore = 3 * DefaultVerticalMargin,
                Alignment = ParagraphAlignment.Left
            }
        };
        AddBorder(style, Colors.Black, 0, 0, 0, 2);
        return style;
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Tof"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateTofStyle()
    {
        var style = new Style("Tof", "Normal")
        {
            Font =
            {
                Name = _typography.FontName,
                Size = DefaultFontSize,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceAfter = DefaultVerticalMargin,
                SpaceBefore = 0,
                Alignment = ParagraphAlignment.Left
            }
        };

        style.ParagraphFormat.TabStops.AddTabStop(TypeAreaWidth, TabAlignment.Right);
        return style;
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.TofHeading"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateTofHeadingStyle()
    {
        var style = new Style("TofHeading", "Normal")
        {
            Font =
            {
                Name = _typography.HeadingFontName,
                Size = _typography.HeadingFontSize1,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                PageBreakBefore = true,
                KeepTogether = true,
                KeepWithNext = true,
                SpaceAfter = 3 * DefaultVerticalMargin,
                SpaceBefore = 3 * DefaultVerticalMargin,
                Alignment = ParagraphAlignment.Left
            }
        };
        AddBorder(style, Colors.Black, 0, 0, 0, 2);
        return style;
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Toe"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateToeStyle()
    {
        var style = new Style("Toe", "Normal")
        {
            Font =
            {
                Name = _typography.FontName,
                Size =  DefaultFontSize,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceAfter = DefaultVerticalMargin,
                SpaceBefore = 0,
                Alignment = ParagraphAlignment.Left
            }
        };
        style.ParagraphFormat.TabStops.AddTabStop(TypeAreaWidth, TabAlignment.Right);

        return style;
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.ToeHeading"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateToeHeadingStyle()
    {
        var style = new Style("ToeHeading", "Normal")
        {
            Font =
            {
                Name = _typography.HeadingFontName,
                Size = _typography.HeadingFontSize1,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                PageBreakBefore = true,
                KeepTogether = true,
                KeepWithNext = true,
                SpaceAfter = 3 * DefaultVerticalMargin,
                SpaceBefore = 3 * DefaultVerticalMargin,
                Alignment = ParagraphAlignment.Left
            }
        };

        AddBorder(style, Colors.Black, 0, 0, 0, 2);

        return style;
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Toc5"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateToc5Style()
    {
        var style = new Style("TOC5", "Normal")
        {
            Font =
            {
                Name = _typography.FontName,
                Size = DefaultFontSize,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceBefore = DefaultVerticalMargin,
                SpaceAfter = DefaultVerticalMargin
            }
        };

        style.ParagraphFormat.Borders.Bottom.Width = 0;
        style.ParagraphFormat.Borders.Top.Width = 0;
        style.ParagraphFormat.Borders.Right.Width = 0;
        style.ParagraphFormat.Borders.Left.Width = 0;
        style.ParagraphFormat.LeftIndent = Unit.FromCentimeter(4);
        style.ParagraphFormat.TabStops.AddTabStop(TypeAreaWidth, TabAlignment.Right);

        return style;
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Toc4"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateToc4Style()
    {
        var style = new Style("TOC4", "Normal")
        {
            Font =
            {
                Name = _typography.FontName,
                Size = DefaultFontSize,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceBefore = DefaultVerticalMargin,
                SpaceAfter = DefaultVerticalMargin
            }
        };

        style.ParagraphFormat.Borders.Bottom.Width = 0;
        style.ParagraphFormat.Borders.Top.Width = 0;
        style.ParagraphFormat.Borders.Right.Width = 0;
        style.ParagraphFormat.Borders.Left.Width = 0;
        style.ParagraphFormat.LeftIndent = Unit.FromCentimeter(3);
        style.ParagraphFormat.TabStops.AddTabStop(TypeAreaWidth, TabAlignment.Right);

        return style;
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Toc3"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateToc3Style()
    {
        var style = new Style("TOC3", "Normal")
        {
            Font =
            {
                Name = _typography.FontName,
                Size = DefaultFontSize,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceBefore = DefaultVerticalMargin,
                SpaceAfter = DefaultVerticalMargin
            }
        };

        style.ParagraphFormat.Borders.Bottom.Width = 0;
        style.ParagraphFormat.Borders.Top.Width = 0;
        style.ParagraphFormat.Borders.Right.Width = 0;
        style.ParagraphFormat.Borders.Left.Width = 0;
        style.ParagraphFormat.LeftIndent = Unit.FromCentimeter(2);
        style.ParagraphFormat.TabStops.AddTabStop(TypeAreaWidth, TabAlignment.Right);

        return style;
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Toc2"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateToc2Style()
    {
        var style = new Style("TOC2", "Normal")
        {
            Font =
            {
                Name = _typography.FontName,
                Size = DefaultFontSize,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceBefore = 2 * DefaultVerticalMargin,
                SpaceAfter = DefaultVerticalMargin
            }
        };
        style.ParagraphFormat.Borders.Bottom.Width = 0;
        style.ParagraphFormat.Borders.Top.Width = 0;
        style.ParagraphFormat.Borders.Right.Width = 0;
        style.ParagraphFormat.Borders.Left.Width = 0;
        style.ParagraphFormat.LeftIndent = Unit.FromCentimeter(1);
        style.ParagraphFormat.TabStops.AddTabStop(TypeAreaWidth, TabAlignment.Right);
        return style;
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Toc1"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateToc1Style()
    {
        var style = new Style("TOC1", "Normal")
        {
            Font =
            {
                Name = "Arial Black",
                Size =  DefaultFontSize,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceBefore = 2 * DefaultVerticalMargin,
                SpaceAfter = DefaultVerticalMargin
            }
        };
        style.ParagraphFormat.Borders.Bottom.Width = 0;
        style.ParagraphFormat.Borders.Top.Width = 0;
        style.ParagraphFormat.Borders.Right.Width = 0;
        style.ParagraphFormat.Borders.Left.Width = 0;
        style.ParagraphFormat.TabStops.AddTabStop(TypeAreaWidth, TabAlignment.Right);
        return style;
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.TocHeading"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateTocHeadingStyle()
    {
        var style = new Style("TocHeading", "Normal")
        {
            Font =
            {
                Name = _typography.HeadingFontName,
                Size = _typography.HeadingFontSize1,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                PageBreakBefore = true,
                KeepTogether = true,
                KeepWithNext = true,
                SpaceAfter = DefaultVerticalMargin,
                SpaceBefore = 3 * DefaultVerticalMargin,
                Alignment = ParagraphAlignment.Left
            }
        };
        AddBorder(style, Colors.Black, 0, 0, 0, 2);
        return style;
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.ChartYLabel"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateChartYLabelStyle()
    {
        return new Style("ChartYLabel", "Normal")
        {
            Font =
            {
                Name = "Arial Narrow",
                Size = DefaultFontSize - 2,
                Color = Colors.Black
            }
        };
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.ChartTitle"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateChartTitleStyle()
    {
        return new Style("ChartTitle", "Normal")
        {
            Font =
            {
                Name = "Arial Narrow",
                Size =  DefaultFontSize,
                Bold = true,
                Color = Colors.Red
            },
            ParagraphFormat =
            {
                SpaceBefore = 1.5 * DefaultVerticalMargin,
                Alignment = ParagraphAlignment.Center,
                SpaceAfter = 1.5 * DefaultVerticalMargin
            }
        };
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.NoHeading1"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateNoHeading1Style()
    {
        return new Style("NoHeading1", "Normal")
        {
            Font =
            {
                Name = "Arial Black",
                Size =  DefaultFontSize,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceAfter = DefaultVerticalMargin,
                SpaceBefore = 2 * DefaultVerticalMargin,
                Alignment = ParagraphAlignment.Center
            }
        };
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.SectionSubtitle"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateSectionSubtitleStyle()
    {
        return new Style("SectionSubtitle", "Normal")
        {
            Font =
            {
                Name = "Arial Black",
                Size = DefaultFontSize+5,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceAfter = 4 * DefaultVerticalMargin,
                SpaceBefore = 5 * DefaultVerticalMargin,
                Alignment = ParagraphAlignment.Center
            }
        };
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Subtitle"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateSubtitleStyle()
    {
        return new Style("Subtitle", "Normal")
        {
            Font =
            {
                Name = "Arial Black",
                Size = DefaultFontSize+7,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceAfter = 4 * DefaultVerticalMargin,
                SpaceBefore = 5 * DefaultVerticalMargin,
                Alignment = ParagraphAlignment.Center
            }
        };
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.SectionTitle"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateSectionTitleStyle()
    {
        return new Style("SectionTitle", "Normal")
        {
            Font =
            {
                Name = "Arial Black",
                Size = DefaultFontSize+7,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceAfter = 4 * DefaultVerticalMargin,
                SpaceBefore = 30 * DefaultVerticalMargin,
                Alignment = ParagraphAlignment.Center
            }
        };
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Title"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateTitleStyle()
    {
        return new Style("Title", "Normal")
        {
            Font =
            {
                Name = "Arial Black",
                Size = DefaultFontSize+7,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceAfter = 5 * DefaultVerticalMargin,
                SpaceBefore = 30 * DefaultVerticalMargin,
                Alignment = ParagraphAlignment.Center
            }
        };
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Heading5"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateHeading5Style()
    {
        return new Style("Heading5", "Normal")
        {
            Font =
            {
                Name = _typography.FontName,
                Size = DefaultFontSize+1,
                Color = Colors.Black,
                Italic = true
            },
            ParagraphFormat =
            {
                KeepTogether = true,
                KeepWithNext = true,
                SpaceAfter = DefaultVerticalMargin,
                SpaceBefore = DefaultVerticalMargin,
                Alignment = ParagraphAlignment.Left
            }
        };
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Heading4"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateHeading4Style()
    {
        return new Style("Heading4", "Normal")
        {
            Font =
            {
                Name = _typography.FontName,
                Size = DefaultFontSize+1,
                Color = Colors.Black,
                Bold = true
            },
            ParagraphFormat =
            {
                KeepTogether = true,
                KeepWithNext = true,
                SpaceAfter = DefaultVerticalMargin,
                SpaceBefore = DefaultVerticalMargin,
                Alignment = ParagraphAlignment.Left
            }
        };
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Heading3"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateHeading3Style()
    {
        return new Style("Heading3", "Normal")
        {
            Font =
            {
                Name = _typography.FontName,
                Size = DefaultFontSize+3,
                Color = Colors.Black,
                Bold = true
            },
            ParagraphFormat =
            {
                KeepTogether = true,
                KeepWithNext = true,
                SpaceAfter = DefaultVerticalMargin,
                SpaceBefore = 2 * DefaultVerticalMargin,
                Alignment = ParagraphAlignment.Left
            }
        };
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Heading2"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateHeading2Style()
    {
        var style = new Style("Heading2", "Normal")
        {
            Font =
            {
                Name = "Arial Black",
                Size = DefaultFontSize+5,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                KeepTogether = true,
                KeepWithNext = true,
                SpaceAfter = 3 * DefaultVerticalMargin,
                SpaceBefore = 4 * DefaultVerticalMargin,
                Alignment = ParagraphAlignment.Left
            }
        };
        AddBorder(style, Colors.Black, 0, 0, 0, 1);
        return style;
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Heading1"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateHeading1Style()
    {
        var style = new Style("Heading1", "Normal")
        {
            Font =
            {
                Name = "Arial Black",
                Size = DefaultFontSize+7,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                PageBreakBefore = true,
                KeepTogether = true,
                KeepWithNext = true,
                SpaceAfter = 6 * DefaultVerticalMargin,
                SpaceBefore = 3 * DefaultVerticalMargin,
                Alignment = ParagraphAlignment.Left,
            }
        };
        AddBorder(style, Colors.Black, 0, 0, 0, 2);
        return style;
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.NormalTable"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateNormalTableStyle()
    {
        return new Style("NormalTable", "Normal")
        {
            Font =
            {
                Name = "Arial Narrow",
                Size = DefaultFontSize-2
            },
            ParagraphFormat =
            {
                SpaceBefore = 3 * DefaultVerticalMargin,
                SpaceAfter = 3 * DefaultVerticalMargin,
                PageBreakBefore = false,
                Alignment = ParagraphAlignment.Center
            }
        };
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.ParagraphJustify"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateParagrapghJustifyStyle()
    {
        return new Style("ParagraphJustify", "Normal")
        {
            Font =
            {
                Name = _typography.FontName,
                Size =  DefaultFontSize,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceBefore = DefaultVerticalMargin,
                SpaceAfter = DefaultVerticalMargin,
                PageBreakBefore = false,
                Alignment = ParagraphAlignment.Justify,
            }
        };
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.ParagraphCenter"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateParameterCenterStyle()
    {
        return new Style("ParagraphCenter", "Normal")
        {
            Font =
            {
                Name = _typography.FontName,
                Size =  DefaultFontSize,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceBefore = DefaultVerticalMargin,
                SpaceAfter = DefaultVerticalMargin,
                PageBreakBefore = false,
                Alignment = ParagraphAlignment.Center,
            }
        };
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.ParagraphRight"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateParameterRightStyle()
    {
        return new Style("ParagraphRight", "Normal")
        {
            Font =
            {
                Name = _typography.FontName,
                Size =  DefaultFontSize,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceBefore = DefaultVerticalMargin,
                SpaceAfter = DefaultVerticalMargin,
                PageBreakBefore = false,
                Alignment = ParagraphAlignment.Right,
            }
        };
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Normal"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateNormalStyle()
    {
        return new Style("Normal", null)
        {
            Font =
            {
                Name = _typography.FontName,
                Size =  DefaultFontSize,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceBefore = DefaultVerticalMargin,
                SpaceAfter = DefaultVerticalMargin,
                PageBreakBefore = false,
                Alignment = ParagraphAlignment.Left,

            }
        };
    }

    /// <summary>
    /// Create <see cref="DefaultStyleSet.Empty"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected override Style CreateEmptyStyle()
    {
        return new Style("Empty", null)
        {
            Font =
            {
                Name = _typography.FontName,
                Size = 2,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceBefore = 0,
                SpaceAfter = 0,
                LineSpacing = 0.2,
                PageBreakBefore = false,
                Alignment = ParagraphAlignment.Left,
            }
        };
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using MigraDoc.DocumentObjectModel;

namespace Bodoconsult.Pdf.Stylesets;

/// <summary>
/// Defines a default style set.Use subclassing for creating advanced stylesets
/// </summary>
public class DefaultStyleSet : IStyleSet
{
    /// <summary>
    /// Default font size
    /// </summary>
    public Unit DefaultFontSize { get; set; } = 11;

    /// <summary>
    /// Type area width
    /// </summary>
    public Unit TypeAreaWidth { get; private set; }

    /// <summary>
    /// Default vertical margin
    /// </summary>
    protected double DefaultVerticalMargin = 3;

    /// <summary>
    /// Number of text columns the type area is divided in
    /// </summary>
    public int NumberOfColumns { get; set; } = 1;

    /// <summary>
    /// The space between text columns in the type area in cm
    /// </summary>
    public Unit Space { get; set; } = 0;

    /// <summary>
    /// The resulting text column width
    /// </summary>
    public Unit ColumnWidth { get; set; }

    /// <summary>
    /// Calculate measures <see cref="IStyleSet.TypeAreaWidth"/> and ColumnWidth!
    /// </summary>
    public void CalculateMeasures()
    {
        TypeAreaWidth = PageSetup.PageWidth.Centimeter - PageSetup.LeftMargin.Centimeter - PageSetup.RightMargin.Centimeter;
    }

    /// <summary>
    /// Initialize all styles
    /// </summary>
    public void InitializeStyles()
    {
        Empty = CreateEmptyStyle();
        Normal = CreateNormalStyle();
        //Normal.ParagraphFormat.Borders.Left.Width = 1;
        //Normal.ParagraphFormat.Borders.Left.Color = Colors.Black;
        ParagraphRight = CreateParameterRightStyle();
        ParagraphCenter = CreateParameterCenterStyle();
        ParagraphJustify = CreateParagrapghJustifyStyle();

        NormalTable = CreateNormalTableStyle();

        Heading1 = CreateHeading1Style();
        Heading2 = CreateHeading2Style();
        Heading3 = CreateHeading3Style();
        Heading4 = CreateHeading4Style();
        Heading5 = CreateHeading5Style();

        Title = CreateTitleStyle();
        SectionTitle = CreateSectionTitleStyle();
        Subtitle = CreateSubtitleStyle();
        SectionSubtitle = CreateSectionSubtitleStyle();
        NoHeading1 = CreateNoHeading1Style();

        //style.ParagraphFormat.
        //style.ParagraphFormat.Shading.Color = Colors.Orange;

        ChartTitle = CreateChartTitleStyle();
        ChartYLabel = CreateChartYLabelStyle();
        ChartCell = CreateChartCellStyle();

        TocHeading = CreateTocHeadingStyle();
        Toc1 = CreateToc1Style();
        Toc2 = CreateToc2Style();
        Toc3 = CreateToc3Style();
        Toc4 = CreateToc4Style();
        Toc5 = CreateToc5Style();

        ToeHeading = CreateToeHeadingStyle();
        Toe = CreateToeStyle();

        TofHeading = CreateTofHeadingStyle();
        Tof = CreateTofStyle();

        TotHeading = CreateTotHeadingStyle();
        Tot = CreateTotStyle();

        Header = CreateHeaderStyle();
        Footer = CreateFooterStyle();

        Details = CreateDetailsStyle();
        Code = CreateCodeStyle();
        Citation = CreateCitationStyle();
        CitationSource = CreateCitationSourceStyle();
        Info = CreateInfoStyle();
        Warning = CreateWarningStyle();
        Error = CreateErrorStyle();
        Figure = CreateFigureStyle();
        Equation = CreateEquationStyle();

        Table = CreateTableStyle();
        TableLegend = CreateTableLegendStyle();

        Bullet1 = CreateBullet1Style();

        DefinitionListTerm = CreateDefinitionListTermStyle();
        DefinitionListItem = CreateDefinitionListItemStyle();
    }

    /// <summary>
    /// Create <see cref="DefinitionListItem"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateDefinitionListItemStyle()
    {
        return new Style("DefinitionListItem", "Normal")
        {
            Font =
            {
                Name = "Arial",
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
    /// Create <see cref="DefinitionListTerm"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateDefinitionListTermStyle()
    {
        return new Style("DefinitionListTerm", "Normal")
        {
            Font =
            {
                Name = "Arial",
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
    /// Create <see cref="Bullet1"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateBullet1Style()
    {
        var style = new Style("Bullet1", "Normal")
        {
            Font =
            {
                Name = "Arial",
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
    /// Create <see cref="Code"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateCodeStyle()
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
    /// Create <see cref="Footer"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateFooterStyle()
    {
        var style = new Style("Footer", "Normal")
        {
            Font =
            {
                Name = "Arial Narrow",
                Size = DefaultFontSize- 2,
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
        style.ParagraphFormat.AddTabStop(ColumnWidth, TabAlignment.Right);
        return style;
    }

    /// <summary>
    /// Create <see cref="ChartCell"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateChartCellStyle()
    {
        return new Style("ChartCell", "Normal")
        {
            ParagraphFormat = { Alignment = ParagraphAlignment.Center }
        };
    }

    /// <summary>
    /// Create <see cref="Equation"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateEquationStyle()
    {
        return new Style("Equation", "Normal")
        {
            Font =
            {
                Name = "Arial",
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
    /// Create <see cref="Figure"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateFigureStyle()
    {
        return new Style("Figure", "Normal")
        {
            Font =
            {
                Name = "Arial",
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
    /// Create <see cref="TableLegend"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateTableLegendStyle()
    {
        return new Style("TableLegend", "Normal")
        {
            Font =
            {
                Name = "Arial",
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
    /// Create <see cref="Table"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateTableStyle()
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
    /// Create <see cref="Error"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateErrorStyle()
    {
        var style = new Style("Error", "Normal")
        {
            Font =
            {
                Name = "Arial",
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
    /// Create <see cref="Warning"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateWarningStyle()
    {
        var style = new Style("Warning", "Normal")
        {
            Font =
            {
                Name = "Arial",
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
    /// Create <see cref="Info"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateInfoStyle()
    {
        var style = new Style("Info", "Normal")
        {
            Font =
            {
                Name = "Arial",
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
    /// Create <see cref="CitationSource"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateCitationSourceStyle()
    {
        return new Style("CitationSource", "Normal")
        {
            Font =
            {
                Name = "Arial",
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
    /// Create <see cref="Citation"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateCitationStyle()
    {
        return new Style("Citation", "Normal")
        {
            Font =
            {
                Name = "Arial",
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
    /// Create <see cref="Details"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateDetailsStyle()
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
    /// Create <see cref="Header"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateHeaderStyle()
    {
        var style = new Style("Header", "Normal")
        {
            Font =
            {
                Name = "Arial Narrow",
                Size = DefaultFontSize- 2,
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
        style.ParagraphFormat.AddTabStop(ColumnWidth, TabAlignment.Right);
        return style;
    }

    /// <summary>
    /// Create <see cref="Tot"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateTotStyle()
    {
        var style = new Style("Tot", "Normal")
        {
            Font =
            {
                Name = "Arial",
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
        style.ParagraphFormat.TabStops.AddTabStop(ColumnWidth, TabAlignment.Right);
        return style;
    }

    /// <summary>
    /// Create <see cref="TotHeading"/> style
    /// </summary>
    /// <returns>Style</returns>

    protected virtual Style CreateTotHeadingStyle()
    {
        var style = new Style("TotHeading", "Normal")
        {
            Font =
            {
                Name = "Arial Black",
                Size = DefaultFontSize+ 7,
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
    /// Create <see cref="Tof"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateTofStyle()
    {
        var style = new Style("Tof", "Normal")
        {
            Font =
            {
                Name = "Arial",
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

        style.ParagraphFormat.TabStops.AddTabStop(ColumnWidth, TabAlignment.Right);
        return style;
    }

    /// <summary>
    /// Create <see cref="TofHeading"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateTofHeadingStyle()
    {
        var style = new Style("TofHeading", "Normal")
        {
            Font =
            {
                Name = "Arial Black",
                Size = DefaultFontSize+ 7,
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
    /// Create <see cref="Toe"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateToeStyle()
    {
        var style = new Style("Toe", "Normal")
        {
            Font =
            {
                Name = "Arial",
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
        style.ParagraphFormat.TabStops.AddTabStop(ColumnWidth, TabAlignment.Right);

        return style;
    }

    /// <summary>
    /// Create <see cref="ToeHeading"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateToeHeadingStyle()
    {
        var style = new Style("ToeHeading", "Normal")
        {
            Font =
            {
                Name = "Arial Black",
                Size = DefaultFontSize+ 7,
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
    /// Create <see cref="Toc5"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateToc5Style()
    {
        var style = new Style("TOC5", "Normal")
        {
            Font =
            {
                Name = "Arial",
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
        style.ParagraphFormat.TabStops.AddTabStop(ColumnWidth, TabAlignment.Right);

        return style;
    }

    /// <summary>
    /// Create <see cref="Toc4"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateToc4Style()
    {
        var style = new Style("TOC4", "Normal")
        {
            Font =
            {
                Name = "Arial",
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
        style.ParagraphFormat.TabStops.AddTabStop(ColumnWidth, TabAlignment.Right);

        return style;
    }

    /// <summary>
    /// Create <see cref="Toc3"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateToc3Style()
    {
        var style = new Style("TOC3", "Normal")
        {
            Font =
            {
                Name = "Arial",
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
        style.ParagraphFormat.TabStops.AddTabStop(ColumnWidth, TabAlignment.Right);

        return style;
    }

    /// <summary>
    /// Create <see cref="Toc2"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateToc2Style()
    {
        var style = new Style("TOC2", "Normal")
        {
            Font =
            {
                Name = "Arial",
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
        style.ParagraphFormat.TabStops.AddTabStop(ColumnWidth, TabAlignment.Right);
        return style;
    }

    /// <summary>
    /// Create <see cref="Toc1"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateToc1Style()
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
                SpaceAfter = DefaultVerticalMargin,
                LeftIndent = 0,
                RightIndent = 0
            }
        };

        style.ParagraphFormat.Borders.Bottom.Width = 0;
        style.ParagraphFormat.Borders.Top.Width = 0;
        style.ParagraphFormat.Borders.Right.Width = 0;
        style.ParagraphFormat.Borders.Left.Width = 0;
        style.ParagraphFormat.TabStops.AddTabStop(ColumnWidth, TabAlignment.Right);
        return style;
    }

    /// <summary>
    /// Create <see cref="TocHeading"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateTocHeadingStyle()
    {
        var style = new Style("TocHeading", "Normal")
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
                SpaceAfter = DefaultVerticalMargin,
                SpaceBefore = 3 * DefaultVerticalMargin,
                Alignment = ParagraphAlignment.Left
            }
        };
        AddBorder(style, Colors.Black, 0, 0, 0, 2);
        return style;
    }

    /// <summary>
    /// Create <see cref="ChartYLabel"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateChartYLabelStyle()
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
    /// Create <see cref="ChartTitle"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateChartTitleStyle()
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
    /// Create <see cref="NoHeading1"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateNoHeading1Style()
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
    /// Create <see cref="SectionSubtitle"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateSectionSubtitleStyle()
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
    /// Create <see cref="Subtitle"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateSubtitleStyle()
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
    /// Create <see cref="SectionTitle"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateSectionTitleStyle()
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
    /// Create <see cref="Title"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateTitleStyle()
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
    /// Create <see cref="Heading5"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateHeading5Style()
    {
        return new Style("Heading5", "Normal")
        {
            Font =
            {
                Name = "Arial",
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
    /// Create <see cref="Heading4"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateHeading4Style()
    {
        return new Style("Heading4", "Normal")
        {
            Font =
            {
                Name = "Arial",
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
    /// Create <see cref="Heading3"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateHeading3Style()
    {
        return new Style("Heading3", "Normal")
        {
            Font =
            {
                Name = "Arial",
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
    /// Create <see cref="Heading2"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateHeading2Style()
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
    /// Create <see cref="Heading1"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateHeading1Style()
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
    /// Create <see cref="NormalTable"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateNormalTableStyle()
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
    /// Create <see cref="ParagraphJustify"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateParagrapghJustifyStyle()
    {
        return new Style("ParagraphJustify", "Normal")
        {
            Font =
            {
                Name = "Arial",
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
    /// Create <see cref="ParagraphCenter"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateParameterCenterStyle()
    {
        return new Style("ParagraphCenter", "Normal")
        {
            Font =
            {
                Name = "Arial",
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
    /// Create <see cref="ParagraphRight"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateParameterRightStyle()
    {
        return new Style("ParagraphRight", "Normal")
        {
            Font =
            {
                Name = "Arial",
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
    /// Create <see cref="Normal"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateNormalStyle()
    {
        return new Style("Normal", null)
        {
            Font =
            {
                Name = "Arial",
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
    /// Create <see cref="Empty"/> style
    /// </summary>
    /// <returns>Style</returns>
    protected virtual Style CreateEmptyStyle()
    {
        return new Style("Empty", null)
        {
            Font =
            {
                Name = "Arial",
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

    /// <summary>
    /// Create the page setup. This method must set <see cref="IStyleSet.PageSetup"/>!
    /// </summary>
    /// <returns>Page setup</returns>
    public virtual PageSetup CreatePageSetup()
    {
        var ps = new PageSetup
        {
            Orientation = Orientation.Portrait,
            PageFormat = PageFormat.A4,
            PageWidth = Unit.FromCentimeter(21.0),
            PageHeight = Unit.FromCentimeter(29.7),
            LeftMargin = Unit.FromCentimeter(3),
            TopMargin = Unit.FromCentimeter(2.5),
            RightMargin = Unit.FromCentimeter(1.5),
            BottomMargin = Unit.FromCentimeter(2.5)
        };
        
        PageSetup = ps;
        return ps;
    }

    /// <summary>
    /// Add a border around a paragraph
    /// </summary>
    /// <param name="style">Paragraph style to use</param>
    /// <param name="color">Color used for all borders</param>
    /// <param name="left">Left border width in pt or 0 to not show the left border</param>
    /// <param name="top">Top border width in pt or 0 to not show the top border</param>
    /// <param name="right">Right border width in pt or 0 to not show the right border</param>
    /// <param name="bottom">Bottom border width in pt or 0 to not show the bottom border</param>
    protected static void AddBorder(Style style, Color color, int left, int top, int right, int bottom)
    {
        if (left > 0)
        {
            style.ParagraphFormat.Borders.Left.Width = left;
            style.ParagraphFormat.Borders.Left.Color = color;
            style.ParagraphFormat.Borders.Left.Visible = true;
            style.ParagraphFormat.Borders.DistanceFromLeft = 5;
        }
        if (top > 0)
        {
            style.ParagraphFormat.Borders.Top.Width = top;
            style.ParagraphFormat.Borders.Top.Color = color;
            style.ParagraphFormat.Borders.Top.Visible = true;
            style.ParagraphFormat.Borders.DistanceFromTop = 5;
        }
        if (right > 0)
        {
            style.ParagraphFormat.Borders.Right.Width = right;
            style.ParagraphFormat.Borders.Right.Color = color;
            style.ParagraphFormat.Borders.Right.Visible = true;
            style.ParagraphFormat.Borders.DistanceFromRight = 5;
        }
        if (bottom > 0)
        {
            style.ParagraphFormat.Borders.Bottom.Width = bottom;
            style.ParagraphFormat.Borders.Bottom.Color = color;
            style.ParagraphFormat.Borders.Bottom.Visible = true;
            style.ParagraphFormat.Borders.DistanceFromBottom = 5;
        }
    }

    /// <summary>
    /// Add a border on all sides to a style
    /// </summary>
    /// <param name="style">Current style</param>
    /// <param name="color">Border color</param>
    /// <param name="width">Border with</param>
    protected static void AddBorder(Style style, Color color, Unit width)
    {
        style.ParagraphFormat.Borders.Bottom.Width = width;
        style.ParagraphFormat.Borders.Bottom.Color = color;
        style.ParagraphFormat.Borders.Bottom.Visible = true;
        style.ParagraphFormat.Borders.DistanceFromBottom = 5;

        style.ParagraphFormat.Borders.Top.Width = width;
        style.ParagraphFormat.Borders.Top.Color = color;
        style.ParagraphFormat.Borders.Top.Visible = true;
        style.ParagraphFormat.Borders.DistanceFromTop = 5;

        style.ParagraphFormat.Borders.Left.Width = width;
        style.ParagraphFormat.Borders.Left.Color = color;
        style.ParagraphFormat.Borders.Left.Visible = true;
        style.ParagraphFormat.Borders.DistanceFromLeft = 5;

        style.ParagraphFormat.Borders.Right.Width = width;
        style.ParagraphFormat.Borders.Right.Color = color;
        style.ParagraphFormat.Borders.Right.Visible = true;
        style.ParagraphFormat.Borders.DistanceFromRight = 5;
    }

    /// <summary>
    /// Get the page format from a page format name
    /// </summary>
    /// <param name="paperFormatName">Page format name</param>
    /// <returns>Page format</returns>
    public static PageFormat GetPageFormat(string paperFormatName)
    {
        switch (paperFormatName.ToUpperInvariant())
        {
            case "A1":
            case "DIN A1":
                return PageFormat.A1;
            case "A2":
            case "DIN A2":
                return PageFormat.A2;
            case "A3":
            case "DIN A3":
                return PageFormat.A3;
            case "A5":
            case "DIN A5":
                return PageFormat.A5;
            case "B5":
            case "DIN B5":
                return PageFormat.B5;
            case "LETTER":
                return PageFormat.Letter;
            case "LEGAL":
                return PageFormat.Legal;
            case "LEDGER":
                return PageFormat.Ledger;
            case "P11x17":
                return PageFormat.P11x17;
            //case "A4":
            //case "DIN A4":
            default:
                return PageFormat.A4;
        }
    }

    /// <summary>
    /// Style for definition list item
    /// </summary>
    public Style DefinitionListItem { get; set; }

    /// <summary>
    /// Normal paragraphs (default style)
    /// </summary>
    public PageSetup PageSetup { get; set; }

    /// <summary>
    /// The original page setup. Used only for multi-column text PDF documents
    /// </summary>
    public PageSetup PageSetupOriginal { get; set; }

    /// <summary>
    /// Normal paragraphs (default style)
    /// </summary>
    public Style Normal { get; set; }

    /// <summary>
    /// Centered paragraph style
    /// </summary>
    public Style ParagraphCenter { get; set; }

    /// <summary>
    /// Right-aligned paragraph style
    /// </summary>
    public Style ParagraphRight { get; set; }

    /// <summary>
    /// Justified paragraph style
    /// </summary>
    public Style ParagraphJustify { get; set; }

    /// <summary>
    /// Style used a table base. Do NOT change this style
    /// </summary>
    public Style NormalTable { get; set; }

    /// <summary>
    /// Table format
    /// </summary>
    public Style Table { get; set; }

    /// <summary>
    /// Style used for table legends
    /// </summary>
    public Style TableLegend { get; set; }

    /// <summary>
    /// Style used for figure legends
    /// </summary>
    public Style Figure{ get; set; }
    
    /// <summary>
    /// Style used for equation legends
    /// </summary>
    public Style Equation { get; set; }

    /// <summary>
    /// Style for TOC section heading
    /// </summary>
    public Style TocHeading { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    public Style Title { get; set; }

    /// <summary>
    /// Subtitle
    /// </summary>
    public Style Subtitle { get; set; }

    /// <summary>
    /// Section title
    /// </summary>
    public Style SectionTitle { get; set; }

    /// <summary>
    /// Section subtitle
    /// </summary>
    public Style SectionSubtitle { get; set; }

    /// <summary>
    /// Heading level 1
    /// </summary>
    public Style Heading1 { get; set; }

    /// <summary>
    /// Heading level 2
    /// </summary>
    public Style Heading2 { get; set; }

    /// <summary>
    /// Heading level 3
    /// </summary>
    public Style Heading3 { get; set; }

    /// <summary>
    /// Heading level 4
    /// </summary>
    public Style Heading4 { get; set; }

    /// <summary>
    /// Heading level 5
    /// </summary>
    public Style Heading5 { get; set; }

    /// <summary>
    /// No heading 1 for small tables
    /// </summary>
    public Style NoHeading1 { get; set; }

    /// <summary>
    /// Chart title style
    /// </summary>
    public Style ChartTitle { get; set; }

    /// <summary>
    /// Chart y-axis label style
    /// </summary>
    public Style ChartYLabel { get; set; }

    /// <summary>
    /// Style for TOC heading 1
    /// </summary>
    public Style Toc1 { get; set; }

    /// <summary>
    /// Style for TOC heading 2
    /// </summary>
    public Style Toc2 { get; set; }

    /// <summary>
    /// Style for TOC heading 3
    /// </summary>
    public Style Toc3 { get; set; }

    /// <summary>
    /// Style for TOC heading 4
    /// </summary>
    public Style Toc4 { get; set; }

    /// <summary>
    /// Style for TOC heading 5
    /// </summary>
    public Style Toc5 { get; set; }

    /// <summary>
    /// Style for TOE section heading
    /// </summary>
    public Style ToeHeading { get; set; }

    /// <summary>
    /// Style for TOE entry
    /// </summary>
    public Style Toe { get; set; }

    /// <summary>
    /// Style for TOF section heading
    /// </summary>
    public Style TofHeading { get; set; }

    /// <summary>
    /// Style for TOE entry
    /// </summary>
    public Style Tof { get; set; }

    /// <summary>
    /// Style for TOT section heading
    /// </summary>
    public Style TotHeading { get; set; }

    /// <summary>
    /// Style for TOE entry
    /// </summary>
    public Style Tot { get; set; }

    /// <summary>
    /// Page header style
    /// </summary>
    public Style Header { get; set; }

    /// <summary>
    /// Details style
    /// </summary>
    public Style Details { get; set; }

    /// <summary>
    /// Chart cell style
    /// </summary>
    public Style ChartCell { get; set; }

    /// <summary>
    /// Page footer style
    /// </summary>
    public Style Footer { get; set; }

    /// <summary>
    /// Style for code segment paragraphs
    /// </summary>
    public Style Code { get; set; }

    /// <summary>
    /// Style for info segment paragraphs
    /// </summary>
    public Style Info { get; set; }

    /// <summary>
    /// Style for warning segment paragraphs
    /// </summary>
    public Style Warning { get; set; }

    /// <summary>
    /// Style for error segment paragraphs
    /// </summary>
    public Style Error { get; set; }

    /// <summary>
    /// Style for citation segment paragraphs
    /// </summary>
    public Style Citation { get; set; }

    /// <summary>
    /// Style for citation source segment paragraphs
    /// </summary>
    public Style CitationSource { get; set; }

    /// <summary>
    /// A style for bulleted lists
    /// </summary>
    public Style Bullet1 { get; set; }

    /// <summary>
    /// A style to add empty space of make margins top on pages possible
    /// </summary>
    public Style Empty { get; set; }

    /// <summary>
    /// Style for definition list term
    /// </summary>
    public Style DefinitionListTerm { get; set; }
}
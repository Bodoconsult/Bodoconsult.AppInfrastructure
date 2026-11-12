// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Pdf.Extensions;
using Bodoconsult.Pdf.Interfaces;
using Bodoconsult.Pdf.PdfSharp;
using Bodoconsult.Pdf.Stylesets;
using MigraDoc.DocumentObjectModel;

namespace BodoWebReporting.Dal.Styles;

/// <summary>
/// Defines a default style set. Use subclassing for creating advanced stylesets
/// </summary>
public class BodoWebReportingStyleSet : DefaultStyleSet
{
    private readonly TypoColor _blueColor = TypoColor.FromArgb(178, 204, 255); //TypoColor.FromHtml("#E4EDFF");

    private const double VerticalMargin = 3;

    //public BodoWebReportingStyleSet()
    //{ }

    protected override Style CreateBullet1Style()
    {
        var style = new Style("Bullet1", "Normal")
        {
            Font =
            {
                Name = "Arial",
                Size = 10,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                Alignment = ParagraphAlignment.Left,
                SpaceBefore = VerticalMargin,
                SpaceAfter = VerticalMargin
            }
        };
        style.ParagraphFormat.AddTabStop("26.7cm", TabAlignment.Right);
        style.ParagraphFormat.LeftIndent = Unit.FromCentimeter(1);
        var li = new ListInfo { ListType = ListType.BulletList1 };
        style.ParagraphFormat.ListInfo = li;

        return style;
    }

    protected override Style CreateCodeStyle()
    {
        var style = new Style("Code", "Normal")
        {
            Font =
            {
                Name = "Courier New",
                Size = 10,
                Color = Colors.Black,
                Italic = true
            },
            ParagraphFormat =
            {
                Alignment = ParagraphAlignment.Left,
                SpaceBefore = 2 * VerticalMargin,
                SpaceAfter = 2 * VerticalMargin
            }
        };
        style.ParagraphFormat.TabStops.ClearAll();
        style.ParagraphFormat.TabStops.AddTabStop("1cm");
        style.ParagraphFormat.TabStops.AddTabStop("2cm");
        style.ParagraphFormat.TabStops.AddTabStop("3cm");
        style.ParagraphFormat.TabStops.AddTabStop("4cm");
        style.ParagraphFormat.TabStops.AddTabStop("5cm");
        style.ParagraphFormat.TabStops.AddTabStop("6cm");
        style.ParagraphFormat.TabStops.AddTabStop("7cm");
        style.ParagraphFormat.TabStops.AddTabStop("8cm");
        style.ParagraphFormat.TabStops.AddTabStop("9cm");
        style.ParagraphFormat.TabStops.AddTabStop("10cm");
        style.ParagraphFormat.TabStops.AddTabStop("11cm");
        style.ParagraphFormat.TabStops.AddTabStop("12cm");
        style.ParagraphFormat.TabStops.AddTabStop("13cm");
        style.ParagraphFormat.TabStops.AddTabStop("14cm");
        style.ParagraphFormat.TabStops.AddTabStop("26.7cm", TabAlignment.Right);

        return style;
    }

    protected override Style CreateFooterStyle()
    {
        var width = PageSetup.PageHeight - PageSetup.LeftMargin - PageSetup.RightMargin;
        var style = new Style("Footer", "Normal")
        {
            Font =
            {
                Name = "Arial Narrow",
                Size = 8,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceBefore = PageSetup.TopMargin / 3,
                Alignment = ParagraphAlignment.Left,
                SpaceAfter = PageSetup.TopMargin / 3
            }
        };

        style.ParagraphFormat.Borders.Top.Visible = true;
        style.ParagraphFormat.Borders.Top.Width = 0.5;
        style.ParagraphFormat.Borders.Top.Color = Colors.Black;
        style.ParagraphFormat.AddTabStop(width, TabAlignment.Right);
        return style;
    }

    protected override Style CreateChartCellStyle()
    {
        return new Style("ChartCell", "Normal")
        {
            ParagraphFormat = { Alignment = ParagraphAlignment.Center }
        };
    }

    protected override Style CreateFigureStyle()
    {
        return new Style("Details", "Normal")
        {
            Font =
            {
                Name = "Arial Narrow",
                Size = 10,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceAfter = 3 * VerticalMargin,
                Alignment = ParagraphAlignment.Center
            }
        };
    }

    protected override Style CreateTableStyle()
    {
        return new Style("Table", "Normal")
        {
            Font =
            {
                Name = "Arial Narrow",
                Size = 9,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceAfter = 3 * VerticalMargin,
                Alignment = ParagraphAlignment.Center
            }
        };
    }

    protected override Style CreateHeaderStyle()
    {
        var style = new Style("Header", "Normal")
        {
            Font =
            {
                Name = "Arial Narrow",
                Size = 8,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                Alignment = ParagraphAlignment.Right,
                SpaceAfter = PageSetup .TopMargin / 3,
                SpaceBefore = PageSetup .TopMargin / 3
            }
        };
        style.ParagraphFormat.Borders.Bottom.Width = 1;
        style.ParagraphFormat.Borders.Bottom.Color = Colors.Black;
        return style;
    }

    protected override Style CreateToc5Style()
    {
        var style = new Style("TOC5", "Normal")
        {
            Font =
            {
                Name = "Arial",
                Size = 10,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceBefore = 2 * VerticalMargin,
                SpaceAfter = VerticalMargin
            }
        };

        style.ParagraphFormat.Borders.Bottom.Width = 0;
        style.ParagraphFormat.Borders.Top.Width = 0;
        style.ParagraphFormat.Borders.Right.Width = 0;
        style.ParagraphFormat.Borders.Left.Width = 0;
        style.ParagraphFormat.LeftIndent = Unit.FromCentimeter(3);
        style.ParagraphFormat.TabStops.AddTabStop(Unit.FromCentimeter(ColumnWidth.Value), TabAlignment.Right);
        return style;
    }

    protected override Style CreateToc4Style()
    {
        var style = new Style("TOC4", "Normal")
        {
            Font =
            {
                Name = "Arial",
                Size = 10,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceBefore = 2 * VerticalMargin,
                SpaceAfter = VerticalMargin
            }
        };

        style.ParagraphFormat.Borders.Bottom.Width = 0;
        style.ParagraphFormat.Borders.Top.Width = 0;
        style.ParagraphFormat.Borders.Right.Width = 0;
        style.ParagraphFormat.Borders.Left.Width = 0;
        style.ParagraphFormat.LeftIndent = Unit.FromCentimeter(3);
        style.ParagraphFormat.TabStops.AddTabStop(Unit.FromCentimeter(ColumnWidth.Value), TabAlignment.Right);
        return style;
    }

    protected override Style CreateToc3Style()
    {
        var style = new Style("TOC3", "Normal")
        {
            Font =
            {
                Name = "Arial",
                Size = 10,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceBefore = 2 * VerticalMargin,
                SpaceAfter = VerticalMargin
            }
        };

        style.ParagraphFormat.Borders.Bottom.Width = 0;
        style.ParagraphFormat.Borders.Top.Width = 0;
        style.ParagraphFormat.Borders.Right.Width = 0;
        style.ParagraphFormat.Borders.Left.Width = 0;
        style.ParagraphFormat.LeftIndent = Unit.FromCentimeter(2);
        style.ParagraphFormat.TabStops.AddTabStop(Unit.FromCentimeter(ColumnWidth.Value), TabAlignment.Right);

        return style;
    }

    protected override Style CreateToc2Style()
    {
        var style = new Style("TOC2", "Normal")
        {
            Font =
            {
                Name = "Arial",
                Size = 10,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceBefore = 2 * VerticalMargin,
                SpaceAfter = VerticalMargin
            }
        };

        style.ParagraphFormat.Borders.Bottom.Width = 0;
        style.ParagraphFormat.Borders.Top.Width = 0;
        style.ParagraphFormat.Borders.Right.Width = 0;
        style.ParagraphFormat.Borders.Left.Width = 0;
        style.ParagraphFormat.LeftIndent = Unit.FromCentimeter(1);
        style.ParagraphFormat.TabStops.AddTabStop(Unit.FromCentimeter(ColumnWidth.Value), TabAlignment.Right);

        return style;
    }

    protected override Style CreateToc1Style()
    {
        var style = new Style("TOC1", "Normal")
        {
            Font =
            {
                Name = "Arial Black",
                Size = 10,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceBefore = 2 * VerticalMargin,
                SpaceAfter = VerticalMargin
            }
        };

        style.ParagraphFormat.Borders.Bottom.Width = 0;
        style.ParagraphFormat.Borders.Top.Width = 0;
        style.ParagraphFormat.Borders.Right.Width = 0;
        style.ParagraphFormat.Borders.Left.Width = 0;

        style.ParagraphFormat.TabStops.ClearAll();

        style.ParagraphFormat.TabStops.AddTabStop(Unit.FromCentimeter(ColumnWidth.Value - 0.4), TabAlignment.Right);
        //style.ParagraphFormat.TabStops.AddTabStop(Unit.FromCentimeter(26), TabAlignment.Right);

        return style;
    }

    protected override Style CreateTocHeadingStyle()
    {
        var style = new Style("TocHeading", "Normal")
        {
            Font =
            {
                Name = "Arial Black",
                Size = 12,
                Color = Colors.Black
            },
            ParagraphFormat = { SpaceAfter = VerticalMargin }
        };
        style.ParagraphFormat.SpaceAfter = VerticalMargin;
        style.ParagraphFormat.Alignment = ParagraphAlignment.Center;

        return style;
    }

    protected override Style CreateChartYLabelStyle()
    {
        return new Style("ChartYLabel", "Normal")
        {
            Font =
            {
                Name = "Arial Narrow",
                Size = 8,
                Color = Colors.Black
            }
        };
    }

    protected override Style CreateChartTitleStyle()
    {
        return new Style("ChartTitle", "Normal")
        {
            Font =
            {
                Name = "Arial Narrow",
                Size = 10,
                Bold = true,
                Color = Colors.Red
            },
            ParagraphFormat =
            {
                SpaceBefore = 1.5 * VerticalMargin,
                Alignment = ParagraphAlignment.Center,
                SpaceAfter = 1.5 * VerticalMargin,
                Shading = new Shading{ Color = _blueColor.ToPdfColor()}
            }
        };
    }

    protected override Style CreateNoHeading1Style()
    {
        return new Style("NoHeading1", "Normal")
        {
            Font =
            {
                Name = "Arial Black",
                Size = 10,
                Color = Colors.Black,
            },
            ParagraphFormat =
            {
                SpaceAfter = 0,
                SpaceBefore = 0,
                Alignment = ParagraphAlignment.Center,
                Shading = new Shading{ Color = _blueColor.ToPdfColor()}
            }
        };
    }

    protected override Style CreateTitleStyle()
    {
        return new Style("Title", "Normal")
        {
            Font =
            {
                Name = "Arial Black",
                Size = 20,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceAfter = 4 * VerticalMargin,
                SpaceBefore = 5 * VerticalMargin,
                Alignment = ParagraphAlignment.Center
            }
        };
    }

    protected override Style CreateHeading4Style()
    {
        return new Style("Heading4", "Normal")
        {
            Font =
            {
                Name = "Arial",
                Size = 12,
                Color = Colors.Black,
                Bold = true
            },
            ParagraphFormat =
            {
                KeepTogether = true,
                KeepWithNext = true,
                SpaceAfter = VerticalMargin,
                SpaceBefore = VerticalMargin,
                Alignment = ParagraphAlignment.Left
            }
        };
    }

    protected override Style CreateHeading3Style()
    {
        return new Style("Heading3", "Normal")
        {
            Font =
            {
                Name = "Arial",
                Size = 12,
                Color = Colors.Black,
                Bold = true
            },
            ParagraphFormat =
            {
                KeepTogether = true,
                KeepWithNext = true,
                SpaceAfter = VerticalMargin,
                SpaceBefore = 2 * VerticalMargin,
                Alignment = ParagraphAlignment.Left
            }
        };
    }

    protected override Style CreateHeading2Style()
    {
        return new Style("Heading2", "Normal")
        {
            Font =
            {
                Name = "Arial Black",
                Size = 14,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                KeepTogether = true,
                KeepWithNext = true,
                SpaceAfter = VerticalMargin,
                SpaceBefore = 2 * VerticalMargin,
                Alignment = ParagraphAlignment.Left
            }
        };
    }

    protected override Style CreateHeading1Style()
    {
        return new Style("Heading1", "Normal")
        {
            Font =
            {
                Name = "Arial Black",
                Size = 16,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                PageBreakBefore = false,
                KeepTogether = true,
                KeepWithNext = true,
                //SpaceAfter = verticalMargin,
                //SpaceBefore = 2 * verticalMargin,
                Alignment = ParagraphAlignment.Center
            }
        };
    }

    protected override Style CreateNormalTableStyle()
    {
        return new Style("NormalTable", "Normal")
        {
            Font =
            {
                Name = "Arial Narrow",
                Size = 8
            },
            ParagraphFormat =
            {
                SpaceBefore = 0,
                SpaceAfter = 0,
                PageBreakBefore = false,
                Alignment = ParagraphAlignment.Center
            }
        };
    }

    protected override Style CreateNormalStyle()
    {
        return new Style("Normal", null)
        {
            Font =
            {
                Name = "Arial",
                Size = 10,
                Color = Colors.Black
            },
            ParagraphFormat =
            {
                SpaceBefore = VerticalMargin,
                SpaceAfter = VerticalMargin,
                PageBreakBefore = false,
                Alignment = ParagraphAlignment.Left,
            }
        };
    }

    /// <summary>
    /// Create the page setup. This method must set <see cref="IStyleSet.PageSetup"/>!
    /// </summary>
    /// <returns>Page setup</returns>
    public override PageSetup CreatePageSetup(bool landscape)
    {
        PageSetup ps;

        if (landscape)
        {
            ps = new PageSetup
            {
                PageFormat = PageFormat.A4,
                Orientation = Orientation.Landscape,
                PageWidth = Unit.FromCentimeter(29.7),
                PageHeight = Unit.FromCentimeter(21.0),
                LeftMargin = Unit.FromCentimeter(1.3),
                TopMargin = Unit.FromCentimeter(1.5),
                RightMargin = Unit.FromCentimeter(1.3),
                BottomMargin = Unit.FromCentimeter(1.2)
            };
        }
        else
        {
            ps = new PageSetup
            {
                PageFormat = PageFormat.A4,
                Orientation = Orientation.Portrait,
                PageWidth = Unit.FromCentimeter(21.0),
                PageHeight = Unit.FromCentimeter(29.7),
                LeftMargin = Unit.FromCentimeter(1.3),
                TopMargin = Unit.FromCentimeter(1.5),
                RightMargin = Unit.FromCentimeter(1.3),
                BottomMargin = Unit.FromCentimeter(1.2)
            };
        }

        PageSetup = ps;
        return ps;
    }

    protected override ITypoTableStyle CreateDefaultTableStyle()
    {
        return new PdfDefaultTableStyle
        {
            TypoTableHeaderBackColor = TypoColors.White,
            TypoTableAlternateBackColor = _blueColor,
            TypoBorderBrush = new TypoSolidColorBrush(_blueColor)
            //TypoTableBackColor = new TypoColor(217, 230, 245, 1)
        };
    }
}
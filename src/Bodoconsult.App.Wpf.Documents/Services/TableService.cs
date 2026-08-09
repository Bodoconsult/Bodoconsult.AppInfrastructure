// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Bodoconsult.App.Wpf.Documents.General;

namespace Bodoconsult.App.Wpf.Documents.Services;

/// <summary>
/// Create a table and add it to the current flow document
/// </summary>
public class TableService
{
    private readonly Table _table1;
    private readonly string[] _styles = new string[3];
    private readonly TableTypes _typeOfTable;
    private readonly TypographySettingsService _typographySettingsService;
    private readonly FlowDocumentService _flowDocumentService;

    private bool _isHeader;
    private bool _isBorder = true;
    private BlockUIContainer _th;
    private BlockUIContainer _tf;

    private Paragraph _tableParagraph;
    private readonly string[,] _data;
    private TextAlignment[] _alignments;
    private double[] _columnWidths;
    private int _start;
    private int _numberOfColumns;
    private readonly bool _keepTogether;

    readonly int _dpi = 300;


    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="data">Data to show</param>
    /// <param name="isPageBreak">Is page break?</param>
    /// <param name="typeOfTable">Type of table</param>
    /// <param name="flowDocumentService">Current FlowDocumentService instance</param>
    /// <param name="keepTogether">Keep table together</param>
    /// <exception cref="ArgumentNullException">Thrown when flowDocumentService is null</exception>
    public TableService(string[,] data, bool isPageBreak,  TableTypes typeOfTable,
        FlowDocumentService flowDocumentService, bool keepTogether = true)
    {
        _keepTogether = keepTogether;
        _data = data;
        _flowDocumentService = flowDocumentService ?? throw new ArgumentNullException(nameof(flowDocumentService));

        _typeOfTable=typeOfTable;
        _typographySettingsService = flowDocumentService.TypographySettingsService;

        // Create the Table...
        _table1 = new Table
        {
            CellSpacing = 0,
            BreakPageBefore = isPageBreak,
            Name = $"table{_flowDocumentService.TableCounter}",
        };

        // ...and add it in a Floater to the FlowDocument Blocks collection.
        var style = _flowDocumentService.FindStyleResource(_isBorder ? "ParagraphTableNoMargin" : "ParagraphTable");
        _tableParagraph = new Paragraph
        {
            Style = style,
            Name = $"tableP{_flowDocumentService.TableCounter}"
        };

        _flowDocumentService.TableCounter++;

        _flowDocumentService.CurrentSection.Blocks.Add(_tableParagraph);
    }

    /// <summary>
    /// Load the styles required for table elements
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Table type not valid</exception>
    public void LoadStyles()
    {
        switch (_typeOfTable)
        {
            case TableTypes.Normal:
                _styles[0] = "TableHeaderCellBordered";
                _styles[1] = "TableCellBordered";
                _styles[2] = "TableBordered";
                _isHeader = true;
                break;

            case TableTypes.NormalNoHeader:
                _styles[0] = "TableHeaderCellBordered";
                _styles[1] = "TableCellBordered";
                _styles[2] = "TableBordered";
                _isHeader = false;
                break;

            case TableTypes.NormalUnbordered:
                _styles[0] = "TableHeaderCellUnbordered";
                _styles[1] = "TableCellUnbordered";
                _styles[2] = "TableUnbordered";
                _isHeader = true;
                _isBorder = false;
                break;

            case TableTypes.NormalNoHeaderUnbordered:
                _styles[0] = "TableHeaderCellUnbordered";
                _styles[1] = "TableCellUnbordered";
                _styles[2] = "TableUnbordered";
                _isHeader = false;
                _isBorder = false;
                break;

            case TableTypes.Small:
                _styles[0] = "SmallTableHeaderCellBordered";
                _styles[1] = "SmallTableCellBordered";
                _styles[2] = "TableBordered";
                _isHeader = true;
                break;

            case TableTypes.SmallNoHeader:
                _styles[0] = "SmallTableHeaderCellBordered";
                _styles[1] = "SmallTableCellBordered";
                _styles[2] = "TableBordered";
                _isHeader = false;
                break;

            case TableTypes.SmallUnbordered:
                _styles[0] = "SmallTableHeaderCellUnbordered";
                _styles[1] = "SmallTableCellUnbordered";
                _styles[2] = "TableUnbordered";
                _isHeader = true;
                _isBorder = false;
                break;

            case TableTypes.SmallNoHeaderUnbordered:
                _styles[0] = "SmallTableHeaderCellUnbordered";
                _styles[1] = "SmallTableCellUnbordered";
                _styles[2] = "TableUnbordered";
                _isHeader = false;
                _isBorder = false;
                break;
            case TableTypes.ExtraSmall:
                _styles[0] = "ExtraSmallTableHeaderCellBordered";
                _styles[1] = "ExtraSmallTableCellBordered";
                _styles[2] = "TableBordered";
                _isHeader = true;
                break;

            case TableTypes.ExtraSmallNoHeader:
                _styles[0] = "ExtraSmallTableHeaderCellBordered";
                _styles[1] = "ExtraSmallTableCellBordered";
                _styles[2] = "TableBordered";
                _isHeader = false;
                break;

            case TableTypes.ExtraSmallUnbordered:
                _styles[0] = "ExtraSmallTableHeaderCellUnbordered";
                _styles[1] = "ExtraSmallTableCellUnbordered";
                _styles[2] = "TableUnbordered";
                _isHeader = true;
                _isBorder = false;
                break;

            case TableTypes.ExtraSmallNoHeaderUnbordered:
                _styles[0] = "ExtraSmallTableHeaderCellUnbordered";
                _styles[1] = "ExtraSmallTableCellUnbordered";
                _styles[2] = "TableUnbordered";
                _isHeader = false;
                _isBorder = false;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_typeOfTable), _typeOfTable, null);
        }

        _table1.Style = _flowDocumentService.FindStyleResource(_styles[2]);
        _table1.Background = _isBorder
            ? _typographySettingsService.TableHeaderBackground
            : _typographySettingsService.TableHeaderUnborderedBackground;
    }

    /// <summary>
    /// Add the header
    /// </summary>
    public void AddHeader()
    {
        var radius = _typographySettingsService.TableCornerRadius;

        var headerRect = new Border
        {
            BorderThickness = new Thickness(_typographySettingsService.TableBorderWidth,
                _typographySettingsService.TableBorderWidth,
                _typographySettingsService.TableBorderWidth,
                0),
            CornerRadius = new CornerRadius(radius, radius, 0, 0),
            Height = radius,
            BorderBrush = _typographySettingsService.TableBorder
            //Background = TypographySettingsService.TableHeaderBackground
        };

        _th = new BlockUIContainer(headerRect)
        {
            Margin = new Thickness(0),
            Padding = new Thickness(0)
        };
    }

    /// <summary>
    /// Add the footer
    /// </summary>
    public void AddFooter()
    {
        var radius = _typographySettingsService.TableCornerRadius;

        var footerRect = new Border
        {
            BorderThickness = new Thickness(_typographySettingsService.TableBorderWidth,
                0,
                _typographySettingsService.TableBorderWidth,
                _typographySettingsService.TableBorderWidth),
            CornerRadius = new CornerRadius(0, 0, radius, radius),
            Height = radius,
            BorderBrush = _typographySettingsService.TableBorder
            //Background = TypographySettingsService.TableHeaderBackground
        };

        _tf = new BlockUIContainer(footerRect)
        {
            Margin = new Thickness(0),
            Padding = new Thickness(0),

        };
    }

    /// <summary>
    /// Add the columns to the table
    /// </summary>
    public void AddColumns()
    {
        // Create columns and add them to the table's Columns collection.
        _numberOfColumns = _data.GetLength(1);
        _alignments = new TextAlignment[_numberOfColumns];
        _columnWidths = new double[_numberOfColumns];

        var checkRow = _isHeader ? 1 : 0;

        for (var x = 0; x < _numberOfColumns; x++)
        {
            _table1.Columns.Add(new TableColumn());



            _alignments[x] = TextAlignment.Left;

            var value = _data[checkRow, x];

            if (string.IsNullOrEmpty(value))
            {
                if (checkRow + 1 == _data.GetLength(0))
                {
                    continue;
                }

                value = _data[checkRow + 1, x];
                if (string.IsNullOrEmpty(value)) continue;
            }

            var erg = double.TryParse(value.Replace("%", string.Empty), NumberStyles.Any, Thread.CurrentThread.CurrentUICulture, out _);

            if (erg)
            {
                _alignments[x] = TextAlignment.Right;
            }


            //// Set alternating background colors for the middle colums.
            //if (x % 2 == 0)
            //    table1.Columns[x].Background = Brushes.Beige;
            //else
            //    table1.Columns[x].Background = Brushes.LightSteelBlue;
        }
    }

    /// <summary>
    /// Add the header row to the table
    /// </summary>
    public void AddHeaderRow()
    {
        if (!_isHeader)
        {
            return;
        }

        _start = 0;

        var style = _flowDocumentService.FindStyleResource(_styles[0]);


        
        var rowGroup = new TableRowGroup
        {
            Background =
                _isBorder
                    ? _typographySettingsService.TableHeaderBackground
                    : _typographySettingsService.TableHeaderUnborderedBackground
        };
        _table1.RowGroups.Add(rowGroup);

        // Add the second (header) row.
        rowGroup.Rows.Add(new TableRow());
        var currentRow = rowGroup.Rows[0];


        //// Global formatting for the header row.
        //currentRow.FontSize = 18;
        //currentRow.FontWeight = FontWeights.Bold;
        for (var column = 0; column < _data.GetLength(1); column++)
        {

            var paragraph = new Paragraph(new Run(_data[0, column]))
            {
                TextAlignment = _alignments[column]
            };


            var cell = new TableCell(paragraph)
            {
                Style = style
            };


            var run = paragraph.Inlines.FirstInline as Run;
            if (run != null && !string.IsNullOrEmpty(run.Text))
            {
                var ft = new FormattedText(run.Text, _flowDocumentService.TypographySettingsService.DocumentMetaData.CultureInfo, FlowDirection.LeftToRight, new Typeface(run.FontFamily, run.FontStyle, run.FontWeight, run.FontStretch), run.FontSize, Brushes.Black, _dpi);
                _columnWidths[column] = Math.Max(_columnWidths[column], ft.Width * 1.1 + cell.Padding.Left + +cell.Padding.Right);
            }

            currentRow.Cells.Add(cell);
        }

        // Add cells with Content to the header row.

        _start = 1;
    }

    /// <summary>
    /// Add the data rows to the table
    /// </summary>
    public void AddDataRows()
    {
        // Add all other rows row.
        var style = _flowDocumentService.FindStyleResource(_styles[1]);
        var rowGroup = new TableRowGroup
        {
            Background = _isBorder ? _typographySettingsService.TableBodyBackground : _typographySettingsService.TableBodyUnborderedBackground
        };
        _table1.RowGroups.Add(rowGroup);

        for (var row = _start; row < _data.GetLength(0); row++)
        {

            var tableRow = new TableRow();
            rowGroup.Rows.Add(tableRow);
            var currentRow = tableRow;

            //// Global formatting for the header row.
            //currentRow.FontSize = 12;
            //currentRow.FontWeight = FontWeights.Normal;


            for (var column = 0; column < _data.GetLength(1); column++)
            {

                var paragraph = new Paragraph(new Run(_data[row, column]))
                {
                    TextAlignment = _alignments[column]
                };
                var cell = new TableCell(paragraph)
                {
                    Style = style
                };
                currentRow.Cells.Add(cell);

                var run = paragraph.Inlines.FirstInline as Run;
                if (run is null || string.IsNullOrEmpty(run.Text))
                {
                    continue;
                }

                var ft = new FormattedText(run.Text, _flowDocumentService.TypographySettingsService.DocumentMetaData.CultureInfo, FlowDirection.LeftToRight, new Typeface(run.FontFamily, run.FontStyle, run.FontWeight, run.FontStretch), run.FontSize, Brushes.Black, _dpi);
                _columnWidths[column] = Math.Max(_columnWidths[column], ft.Width * 1.1 + cell.Padding.Left + +cell.Padding.Right);

            }
        }

        // Column width
        var maxWidth = Math.Round((_typographySettingsService.ContentSize.Width - 4) / _numberOfColumns, 0);


        var currWidth = _columnWidths.Sum();

        if (currWidth > _typographySettingsService.ContentSize.Width)
        {
            for (var x = 0; x < _numberOfColumns; x++)
            {

                if (_columnWidths[x] > maxWidth)
                    _columnWidths[x] = maxWidth;
            }
        }

        var width = 0.0;
        for (var x = 0; x < _numberOfColumns; x++)
        {
            _table1.Columns[x].Width = new GridLength(_columnWidths[x]);
            width += _columnWidths[x];
        }

        var margin = (_typographySettingsService.ContentSize.Width - width - 4) / 2;

        //tf.Margin = new Thickness(margin, 0, margin, 0);
        //th.Margin = new Thickness(margin, 0, margin, 0);
        //footerRect.Width = width;
        //headerRect.Width = width;
        //table1.Margin = new Thickness(0, 0, 0, 0);

        // Add figure or floater
        if (_keepTogether)
        {
            style = _flowDocumentService.FindStyleResource("FigureTable");
            var figure = new Figure
            {
                Style = style,
                Margin = new Thickness(margin, 0, margin, 0)
            };
            if (_isBorder)
            {
                figure.Blocks.Add(_th);
            }
            figure.Blocks.Add(_table1);
            if (_isBorder)
            {
                figure.Blocks.Add(_tf);
            }

            _tableParagraph.Inlines.Add(figure);
        }
        else
        {
            style = _flowDocumentService.FindStyleResource("FloaterTable");
            var floater = new Floater(_table1)
            {
                Style = style,
                Margin = new Thickness(margin, 0, margin, 0)
            };
            _tableParagraph.Inlines.Add(floater);
        }
    }
}
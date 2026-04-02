// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Data;
using System.Globalization;
using System.Text;
using Bodoconsult.Office.Interfaces;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

// ReSharper disable PossiblyMistakenUseOfParamsMethod

namespace Bodoconsult.Office;

/// <summary>
/// Creates an OpenXML XLSX file
/// </summary>
public class XlsxBuilder
{
    private SpreadsheetDocument _wb;
    private WorkbookPart _wp;
    private WorksheetPart _wsp;
    private Worksheet _ws;
    private SheetData _sd;
    private Columns _columns;

    private Cell _range;

    #region Caching variables

    private uint _cacheRowId;
    private uint _cacheColumnId;
    private Row _cacheRow;
    private Cell _cacheCell;

    #endregion

    private readonly string _decimalSeparator;

    /// <summary>
    /// Default ctor
    /// </summary>
    public XlsxBuilder()
    {

        _decimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        NumberFormatDouble = "#,##0.00";
        NumberFormatInteger = "#,##0";
        NumberFormatDate = "dd.MM.yyyy";

        try
        {

        }
        catch (Exception ex)
        {
            ExcelError(ex, null);
            ErrorCode = 1;
        }
    }

    /// <summary>
    /// Destructor
    /// </summary>
    ~XlsxBuilder()
    {
        Quit();
    }

    /// <summary>
    /// Delegate for sending status message to UI
    /// </summary>
    public StatusHandlerDelegate Status { get; set; }

    /// <summary>
    /// Delegate for sending error message to UI
    /// </summary>
    public ErrorHandlerDelegate Error { get; set; }

    /// <summary>
    /// Current style
    /// </summary>
    public SpreadsheetStyles Style { get; set; }

    /// <summary>
    /// Current error code
    /// </summary>
    public int ErrorCode { get; private set; }

    /// <summary>
    /// Numberformat for <see cref="double"/> values
    /// </summary>
    public string NumberFormatDouble { get; set; }

    /// <summary>
    /// Numberformat for <see cref="DateTime"/> values
    /// </summary>
    public string NumberFormatDate { get; set; }

    /// <summary>
    /// Numberformat for <see cref="int"/> values
    /// </summary>
    public string NumberFormatInteger { get; set; }

    /// <summary>
    /// Neue leere Mappe anlegen
    /// </summary>
    public void NewWorkbook(string fileName)
    {
        if (ErrorCode != 0)
        {
            return;
        }

        //try
        //{
        //Add a new workbook.


        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        var excelDocument = new XlsxDocument { NumberFormatDouble = NumberFormatDouble };
        _wb = excelDocument.CreatePackage(fileName);


        //_wb = SpreadsheetDocument.Open(fileName, true);

        _wp = _wb.WorkbookPart;



        //using (var styleXmlReader = new StreamReader("Styles.xml"))
        //{
        //    var xml = styleXmlReader.ReadToEnd();
        //    _wp.WorkbookStylesPart.Stylesheet.InnerXml = xml;
        //    _wp.WorkbookStylesPart.Stylesheet.Save();
        //}





        //}
        //catch (Exception ex)
        //{
        //    ExcelError(ex, null);

        //    _error = 2;
        //}
    }

    /// <summary>
    /// Neue Mappe auf Basis einer Vorlage anlegen
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="template"></param>
    public void NewWorkbook(string fileName, string template)
    {
        if (ErrorCode != 0)
        {
            return;
        }

        try
        {
            ////Add a new workbook.
            //File.Copy(template, fileName);

            //var os = new OpenSettings();

            //_wb = SpreadsheetDocument.Open(fileName, true, os);

            //_wp = _wb.WorkbookPart == null ? _wb.WorkbookPart : _wb.AddWorkbookPart();


        }
        catch (Exception ex)
        {
            ExcelError(ex, template);

            ErrorCode = 3;
        }

    }

    /// <summary>
    /// Tabellenblatt über Index auswählen
    /// </summary>
    /// <param name="index">Indexzahl</param>
    public void SelectSheet(int index)
    {
        if (ErrorCode != 0)
        {
            return;
        }

        //try
        //{

        var name = string.Empty;

        //Get the first worksheet.
        foreach (var x in from Sheet x in _wp.Workbook.Sheets where x.SheetId == index select x)
        {
            name = x.Id;
            break;
        }

        _wsp = (WorksheetPart)_wp.GetPartById(name);

        //_wsp = GetWorksheetPartByName(name);
        _ws = _wsp.Worksheet;

        _sd = _ws.GetFirstChild<SheetData>();




        //// Check if the column collection exists
        //_columns = _ws.Elements<Columns>().FirstOrDefault() ??
        //              _ws.InsertAt(new Columns(), 0);




        //}
        //catch (Exception ex)
        //{
        //    ExcelError(ex, null);
        //    _error = 4;
        //}

    }

    //private  WorksheetPart GetWorksheetPartByName(string sheetName)
    //{
    //    var sheets = _wp.Workbook.GetFirstChild<Sheets>().Elements<Sheet>().Where(s => s.Name == sheetName).ToList();

    //    if (!sheets.Any())
    //    {
    //        // The specified worksheet does not exist.

    //        return null;
    //    }

    //    var relationshipId = sheets.First().Id.Value;
    //    var worksheetPart = (WorksheetPart)_wp.GetPartById(relationshipId);
    //    return worksheetPart;

    //}


    /// <summary>
    /// Select sheet by its name
    /// </summary>
    /// <param name="name">Sheet name</param>
    public void SelectSheet(string name)
    {
        if (ErrorCode != 0)
        {
            return;
        }

        try
        {
            //Get the first worksheet.
            _wp.Workbook.Save();

            var sheet = GetSheetFromName(name);

            _wsp = GetWorkSheetFromSheet(sheet);

            _ws = _wsp.Worksheet;

            _sd = _ws.GetFirstChild<SheetData>();

        }
        catch (Exception ex)
        {
            ExcelError(ex, null);
            ErrorCode = 5;
        }
    }

    /// <summary>
    /// Wähle erstes Tabellenblatt und benenne es um
    /// </summary>
    /// <param name="name">Neuer Name für Tabellenblatt</param>
    public void SelectSheetFirst(string name)
    {
        if (ErrorCode != 0)
        {
            return;
        }

        SelectSheet(1);

        try
        {


            var sheet = _wp.Workbook.GetFirstChild<Sheets>().Elements<Sheet>().FirstOrDefault(x => x.SheetId == 1);
            sheet.Name = name;
        }
        catch (Exception ex)
        {
            ExcelError(ex, null);
            ErrorCode = 6;
        }

    }


    /// <summary>
    /// Neues Blatt anlegen
    /// </summary>
    /// <param name="name">Name der anzulegenden Tabelle</param>
    public void NewSheet(string name)
    {
        //if (_error != 0) return;

        var sheets = _wp.Workbook.GetFirstChild<Sheets>();

        // Add the worksheetpart
        _wsp = _wp.AddNewPart<WorksheetPart>();
        _wsp.Worksheet = new Worksheet(new SheetData());

        var sheetProtection1 = new SheetProtection { Sheet = false, Objects = false, Scenarios = false, FormatCells = true, FormatColumns = true, FormatRows = true, InsertColumns = true, InsertRows = true, InsertHyperlinks = true, DeleteColumns = true, DeleteRows = true };
        _wsp.Worksheet.Append(sheetProtection1);

        _wsp.Worksheet.Append(XlsxDocument.GetPageMargins());

        _wsp.Worksheet.Save();

        // Add the sheet and make relation to workbook
        var sheet = new Sheet
        {
            Id = _wp.GetIdOfPart(_wsp),
            SheetId = (uint)(sheets.Count() + 1),
            Name = name
        };
        // ReSharper disable PossiblyMistakenUseOfParamsMethod
        sheets.InsertAt(sheet, 0);
        // ReSharper restore PossiblyMistakenUseOfParamsMethod
        _wp.Workbook.Save();

        _ws = _wsp.Worksheet;
        _sd = _ws.GetFirstChild<SheetData>();

        //// Check if the column collection exists
        //_columns = _ws.Elements<DocumentFormat.OpenXml.Spreadsheet.Columns>().FirstOrDefault() ??
        //              _ws.InsertAt(new DocumentFormat.OpenXml.Spreadsheet.Columns(), 0);


    }





    //// Generates content of workbookPart1.
    //private static void GenerateDefaultWorkbookContent(WorksheetPart worksheetPart1)
    //{
    //    var worksheet1 = new Worksheet { MCAttributes = new MarkupCompatibilityAttributes { Ignorable = "x14ac" } };
    //    worksheet1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    //    worksheet1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
    //    worksheet1.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");
    //    var sheetDimension1 = new SheetDimension { Reference = "A1" };

    //    var sheetViews1 = new SheetViews();
    //    var sheetView1 = new SheetView { WorkbookViewId = 0U };

    //    // ReSharper disable PossiblyMistakenUseOfParamsMethod
    //    sheetViews1.Append(sheetView1);

    //    var sheetFormatProperties1 = new SheetFormatProperties { BaseColumnWidth = 10U, DefaultRowHeight = 15D, DyDescent = 0.25D };
    //    var sheetData1 = new SheetData();
    //    var pageMargins1 = new PageMargins { Left = 0.7D, Right = 0.7D, Top = 0.78740157499999996D, Bottom = 0.78740157499999996D, Header = 0.3D, Footer = 0.3D };

    //    worksheet1.Append(sheetDimension1);
    //    worksheet1.Append(sheetViews1);
    //    worksheet1.Append(sheetFormatProperties1);
    //    worksheet1.Append(sheetData1);
    //    worksheet1.Append(pageMargins1);

    //    worksheetPart1.Worksheet = worksheet1;

    //    // ReSharper restore PossiblyMistakenUseOfParamsMethod
    //}


    /// <summary>
    /// Select cell range by A1
    /// </summary>
    /// <param name="a1Range">Range in A1 writing</param>
    public void SelectRange(string a1Range)
    {
        if (ErrorCode != 0)
        {
            return;
        }

        var erg = ColumnRowIndexFromName(a1Range);

        //_range = _ws.Descendants<Cell>().FirstOrDefault(c => c.CellReference == a1Bezug);

        //if (_range != null) return;




        //_range = new Cell();
        //_range.CellReference = a1Bezug;
        //_range.CellValue = new CellValue("Test");
        //_ws.AppendChild(_range);

        SelectRange(erg[0], erg[1]);

    }





    /// <summary>
    /// Select range by R1C1
    /// </summary>
    /// <param name="rowIndex">Row index</param>
    /// <param name="colIndex">Column index</param>
    public void SelectRange(uint rowIndex, uint colIndex)
    {
        if (ErrorCode != 0)
        {
            return;
        }

        Row row;
        Row previousRow = null;
        Cell previousCell = null;

        var cellAddress = ColumnNameFromIndex(colIndex) + rowIndex;

        if (rowIndex != _cacheRowId)
        {

            // Check if the row exists, create if necessary
            if (_sd.Elements<Row>().Any(item => item.RowIndex == rowIndex))
            {
                row = _sd.Elements<Row>().First(item => item.RowIndex == rowIndex);
            }
            else
            {
                row = new Row { RowIndex = rowIndex };
                //sheetData.Append(row);
                for (var counter = rowIndex - 1; counter > 0; counter--)
                {
                    previousRow = _sd.Elements<Row>().FirstOrDefault(item => item.RowIndex == counter);
                    if (previousRow != null)
                    {
                        break;
                    }
                }
                _sd.InsertAfter(row, previousRow);
            }
        }
        else
        {
            row = _cacheRow;
        }


        if (_cacheRowId == rowIndex && _cacheColumnId == colIndex)
        {
            _range = _cacheCell;
        }
        else
        {
            _range = row.Elements<Cell>().FirstOrDefault(item => item.CellReference.Value == cellAddress);

            // Check if the cell exists, create if necessary
            if (_range == null)
            {
                if (_cacheColumnId > 0 && rowIndex == _cacheRowId && colIndex == _cacheColumnId + 1)
                {
                    previousCell = _cacheCell;
                }
                else
                {
                    // Find the previous existing cell in the row
                    for (var counter = colIndex - 1; counter > 0; counter--)
                    {
                        previousCell = row.Elements<Cell>().FirstOrDefault(item => item.CellReference.Value == ColumnNameFromIndex(counter) + rowIndex);
                        if (previousCell != null)
                        {
                            break;
                        }
                    }
                }

                _range = new Cell { CellReference = cellAddress };
                row.InsertAfter(_range, previousCell);
            }
        }

        _cacheColumnId = colIndex;
        _cacheCell = _range;
        _cacheRowId = rowIndex;
        _cacheRow = row;

        _columns = _ws.Elements<Columns>().FirstOrDefault() ?? _ws.InsertAt(new Columns(), 0);

        // Check if the column exists
        if (_columns.Elements<Column>().Any(item => item.Min == colIndex))
        {
            return;
        }


        Column previousColumn = null;

        // Find the previous existing column in the columns
        for (var counter = colIndex - 1; counter > 0; counter--)
        {
            previousColumn = _columns.Elements<Column>().FirstOrDefault(item => item.Min == counter);
            if (previousColumn != null)
            {
                break;
            }
        }
        _columns.InsertAfter(
            new Column
            {
                Min = colIndex,
                Max = colIndex,
                CustomWidth = true,
                Width = 9
            }, previousColumn);
    }

    /// <summary>
    /// Converts a column number to column name (i.e. A, B, C..., AA, AB...)
    /// </summary>
    /// <param name="columnIndex">Index of the column</param>
    /// <returns>Column name</returns>
    public static string ColumnNameFromIndex(uint columnIndex)
    {
        var columnName = string.Empty;

        while (columnIndex > 0)
        {
            var remainder = (columnIndex - 1) % 26;
            columnName = Convert.ToChar(65 + remainder).ToString(CultureInfo.InvariantCulture) + columnName;
            columnIndex = (columnIndex - remainder) / 26;
        }

        return columnName;
    }

    /// <summary>
    /// Get column indexes from colum name
    /// </summary>
    /// <param name="columnName">Column name</param>
    /// <returns></returns>
    public static uint[] ColumnRowIndexFromName(string columnName)
    {

        var neu = new uint[columnName.Length];
        var erg = new uint[] { 0, 0 };
        int i;
        for (i = 0; i < columnName.Length; i++)
        {

            var s = columnName.Substring(i, 1);

            var code = Encoding.ASCII.GetBytes(s);

            if (code[0] >= 65 && code[0] < 91)
            {
                neu[i] = code[0];
            }
            else
            {
                break;
            }
        }


        if (neu[1] > 0)
        {
            erg[0] = (neu[0] - 64) * 26 + (neu[1] - 64);
        }
        else
        {
            erg[0] = neu[0] - 64;
        }

        var zahl = columnName.Substring(i);

        erg[1] = Convert.ToUInt32(zahl);

        return erg;
    }


    ///// <summary>
    ///// Gets the Excel column name based on a supplied index number.
    ///// </summary>
    ///// <returns>1 = A, 2 = B... 27 = AA, etc.</returns>
    //private static string GetColumnName(uint columnIndex)
    //{
    //    var dividend = columnIndex;
    //    var columnName = String.Empty;

    //    while (dividend > 0)
    //    {
    //        var modifier = (dividend - 1) % 26;
    //        columnName =
    //            Convert.ToChar(65 + modifier).ToString(CultureInfo.InvariantCulture) + columnName;
    //        dividend = (uint)((dividend - modifier) / 26);
    //    }

    //    return columnName;
    //}



    //// Given a worksheet and a row index, return the row.
    //private Row GetRow(uint rowIndex)
    //{
    //    return _ws.GetFirstChild<SheetData>().
    //               Elements<Row>().First(r => r.RowIndex == rowIndex);
    //} 



    /// <summary>
    /// Set a value in a cell
    /// </summary>
    /// <param name="value">Value to set</param>
    public int SetValue(string value)
    {
        if (ErrorCode != 0)
        {
            return 0;
        }

        var len = 15;

        if (value.StartsWith('='))
        {
            var formula = new CellFormula(value[1..])
            {
                FormulaType = CellFormulaValues.Normal,
                CalculateCell = true,
            };

            _range.CellFormula = formula;

        }
        else
        {
            len = (int)(value.Length * 1.3);
            SetValueInternal(value, CellValues.String);
        }

        Format();

        return len;
    }

    /// <summary>
    /// Load styles
    /// </summary>
    public void LoadStyles()
    {
        //AddPredefinedStyles("Default");
    }

    //private void AddPredefinedStyles(string styleName)
    //{


    //    using (var styleXmlReader = new StreamReader(string.Format(@"Styles\{0}.xml", styleName)))
    //    {
    //        var xml = styleXmlReader.ReadToEnd();

    //        var format = new CellFormat(xml);


    //        _wp.WorkbookStylesPart.Stylesheet.
    //        _wp.WorkbookStylesPart.Stylesheet.Save();
    //    }


    //}


    /// <summary>
    /// Set number as cell content
    /// </summary>
    /// <param name="value">Number to set</param>
    public int SetValue(double value)
    {
        if (ErrorCode != 0)
        {
            return 0;
        }

        var columnValue = _decimalSeparator == "." ? value.ToString(CultureInfo.InvariantCulture) : value.ToString(CultureInfo.InvariantCulture).Replace(",", ".");

        if (!double.IsNaN(value))
        {
            SetValueInternal(columnValue, CellValues.Number);
        }

        Format();

        return value.ToString(NumberFormatDouble).Length;
    }



    /// <summary>
    /// Set number as cell content
    /// </summary>
    /// <param name="value">Number to set</param>
    public int SetValue(long value)
    {
        if (ErrorCode != 0)
        {
            return 0;
        }

        var columnValue = _decimalSeparator == "." ? value.ToString(CultureInfo.InvariantCulture) : value.ToString(CultureInfo.InvariantCulture).Replace(",", ".");

        SetValueInternal(columnValue, CellValues.Number);

        Format();

        return value.ToString(NumberFormatInteger).Length;
    }


    ///// <summary>
    ///// Set number as cell content
    ///// </summary>
    ///// <param name="value">Number to set</param>
    //public void SetValue(int value)
    //{
    //    if (_error != 0) return;

    //    _parameters = new Object[1];
    //    _parameters[0] = value;
    //    _objRangeLate.GetType().InvokeMember("Value", BindingFlags.SetProperty, null, _objRangeLate, _parameters);

    //    var alt = _numberFormat;
    //    _numberFormat = "#,##0";
    //    Format();
    //    _numberFormat = alt;

    //}

    /// <summary>
    /// Set number as cell content
    /// </summary>
    /// <param name="value">Number to set</param>
    public int SetValue(int value)
    {
        if (ErrorCode != 0)
        {
            return 0;
        }

        var columnValue = _decimalSeparator == "." ? value.ToString(CultureInfo.InvariantCulture) : value.ToString(CultureInfo.InvariantCulture).Replace(",", ".");

        SetValueInternal(columnValue, CellValues.Number);

        Format();

        return value.ToString(NumberFormatInteger).Length;
    }


    /// <summary>
    /// Set number as cell content
    /// </summary>
    /// <param name="value">Number to set</param>
    public int SetValue(bool value)
    {
        if (ErrorCode != 0) return 0;

        var columnValue = value ? "1" : "0";

        SetValueInternal(columnValue, CellValues.Boolean);

        Format();

        return 6;
    }


    /// <summary>
    /// Set date as cell content
    /// </summary>
    /// <param name="value">Date to set</param>
    public int SetValue(DateTime value)
    {
        if (ErrorCode != 0)
        {
            return 0;
        }

        //var x = value.ToOADate().ToString(CultureInfo.InvariantCulture);

        var columnValue = _decimalSeparator == "."
            ? value.ToOADate().ToString(CultureInfo.InvariantCulture) :
            value.ToOADate().ToString(CultureInfo.InvariantCulture).Replace(",", ".");

        SetValueInternal(columnValue, CellValues.Date);

        Format();

        return value.ToString(NumberFormatDate).Length;
    }



    private void SetValueInternal(string value, CellValues valueType)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        if (valueType != CellValues.Date)
        {
            _range.DataType = valueType;
        }

        if (value[..1] == "=")
        {
            var f = new CellFormula(value);
            _range.CellFormula = f;
        }
        else
        {
            var v = new CellValue(value);
            _range.CellValue = v;
        }

    }


    /// <summary>
    /// Ausgewählten Zellbereich formatieren
    /// </summary>
    public void Format()
    {
        if (ErrorCode != 0)
        {
            return;
        }


        ////_range.StyleIndex = 2;

        var wert = (uint)Style;

        _range.StyleIndex = wert;
    }



    /// <summary>
    /// Quit file creation
    /// </summary>
    public void Quit()
    {
        try
        {
            //Return control of Excel to the user.
            _wb?.Dispose();
        }
        catch (Exception ex)
        {
            ExcelError(ex, null);
        }
    }

    /// <summary>
    /// Set formats to default
    /// </summary>
    public void SetToDefault()
    {
        Style = SpreadsheetStyles.Default;
    }

    /// <summary>
    /// Write a <see cref="DataTable"/> to the seet
    /// </summary>
    /// <param name="dt">Data to write</param>
    /// <param name="rowIndex">Row index for the left top cell</param>
    /// <param name="colIndex">Column index for the left top cell</param>
    public void FillDataTable(DataTable dt, uint rowIndex, uint colIndex)
    {
        //try
        //{


        if (ErrorCode != 0)
        {
            return;
        }

        var row = rowIndex;
        var col = colIndex;
        var high = false;
        var columns = new List<string>();

        var columnObj = new List<Column>();

        var lengths = new List<int>();

        Style = SpreadsheetStyles.TableHeader;
        foreach (DataColumn d in dt.Columns)
        {
            var col1 = col;

            SelectRange(row, col1);

            var column = _columns.Elements<Column>().FirstOrDefault(x => x.Min == col1);
            column.BestFit = true;

            columnObj.Add(column);

            var s = d.DataType.Name.ToLower();
            columns.Add(s);

            //switch (s)
            //{
            //    case "datetime":
            //        column.Width = 15;
            //        break;
            //    case "boolean":
            //        column.Width = 10;
            //        break;
            //    case "single":
            //    case "double":
            //    case "decimal":
            //        column.Width = 20;
            //        break;
            //    case "int":
            //    case "int32":
            //    case "byte":
            //    case "int16":
            //    case "int64":
            //        column.Width = 15;
            //        break;
            //    default:
            //        column.Width = 36;
            //        break;
            //}

            lengths.Add(d.ColumnName.Length);

            SetValue(d.ColumnName);
            col++;
        }
        row++;

        var colCount = columns.Count;

        //foreach (DataRow r in dt.Rows)

        for (var x = 0; x < dt.Rows.Count; x++)
        {

            var r = dt.Rows[x];

            if (row % 100 == 0) ExcelStatus($"Writing row {row}...");

            col = colIndex;

            high = !high;


            for (var y = 0; y < colCount; y++)
            {
                SelectRange(row, col);

                //if (string.IsNullOrEmpty(r[d.Ordinal].ToString())) continue;

                int length;

                Style = high ? SpreadsheetStyles.TableContent : SpreadsheetStyles.TableContentAlternate;
                if (r.IsNull(y))
                {
                    length = SetValue(string.Empty);

                }
                else
                {
                    switch (columns[y])
                    {
                        case "datetime":
                            Style = high ? SpreadsheetStyles.TableContentDate : SpreadsheetStyles.TableContentAlternateDate;
                            length = SetValue(Convert.ToDateTime(r[y].ToString()));
                            break;
                        case "boolean":
                            length = SetValue(Convert.ToBoolean(r[y].ToString()));
                            break;
                        case "single":
                        case "double":
                        case "decimal":
                            Style = high
                                ? SpreadsheetStyles.TableContentNumeric
                                : SpreadsheetStyles.TableContentAlternateNumeric;
                            var value = Convert.ToDouble(r[y]);
                            length = SetValue(value);
                            break;
                        case "int":
                        case "int32":
                        case "byte":
                        case "int16":
                        case "int64":
                            Style = high
                                ? SpreadsheetStyles.TableContentInteger
                                : SpreadsheetStyles.TableContentAlternateInteger;

                            length = string.IsNullOrEmpty(r[y].ToString())
                                ? SetValue(string.Empty)
                                : SetValue(Convert.ToInt64(r[y].ToString()));
                            break;
                        default:
                            length = SetValue(r[y].ToString());
                            break;
                    }
                }

                if (lengths[y] < length)
                {
                    lengths[y] = length;
                }

                col++;
            }

            row++;
        }

        col = 0;
        foreach (var column in columnObj)
        {
            var len = lengths[(int)col] > 35 ? 35 : lengths[(int)col];

            column.Width = len * 1.2;
            col++;
        }


        //AutoFitColumns();

        //}
        //catch (Exception ex)
        //{
        //    ExcelError(ex, null);
        //    _error = 55;

        //}
    }



    /// <summary>
    /// Adjust column widths automatically
    /// </summary>
    public void AutoFitColumns()
    {
        foreach (var column in _columns.Descendants<Column>())
        {
            column.BestFit = true;
        }
    }


    //private static double GetWidth(string font, int fontSize, string text)
    //{
    //    System.Drawing.Font stringFont = new System.Drawing.Font(font, fontSize);
    //    return GetWidth(stringFont, text);
    //}

    //private static double GetWidth(System.Drawing.Font stringFont, string text)
    //{
    //    // This formula is based on this article plus a nudge ( + 0.2M )
    //    // http://msdn.microsoft.com/en-us/library/documentformat.openxml.spreadsheet.column.width.aspx
    //    // Truncate(((256 * Solve_For_This + Truncate(128 / 7)) / 256) * 7) = DeterminePixelsOfString

    //    Size textSize = TextRenderer.MeasureText(text, stringFont);
    //    double width = (double)(((textSize.Width / (double)7) * 256) - (128 / 7)) / 256;
    //    width = (double)decimal.Round((decimal)width + 0.2M, 2);

    //    return width;
    //}



    /// <summary>
    /// Save file as
    /// </summary>
    /// <param name="fileName">File name</param>
    public void Save(string fileName)
    {
        // Do nothing

        //try
        //{

        //}
        //catch (Exception ex)
        //{
        //    ExcelError(ex, fileName);
        //}
    }

    /// <summary>
    /// Statusanzeige
    /// </summary>
    /// <param name="message">Nachricht für Statusanzeige</param>
    public void ExcelStatus(string message)
    {
        Status?.Invoke(message);
    }

    /// <summary>
    /// Show error message
    /// </summary>
    /// <param name="ex"> </param>
    /// <param name="message">Nachricht für Statusanzeige</param>
    public void ExcelError(Exception ex, string message)
    {
        Error?.Invoke(ex, message);
    }

    /// <summary>
    /// Add a headline to the sheet in cell A1
    /// </summary>
    /// <param name="title">Text for the headline</param>
    public void Header(string title)
    {
        Style = SpreadsheetStyles.Header;
        SelectRange(1, 1);
        SetValue(title);

        Style = SpreadsheetStyles.Default;
        SelectRange(2, 1);
        SetValue(DateTime.Now.ToString("dd.MM.yyyy"));
    }

    /// <summary>
    /// Create a table structure from a data array
    /// </summary>
    /// <param name="data">Data to create table for</param>
    /// <param name="header">Header text</param>
    /// <param name="rowIndex">Row index of the left top corner</param>
    /// <param name="colIndex">Column index of the left top corner</param>
    public void FillDataArray<T>(T[,] data, string[] header, uint rowIndex, uint colIndex)
    {
        var row = rowIndex;
        var col = colIndex;
        var high = false;

        var typeName = typeof(T).Name.ToLower();

        Style = SpreadsheetStyles.TableHeader;
        foreach (var headLine in header)
        {
            var col1 = col;

            SelectRange(row, col1);

            var column = _columns.Elements<Column>().FirstOrDefault(x => x.Min == col1);

            if (column == null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            column.Width = typeName switch
            {
                "datetime" => 15,
                "boolean" => 10,
                "single" or "double" or "decimal" => 25,
                "int" or "int32" or "byte" or "int16" or "int64" => 15,
                _ => 36
            };

            SetValue(headLine);
            col++;
        }
        row++;


        for (var currRow = 0; currRow < data.GetLength(0); currRow++)
        {
            ExcelStatus($"Schreibe Zeile {row}...");
            col = colIndex;
            high = !high;


            for (var currCol = 0; currCol < data.GetLength(1); currCol++)
            {

                SelectRange(row, col);

                //if (string.IsNullOrEmpty(data[currRow, currCol])) continue;

                Style = high ? SpreadsheetStyles.TableContent : SpreadsheetStyles.TableContentAlternate;
                switch (typeName)
                {
                    case "datetime":
                        Style = high ? SpreadsheetStyles.TableContentDate : SpreadsheetStyles.TableContentAlternateDate;
                        SetValue(Convert.ToDateTime(data[currRow, currCol]));
                        break;
                    case "boolean":
                        SetValue(Convert.ToBoolean(data[currRow, currCol]));
                        break;
                    case "single":
                    case "double":
                    case "decimal":
                        Style = high ? SpreadsheetStyles.TableContentNumeric : SpreadsheetStyles.TableContentAlternateNumeric;
                        var value = Convert.ToDouble(data[currRow, currCol]);
                        SetValue(value);
                        break;
                    case "int":
                    case "int32":
                    case "byte":
                    case "int16":
                    case "int64":
                        Style = high ? SpreadsheetStyles.TableContentInteger : SpreadsheetStyles.TableContentAlternateInteger;

                        if (string.IsNullOrEmpty(data[currRow, currCol].ToString()))
                        {
                            SetValue(string.Empty);
                        }
                        else
                        {
                            SetValue(Convert.ToInt64(data[currRow, currCol]));
                        }
                        break;
                    default:
                        SetValue(data[currRow, currCol].ToString());
                        break;
                }
                col++;
            }
            row++;
        }

        AutoFitColumns();
    }

    #region Helpers

    /// <summary>
    /// Select sheet by name
    /// </summary>
    /// <param name="sheetName">Sheet name</param>
    /// <returns><see cref="Sheet"/> instance selected</returns>
    public Sheet GetSheetFromName(string sheetName)
    {
        return _wp.Workbook.Sheets?.Elements<Sheet>()
            .FirstOrDefault(s => s.Name.HasValue && s.Name.Value == sheetName);
    }

    /// <summary>
    /// Get the <see cref="WorksheetPart"/> for a selected sheet
    /// </summary>
    /// <param name="sheet">Sheet instance</param>
    /// <returns><see cref="WorksheetPart"/> rthe seeht is member of</returns>
    public WorksheetPart GetWorkSheetFromSheet(Sheet sheet)
    {
        var worksheetPart = (WorksheetPart)_wp.GetPartById(sheet.Id);
        return worksheetPart;
    }

    #endregion
}
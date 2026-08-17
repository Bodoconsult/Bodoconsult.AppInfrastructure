// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Data;
using System.Reflection;
using System.Runtime.Versioning;
using Bodoconsult.Office.Interfaces;

namespace Bodoconsult.Office;

/// <summary>
/// A helper class for late binding Microsoft (R) Excel via COM and creating XLSX spreadsheets
/// </summary>
[SupportedOSPlatform("windows")]
public class XlsxLateBindingBuilder: IDisposable
{

    private object _objAppLate;
    private object _objBookLate;
    private object _objBooksLate;
    private object _objSheetsLate;
    private object _objSheetLate;
    private object _objRangeLate;

    private readonly Type _typeAppLate;
    private Type _typeBookLate;
    private readonly Type _typeBooksLate;
    private Type _typeSheetsLate;
    private Type _typeSheetLate;
    private Type _typeRangeLate;
    private Type _typeInteriorLate;
    private Type _typeFontLate;
    private Type _typeBorderLate;

    private readonly object[] _parameters = new object[1];
    private readonly object[] _parameters2 = new object[2];
    private int _error;

    private bool _numberFormatting;
    private bool _interiorFormatting;
    private bool _otherFormatting;

    /// <summary>
    /// Defeult ctor starting excel in late binding mode
    /// </summary>
    public XlsxLateBindingBuilder()
    {
        _numberFormatting = true;
        _interiorFormatting = true;
        _otherFormatting = true;

        try
        {
            // Get the class type and instantiate Excel.
            var objClassType = Type.GetTypeFromProgID("Excel.Application");

            if (objClassType is null)
            {
                throw new ArgumentNullException(nameof(objClassType));
            }


            _objAppLate = Activator.CreateInstance(objClassType);
            if (_objAppLate is null)
            {
                throw new ArgumentNullException(nameof(_objAppLate));
            }

            _typeAppLate = _objAppLate.GetType();


            //_parameters = new object[1];
            //_parameters[0] = true;
            //_typeAppLate.InvokeMember("Visible", BindingFlags.SetProperty,
            //    null, _objAppLate, _parameters);

            _parameters[0] = false;
            _typeAppLate.InvokeMember("ScreenUpdating", BindingFlags.SetProperty,
                null, _objAppLate, _parameters);


            //Get the workbooks collection.
            _objBooksLate = _typeAppLate.InvokeMember("Workbooks", BindingFlags.GetProperty, null, _objAppLate, null);
            if (_objBooksLate is null)
            {
                throw new ArgumentNullException(nameof(objClassType));
            }
            _typeBooksLate = _objBooksLate.GetType();

        }
        catch (Exception ex)
        {
            ExcelError(ex, null);
            _error = 1;
        }
    }

    /// <summary>
    /// Destructor
    /// </summary>
    ~XlsxLateBindingBuilder()
    {
        ReleaseUnmanagedResources();
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
    /// Border style for cells
    /// </summary>
    public BorderStyle Border { get; set; } = BorderStyle.None;

    /// <summary>
    /// Set background color for not elevated rows
    /// </summary>
    public int RowBackColor1 { get; set; } = -4142;

    /// <summary>
    /// Set background color for elevated rows
    /// </summary>
    public int RowBackColor2 { get; set; } = 34;

    /// <summary>
    /// Set background color
    /// </summary>
    public int BackColor { get; set; } = -4142;

    /// <summary>
    /// Font size in pt. Default 9pt
    /// </summary>
    public int FontSize { get; set; } = 9;

    /// <summary>
    /// Font name. Default = Aptos
    /// </summary>
    public string FontName { get; set; } = "Aptos";

    /// <summary>
    /// Numberformat for cells. Default: #,##0.00
    /// </summary>
    public string NumberFormat { get; set; } = "#,##0.00";

    /// <summary>
    /// Date format. Default = t/M/jjjj
    /// </summary>
    public string DateFormat { get; set; } = @"t/M/jjjj";

    /// <summary>
    /// Use bold font
    /// </summary>
    public bool Bold { get; set; }

    /// <summary>
    /// Use italic font
    /// </summary>
    public bool Italic { get; set; }

    /// <summary>
    /// Neue leere Mappe anlegen
    /// </summary>
    public void NewWorkbook()
    {
        if (_error != 0) return;

        try
        {
            //Add a new workbook
            _objBookLate = _typeBooksLate.InvokeMember("Add", BindingFlags.InvokeMethod, null, _objBooksLate, null);
            if (_objBookLate is null)
            {
                throw new ArgumentNullException(nameof(_objBookLate));
            }
            _typeBookLate = _objBookLate.GetType();

            //Get the worksheets collection
            _objSheetsLate = _typeBookLate.InvokeMember("Worksheets", BindingFlags.GetProperty, null, _objBookLate, null);
            if (_objSheetsLate is null)
            {
                throw new ArgumentNullException(nameof(_objSheetsLate));
            }
            _typeSheetsLate = _objSheetsLate.GetType();

            // Turn off calculation
            _parameters[0] = -4135;
            _typeAppLate.InvokeMember("Calculation", BindingFlags.SetProperty,
                null, _objAppLate, _parameters);

        }
        catch (Exception ex)
        {
            ExcelError(ex, null);

            _error = 2;
        }
    }

    /// <summary>
    /// Neue Mappe auf Basis einer Vorlage anlegen
    /// </summary>
    /// <param name="template"></param>
    public void NewWorkbook(string template)
    {
        if (_error != 0) return;

        try
        {
            //Add a new workbook.
            _objBookLate = _typeBooksLate.InvokeMember("Add", BindingFlags.InvokeMethod, null, _objBooksLate, null);
            if (_objBookLate is null)
            {
                throw new ArgumentNullException(nameof(_objBookLate));
            }
            _typeBookLate = _objBookLate.GetType();

            //Get the worksheets collection.
            _objSheetsLate = _typeBookLate.InvokeMember("Worksheets", BindingFlags.GetProperty, null, _objBookLate, null);
            if (_objSheetsLate is null)
            {
                throw new ArgumentNullException(nameof(_objSheetsLate));
            }
            _typeSheetsLate = _objSheetsLate.GetType();

            // Turn off calculation
            _parameters[0] = -4135;
            _typeAppLate.InvokeMember("Calculation", BindingFlags.SetProperty,
                null, _objAppLate, _parameters);
        }
        catch (Exception ex)
        {
            ExcelError(ex, template);

            _error = 3;
        }

    }

    /// <summary>
    /// Tabellenblatt über Index auswählen
    /// </summary>
    /// <param name="index">Indexzahl</param>
    public void SelectSheet(int index)
    {
        if (_error != 0) return;

        try
        {
            //Get the first worksheet.
            _parameters[0] = index;
            _objSheetLate = _typeSheetsLate.InvokeMember("Item", BindingFlags.GetProperty, null, _objSheetsLate, _parameters);
            if (_objSheetLate is null)
            {
                throw new ArgumentNullException(nameof(_objSheetLate));
            }
            _typeSheetLate = _objSheetLate.GetType();
        }
        catch (Exception ex)
        {
            ExcelError(ex, null);
            _error = 4;
        }

    }

    /// <summary>
    /// Tabellenblatt über Namen auswählen
    /// </summary>
    /// <param name="name">Tabellenname</param>
    public void SelectSheet(string name)
    {
        if (_error != 0) return;

        try
        {
            //Get the first worksheet.
            _parameters[0] = name;
            _objSheetLate = _typeSheetsLate.InvokeMember("Item", BindingFlags.GetProperty, null, _objSheetsLate, _parameters);
            if (_objSheetLate is null)
            {
                throw new ArgumentNullException(nameof(_objSheetLate));
            }
            _typeSheetLate = _objSheetLate.GetType();
        }
        catch (Exception ex)
        {
            ExcelError(ex, null);
            _error = 5;
        }

    }

    /// <summary>
    /// Wähle erstes Tabellenblatt und benenne es um
    /// </summary>
    /// <param name="name">Neuer Name für Tabellenblatt</param>
    public void SelectSheetFirst(string name)
    {
        if (_error != 0) return;

        try
        {
            //Get the first worksheet.
            _parameters[0] = 1;
            _objSheetLate = _typeSheetsLate.InvokeMember("Item", BindingFlags.GetProperty, null, _objSheetsLate, _parameters);
            if (_objSheetLate is null)
            {
                throw new ArgumentNullException(nameof(_objSheetLate));
            }
            _typeSheetLate = _objSheetLate.GetType();

            try
            {
                _parameters[0] = name;
                _typeSheetLate.InvokeMember("Name", BindingFlags.SetProperty, null, _objSheetLate, _parameters);
            }
            catch (Exception ex)
            {
                ExcelError(ex, null);
            }
        }
        catch (Exception ex)
        {
            ExcelError(ex, null);
            _error = 6;
        }

    }


    /// <summary>
    /// Neues Blatt anlegen
    /// </summary>
    /// <param name="name">Name der anzulegenden Tabelle</param>
    public void NewSheet(string name)
    {
        //if (_error != 0) return;

        _objSheetLate = _typeSheetsLate.InvokeMember("Add", BindingFlags.GetProperty, null, _objSheetsLate, null);
        if (_objSheetLate is null)
        {
            throw new ArgumentNullException(nameof(_objSheetLate));
        }
        _typeSheetLate = _objSheetLate.GetType();

        try
        {
            _parameters[0] = name;
            _typeSheetLate.InvokeMember("Name", BindingFlags.SetProperty, null, _objSheetLate, _parameters);
        }
        catch (Exception ex)
        {
            ExcelError(ex, null);
        }

    }

    /// <summary>
    /// Zellbereich auswählen
    /// </summary>
    /// <param name="a1Bezug"></param>
    public void SelectRange(string a1Bezug)
    {
        if (_error != 0)
        {
            return;
        }

        if (_typeSheetLate is null)
        {
            return;
        }

        //Get a range object .
        _parameters2[0] = a1Bezug;
        _parameters2[1] = Missing.Value;
        _objRangeLate = _typeSheetLate.InvokeMember("Range", BindingFlags.GetProperty, null, _objSheetLate, _parameters2);
        
        if (_objRangeLate is null)
        {
            throw new ArgumentNullException(nameof(_objSheetLate));
        }
        _typeRangeLate = _objRangeLate.GetType();

    }
    /// <summary>
    /// Zelle über R1C1 auswählen
    /// </summary>
    /// <param name="rowIndex"></param>
    /// <param name="colIndex"></param>
    public void SelectRange(int rowIndex, int colIndex)
    {
        if (_error != 0)
        {
            return;
        }

        if (_typeSheetLate is null)
        {
            return;
        }

        //Get a range object .
        _parameters2[0] = rowIndex;
        _parameters2[1] = colIndex;
        _objRangeLate = _typeSheetLate.InvokeMember("Cells", BindingFlags.GetProperty, null, _objSheetLate, _parameters2);

        if (_objRangeLate is null)
        {
            throw new ArgumentNullException(nameof(_objSheetLate));
        }
        _typeRangeLate = _objRangeLate.GetType();
    }

    /// <summary>
    /// Wert in eine Zelle einsetzen
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(string value)
    {
        if (_error != 0)
        {
            return;
        }

        if (_typeSheetLate is null)
        {
            return;
        }

        _parameters[0] = value;

        _typeRangeLate.InvokeMember(value.StartsWith("=") ? "FormulaLocal" : "Value", BindingFlags.SetProperty, null,
            _objRangeLate, _parameters);

        Format();
    }

    /// <summary>
    /// >Zahl in eine Zelle einsetzen
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(double value)
    {
        if (_error != 0)
        {
            return;
        }
        if (_typeSheetLate is null)
        {
            return;
        }

        _parameters[0] = value;
        _typeRangeLate.InvokeMember("Value", BindingFlags.SetProperty, null, _objRangeLate, _parameters);

        var alt = NumberFormat;
        Format();
        NumberFormat = alt;

    }

    /// <summary>
    /// >Zahl in eine Zelle einsetzen
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(long value)
    {
        if (_error != 0)
        {
            return;
        }

        if (_typeSheetLate is null)
        {
            return;
        }

        _parameters[0] = value;
        _typeRangeLate.InvokeMember("Value", BindingFlags.SetProperty, null, _objRangeLate, _parameters);

        var alt = NumberFormat;
        NumberFormat = "#,##0";
        Format();
        NumberFormat = alt;

    }


    ///// <summary>
    ///// >Zahl in eine Zelle einsetzen
    ///// </summary>
    ///// <param name="value"></param>
    //public void SetValue(int value)
    //{
    //    if (_error != 0) return;

    //    _parameters = new object[1];
    //    _parameters[0] = value;
    //    _objRangeLate.GetType().InvokeMember("Value", BindingFlags.SetProperty, null, _objRangeLate, _parameters);

    //    var alt = _numberFormat;
    //    _numberFormat = "#,##0";
    //    Format();
    //    _numberFormat = alt;

    //}

    /// <summary>
    /// >Zahl in eine Zelle einsetzen
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(int value)
    {
        if (_error != 0)
        {
            return;
        }

        if (_typeSheetLate is null)
        {
            return;
        }

        _parameters[0] = value;
        _typeRangeLate.InvokeMember("Value", BindingFlags.SetProperty, null, _objRangeLate, _parameters);

        var alt = NumberFormat;
        NumberFormat = "#,##0";
        Format();
        NumberFormat = alt;

    }


    /// <summary>
    /// >Zahl in eine Zelle einsetzen
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(bool value)
    {
        if (_error != 0)
        {
            return;
        }

        if (_typeSheetLate is null)
        {
            return;
        }

        _parameters[0] = value;
        _typeRangeLate.InvokeMember("Value", BindingFlags.SetProperty, null, _objRangeLate, _parameters);

        Format();
    }


    /// <summary>
    /// Datum in eine Zelle einsetzen
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(DateTime value)
    {

        if (_error != 0)
        {
            return;
        }

        if (_typeSheetLate is null)
        {
            return;
        }

        _parameters[0] = value;
        _typeRangeLate.InvokeMember("Value", BindingFlags.SetProperty, null, _objRangeLate, _parameters);

        var alt = NumberFormat;
        NumberFormat = DateFormat;
        Format();
        NumberFormat = alt;
    }




    /// <summary>
    /// Ausgewählten Zellbereich formatieren
    /// </summary>
    public void Format()
    {

        if (_error != 0)
        {
            return;
        }

        if (_typeSheetLate is null)
        {
            return;
        }

        if (_numberFormatting)
        {
            //_parameters = new object[1];
            _parameters[0] = NumberFormat;
            _typeRangeLate.InvokeMember("NumberFormat", BindingFlags.SetProperty, null, _objRangeLate, _parameters);
        }


        if (_interiorFormatting)
        {
            // Hintergrundfarbe einstellen
            var objInteriorLate = _typeRangeLate.InvokeMember("Interior", BindingFlags.GetProperty, null, _objRangeLate, null);
            if (objInteriorLate is null)
            {
                return;
            }

            _typeInteriorLate ??= objInteriorLate.GetType();

            //_parameters = new object[1];
            _parameters[0] = BackColor;
            _typeInteriorLate.InvokeMember("ColorIndex", BindingFlags.SetProperty, null, objInteriorLate, _parameters);


        }


        if (!_otherFormatting)
        {
            return;
        }

        // Schrifteinstellungen
        var objFontLate = _typeRangeLate.InvokeMember("Font", BindingFlags.GetProperty, null, _objRangeLate, null);
        if (objFontLate is null)
        {
            return;
        }

        _typeFontLate ??= objFontLate.GetType();

        //_parameters = new object[1];
        _parameters[0] = FontName;
        _typeFontLate.InvokeMember("Name", BindingFlags.SetProperty, null, objFontLate, _parameters);

        //_parameters = new object[1];
        _parameters[0] = FontSize;
        _typeFontLate.InvokeMember("Size", BindingFlags.SetProperty, null, objFontLate, _parameters);

        //_parameters = new object[1];
        _parameters[0] = Italic;
        _typeFontLate.InvokeMember("Italic", BindingFlags.SetProperty, null, objFontLate, _parameters);

        //_parameters = new object[1];
        _parameters[0] = Bold;
        _typeFontLate.InvokeMember("Bold", BindingFlags.SetProperty, null, objFontLate, _parameters);



        // Rand oben
        switch (Border)
        {
            case BorderStyle.All:
            case BorderStyle.Top:
                //_parameters = new object[1];
                _parameters[0] = 8;
                ShowBorder(_typeRangeLate.InvokeMember("Borders", BindingFlags.GetProperty,
                    null, _objRangeLate, _parameters));

                _parameters[0] = 9;
                ShowBorder(_typeRangeLate.InvokeMember("Borders", BindingFlags.GetProperty,
                    null, _objRangeLate, _parameters));

                _parameters[0] = 7;
                ShowBorder(_typeRangeLate.InvokeMember("Borders", BindingFlags.GetProperty,
                    null, _objRangeLate, _parameters));

                _parameters[0] = 10;
                ShowBorder(_typeRangeLate.InvokeMember("Borders", BindingFlags.GetProperty,
                    null, _objRangeLate, _parameters));

                _parameters[0] = 11;
                ShowBorder(_typeRangeLate.InvokeMember("Borders", BindingFlags.GetProperty,
                    null, _objRangeLate, _parameters));

                _parameters[0] = 12;
                ShowBorder(_typeRangeLate.InvokeMember("Borders", BindingFlags.GetProperty,
                    null, _objRangeLate, _parameters));

                break;
        }

        //// Rand unten
        //switch (_border)
        //{
        //    case BorderStyle.All:
        //    case BorderStyle.Down:
        //        //_parameters = new object[1];
        //        _parameters[0] = 9;
        //        ShowBorder(_typeRangeLate.InvokeMember("Borders", BindingFlags.GetProperty,
        //            null, _objRangeLate, _parameters));
        //        break;
        //}

        //// Rand links
        //switch (_border)
        //{
        //    case BorderStyle.All:
        //    case BorderStyle.Left:
        //        //_parameters = new object[1];
        //        _parameters[0] = 7;
        //        ShowBorder(_typeRangeLate.InvokeMember("Borders", BindingFlags.GetProperty,
        //            null, _objRangeLate, _parameters));
        //        break;
        //}

        //// Rand rechts
        //switch (_border)
        //{
        //    case BorderStyle.All:
        //    case BorderStyle.Right:
        //        //_parameters = new object[1];
        //        _parameters[0] = 10;
        //        ShowBorder(_typeRangeLate.InvokeMember("Borders", BindingFlags.GetProperty,
        //            null, _objRangeLate, _parameters));
        //        break;
        //}

    }

    /// <summary>
    /// Rahmen anzeigen
    /// </summary>
    /// <param name="objBorderLate"></param>
    private void ShowBorder(object objBorderLate)
    {
        try
        {

            _typeBorderLate ??= objBorderLate.GetType();

            //_parameters = new object[1];
            _parameters[0] = 1;
            _typeBorderLate.InvokeMember("LineStyle", BindingFlags.SetProperty,
                null, objBorderLate, _parameters);

            _parameters[0] = 2;
            _typeBorderLate.InvokeMember("Weight", BindingFlags.SetProperty,
                null, objBorderLate, _parameters);

            _parameters[0] = -4105;
            _typeBorderLate.InvokeMember("ColorIndex", BindingFlags.SetProperty,
                null, objBorderLate, _parameters);
        }
        catch (Exception ex)
        {
            ExcelError(ex, null);
        }

    }

    /// <summary>
    /// Automatisierung beenden und Kontrolle an Benutzer übergeben
    /// </summary>
    public void Quit()
    {
        try
        {
            //Return control of Excel to the user.
            //_parameters = new object[1];
            _parameters[0] = true;

            _typeAppLate.InvokeMember("Visible", BindingFlags.SetProperty,
                null, _objAppLate, _parameters);

            _typeAppLate.InvokeMember("ScreenUpdating", BindingFlags.SetProperty,
                null, _objAppLate, _parameters);

            _typeAppLate.InvokeMember("UserControl", BindingFlags.SetProperty,
                null, _objAppLate, _parameters);


        }
        catch (Exception ex)
        {
            ExcelError(ex, null);
        }
    }

    /// <summary>
    /// Formatierungen auf Standardwerte setzen
    /// </summary>
    public void SetToDefault()
    {
        FontSize = 9;
        FontName = "Aptos";
        NumberFormat = "#,##0.00";
        DateFormat = "m/d/yyyy";
        Border = BorderStyle.None;
        Bold = false;
        Italic = false;
    }

    /// <summary>
    /// Datentabelle anzeigen
    /// </summary>
    /// <param name="dt">DataTable mit anzuzeigenden Daten</param>
    /// <param name="rowIndex">Zeilennummer der linken oberen Ecke</param>
    /// <param name="colIndex">Spaltennummer der linken oberen Eck</param>
    public void FillDataTable(DataTable dt, int rowIndex, int colIndex)
    {
        try
        {
            if (_error != 0)
            {
                return;
            }

            var parameters = new object[2];
            var row = rowIndex;
            var col = colIndex;
            var high = false;

            Border = BorderStyle.All;

            _numberFormatting = true;
            _interiorFormatting = true;
            _otherFormatting = true;

            Bold = true;
            ExcelStatus("Schreibe Kopfzeile...");
            foreach (DataColumn d in dt.Columns)
            {
                SelectRange(row, col);
                SetValue(d.ColumnName);
                col++;
            }
            row++;


            var colCount = dt.Columns.Count;
            var datatypes = new string[colCount];

            for (var c = 0; c < colCount; c++)
            {
                datatypes[c] = dt.Columns[c].DataType.Name.ToLower();
            }


            var ordinals = new int[colCount];

            for (var c = 0; c < colCount; c++)
            {
                ordinals[c] = dt.Columns[c].Ordinal;
            }

            var anzRows = dt.Rows.Count;

            Bold = false;
            _otherFormatting = false;
            for (var i = 0; i < anzRows; i++)
            {

                _numberFormatting = true;
                _interiorFormatting = true;

                var r = dt.Rows[i];

                if (i % 20 < 0.01)
                {
                    ExcelStatus($"Writing row {i}...");
                }

                if (high)
                {
                    BackColor = RowBackColor2;
                    high = false;
                }
                else
                {
                    BackColor = RowBackColor1;
                    high = true;
                }


                for (var c = 0; c < colCount; c++)
                {

                    SelectRange(row + i, colIndex + c);

                    var value1 = r[ordinals[c]].ToString();

                    if (string.IsNullOrEmpty(value1)) continue;

                    switch (datatypes[c])
                    {
                        case "datetime":
                            SetValue(Convert.ToDateTime(value1));
                            break;
                        case "boolean":
                            SetValue(Convert.ToBoolean(value1));
                            break;
                        case "single":
                        case "double":
                        case "decimal":
                            var value = Convert.ToDouble(value1);
                            SetValue(value);
                            break;
                        case "int":
                        case "int32":
                        case "byte":
                        case "int16":
                        case "int64":
                            SetValue(Convert.ToInt64(value1));
                            break;
                        default:
                            SetValue(value1);
                            break;
                    }
                }

                // Background

                _parameters2[0] = row + i;
                _parameters2[1] = colIndex;
                parameters[0] = _typeSheetLate.InvokeMember("Cells", BindingFlags.GetProperty, null, _objSheetLate,
                    _parameters2);

                _parameters2[0] = row + 1;
                _parameters2[1] = colIndex + colCount-1;
                parameters[1] = _typeSheetLate.InvokeMember("Cells", BindingFlags.GetProperty, null, _objSheetLate,
                    _parameters2);


                _objRangeLate = _typeSheetLate.InvokeMember("Range", BindingFlags.GetProperty, null, _objSheetLate,
                    parameters);

                _numberFormatting = false;
                _interiorFormatting = true;
            }




            _parameters2[0] = rowIndex + 1;
            _parameters2[1] = colIndex;
            parameters[0] = _typeSheetLate.InvokeMember("Cells", BindingFlags.GetProperty, null, _objSheetLate,
                _parameters2);

            _parameters2[0] = rowIndex + anzRows;
            _parameters2[1] = colIndex + colCount -1;
            parameters[1] = _typeSheetLate.InvokeMember("Cells", BindingFlags.GetProperty, null, _objSheetLate,
                _parameters2);


            _objRangeLate = _typeSheetLate.InvokeMember("Range", BindingFlags.GetProperty, null, _objSheetLate,
                parameters);


            _numberFormatting = false;
            _interiorFormatting = false;
            _otherFormatting = true;
            Format();




            //switch (_border)
            //{
            //    case BorderStyle.All:
            //    case BorderStyle.Top:
            //        //_parameters = new object[1];
            //        _parameters[0] = 8;
            //        ShowBorder(_typeRangeLate.InvokeMember("Borders", BindingFlags.GetProperty,
            //            null, _objRangeLate, _parameters));

            //        _parameters[0] = 9;
            //        ShowBorder(_typeRangeLate.InvokeMember("Borders", BindingFlags.GetProperty,
            //            null, _objRangeLate, _parameters));

            //        _parameters[0] = 7;
            //        ShowBorder(_typeRangeLate.InvokeMember("Borders", BindingFlags.GetProperty,
            //            null, _objRangeLate, _parameters));

            //        _parameters[0] = 10;
            //        ShowBorder(_typeRangeLate.InvokeMember("Borders", BindingFlags.GetProperty,
            //            null, _objRangeLate, _parameters));



            //        break;
            //}



            AutoFitColumns();

            _parameters[0] = -4105;
            _typeAppLate.InvokeMember("Calculation", BindingFlags.SetProperty,
                null, _objAppLate, _parameters);
        }
        catch (Exception ex)
        {
            ExcelError(ex, null);
            _error = 55;

        }
        finally
        {
            _numberFormatting = true;
            _interiorFormatting = true;
            _otherFormatting = true;
        }
    }



    /// <summary>
    /// Spaltenbreite automatisch anpassen
    /// </summary>
    public void AutoFitColumns()
    {
        var objColumnsLate = _typeSheetLate.InvokeMember("Columns", BindingFlags.GetProperty, null, _objSheetLate, null);
        objColumnsLate?.GetType().InvokeMember("Autofit", BindingFlags.InvokeMethod, null, objColumnsLate, null);
    }

    /// <summary>
    /// Datei speichern als 
    /// </summary>
    /// <param name="fileName"></param>
    public void Save(string fileName)
    {

        var parameters = new object[12];
        parameters[0] = fileName;
        parameters[1] = Missing.Value;
        parameters[2] = Missing.Value;
        parameters[3] = Missing.Value;
        parameters[4] = Missing.Value;
        parameters[5] = Missing.Value;
        parameters[6] = Missing.Value;
        parameters[7] = Missing.Value;
        parameters[8] = Missing.Value;
        parameters[9] = Missing.Value;
        parameters[10] = Missing.Value;
        parameters[11] = Missing.Value;

        try
        {
            _typeBookLate.InvokeMember("SaveAs", BindingFlags.InvokeMethod, null, _objBookLate, parameters);
        }
        catch (Exception ex)
        {
            ExcelError(ex, fileName);
        }
    }

    /// <summary>
    /// Statusanzeige
    /// </summary>
    /// <param name="message">Nachricht für Statusanzeige</param>
    public void ExcelStatus(string message)
    {
        var helper = Status;
        helper?.Invoke(message);
    }

    /// <summary>
    /// Fehleranzeige
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
        FontSize = 14;
        Bold = true;
        SelectRange(1, 1);
        SetValue(title);

        SetToDefault();
        SelectRange(2, 1);
        SetValue(DateTime.Now.ToString("dd.MM.yyyy"));
    }

    //private static ADODB.Recordset ConvertToRecordset(DataTable inTable)
    //{
    //    var result = new ADODB.Recordset {CursorLocation = ADODB.CursorLocationEnum.adUseClient};

    //    var resultFields = result.Fields;
    //    var inColumns = inTable.Columns;

    //    foreach (DataColumn inColumn in inColumns)
    //    {
    //        resultFields.Append(inColumn.ColumnName
    //            , TranslateType(inColumn.DataType)
    //            , inColumn.MaxLength
    //            , inColumn.AllowDBNull ? ADODB.FieldAttributeEnum.adFldIsNullable :
    //                                     ADODB.FieldAttributeEnum.adFldUnspecified
    //            , null);
    //    }

    //    result.Open(Missing.Value
    //            , Missing.Value
    //            , ADODB.CursorTypeEnum.adOpenStatic
    //            , ADODB.LockTypeEnum.adLockOptimistic, 0);

    //    foreach (DataRow dr in inTable.Rows)
    //    {
    //        result.AddNew(Missing.Value,
    //                      Missing.Value);

    //        for (var columnIndex = 0; columnIndex < inColumns.Count; columnIndex++)
    //        {
    //            resultFields[columnIndex].Value = dr[columnIndex];
    //        }
    //    }

    //    return result;
    //}


    //private static ADODB.DataTypeEnum TranslateType(Type columnType)
    //{
    //    switch (columnType.UnderlyingSystemType.ToString())
    //    {
    //        case "System.Boolean":
    //            return ADODB.DataTypeEnum.adBoolean;

    //        case "System.Byte":
    //            return ADODB.DataTypeEnum.adUnsignedTinyInt;

    //        case "System.Char":
    //            return ADODB.DataTypeEnum.adChar;

    //        case "System.DateTime":
    //            return ADODB.DataTypeEnum.adDate;

    //        case "System.Decimal":
    //            return ADODB.DataTypeEnum.adCurrency;

    //        case "System.Double":
    //            return ADODB.DataTypeEnum.adDouble;

    //        case "System.Int16":
    //            return ADODB.DataTypeEnum.adSmallInt;

    //        case "System.Int32":
    //            return ADODB.DataTypeEnum.adInteger;

    //        case "System.Int64":
    //            return ADODB.DataTypeEnum.adBigInt;

    //        case "System.SByte":
    //            return ADODB.DataTypeEnum.adTinyInt;

    //        case "System.Single":
    //            return ADODB.DataTypeEnum.adSingle;

    //        case "System.UInt16":
    //            return ADODB.DataTypeEnum.adUnsignedSmallInt;

    //        case "System.UInt32":
    //            return ADODB.DataTypeEnum.adUnsignedInt;

    //        case "System.UInt64":
    //            return ADODB.DataTypeEnum.adUnsignedBigInt;

    //        case "System.String":
    //        default:
    //            return ADODB.DataTypeEnum.adVarChar;
    //    }
    //}

    private void ReleaseUnmanagedResources()
    {
        Quit();

        _objRangeLate = null;
        _objSheetLate = null;
        _objSheetsLate = null;
        _objBookLate = null;
        _objBooksLate = null;
        _objAppLate = null;
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }
}
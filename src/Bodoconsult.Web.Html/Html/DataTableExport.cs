// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Data;
using System.IO;
using System.Text;

namespace Bodoconsult.Web.Html.Html;

/// <summary>
/// Export a <see cref="DataTable"/> instance to HTML
/// </summary>
public class DataTableExport
{
    private const string TableElement = "<table{0}>{1}</table>";
    private const string ThElement = "<th{0}>{1}</th>";
    private const string TrElement = "<tr{0}>{1}</tr>";
    private const string TdElement = "<td{0}>{1}</td>";

    private int _start;

    private string[] _formats;

    private readonly StringBuilder _content = new();

    /// <summary>
    /// The data to export as HTML
    /// </summary>
    public DataTable Data { get; set; }

    /// <summary>
    /// CSS to use for table element
    /// </summary>
    public string CssTable { get; set; }

    /// <summary>
    /// CSS to use for table header element
    /// </summary>
    public string CssTableHeader { get; set; }

    /// <summary>
    /// CSS to use for table row element
    /// </summary>
    public string CssTableRow { get; set; }

    /// <summary>
    /// CSS to use for table cell element
    /// </summary>
    public string CssTableCell { get; set; }

    /// <summary>
    /// CSS to use for alternating table rows
    /// </summary>
    public string CssAlternatingRows { get; set; }

    /// <summary>
    /// Create the table as HTML
    /// </summary>
    public void CreateTable()
    {
        CreateFormats();

        CreateHeader();

        CreateBody();
    }

    private void CreateFormats()
    {
        _formats = new string[Data.Columns.Count];

        var i = 0;

        foreach (DataColumn col in Data.Columns)
        {

            if (col.ColumnName.Equals("cssstyle", StringComparison.OrdinalIgnoreCase))
            {
                _start = 1;
                continue;
            }

            var t = col.DataType;

            switch (t.Name.ToLowerInvariant().Replace("system.", string.Empty))
            {
                case "byte":
                case "int":
                case "int16":
                case "int32":
                case "int64":
                    _formats[i] = "#,##0";
                    break;
                case "single":
                case "double":
                case "float":
                case "decimal":
                    _formats[i] = "#,##0.00";
                    break;
                case "datetime":
                    _formats[i] = "dd.MM.yyyy";
                    break;
                default:
                    _formats[i] = string.Empty;
                    break;
            }

            i++;
        }
    }

    private void CreateBody()
    {
        var count = Data.Columns.Count;
        var alternating = false;
        var altCssTable = string.Empty;
        var r = new StringBuilder();

        foreach (DataRow row in Data.Rows)
        {
            r.Clear();
                
            var cssRow =    string.IsNullOrEmpty(CssTableRow) ? string.Empty : $" class=\"{CssTableRow}\"";

            var cssTableCell = _start == 1 && !string.IsNullOrEmpty(row[0].ToString())
                ? row[0].ToString()
                : CssTableCell;

            if (_start == 1 && !string.IsNullOrEmpty(row[0].ToString()) && cssTableCell != altCssTable)
            {
                alternating = true;
            }

            var cssCell = string.Empty;
                
                
            for (var j = _start; j < count; j++)
            {
                var value = row[j].ToString();

                var t = Data.Columns[j].DataType.Name.ToLower().Replace("system.", string.Empty);

                if (string.IsNullOrEmpty(value))
                {
                    value = "&nbsp;";
                    t = "string";
                }

                switch (t)
                {
                    case "datetime":
                        cssCell = string.IsNullOrEmpty(CssTableCell) ? string.Empty : $" class=\"{cssTableCell}_center{(alternating && cssTableCell == CssTableCell ? CssAlternatingRows : string.Empty)}\"";
                        value = Convert.ToDateTime(value).ToString(@"dd.MM.yyyy");
                        break;
                    case "decimal":
                    case "double":
                    case "single":
                        //column.Format.Alignment = ParagraphAlignment.Right;
                        value = Convert.ToDouble(value).ToString("#,##0.00");
                        cssCell = string.IsNullOrEmpty(cssCell) ? string.Empty : $" class=\"{cssTableCell}_right{(alternating && cssTableCell == CssTableCell ? CssAlternatingRows : string.Empty)}\"";
                        break;
                    case "int":
                    case "int16":
                    case "int32":
                    case "int64":
                        //column.Format.Alignment = ParagraphAlignment.Right;
                        value = Convert.ToInt32(value).ToString(@"#,##0");
                        cssCell = string.IsNullOrEmpty(CssTableCell) ? string.Empty : $" class=\"{cssTableCell}_right{(alternating && cssTableCell == CssTableCell ? CssAlternatingRows : string.Empty)}\"";
                        break;
                    default:
                        cssCell = string.IsNullOrEmpty(CssTableCell) ? string.Empty : $" class=\"{cssTableCell}{(alternating && cssTableCell == CssTableCell ? CssAlternatingRows : string.Empty)}\"";
                        break;
                }

                r.Append(string.Format(TdElement, cssCell, value));
            }

            _content.AppendFormat(TrElement, cssRow, r);
            alternating = !alternating;
            altCssTable = cssTableCell;
        }
    }


    private void CreateHeader()
    {
        var css = string.IsNullOrEmpty(CssTableHeader) ? string.Empty : $" class=\"{CssTableHeader}\"";

        var s = new StringBuilder();

        foreach (DataColumn col in Data.Columns)
        {
            if (col.ColumnName.Equals("cssstyle", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            s.AppendFormat(ThElement, css, col.ColumnName);
        }

        _content.AppendFormat(TrElement, CssTableRow, s);
    }

    /// <summary>
    /// Get the HTML of the table
    /// </summary>
    public string Result
    {
        get
        {
            var css = string.IsNullOrEmpty(CssTable) ? string.Empty : $" class=\"{CssTable}\"";
            var erg = string.Format(TableElement, css, _content);
            return erg;

        }
    }

    /// <summary>
    /// Save the HTML as file
    /// </summary>
    /// <param name="fileName">Filename for the table as HTML file</param>
    public void SaveAsFile(string fileName)
    {
        var sw = new StreamWriter(fileName, false, Encoding.UTF8);
        sw.Write(Result);
        sw.Close();
    }
}
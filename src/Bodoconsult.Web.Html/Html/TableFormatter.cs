// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Data;
using System.Globalization;
using System.Text;
using Bodoconsult.Web.Html.HtmlTables;

namespace Bodoconsult.Web.Html.Html;

/// <summary>
/// Format a table
/// </summary>
public static class TableFormatter
{
    /// <summary>
    /// Cultur to use for table formatting
    /// </summary>
    public static CultureInfo Culture { get; set; } = new("en-us");

    /// <summary>
    /// Format as CSV text
    /// </summary>
    /// <param name="dataTable">Data to format</param>
    /// <param name="header">True if header with column names should be shown else false</param>
    /// <returns>CSV formatted string</returns>
    public static string FormatAsCsv(DataTable dataTable, bool header)
    {
        var erg = new StringBuilder();
        erg.Append(string.Empty);

        if (header)
        {
            foreach (DataColumn column in dataTable.Columns)
            {
                erg.Append($"{column.ColumnName};");
            }

            //if (erg.EndsWith(";")) erg = erg.Substring(0, erg.Length - 1);

            erg.Append("\r\n");
        }

        foreach (DataRow row in dataTable.Rows)
        {
            foreach (DataColumn column in dataTable.Columns)
            {
                erg.Append($"{FormattedValue(column.DataType.Name.ToLower(), row[column.ColumnName].ToString())};");
            }

            //if (rowdata.EndsWith(";")) rowdata = rowdata.Substring(0, rowdata.Length - 1);

            erg.Append("\r\n");
        }

        return erg.ToString();
    }

    /// <summary>
    /// Format as HTML text
    /// </summary>
    /// <param name="dataTable">Data to format</param>
    /// <param name="header">True if header with column names should be shown else false</param>
    /// <param name="style">Stylesheet</param>
    /// <returns>HTML formatted string</returns>
    public static string FormatAsHtml(DataTable dataTable, bool header, Stylesheet style)
    {
        var table= new Table();

        if (style != null)
        {
            table.Stylesheet = style;
        }

        if (header)
        {
            var headerRow = new TableRow(table);

            foreach (DataColumn column in dataTable.Columns)
            {
                // ReSharper disable once ObjectCreationAsStatement
                new TableHeaderCell(headerRow) { Text = column.ColumnName };
            }
        }

        foreach (DataRow row in dataTable.Rows)
        {
            var tableRow = new TableRow(table);

            foreach (DataColumn column in dataTable.Columns)
            {
                // ReSharper disable once ObjectCreationAsStatement
                new TableCell(tableRow) { Text = row[column.ColumnName].ToString() };
            }
        }

        return table.RenderIt();
    }

    /// <summary>
    /// Format as HTML text
    /// </summary>
    /// <param name="dataTable">Data to format</param>
    /// <param name="header">True if header with column names should be shown else false</param>
    /// <returns>HTML formatted string</returns>
    public static string FormatAsHtml(DataTable dataTable, bool header)
    {
        return FormatAsHtml(dataTable, header, null);
    }

    private static string FormattedValue(string type, string value)
    {
        switch (type)
        {
            case "double":
            case "real":
            case "float":
            case "decimal":
                double.TryParse(value, NumberStyles.Any, Culture, out var number1);
                return string.Format(Culture, "{0:0.000000000000}", number1);
            case "tinyint":
            case "smallint":
            case "int":
            case "int16":
            case "int32":
            case "int64":
            case "bigint":
                long.TryParse(value, NumberStyles.Any, Culture, out var number2);
                return string.Format(Culture, "{0}", number2);
            case "smalldatetime":
            case "datetime":
                DateTime.TryParse(value, Culture, DateTimeStyles.AssumeUniversal, out var date);
                return string.Format(Culture, "{0:f}", date);
            default:
                return value;
        }
    }
}
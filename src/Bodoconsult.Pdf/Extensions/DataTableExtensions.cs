// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Pdf.PdfSharp;
using MigraDoc.DocumentObjectModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading;

namespace Bodoconsult.Pdf.Extensions;

/// <summary>
/// Extension methods for <see cref="DataTable"/> instances
/// </summary>
public static class DataTableExtensions
{

    /// <summary>
    /// Enumerate all rows
    /// </summary>
    /// <param name="table">Current data table</param>
    /// <returns>Enumerable list of rows</returns>
    public static IEnumerable<DataRow> EnumerateRows(this DataTable table)
    {
        foreach (DataRow row in table.Rows)
        {
            yield return row;
        }
    }

    /// <summary>
    /// Enumerate all colums
    /// </summary>
    /// <param name="table">Current data table</param>
    /// <returns>Enumerable list of columns</returns>
    public static IEnumerable<DataColumn> EnumerateColumns(this DataTable table)
    {
        foreach (DataColumn column in table.Columns)
        {
            yield return column;
        }
    }

    /// <summary>
    /// Convert a <see cref="DataTable"/> to a <see cref="PdfTable"/>
    /// </summary>
    /// <param name="dt">Current <see cref="DataTable"/> to convert</param>
    /// <returns>Resulting <see cref="PdfTable"/> instance</returns>
    public static PdfTable ToPdfTable(this DataTable dt)
    {
        const int startIndex = 0;
        var result = GetPdfTable(dt, startIndex, Thread.CurrentThread.CurrentCulture, new Dictionary<string, Color>());
        return result;
    }

    /// <summary>
    /// Convert a <see cref="DataTable"/> to a <see cref="PdfTable"/>
    /// </summary>
    /// <param name="dt">Current <see cref="DataTable"/> to convert</param>
    /// <param name="cultureInfo">Current culture info</param>
    /// <returns>Resulting <see cref="PdfTable"/> instance</returns>
    public static PdfTable ToPdfTable(this DataTable dt, CultureInfo cultureInfo)
    {
        const int startIndex = 0;
        var result = GetPdfTable(dt, startIndex, cultureInfo, new Dictionary<string, Color>());
        return result;
    }

    /// <summary>
    /// Convert a <see cref="DataTable"/> to a <see cref="PdfTable"/>
    /// </summary>
    /// <param name="dt">Current <see cref="DataTable"/> to convert. First column should be named CssStyle</param>
    /// <param name="cssColors">Color translation table. If first column is named CssStyle the style name is translated to a color with this table</param>
    /// <returns>Resulting <see cref="PdfTable"/> instance</returns>
    public static PdfTable ToPdfTableWithCssInfo(this DataTable dt, Dictionary<string, Color> cssColors)
    {
        const int startIndex = 1;
        var result = GetPdfTable(dt, startIndex, Thread.CurrentThread.CurrentCulture, cssColors);
        return result;
    }

    /// <summary>
    /// Convert a <see cref="DataTable"/> to a <see cref="PdfTable"/>
    /// </summary>
    /// <param name="dt">Current <see cref="DataTable"/> to convert. First column should be named CssStyle</param>
    /// <param name="cssColors">Color translation table. If first column is named CssStyle the style name is translated to a color with this table</param>
    /// <param name="cultureInfo">Current culture info</param>
    /// <returns>Resulting <see cref="PdfTable"/> instance</returns>
    public static PdfTable ToPdfTableWithCssInfo(this DataTable dt, Dictionary<string, Color> cssColors, CultureInfo cultureInfo)
    {
        const int startIndex = 1;
        var result = GetPdfTable(dt, startIndex, cultureInfo, cssColors);
        return result;
    }

    private static PdfTable GetPdfTable(DataTable dt, int startIndex, CultureInfo cultureInfo,
        Dictionary<string, Color> cssColors)
    {
        var result = new PdfTable();

        List<Type> columnTypes = [];

        for (var index = 0; index < dt.Columns.Count; index++)
        {
            var column = dt.Columns[index];
            columnTypes.Add(column.DataType);

            if (index < startIndex)
            {
                continue;
            }

            var col = new PdfColumn(column.ColumnName)
            {
                TextAlignment = GetAlignment(column.DataType),
                MaxLength = column.ColumnName.Length,
            };

            result.Columns.Add(col);
        }

        for (var i = 0; i < dt.Rows.Count; i++)
        {
            var dataRow = dt.Rows[i];
            var row = new PdfRow();
            var dataCells = dataRow.ItemArray;

            if (startIndex == 1)
            {
                var css = dataCells[0]?.ToString() ?? string.Empty;
                if (cssColors.TryGetValue(css, out var color))
                {
                    row.ShadingColor = color;
                }
            }

            for (var index = 0; index < dataCells.Length; index++)
            {
                if (index < startIndex)
                {
                    continue;
                }

                var column = result.Columns[index - startIndex];
                var dataCell = dataCells[index];
                

                var type = columnTypes[index];

                var value = GetValue(dataCell, type, cultureInfo);
                if (value.Length > column.MaxLength)
                {
                    column.MaxLength = value.Length;
                }

                row.Cells.Add(new PdfCell(value));
            }

            result.Rows.Add(row);
        }

        return result;
    }

    private static string GetValue(object dataCell, Type type, CultureInfo cultureInfo)
    {
        if (dataCell is null)
        {
            return String.Empty;
        }

        var t = type.Name.Replace("System.", string.Empty).ToLower();
        return t switch
        {
            "datetime" => Convert.ToDateTime(dataCell).ToString("d", cultureInfo),
            "decimal" or "double" or "single" => Convert.ToDouble(dataCell).ToString("#,##0.00", cultureInfo),
            "int" or "int16" or "int32" or "int64" => Convert.ToInt64(dataCell).ToString("#,##0", cultureInfo),
            _ => dataCell.ToString()
        };
    }

    /// <summary>
    /// CSS color settings for Bodoconsult website
    /// </summary>
    public static Dictionary<string, Color> BodoconsultCssColors => new Dictionary<string, Color>
    {
        { "wr_cell_h1", Colors.GreenYellow },
        { "wr_cell_h2", Colors.YellowGreen },
        { "wr_cell_h3", Colors.Gold },
        { "wr_cell_risk1", Colors.Red },
        { "wr_cell_risk2", Colors.Orange }
    };

    /// <summary>
    /// Get the alignment for a datatype
    /// </summary>
    /// <param name="type">Datatype</param>
    /// <returns>Alignment</returns>
    public static PdfTextAlignment GetAlignment(Type type)
    {
        // Right aligned
        if (type == typeof(double) || type == typeof(float) ||
            type == typeof(short) || type == typeof(int) ||
            type == typeof(long) || type == typeof(Int128) ||
            type == typeof(byte))
        {
            return PdfTextAlignment.Right;
        }

        // Centered aligned
        if (type == typeof(bool) || type == typeof(DateTime))
        {
            return PdfTextAlignment.Center;
        }

        // Default: left aligned
        return PdfTextAlignment.Left;
    }
}
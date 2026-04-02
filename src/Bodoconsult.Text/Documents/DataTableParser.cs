// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Data;

namespace Bodoconsult.Text.Documents;

/// <summary>
/// Helper class to parse a <see cref="DataTable"/> instance in a LDML <see cref="Table"/> instance
/// </summary>
public class DataTableParser
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="dataTable">Current <see cref="DataTable"/> to parse</param>
    public DataTableParser(DataTable dataTable)
    {
        DataTable = dataTable ?? throw new ArgumentNullException(nameof(dataTable));
        Table = new Table();
    }

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="dataTable">Current <see cref="DataTable"/> to parse</param>
    /// <param name="legend"></param>
    public DataTableParser(DataTable dataTable, string legend)
    {
        DataTable = dataTable ?? throw new ArgumentNullException(nameof(dataTable));
        Table = new Table(legend);
    }



    /// <summary>
    /// Current <see cref="DataTable"/> to parse
    /// </summary>
    public DataTable DataTable  { get;  }


    /// <summary>
    /// The <see cref="Table"/> instance as result
    /// </summary>
    public Table Table { get; set; }

    /// <summary>
    /// Parse the columns of the data table
    /// </summary>
    public void ParseColumns()
    {
        foreach (DataColumn column in DataTable.Columns)
        {
            var col = new Column
            {
                Name = column.ColumnName,
                DataType = column.DataType,
                Format = GetFormat(column.DataType),
                Parent = Table
            };

            Table.AddBlock(col);
        }
    }

    private string GetFormat(Type type)
    {
        var dt = type.ToString().ToLower().Replace("system.", string.Empty);

        return dt switch
        {
            "decimal" or "double" or "single" => "#,##0.00",
            "datetime" or "timespan" => "dd.mm.yyyy",
            "byte" or "int16" or "int32" or "int64" or "uint16" or "uint32" or "uint64" => "#,##0",
            _ => null
        };
    }

    /// <summary>
    /// Parse the columns of the data table
    /// </summary>
    public void ParseRows()
    {
        foreach (DataRow row in DataTable.Rows)
        {
            var newRow = new Row();

            foreach (var column in Table.Columns)
            {
                var value = row[column.Name].ToString();

                var cell = new Cell(value)
                {
                    Parent = newRow
                };

                newRow.AddBlock(cell);
            }

            newRow.Parent = Table;

            Table.AddBlock(newRow);
        }
    }
}
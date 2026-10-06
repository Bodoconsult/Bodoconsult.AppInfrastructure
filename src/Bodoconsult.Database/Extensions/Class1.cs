// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Collections.Generic;
using System.Data;

namespace Bodoconsult.Database.Extensions;

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
}

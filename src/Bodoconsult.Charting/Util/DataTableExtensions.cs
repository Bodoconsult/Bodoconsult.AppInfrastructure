// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Data;

namespace Bodoconsult.Charting.Util;

/// <summary>
/// Extensions for <see cref="DataTable"/> objects
/// </summary>
public static class DataTableExtensions
{
    /// <summary>
    /// Convert <see cref="DataTable"/> to a generic <see cref="IList{DataRow}"/>
    /// </summary>
    /// <param name="datatable">data table to convert</param>
    /// <returns>List with <see cref="DataRow"/> objects</returns>
    public static IReadOnlyList<DataRow> ToGenericList(this DataTable datatable)
    {
        return (from row in datatable.AsEnumerable()
            select row).ToArray();
    }

}
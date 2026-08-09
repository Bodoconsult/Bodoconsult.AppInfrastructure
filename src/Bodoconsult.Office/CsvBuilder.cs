// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Data;
using System.Text;

namespace Bodoconsult.Office;

/// <summary>
/// Class representing a CSV file. Use it to export database data to a CSV file
/// </summary>
public class CsvBuilder
{

    private readonly StringBuilder _erg = new();

    private int _columnCount;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="data">Data to write to the CSV file</param>
    public CsvBuilder(DataTable data)
    {
        Data = data;
    }

    /// <summary>
    /// Add a header: true/false. Default true;
    /// </summary>
    public bool Header { get; set; } = true;

    /// <summary>
    /// Char(s) used as line separator. Default: \r\n
    /// </summary>
    public string LineSeparator { get; set; } = "\r\n";

    /// <summary>
    /// Char used as field separator. Default: semicolon (;)
    /// </summary>
    public string FieldSeparator { get; set; } = ";";

    /// <summary>
    /// Data to write in the CSV file
    /// </summary>
    public DataTable Data { get; }

    /// <summary>
    /// Full filename
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// Export the data table
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <see cref="Data"/> is null</exception>
    public void Export()
    {

        if (Data is null)
        {
            throw new ArgumentNullException(nameof(Data));
        }

        _columnCount = Data.Columns.Count - 1;

        if (Header)
        {
            var i = 0;

            foreach (DataColumn f in Data.Columns)
            {
                _erg.Append(f.ColumnName + (i < _columnCount ? FieldSeparator : string.Empty));
                i++;
            }

            _erg.Append(LineSeparator);
        }


        foreach (DataRow r in Data.Rows)
        {
            var i = 0;

            foreach (DataColumn f in Data.Columns)
            {
                _erg.Append(r[f.ColumnName] + (i < _columnCount ? FieldSeparator : string.Empty));
                i++;
            }

            _erg.Append(LineSeparator);
        }

        var sw = new StreamWriter(FileName, false, Encoding.GetEncoding("utf-8"));
        sw.Write(_erg.ToString());
        sw.Close();
        sw.Dispose();

    }

    // ToDo: Document class
}

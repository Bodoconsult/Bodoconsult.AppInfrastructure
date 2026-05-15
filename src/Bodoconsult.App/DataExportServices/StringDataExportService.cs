// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.DataExportServices;

/// <summary>
/// Data export service for UTF8 strings
/// </summary>
public class StringDataExportService : BaseDataExportService<string>
{
    /// <summary>
    /// Converts an object of type T into a ReadOnlyMemory&lt;byte&gt; instance
    /// </summary>
    /// <param name="data">Data object to serialize</param>
    /// <returns>Byte array</returns>
    /// <exception cref="NotSupportedException">Thrown if type T is NOT string, ReadOnlyMemory&lt;byte&gt; or byte[]</exception>
    public override ReadOnlyMemory<byte> ToMemory(string data)
    {
        data ??= string.Empty;

        var arr = Encoding.GetBytes(data);
        return arr;
    }
}
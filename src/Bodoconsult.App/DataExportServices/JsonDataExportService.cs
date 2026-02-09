// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Text;

namespace Bodoconsult.App.DataExportServices;

/// <summary>
/// Data export service for JSON
/// </summary>
public class JsonDataExportService<T> : DataExportServiceBase<T> where T : class
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public JsonDataExportService()
    {
        HeaderData = Encoding.UTF8.GetBytes("[");
        FooterData = Encoding.UTF8.GetBytes("]");
        TokenSeparatorData = Encoding.UTF8.GetBytes(",");
    }

    /// <summary>
    /// Converts an object of type T into a ReadOnlyMemory&lt;byte&gt; instance
    /// </summary>
    /// <param name="data">Data object to serialize</param>
    /// <returns>Byte array</returns>
    public override ReadOnlyMemory<byte> ToMemory(T data)
    {
        var s = System.Text.Json.JsonSerializer.Serialize(data);
        var b = Encoding.UTF8.GetBytes(s);
        return b;
    }
}
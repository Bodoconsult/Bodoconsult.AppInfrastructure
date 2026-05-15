// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Text;
using System.Text.Json;

namespace Bodoconsult.App.DataExportServices;

/// <summary>
/// Data export service for JSON
/// </summary>
public class JsonDataExportService<T> : BaseDataExportService<T> where T : class
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public JsonDataExportService()
    {
        HeaderData = Encoding.GetBytes("[");
        FooterData = Encoding.GetBytes("]");
        TokenSeparatorData = Encoding.GetBytes(",");
    }

    /// <summary>
    /// Ctor supplying an encoding
    /// </summary>
    public JsonDataExportService(Encoding encoding) : base(encoding)
    {
        HeaderData = Encoding.GetBytes("[");
        FooterData = Encoding.GetBytes("]");
        TokenSeparatorData = Encoding.GetBytes(",");
    }

    /// <summary>
    /// Converts an object of type T into a ReadOnlyMemory&lt;byte&gt; instance
    /// </summary>
    /// <param name="data">Data object to serialize</param>
    /// <returns>Byte array</returns>
    public override ReadOnlyMemory<byte> ToMemory(T data)
    {
        var s = JsonSerializer.Serialize(data);
        var b = Encoding.GetBytes(s);
        return b;
    }
}
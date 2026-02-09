// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Text;
using System.Xml.Serialization;

namespace Bodoconsult.App.DataExportServices;

/// <summary>
/// Data export service for XML
/// </summary>
public class XmlDataExportService<T> : DataExportServiceBase<T> where T : class
{
    private readonly XmlSerializer _xmlSerializer = new(typeof(T));

    /// <summary>
    /// Default ctor
    /// </summary>
    public XmlDataExportService()
    {
        var name = typeof(T).Name;
        HeaderData = Encoding.UTF8.GetBytes($"<?xml version=\"1.0\" encoding=\"utf-16\"?>{Environment.NewLine}<ArrayOf{name} xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">{Environment.NewLine}");
        FooterData = Encoding.UTF8.GetBytes($"{Environment.NewLine}</ArrayOf{name}>");
    }

    /// <summary>
    /// Converts an object of type T into a ReadOnlyMemory&lt;byte&gt; instance
    /// </summary>
    /// <param name="data">Data object to serialize</param>
    /// <returns>Byte array</returns>
    public override ReadOnlyMemory<byte> ToMemory(T data)
    {
        var s = Serialize(data);
        var b = Encoding.UTF8.GetBytes(s);
        return b;
    }

    /// <summary>
    /// Serialize an object to an XML string
    /// </summary>
    /// <param name="obj">Object to serialize</param>
    /// <returns>String with serialized object</returns>
    public string Serialize(object obj)
    {
        using var textWriter = new StringWriter();
        _xmlSerializer.Serialize(textWriter, obj);
        return textWriter.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "", StringComparison.InvariantCultureIgnoreCase);
    }
}
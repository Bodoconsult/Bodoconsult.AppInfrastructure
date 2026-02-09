// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Numerics;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Bodoconsult.App.DataExportServices;

/// <summary>
/// Data export service for XML
/// </summary>
public class XmlDataExportService<T> : DataExportServiceBase<T> where T : class
{
    private readonly XmlSerializer _xmlSerializer = new(typeof(T));
    private readonly XmlSerializerNamespaces _namespaces = new([XmlQualifiedName.Empty]);
    private readonly XmlWriterSettings _settings = new() { OmitXmlDeclaration = true, Indent = true, Encoding = Encoding.UTF8 };
    private readonly StringBuilder _output = new();

    /// <summary>
    /// Default ctor
    /// </summary>
    public XmlDataExportService()
    {
        var name = typeof(T).Name;
        HeaderData = Encoding.GetBytes($"<?xml version=\"1.0\" encoding=\"utf-16\"?>{Environment.NewLine}<ArrayOf{name} xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">{Environment.NewLine}");
        FooterData = Encoding.GetBytes($"{Environment.NewLine}</ArrayOf{name}>");
    }

    /// <summary>
    /// Converts an object of type T into a ReadOnlyMemory&lt;byte&gt; instance
    /// </summary>
    /// <param name="data">Data object to serialize</param>
    /// <returns>Byte array</returns>
    public override ReadOnlyMemory<byte> ToMemory(T data)
    {
        var s = Serialize(data);
        var b = Encoding.GetBytes(s);
        return b;
    }

    /// <summary>
    /// Serialize an object to an XML string
    /// </summary>
    /// <param name="obj">Object to serialize</param>
    /// <returns>String with serialized object</returns>
    public string Serialize(object obj)
    {
        _output.Clear();

        var writer = XmlWriter.Create(_output, _settings);
        _xmlSerializer.Serialize(writer, obj, _namespaces);

        return _output.ToString();

        //using var textWriter = new StringWriter();
        //_xmlSerializer.Serialize(textWriter, obj, _namespaces);
        //return textWriter.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "", StringComparison.InvariantCultureIgnoreCase);
    }
}
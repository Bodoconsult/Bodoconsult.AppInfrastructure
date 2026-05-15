// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Bodoconsult.App.DataExportServices;

/// <summary>
/// Data export service using XML as export format. XML creation is slowly compared to other formats like JSON. So pay attention to performance if you use this service
/// </summary>
public class XmlDataExportService<T> : BaseDataExportService<T> where T : class
{
    private readonly XmlSerializer _xmlSerializer = new(typeof(T));
    private readonly XmlSerializerNamespaces _namespaces = new([XmlQualifiedName.Empty]);
    private readonly StringBuilder _output = new();

    private readonly MemoryStream _stream = new();
    private XmlWriter _writer;
    private XmlWriterSettings _settings;

    /// <summary>
    /// Default ctor
    /// </summary>
    public XmlDataExportService()
    {
        _settings = new()
        {
            OmitXmlDeclaration = true,
            Indent = true,
            Encoding = Encoding,
            ConformanceLevel = ConformanceLevel.Auto
        };

        _writer = XmlWriter.Create(_stream, _settings);

        LoadBaseData();
    }

    /// <summary>
    /// Ctor supplying an encoding
    /// </summary>
    public XmlDataExportService(Encoding encoding) : base(encoding)
    {
        _settings = new()
        {
            OmitXmlDeclaration = true,
            Indent = true,
            Encoding = Encoding,
            ConformanceLevel = ConformanceLevel.Auto
        };
      

        LoadBaseData();
    }

    private void LoadBaseData()
    {
        var encodingName = Encoding.HeaderName.ToLowerInvariant() switch
        {
            "utf-8" => "utf-8",
            "utf-32" => "utf-32",
            _ => "utf-16"
        };
        var name = typeof(T).Name;
        HeaderData = Encoding.GetBytes($"<?xml version=\"1.0\" encoding=\"{encodingName}\"?>{Environment.NewLine}<ArrayOf{name}>{Environment.NewLine}");
        FooterData = Encoding.GetBytes($"</ArrayOf{name}>");
    }


    /// <summary>
    /// Converts an object of type T into a ReadOnlyMemory&lt;byte&gt; instance
    /// </summary>
    /// <param name="data">Data object to serialize</param>
    /// <returns>Byte array</returns>
    public override ReadOnlyMemory<byte> ToMemory(T data)
    {
        _output.Clear();

        var writer = XmlWriter.Create(_output, _settings);
        _xmlSerializer.Serialize(writer, data, _namespaces);
        var s = $"{_output}{Environment.NewLine}";
        return Encoding.GetBytes(s);

        //_writer = XmlWriter.Create(_stream, settings);

        //_xmlSerializer.Serialize(_writer, data, _namespaces);

        //_stream.Position = 0;

        //var b = _stream.ToArray();

        //Debug.Print(Encoding.GetString(b));

        //_stream.Position = 0;
        //_stream.SetLength(0);

        //return b;
    }

    ///// <summary>
    ///// Serialize an object to an XML string
    ///// </summary>
    ///// <param name="obj">Object to serialize</param>
    ///// <returns>String with serialized object</returns>
    //public ReadOnlyMemory<byte> Serialize(T obj)
    //{
    //    _output.Clear();

    //    var writer = XmlWriter.Create(_output, _settings);
    //    _xmlSerializer.Serialize(writer, obj, _namespaces);
    //    return _output.ToString().Ass
    //}
}
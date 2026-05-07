// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Bodoconsult.Test.Interfaces;
using Bodoconsult.Test.MetaData.Model;

namespace Bodoconsult.Test.MetaData.Handler;

/// <summary>
/// Class to handle type metadata
/// </summary>
public class MetaDataHandler
{

    /// <summary>
    /// Current documentation files loaded
    /// </summary>
    public IList<AssemblyDocumentation> Documentation { get; }

    /// <summary>
    ///  default ctor
    /// </summary>
    public MetaDataHandler()
    {
        TypeMetaDatas = new List<TypeMetaData>();
        Documentation = new List<AssemblyDocumentation>();
    }

    /// <summary>
    /// Load all referenced assemblies
    /// </summary>
    /// <param name="assembly"></param>
    public void LoadReferencedAssemblies(Assembly assembly)
    {

        var loadedAssemblies = assembly.GetReferencedAssemblies();

        foreach (var ass in loadedAssemblies)
        {
            //try
            //{

            LoadAssemblyDocumentation(Assembly.Load(ass));
            //}
            //catch
            //{
            //    // ignored
            //}
        }
    }

    /// <summary>
    /// Load a assembly directly (and not their referenced assemblies)
    /// </summary>
    /// <param name="assembly"></param>
    public void LoadAssembly(Assembly assembly)
    {
        LoadAssemblyDocumentation(assembly);
    }


    private void LoadAssemblyDocumentation(Assembly assembly)
    {

        // Do not replace with Environment.ProcessPath
        var fileName = new FileInfo(assembly.Location).Name;

        var xmlFile = assembly.Location.Replace(".dll", ".xml");

        if (!File.Exists(xmlFile))
        {
            return;
        }

        Debug.Print(fileName);
        Debug.Print(xmlFile);

        var docu = new AssemblyDocumentation { AssemblyFileName = fileName };
        var xml = new XmlDocument();
        xml.Load(xmlFile);

        docu.Documentation = xml;

        Documentation.Add(docu);
    }


    /// <summary>
    /// List of the types to get metadata for
    /// </summary>
    public IList<TypeMetaData> TypeMetaDatas { get; set; }


    /// <summary>
    /// Add a type to the list of types <see cref="TypeMetaDatas"/>
    /// </summary>
    /// <param name="type"></param>
    public TypeMetaData AddType(Type type)
    {
        var item = new TypeMetaData
        {
            FullName = string.Empty,
            Name = type.Name,
            CurrentType = type
        };

        TypeMetaDatas.Add(item);

        // Read data from 
        FindXmlDataClass(item);

        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var pItem = new TypeItemMetaData
            {
                Type = 'P',
                FullName = $".{prop.Name}",
                Name = prop.Name,
                DataType = prop.PropertyType.ToString(),
                CurrentType = type
            };

            item.TypeItemMetaDatas.Add(pItem);

            FindXmlDataProperties(pItem);
        }


        //foreach (var prop in type.GetEvents(BindingFlags.Public | BindingFlags.Instance))
        //{
        //    var pItem = new TypeItemMetaData
        //    {
        //        Type = 'E',
        //        FullName = type.FullName + "." + prop.Name,
        //        Name = prop.Name
        //    };
        //    if (type.BaseType != null) pItem.BaseTypeFullName = type.BaseType.FullName + "." + prop.Name;

        //    item.TypeItemMetaDatas.Add(pItem);

        //    FindXmlDataEvents(pItem);
        //}


        //foreach (var prop in type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
        //{
        //    var pItem = new TypeItemMetaData
        //    {
        //        Type = 'M',
        //        FullName = type.FullName + "." + prop.Name,
        //        Name = prop.Name
        //    };
        //    if (type.BaseType != null) pItem.BaseTypeFullName = type.BaseType.FullName + "." + prop.Name;

        //    item.TypeItemMetaDatas.Add(pItem);

        //    FindXmlDataMethods(pItem);
        //}

        return item;
    }

    private void FindXmlDataProperties(IMetaData item)
    {
        FindXmlDataRaw("P:", item);
    }

    //private void FindXmlDataMethods(IMetaData item)
    //{
    //    FindXmlDataRaw("M:", item);
    //}

    //private void FindXmlDataEvents(IMetaData item)
    //{
    //    FindXmlDataRaw("P:", item);
    //}

    private void FindXmlDataClass(IMetaData item)
    {
        FindXmlDataRaw("T:", item);

    }

    private void FindXmlDataRaw(string prefix, IMetaData item)
    {
        //FindXmlData(prefix, item, item.CurrentType);


        //var intf = item.CurrentType.GetInterfaces();
        //foreach (var oInterface in intf)
        //{
        //    FindXmlData(prefix, item, oInterface);
        //}

        var type = item.CurrentType;
        while (type != null)
        {
            FindXmlData(prefix, item, type);

            var intf = type.GetInterfaces();
            foreach (var oInterface in intf)
            {
                FindXmlData(prefix, item, oInterface);
            }

            type = type.BaseType;
        }
    }



    private void FindXmlData(string prefix, IMetaData item, Type type)
    {
        // Do not replace with Environment.ProcessPath
        var assFileName = new FileInfo(type.Assembly.Location).Name;

        var docu = Documentation.FirstOrDefault(x => x.AssemblyFileName == assFileName);

        var xml = docu?.Documentation;

        if (xml == null) return;

        var xPath = string.Format("doc/members/member[attribute::name='{1}{0}']", type.FullName + item.FullName, prefix);

        var node = xml.SelectSingleNode(xPath);
        if (node == null)
        {
            Debug.Print(prefix + type.FullName + item.FullName);

            return;
            //xPath = string.Format("doc/members/member[attribute::name='{1}{0}']", item.BaseTypeFullName, prefix);
            //node = _xml.SelectSingleNode(xPath);

            //if (node == null)
            //{
            //    xPath = string.Format("doc/members/member[attribute::name='{1}{0}']", item.BaseTypeFullName2, prefix);
            //    node = _xml.SelectSingleNode(xPath);
            //    if (node == null) return;
            //}
        }


        if (string.IsNullOrEmpty(item.Summary))
        {
            var summaryNode = node.SelectSingleNode("summary");
            if (summaryNode != null) item.Summary = summaryNode.InnerXml;

            if (!string.IsNullOrEmpty(item.Summary))
            {
                item.Summary = ClearString(item.Summary);
            }

        }

        if (string.IsNullOrEmpty(item.Remarks))
        {
            var remarksNode = node.SelectSingleNode("remarks");
            if (remarksNode != null) item.Remarks = remarksNode.InnerXml;

            if (!string.IsNullOrEmpty(item.Remarks))
            {
                item.Remarks = ClearString(item.Remarks);
            }
        }

        if (string.IsNullOrEmpty(item.Example))
        {
            var exampleNode = node.SelectSingleNode("example");
            if (exampleNode != null) item.Example = exampleNode.InnerXml;

            if (!string.IsNullOrEmpty(item.Example))
            {
                item.Example = ClearString(item.Example);
            }
        }

        if (string.IsNullOrEmpty(item.Code))
        {
            var codeNode = node.SelectSingleNode("code");
            if (codeNode != null) item.Code = codeNode.InnerXml;

            if (!string.IsNullOrEmpty(item.Code))
            {
                item.Code = ClearString(item.Code);
            }

        }

    }

    private static string ClearString(string value)
    {
        try
        {

            value = value.Replace("<para>", "??pa??").Replace("</para>", "??pe??");


            var i = value.IndexOf("<see cref=\"", StringComparison.Ordinal);

            if (i < 0) return value;

            while (i >= 0)
            {
                var j = value.IndexOf("\" />", i, StringComparison.Ordinal);

                var s = value.Substring(i + 13, j - i - 13);

                var z = s.LastIndexOf(".", s.LastIndexOf(".", StringComparison.Ordinal) - 1, StringComparison.Ordinal);

                s = s[(z + 1)..];

                value = value[..i] + s + value[(j + 4)..];

                i = value.IndexOf("<see cref=\"", StringComparison.Ordinal);
            }
        }
        catch
        {
            // ignored
        }

        return value.Replace("<sample>", "\r\n").Replace("</sample>", "\r\n");
    }
}
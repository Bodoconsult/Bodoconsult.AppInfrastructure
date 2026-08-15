// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Bodoconsult.App.Extensions;
using Bodoconsult.Text.Helpers;
using Bodoconsult.Text.Interfaces;

namespace Bodoconsult.Text.Documents;

/// <summary>
/// Reads a LDML file and parses it
/// </summary>
public class LdmlReader
{

    private IDocumentElement _textElement;

    private readonly Type _popertyElementTpye = typeof(IPropertyAsAttributeElement);

    /// <summary>
    /// Read-only access to current <see cref="IDocumentElement"/> instance
    /// </summary>
    public IDocumentElement TextElement => _textElement;

    /// <summary>
    /// LDML string to parse. Public only for unit testing
    /// </summary>
    public string Ldml;

    /// <summary>
    /// Default ctor. Used together with <see cref="LoadLdmlFile"/> to load a LDML file from disk
    /// </summary>
    public LdmlReader()
    { }

    /// <summary>
    /// Ctor loading a LDML string
    /// </summary>
    /// <param name="ldml">LDML string</param>
    public LdmlReader(string ldml)
    {
        Ldml = ldml;
    }

    /// <summary>
    /// Load the LDML string fom a utf-8 text file
    /// </summary>
    /// <param name="filePath">Full path to the LDML file</param>
    public void LoadLdmlFile(string filePath)
    {
        Ldml = File.ReadAllText(filePath);
    }


    /// <summary>
    /// Parse the LDML to a <see cref="Document"/> instance
    /// </summary>
    public void ParseLdml()
    {
        var xmlDoc = XDocument.Parse(Ldml);

        var root = xmlDoc.Descendants().FirstOrDefault();

        if (root is null)
        {
            return;
        }

        var name = root.Name.ToString();

        _textElement = GetDocumentElement(name, root, null);

        var sb = new StringBuilder();
        _textElement.ToLdmlString(sb, string.Empty);

        Debug.Print(sb.ToString());
    }

    private IDocumentElement GetDocumentElement(string elementName, XElement node, DocumentElement parent)
    {
        var type = Type.GetType($"Bodoconsult.Text.Documents.{elementName}");

        //Debug.Print($"Bodoconsult.Text.Documents.{elementName}");

        if (type is null)
        {
            HandleObjectTypes(elementName, node, parent);
            return null;
        }

        object obj;

        try
        {
            obj = Activator.CreateInstance(type);
        }
        catch //(Exception e)
        {
            return null;
        }

        switch (obj)
        {
            case IPropertyAsBlockElement propertyAsBlock:
                LoadProperties(propertyAsBlock, node);
                return propertyAsBlock;
            case SpanBase span:
                span.Content = node.Value;
                return span;
            default:
                return obj is not TextElement textElement ? null : GetTextElement(elementName, node, textElement);
        }
    }

    private void HandleObjectTypes(string elementName, XElement node, DocumentElement parent)
    {
        var pis = DocumentReflectionHelper.GetPropertiesForBlocks(parent.GetType());
        var pi = pis.FirstOrDefault(x => x.Name == elementName);

        if (pi is null)
        {
            return;
        }
            
        // List?
        if (!(pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(LdmlList<>)))
        {
            // No list
            GetPropertyAsBlockElement(parent, node, pi);
            return;
        }

        // List
        GetListPropertyAsBlockElements(parent, pi, node);
    }

    private void GetListPropertyAsBlockElements(DocumentElement parent, PropertyInfo pi, XElement node)
    {
        //var genType = pi.PropertyType.GenericTypeArguments[0];
        //Debug.Print(genType.Name);

        var list = pi.GetValue(parent);

        foreach (var childNode in node.Nodes())
        {
            GetListItemAsBlock(pi, childNode, list);
        }
    }

    private void GetListItemAsBlock(PropertyInfo pi, XNode childNode, object list)
    {
        if (childNode is XElement element)
        {
            Debug.Print($"{childNode}: {pi.Name}: child {element.Name}");

            var child = GetDocumentElement(element.Name.ToString(), element, null);

            var method = pi.PropertyType.GetMethod("Add");
            method?.Invoke(list, [child]);
        }
        else
        {
            Debug.Print(childNode.ToString());
        }
    }

    private void GetPropertyAsBlockElement(DocumentElement parent, XElement node, PropertyInfo pi)
    {
        // Check child nodes
        var childs = node.Nodes().ToArray();

        foreach (var childNode in childs)
        {
            if (childNode is XElement element)
            {

                Debug.Print($"{pi.Name}: child {element.Name}");

                var child = GetDocumentElement(element.Name.ToString(), element, null);
                pi.SetValue(parent, child);
            }
            else
            {
                Debug.Print(childNode.ToString());
            }
        }
    }

    private TextElement GetTextElement(string elementName, XElement node, TextElement textElement)
    {
        // Load properties
        LoadProperties(textElement, node);

        // Check child nodes
        var childs = node.Nodes().ToArray();

        switch (textElement)
        {
            case Block block:
            {
                foreach (var childNode in childs)
                {
                    HandleChildsForBlocks(elementName, textElement, childNode, block);
                }

                break;
            }
            case Inline inline:
            {
                //if (obj is Span)
                //{
                //    Debug.Print("Blubb");
                //}

                foreach (var childNode in childs)
                {
                    HandleChildsForInlines(textElement, childNode, inline);
                }

                break;
            }
        }

        return textElement;
    }

    private void HandleChildsForInlines(TextElement textElement, XNode childNode, Inline inline)
    {
        switch (childNode)
        {
            case XElement element:
            {
                var child = GetDocumentElement(element.Name.ToString(), element, textElement);

                if (child is Inline childInline)
                {
                    inline.AddInline(childInline);
                }

                break;
            }
            case XText text:
            {
                if (TextElement is Span parentInline)
                {
                    parentInline.Content = text.Value;
                }
                Debug.Print(childNode.ToString());
                break;
            }
        }
    }

    private void HandleChildsForBlocks(string elementName, TextElement textElement, XNode childNode, Block block)
    {
        if (childNode is XElement element)
        {
            Debug.Print($"{elementName}: child {element.Name}");

            var child = GetDocumentElement(element.Name.ToString(), element, textElement);

            switch (child)
            {
                case Block childBlock:
                    block.AddBlock(childBlock);
                    break;
                case Inline childInline:
                    block.AddInline(childInline);
                    break;
            }
        }
        else
        {
            Debug.Print(childNode.ToString());
        }
    }

    private void LoadProperties(IDocumentElement element, XElement node)
    {
        if (!node.HasAttributes)
        {
            return;
        }

        var attributes = node.Attributes().ToArray();

        var pis = DocumentReflectionHelper.GetPropertiesForAttributes(element.GetType());

        foreach (var prop in pis)
        {
            LoadProperty(element, prop, attributes);
        }
    }

    private void LoadProperty(IDocumentElement element, PropertyInfo prop, XAttribute[] attributes)
    {
        var propType = prop.PropertyType;

        var attr = attributes.FirstOrDefault(x => x.Name == prop.Name);

        if (attr is null)
        {
            return;
        }

        // Object as property based on PropertyElement
        if (_popertyElementTpye.IsAssignableFrom(propType))
        {
            var objValue = Activator.CreateInstance(propType, attr.Value);
            prop.SetValue(element, objValue);
            return;
        }

        // Property is a Type property
        if (propType == typeof(Type))
        {
            var type = Type.GetType(attr.Value);
            prop.SetValue(element, type);
            return;
        }

        // Normal property

        //// Prop Name
        //if (propType.IsEnum)
        //{
        //    var value = Enum.Parse(propType, attr.Value);
        //    prop.SetValue(element, value);
        //}
        //else
        //{

        try
        {
            prop.SetValue(element, attr.Value.Convert(propType));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        //}
    }
}
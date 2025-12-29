// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using Bodoconsult.Text.Documents;
using Bodoconsult.Text.Interfaces;

namespace Bodoconsult.Text.Helpers;

/// <summary>
/// Helper for getting type properties for document related types
/// </summary>
public static class DocumentReflectionHelper
{

    private static readonly Dictionary<Type, PropertyInfo[]> TypeInfoAttributes = new();
    private static readonly Dictionary<Type, PropertyInfo[]> TypeInfoBlocks = new();

    /// <summary>
    /// Get the relevante properties of a document related type to be shown as LDML attributes
    /// </summary>
    /// <param name="type">Document related type like Document, Paragraph etc.</param>
    /// <returns>Array with property infos</returns>
    public static PropertyInfo[] GetPropertiesForAttributes(Type type)
    {

        if (TypeInfoAttributes.TryGetValue(type, out var properties))
        {
            return properties;
        }

        var props = type.GetProperties();

        var pi = new List<PropertyInfo>();

        foreach (var propInfo in props)
        {

            var propType = propInfo.PropertyType;

            var isPropElement = typeof(IPropertyAsAttributeElement).IsAssignableFrom(propType);

            if (!propType.IsPrimitive && propType != typeof(string) && !propType.IsEnum && !isPropElement && !(propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(LdmlList<>)) && propType != typeof(Type))
            {
                continue;
            }

            var attr = propInfo.GetCustomAttributes(typeof(DoNotSerializeAttribute), true);
            if (attr.Length != 0)
            {
                continue;
            }

            pi.Add(propInfo);
        }

        var pia = pi.ToArray();

        TypeInfoAttributes.Add(type, pia);

        return pia;
    }

    /// <summary>
    /// Get the relevante properties of a document related type to be shown as LDML blocks
    /// </summary>
    /// <param name="type">Document related type like Document, Paragraph etc.</param>
    /// <returns>Array with property infos</returns>
    public static PropertyInfo[] GetPropertiesForBlocks(Type type)
    {

        if (TypeInfoBlocks.TryGetValue(type, out var properties))
        {
            return properties;
        }

        var props = type.GetProperties();

        var pi = new List<PropertyInfo>();

        foreach (var propInfo in props)
        {

            var propType = propInfo.PropertyType;

            var isPropElement = typeof(IPropertyAsBlockElement).IsAssignableFrom(propType);

            if (!isPropElement)
            {
                if (!(propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(LdmlList<>)) && propType != typeof(Type))
                {
                    continue;
                }
            }

            var attr = propInfo.GetCustomAttributes(typeof(DoNotSerializeAttribute), true);
            if (attr.Length != 0)
            {
                continue;
            }

            pi.Add(propInfo);
        }

        var pia = pi.ToArray();

        TypeInfoBlocks.Add(type, pia);

        return pia;
    }

}



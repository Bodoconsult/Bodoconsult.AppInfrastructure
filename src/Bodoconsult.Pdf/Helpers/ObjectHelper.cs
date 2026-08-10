// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bodoconsult.Pdf.Helpers;

/// <summary>
/// Basic tools for object handling
/// </summary>
public static class ObjectHelper
{

    /// <summary>
    /// Copy property values by property name from source object to target object
    /// </summary>
    /// <param name="source">Source object</param>
    /// <param name="target">Target object</param>
    public static void MapProperties(object source, object target)
    {

        var sourceType = source.GetType();
        var targetType = target.GetType();

        var propMap = GetMatchingProperties(sourceType, targetType);

        for (var i = 0; i < propMap.Count; i++)
        {
            var prop = propMap[i];

            var sourceValue = prop.SourceProperty.GetValue(source, null);

            prop.TargetProperty.SetValue(target, sourceValue, null);
        }
    }

    /// <summary>
    /// Get a list of all properties existing in a source and a target object
    /// </summary>
    /// <param name="source">Source object</param>
    /// <param name="target">Target object</param>
    /// <returns>List with properties existing in both objects</returns>
    public static IReadOnlyList<PropertyMap> GetMatchingProperties(Type source, Type target)

    {

        var sourceProperties = source.GetProperties();

        var targetProperties = target.GetProperties();

        var properties = (from s in sourceProperties
                          from t in targetProperties
                          where s.Name == t.Name &&

                  s.CanRead &&
                  t.CanWrite &&
                  s.PropertyType.IsPublic &&
                  t.PropertyType.IsPublic &&

                  s.PropertyType == t.PropertyType &&
                  (
                      s.PropertyType.IsValueType &&
                      t.PropertyType.IsValueType ||
                      s.PropertyType == typeof(string) &&
                      t.PropertyType == typeof(string)
                  )

                          select new PropertyMap
                          {
                              SourceProperty = s,
                              TargetProperty = t

                          }).ToArray();

        return properties;

    }

    /// <summary>
    /// A pair of two properties with same name existing for source object and target object
    /// </summary>
    public class PropertyMap
    {
        /// <summary>
        /// Property of the source object
        /// </summary>
        public PropertyInfo SourceProperty { get; set; }

        /// <summary>
        /// Property of the target object
        /// </summary>
        public PropertyInfo TargetProperty { get; set; }
    }
}
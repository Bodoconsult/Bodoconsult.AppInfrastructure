// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Bodoconsult.App.Helpers;

/// <summary>
/// Helper class for copying properties from one object to another
/// </summary>
public static class ObjectHelper
{

    /// <summary>
    /// Tolerated difference value for numeric values being equal as decimal
    /// </summary>
    public static decimal ToleranceValueComparisons { get; set; } = new(0.00000000000001);


    /// <summary>
    /// Tolerated difference value for numeric values being equal as double
    /// </summary>
    public static double ToleranceValueComparisonsDouble { get; set; } = 0.00000000000001;

    /// <summary>
    /// Check if two objects have the same content
    /// </summary>
    /// <param name="original">original value in the database</param>
    /// <param name="current">current value in the entity</param>
    /// <returns></returns>
    public static bool CheckIfValuesAreEqual(object? original, object? current)
    {

        if (original == current)
        {
            return true;
        }

        if (original is null)
        {
            return current is null;
        }

        if (original.Equals(current))
        {
            return true;
        }

        if (current is null)
        {
            return false;
        }

        switch (original.GetType().Name.ToUpperInvariant())
        {
            case "BYTE[]":
                return ByteArrayCompare((byte[])original, (byte[])current);
            case "SINGLE":
            case "DOUBLE":
            case "DECIMAL":
            case "FLOAT":
                var diff = Math.Abs(Convert.ToDecimal(original, CultureInfo.InvariantCulture) - Convert.ToDecimal(current, CultureInfo.InvariantCulture));
                return diff < ToleranceValueComparisons;
        }

        return false;
    }

    /// <summary>
    /// Compare two byte arrays
    /// </summary>
    /// <param name="a1">Byte array 1 to check</param>
    /// <param name="a2">Byte array 2 to check</param>
    /// <returns>true if the arrays are equal</returns>
    public static bool ByteArrayCompare(IReadOnlyList<byte>? a1, IReadOnlyList<byte>? a2)
    {
        if (a1 is null && a2 is null)
        {
            return true;
        }

        if (a1 is null)
        {
            return false;
        }

        if (a2 is null)
        {
            return false;
        }

        if (a1.Count != a2.Count)
        {
            return false;
        }

        // Fastest way to compare arrays: iterate it
        for (var i = 0; i < a1.Count; i++)
        {
            if (a1[i] != a2[i])
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Copy property values from one object to the other
    /// </summary>
    /// <param name="source"></param>
    /// <param name="target"></param>
    public static void MapProperties(object? source, object? target)
    {

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (target is null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        var sourceType = source.GetType();

        var targetType = target.GetType();

        var propMap = GetMatchingProperties(sourceType, targetType);

        for (var i = 0; i < propMap.Count; i++)
        {

            var prop = propMap[i];

            if (prop.SourceProperty is null)
            {
                continue;
            }

            var sourceValue = prop.SourceProperty.GetValue(source, null);

            if (prop.TargetProperty is null)
            {
                continue;
            }

            prop.TargetProperty.SetValue(target, sourceValue, null);
        }
    }


    /// <summary>
    /// Copy property values from one object to the other
    /// </summary>
    /// <param name="source"></param>
    /// <param name="target"></param>
    /// <returns>true if objects have the same properties values</returns>
    public static bool CompareProperties(object? source, object? target)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (target is null)
        {
            throw new ArgumentNullException(nameof(target));
        }


        var sourceType = source.GetType();

        var targetType = target.GetType();

        var propMap = GetMatchingProperties(sourceType, targetType);

        for (var i = 0; i < propMap.Count; i++)
        {

            var prop = propMap[i];

            //if (prop.SourceProperty.Name == "RowVersion") continue;

            if (prop.SourceProperty is null || prop.TargetProperty is null)
            {
                continue;
            }

            var sourceValue = prop.SourceProperty.GetValue(source, null);

            var targetValue = prop.TargetProperty.GetValue(target, null);

            if (sourceValue is null || targetValue is null)
            {
                continue;
            }

            if (sourceValue.ToString() != targetValue.ToString())
            {

                return false;
            }
        }

        return true;
    }


    internal static IList<PropertyMap> GetMatchingProperties(Type? sourceType, Type? targetType)
    {
        if (sourceType is null)
        {
            throw new ArgumentNullException(nameof(sourceType));
        }

        if (targetType is null)
        {
            throw new ArgumentNullException(nameof(targetType));
        }

        var sourceProperties = sourceType.GetProperties();

        var targetProperties = targetType.GetProperties();

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

                          }).ToList();

        return properties;

    }


    /// <summary>
    /// Fill an object with sample data
    /// </summary>
    /// <param name="data"></param>
    public static void FillProperties(object? data)
    {
        if (data is null)
        {
            return;
        }

        var theType = data.GetType();
        var p = theType.GetProperties();

        foreach (var pi in p)
        {

            switch (pi.Name.ToUpperInvariant())
            {
                case "ID":
                case "ROWVERSION":
                    continue;
            }

            try
            {
                var custAttr = pi.GetCustomAttributes(true);

                string typeName;
                if (pi.PropertyType.IsGenericType &&
                    pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    typeName = pi.PropertyType.GetGenericArguments()[0].Name.ToUpperInvariant();
                }
                else
                {
                    typeName = pi.PropertyType.Name.ToUpperInvariant();
                }

                switch (typeName)
                {
                    case "STRING":
                        var length = 50;
                        if (custAttr.Length > 0)
                        {

                            var attr = custAttr.FirstOrDefault(x => x is StringLengthAttribute);


                            if (attr != null)
                            {
                                length = ((StringLengthAttribute)attr).MaximumLength;
                            }
                        }

                        pi.SetValue(data, new string('a', length));
                        break;
                    case "INT32":
                        pi.SetValue(data, 1);
                        break;
                    case "DATETIME":
                        pi.SetValue(data, DateTime.Now);
                        break;
                    case "BOOLEAN":
                        pi.SetValue(data, true);
                        break;
                    case "DOUBLE":
                        pi.SetValue(data, 1.99);
                        break;
                    default:
                        Debug.Print($"{pi.Name} {typeName}");
                        break;
                }

            }
            catch //(Exception)
            {
                // No action
            }
        }

    }

    /// <summary>
    /// Get the values of an object as string
    /// </summary>
    /// <param name="o">Current object</param>
    /// <returns>Properties of the object and their values as string</returns>
    public static string? GetObjectPropertiesAsString(object o)
    {
        var type = o.GetType();

        if (o is string)
        {
            return o.ToString();
        }

        if (type.IsValueType)
        {
            return o.ToString();
        }

        var props = type.GetProperties();

        var str = new StringBuilder();
        str.Append('{');
        foreach (var prop in props)
        {
            var v = prop.GetValue(o);

            if (v is null)
            {
                continue;
            }

            if (prop.PropertyType == typeof(byte[]))
            {
                str.Append($"{prop.Name}:{GetStringFromArray(((byte[])v).AsMemory())},");
            }
            else if (prop.PropertyType == typeof(Memory<byte>))
            {
                str.Append($"{prop.Name}:{GetStringFromArray((Memory<byte>)v)},");
            }
            else
            {
                str.Append($"{prop.Name}:{v},");
            }
        }

        var result = str.ToString();
        return $"{result.Remove(result.Length - 1)}}}";
    }

    /// <summary>
    /// Get the values of an object as string
    /// </summary>
    /// <param name="o">Current object</param>
    /// <param name="s">Current string builder</param>
    /// <returns>Properties of the object and their values as string</returns>
    public static void GetObjectPropertiesAsString(object o, StringBuilder s)
    {
        var type = o.GetType();

        if (o is string)
        {
            s.Append(o);
            return;
        }

        if (type.IsValueType)
        {
            s.Append(o);
            return;
        }

        var props = type.GetProperties();

        s.Append("{");

        var len = props.Length;
        var i = 1;
        foreach (var prop in props)
        {
            var v = prop.GetValue(o);

            if (v is null)
            {
                continue;
            }

            if (prop.PropertyType == typeof(byte[]))
            {
                s.Append($"{prop.Name}:");
                GetStringFromArray(((byte[])v).AsMemory(), s);
            }
            else if (prop.PropertyType == typeof(Memory<byte>))
            {
                s.Append($"{prop.Name}:");
                GetStringFromArray((Memory<byte>)v, s);
            }
            else
            {
                s.Append($"{prop.Name}:{v}");
            }

            if (i < len)
            {
                s.Append(",");
            }
            i++;
        }

        s.Append("}");
    }

    /// <summary>
    /// Get a string from a byte array
    /// </summary>
    /// <param name="data">Byte array</param>
    /// <returns>Byte array as string</returns>
    private static string GetStringFromArray(Memory<byte> data)
    {
        var value = new StringBuilder();
        byte b;

        for (var i = 0; i < data.Length; i++)
        {
            b = data.Slice(i, 1).Span[0];
            if (b <= 33 || b >= 127)
            {
                value.Append($"[{b:X2}]");
            }
            else
            {
                value.Append(Convert.ToChar(b));
            }
        }
        return value.ToString();
    }

    /// <summary>
    /// Get a string from a byte array
    /// </summary>
    /// <param name="data">Byte array</param>
    /// <param name="s">Current string to write the array in</param>
    /// <returns>Byte array as string</returns>
    private static void GetStringFromArray(Memory<byte> data, StringBuilder s)
    {
        byte b;

        for (var i = 0; i < data.Length; i++)
        {
            b = data.Slice(i, 1).Span[0];
            if (b is <= 33 or >= 127)
            {
                s.Append($"[{b:X2}]");
            }
            else
            {
                s.Append(Convert.ToChar(b));
            }
        }
    }

}

internal class PropertyMap
{
    public PropertyInfo? SourceProperty { get; set; }

    public PropertyInfo? TargetProperty { get; set; }
}

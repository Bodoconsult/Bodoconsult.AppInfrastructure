// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Extensions;

/// <summary>
/// Extension methods for objects
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Conversion of a given object to a target type
    /// </summary>
    /// <param name="value">Given object</param>
    /// <param name="t">Target type</param>
    /// <returns>Instance of target type containing the value of the given object</returns>
    public static object? Convert(this object? value, Type t)
    {
        var underlyingType = Nullable.GetUnderlyingType(t);
        if (underlyingType != null && value == null)
        {
            return null;
        }

        var basetype = underlyingType ?? t;

        // Enum value
        if (t.IsEnum)
        {
            var x = Enum.Parse(t, value?.ToString() ?? string.Empty);
            return x;
        }

        // Other types
        return System.Convert.ChangeType(value, basetype);
    }

    /// <summary>
    /// Generic conversion of a given object to a target type
    /// </summary>
    /// <typeparam name="T">Target type</typeparam>
    /// <param name="value">Given object</param>
    /// <returns>Instance of target type containing the value of the given object</returns>
    public static T? Convert<T>(this object value)
    {
        return (T?)value.Convert(typeof(T));
    }
}
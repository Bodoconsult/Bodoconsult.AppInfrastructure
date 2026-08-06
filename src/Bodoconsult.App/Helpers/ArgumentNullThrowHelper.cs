// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.Helpers;

/// <summary>
/// Helper class for <see cref="ArgumentNullException"/> handling
/// </summary>
public static class ArgumentNullThrowHelper
{
    /// <summary>
    /// Throw exception if value is null or empty
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="ArgumentNullException">Thrown if value is null or empty</exception>
    public static void ThrowIfNullOrEmpty(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(nameof(value));
        }
    }

    /// <summary>
    /// Throw exception if object is null 
    /// </summary>
    /// <param name="obj">Object to check</param>
    /// <exception cref="ArgumentNullException">Thrown if object is null</exception>
    public static void ThrowIfNull(object? obj)
    {
        if (obj != null)
        {
            return;
        }
        throw new ArgumentNullException(nameof(obj));
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

// Copyright (c) 2020 Widauer Patrick. All rights reserved.

using System;

namespace Bodoconsult.App.Windows.CredentialManager;

/// <summary>
/// Credential attribute struct
/// </summary>
public readonly struct CredentialAttribute : IEquatable<CredentialAttribute>
{

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="keyword">Keyword</param>
    /// <param name="value">Value</param>
    /// <exception cref="ArgumentNullException"></exception>
    public CredentialAttribute (string keyword, string value)
    {
        Keyword = keyword ?? throw new ArgumentNullException (nameof(keyword));
        Value = value ?? throw new ArgumentNullException (nameof(value));
    }

    /// <summary>
    /// Keyword
    /// </summary>
    public string Keyword { get; }

    /// <summary>
    /// Value
    /// </summary>
    public string Value { get; }

    /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
    public bool Equals (CredentialAttribute other)
    {
        return Keyword == other.Keyword && Value == other.Value;
    }

    /// <summary>Indicates whether this instance and a specified object are equal.</summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, <see langword="false" />.</returns>
    public override bool Equals (object? obj)
    {
        return obj is CredentialAttribute other && Equals (other);
    }

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
    public override int GetHashCode()
    {
        unchecked
        {
            return (Keyword.GetHashCode() * 397) ^ Value.GetHashCode();
        }
    }

    /// <summary>
    /// == operator
    /// </summary>
    /// <param name="left">Left object</param>
    /// <param name="right">Right object</param>
    /// <returns>True is both objects are equal else false</returns>
    public static bool operator == (CredentialAttribute left, CredentialAttribute right)
    {
        return left.Equals (right);
    }

    /// <summary>
    /// != operator
    /// </summary>
    /// <param name="left">Left object</param>
    /// <param name="right">Right object</param>
    /// <returns>True is both objects are NOT equal else false</returns>
    public static bool operator != (CredentialAttribute left, CredentialAttribute right)
    {
        return !left.Equals (right);
    }

    /// <summary>Returns the fully qualified type name of this instance.</summary>
    /// <returns>The fully qualified type name.</returns>
    public override string ToString()
    {
        return Value != null
            ? $"'{Keyword}' = '{Value}'"
            : $"'{Keyword}' = null";
    }
}
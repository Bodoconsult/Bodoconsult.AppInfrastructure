// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Wpf.ReactiveUI.App;

/// <summary>
/// Represents a placeholder service type for null service registrations.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Splat.NullServiceType"/> class.
/// </remarks>
/// <param name="factory">The value factory.</param>
public class NullServiceType(Func<object?> factory)
{
    /// <summary>
    /// Cached Type instance for NullServiceType to avoid repeated typeof() calls.
    /// </summary>
    public static readonly Type CachedType = typeof(Splat.NullServiceType);

    /// <summary>
    /// Gets the Factory.
    /// </summary>
    public Func<object?> Factory { get; } = factory;
}
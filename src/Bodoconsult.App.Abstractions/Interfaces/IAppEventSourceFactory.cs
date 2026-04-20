// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Factory for creating <see cref="IAppEventSource"/> based implementation instances
/// </summary>
public interface IAppEventSourceFactory
{
    /// <summary>
    /// Create an instance of an <see cref="IAppEventSource"/> based implementation
    /// </summary>
    /// <returns><see cref="IAppEventSource"/> based instance</returns>
    IAppEventSource CreateInstance();
}
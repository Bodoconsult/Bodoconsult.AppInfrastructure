// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.Concurrent;
using System.Diagnostics.Tracing;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Logging;

/// <summary>
/// Dummy implementation of <see cref="IAppEventListener"/> doing nothing
/// </summary>
public class DummyAppEventListener : IAppEventListener
{
    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        // Do nothing
    }

    /// <summary>
    /// Event level for the listener. Default: EventLevel.Warning
    /// </summary>
    public EventLevel EventLevel { get; set; } = EventLevel.Warning;

    /// <summary>
    /// Stores the log messages for later use
    /// </summary>
    public ConcurrentQueue<string> Messages { get; } = new();
}
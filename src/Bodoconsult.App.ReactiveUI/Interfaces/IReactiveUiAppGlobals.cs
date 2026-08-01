// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using ReactiveUI.Builder;
using ReactiveUI.Primitives.Concurrency;

namespace Bodoconsult.App.ReactiveUI.Interfaces;

/// <summary>
/// <see cref="IAppGlobals"/> with enhancements for ReactiveUI usage
/// </summary>
public interface IReactiveUiAppGlobals: IAppGlobals
{
    /// <summary>
    /// Current UI instance
    /// </summary>
    IReactiveUIInstance? ReactiveUiInstance { get; set; }

    /// <summary>
    /// Current thread scheduler use for UI tasks
    /// </summary>
    ISequencer? MainUiThreadScheduler { get; set; }

    /// <summary>
    /// Current thread scheduler used for non-UI tasks
    /// </summary>
    ISequencer? TaskpoolScheduler { get; set; }
}
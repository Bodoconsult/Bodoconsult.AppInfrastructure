// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Reactive.Concurrency;
using Bodoconsult.App.Abstractions.Interfaces;
using ReactiveUI.Builder;

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
    IScheduler? MainUiThreadScheduler { get; set; }

    /// <summary>
    /// Current thread scheduler used for non-UI tasks
    /// </summary>
    IScheduler? TaskpoolScheduler { get; set; }
}
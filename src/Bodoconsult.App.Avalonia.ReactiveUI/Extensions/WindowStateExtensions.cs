//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Avalonia.Controls;
using Bodoconsult.App.Avalonia.ReactiveUI.Regions;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Ui;

namespace Bodoconsult.App.Avalonia.ReactiveUI.Extensions;

/// <summary>
/// Extension methods for <see cref="IRegionManager"/>
/// </summary>
public static class WindowStateExtensions
{
    /// <summary>
    /// Create a <see cref="AvaloniaUiRegion"/>
    /// </summary>
    /// <param name="windowState">Avalonia window state</param>
    /// <returns><see cref="UiWindowState"/> created and registered to region manager</returns>
    public static UiWindowState ToUiWindowState(this WindowState windowState)
    {
        return windowState switch
        {
            WindowState.Normal => UiWindowState.Normal,
            WindowState.Minimized => UiWindowState.Minimized,
            WindowState.Maximized => UiWindowState.Maximized,
            _ => throw new ArgumentOutOfRangeException(nameof(windowState), windowState, null)
        };
    }
}
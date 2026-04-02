//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Windows;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Ui;
using Bodoconsult.App.Wpf.ReactiveUI.Regions;

namespace Bodoconsult.App.Wpf.ReactiveUI.Extensions;

/// <summary>
/// Extension methods for <see cref="IRegionManager"/>
/// </summary>
public static class WindowStateExtensions
{
    /// <summary>
    /// Create a <see cref="WpfUiRegion"/>
    /// </summary>
    /// <param name="windowState">WPF window state</param>
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
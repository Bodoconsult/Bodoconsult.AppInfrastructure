// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Avalonia.Controls;
using Bodoconsult.App.Avalonia.ReactiveUI.Regions;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Ui;

namespace Bodoconsult.App.Avalonia.ReactiveUI.Extensions;

/// <summary>
/// Extension methods for <see cref="IRegionManager"/>
/// </summary>
public static class UiWindowStateExtensions
{
    /// <summary>
    /// Create a <see cref="AvaloniaUiRegion"/>
    /// </summary>
    /// <param name="uiWindowState">UI window state</param>
    /// <returns><see cref="UiWindowState"/> created and registered to region manager</returns>
    public static WindowState ToWindowState(this UiWindowState uiWindowState)
    {
        switch (uiWindowState)
        {
            case UiWindowState.Normal:
                return WindowState.Normal;
            case UiWindowState.Minimized:
                return WindowState.Minimized;
            case UiWindowState.Maximized:
                return WindowState.Maximized;
            default:
                throw new ArgumentOutOfRangeException(nameof(uiWindowState), uiWindowState, null);
        }
    }

}
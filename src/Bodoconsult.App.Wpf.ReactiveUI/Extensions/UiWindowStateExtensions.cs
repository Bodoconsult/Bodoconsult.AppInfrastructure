// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Windows;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Ui;
using Bodoconsult.App.Wpf.ReactiveUI.Regions;

namespace Bodoconsult.App.Wpf.ReactiveUI.Extensions;

/// <summary>
/// Extension methods for <see cref="IRegionManager"/>
/// </summary>
public static class UiWindowStateExtensions
{
    /// <summary>
    /// Create a <see cref="WpfUiRegion"/>
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
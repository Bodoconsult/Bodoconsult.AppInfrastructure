// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.ReactiveUI.Ui;

/// <summary>Specifies whether a window is minimized, maximized, or restored. Used by the <see cref="P:System.Windows.Window.WindowState" /> property.</summary>
public enum UiWindowState
{
    /// <summary>The window is restored.</summary>
    Normal,
    /// <summary>The window is minimized.</summary>
    Minimized,
    /// <summary>The window is maximized.</summary>
    Maximized,
}
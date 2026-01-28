// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Reactive;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Bodoconsult.App.Wpf.ReactiveUI.Models;

/// <summary>
/// Represents a menu item for a menubar
/// </summary>
public partial class WpfUiMenuItem : ReactiveObject
{
    /// <summary>
    /// Ctor providing data
    /// </summary>
    /// <param name="header">Header text</param>
    /// <param name="command"></param>
    public WpfUiMenuItem(string header, ReactiveCommand<Unit, Unit> command)
    {
        Header = header;
        Command = command;
        Items = new();
    }

    /// <summary>
    /// Default ctor
    /// </summary>
    public WpfUiMenuItem()
    {
        Items = new();
    }

    /// <summary>
    /// Header text
    /// </summary>
    [Reactive] public partial string? Header { get; set; }

    /// <summary>
    /// Contains submenu items of the current menu item
    /// </summary>
    [Reactive]
    public partial List<WpfUiMenuItem> Items { get; set; } 

    /// <summary>
    /// Command to be executed by the menu item
    /// </summary>
    [Reactive] public partial ReactiveCommand<Unit, Unit>? Command { get; set; }

    /// <summary>
    /// Command name
    /// </summary>
    [Reactive] public partial string? CommandName { get; set; }

    /// <summary>
    /// Icon for the command button
    /// </summary>
    [Reactive] public partial object? Icon { get; set; }

    /// <summary>
    /// Is checkable?
    /// </summary>
    [Reactive] public partial bool IsCheckable { get; set; }

    /// <summary>
    /// Is checked?
    /// </summary>
    [Reactive] public partial bool IsChecked { get; set; }

    /// <summary>
    /// Is visible?
    /// </summary>

    [Reactive] public partial bool Visible { get; set; }

    /// <summary>
    /// Is separator
    /// </summary>
    [Reactive] public partial bool IsSeparator { get; set; }

    /// <summary>
    /// Input gesture text
    /// </summary>
    [Reactive] public partial string? InputGestureText { get; set; }

    /// <summary>
    /// Tooltip
    /// </summary>
    [Reactive] public partial string? ToolTip { get; set; }

    /// <summary>
    /// ID in the menu hierarchy
    /// </summary>
    [Reactive] public partial int MenuHierarchyId { get; set; }

    /// <summary>
    /// ID of the parent in the menu hierarchy
    /// </summary>
    [Reactive] public partial int ParentMenuHierarchyId { get; set; }

    /// <summary>
    /// Icon path
    /// </summary>
    [Reactive] public partial string? IconPath { get; set; }

    /// <summary>
    /// Is for admins only
    /// </summary>
    [Reactive] public partial bool IsAdminOnly { get; set; }

    /// <summary>
    /// Context object
    /// </summary>
    [Reactive] public partial object? Context { get; set; }

    /// <summary>
    /// Integer sequence
    /// </summary>
    [Reactive] public partial int IntSequence { get; set; }

    /// <summary>
    /// Integer key index
    /// </summary>
    [Reactive] public partial int IntKeyIndex { get; set; }
}
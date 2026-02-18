// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.ReactiveUI.Interfaces;

/// <summary>
/// Interface for platform independent menu item
/// </summary>
public interface IUiMenuItem
{
    /// <summary>
    /// Name of the menu item or an I18N identifier
    /// </summary>
    string Name { get;  }

    /// <summary>
    /// Parent menu item or null
    /// </summary>
    IUiMenuItem? Parent { get; set; }

    /// <summary>
    /// Stores the final parent object during menu building process
    /// </summary>
    object? ParentObject { get; set; }

    /// <summary>
    /// Is the menu item visible
    /// </summary>
    bool Visible { get; set; }

    /// <summary>
    /// Input gesture text
    /// </summary>
    string? InputGestureText { get; set; }

    /// <summary>
    /// Tooltip for the menu item
    /// </summary>
    string? ToolTip { get; set; }

    /// <summary>
    /// Style name to use for formatting the current menuitem
    /// </summary>
    string? StyleName { get; set; }

    /// <summary>
    /// Style resource path to use for formatting the current menuitem. Default: null
    /// </summary>
    string? StyleResourcePath { get; set; }

    /// <summary>
    /// Build the menu item
    /// </summary>
    void BuildIt();
}
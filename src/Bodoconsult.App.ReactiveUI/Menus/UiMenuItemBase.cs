// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Interfaces;

namespace Bodoconsult.App.ReactiveUI.Menus;

/// <summary>
/// Base class for <see cref="IUiMenuItem"/> implementations
/// </summary>
public abstract class UiMenuItemBase : IUiMenuItem
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="name">Name of the menu item or an I18N identifier</param>
    public UiMenuItemBase(string name)
    {
        Name=name;
    }

    /// <summary>
    /// Name of the menu item or an I18N identifier
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Parent menu item or null
    /// </summary>
    public IUiMenuItem? Parent { get; set; }

    /// <summary>
    /// Stores the final parent object during menu building process
    /// </summary>
    public object? ParentObject { get; set; }

    /// <summary>
    /// Is the menu item visible
    /// </summary>
    public bool Visible { get; set; }

    /// <summary>
    /// Input gesture text
    /// </summary>
    public string? InputGestureText { get; set; }

    /// <summary>
    /// Tooltip for the menu item
    /// </summary>
    public string? ToolTip { get; set; }

    /// <summary>
    /// Style name to use for formatting the current menuitem
    /// </summary>
    public string? StyleName { get; set; }

    /// <summary>
    /// Style resource path to use for formatting the current menuitem. Default: null
    /// </summary>
    public string? StyleResourcePath { get; set; }

    /// <summary>
    /// Build the menu item
    /// </summary>
    public virtual void BuildIt()
    {
        throw new NotSupportedException("Override in derived classes!");
    }
}
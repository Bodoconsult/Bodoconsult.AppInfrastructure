// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.ReactiveUI.Interfaces;

/// <summary>
/// Defines a viewmodel for a window with a menu
/// </summary>
public interface IUiMenuWindow
{
    /// <summary>
    /// Menu items for a menu in the window
    /// </summary>
    public List<IUiMenuItem> MenuItems { get; }

    /// <summary>
    /// <see cref="IUiMenuBuilder"/> instance used for the current window
    /// </summary>
    public IUiMenuBuilder? MenuBuilder { get; set; }

    /// <summary>
    /// Define the menu items to be stored in <see cref="MenuItems"/>
    /// </summary>
    public void DefineMenuItems();

    /// <summary>
    /// Build the menu with the menu builder <see cref="MenuBuilder"/> from the menu items <see cref="MenuItems"/>
    /// </summary>
    void BuildIt();
}
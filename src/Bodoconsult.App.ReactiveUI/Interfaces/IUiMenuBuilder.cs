// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.ReactiveUI.Interfaces;

/// <summary>
/// Interface for menu builder implementations
/// </summary>
public interface IUiMenuBuilder
{
    /// <summary>
    /// Current translation service
    /// </summary>
    II18N TranslationService { get; }

    /// <summary>
    /// Is this menu builder used to build the app main menu?
    /// </summary>
    bool IsMainMenu { get; set; }

    /// <summary>
    /// Readonly list of all menu items. Must contain at least one element without parent menu item
    /// </summary>
    List<IUiMenuItem> MenuItems { get; }

    /// <summary>
    /// List of all menu items. Must contain at least one element without parent menu item
    /// </summary>
    List<IUiMenuItem> TopLevelMenuItems { get; }

    /// <summary>
    /// Add a menu item
    /// </summary>
    /// <param name="item">Menu item to add</param>
    void Add(IUiMenuItem item);

    /// <summary>
    /// Add a range of menu items. Checks performed as with Add(item)
    /// </summary>
    /// <param name="items">List with menu items</param>
    void AddRange(IEnumerable<IUiMenuItem>  items);

    /// <summary>
    /// Build the menu from the menu items
    /// </summary>
    void BuildIt();

    /// <summary>
    /// Build a single menu item
    /// </summary>
    /// <param name="item">Current menu item</param>
    void BuildMenuItem(IUiMenuItem item);
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.ReactiveUI.Delegates;
using Bodoconsult.App.ReactiveUI.Menus;

namespace Bodoconsult.App.ReactiveUI.Interfaces;

/// <summary>
/// Interface for menu builder implementations
/// </summary>
public interface IUiMenuBuilder
{
    /// <summary>
    /// Menu is ready built delegate
    /// </summary>
    MenuBuiltDelegate? MenuBuiltDelegate { get; set; }

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
    /// <param name="parentItem">Parent menu item or null if item is part of the top level</param>
    void BuildMenuItem(IUiMenuItem item, IUiMenuItem? parentItem);

    /// <summary>
    /// Build the final object for a <see cref="CommandUiMenuItem"/>
    /// </summary>
    /// <param name="item">Command menu item</param>
    /// <param name="parentItem">Parent item or null</param>
    void BuildCommandUiMenuItem(CommandUiMenuItem item, GroupUiMenuItem? parentItem);

    /// <summary>
    /// Build the final object for a <see cref="GroupUiMenuItem"/>
    /// </summary>
    /// <param name="item">Command menu item</param>
    /// <param name="parentItem">Parent item or null</param>
    void BuildGroupUiMenuItem(GroupUiMenuItem item, GroupUiMenuItem? parentItem);

    /// <summary>
    /// Build the final object for a <see cref="SeparatorUiMenuItem"/>
    /// </summary>
    /// <param name="item">Command menu item</param>
    /// <param name="parentItem">Parent item or null</param>
    void BuildSeparatorUiMenuItem(SeparatorUiMenuItem item, GroupUiMenuItem? parentItem);

    /// <summary>
    /// Clear existing menu items
    /// </summary>
    void Clear();
}
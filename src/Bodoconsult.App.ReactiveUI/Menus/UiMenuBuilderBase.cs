// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.ReactiveUI.Delegates;
using Bodoconsult.App.ReactiveUI.Interfaces;

namespace Bodoconsult.App.ReactiveUI.Menus;

/// <summary>
/// Base implementation for <see cref="IUiMenuBuilder"/>
/// </summary>
public abstract class UiMenuBuilderBase : IUiMenuBuilder
{
    private readonly List<IUiMenuItem> _menuItems = [];

    /// <summary>
    /// Menu is ready built delegate
    /// </summary>
    public MenuBuiltDelegate? MenuBuiltDelegate { get; set; }

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="translationService">Current translation service</param>
    protected UiMenuBuilderBase(II18N translationService)
    {
        TranslationService = translationService;
    }

    /// <summary>
    /// Current translation service
    /// </summary>
    public II18N TranslationService { get; }

    /// <summary>
    /// Is this menu builder used to build the app main menu? Default: false
    /// </summary>
    public bool IsMainMenu { get; set; }

    /// <summary>
    /// Readonly list of all menu items. Must contain at least one element without parent menu item
    /// </summary>
    public IReadOnlyList<IUiMenuItem> MenuItems => _menuItems.ToArray();

    /// <summary>
    /// List of all menu items. Must contain at least one element without parent menu item
    /// </summary>
    public IReadOnlyList<IUiMenuItem> TopLevelMenuItems => _menuItems.Where(x => x.Parent is null).ToArray();

    /// <summary>
    /// Add a menu item to the menu items if the name is not null or string.Empty. For top-level menu items it is checked if the name is unique for the top-level menu items
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="ArgumentNullException">item.Name must not be null or string.Empty</exception>
    /// <exception cref="ArgumentException">Top-level names of menu items must be unique</exception>
    public void Add(IUiMenuItem item)
    {
        if (string.IsNullOrEmpty(item.Name))
        {
            ArgumentNullException.ThrowIfNull(item.Name, "item.Name must not be null or string.Empty");
        }

        if (item.Parent is null)
        {
            if (TopLevelMenuItems.Any(x => x.Name == item.Name))
            {
                throw new ArgumentException($"Item {item.Name} already exists on top-level!");
            }
        }

        _menuItems.Add(item);
    }

    /// <summary>
    /// Add a range of menu items. Checks performed as with Add(item)
    /// </summary>
    /// <param name="items">List with menu items</param>
    public void AddRange(IEnumerable<IUiMenuItem> items)
    {
        foreach (var item in items)
        {
            Add(item);
        }
    }

    /// <summary>
    /// Build the menu from the menu items
    /// </summary>
    public void BuildIt()
    {
        foreach (var item in TopLevelMenuItems)
        {
            BuildMenuItem(item, null);
        }

        if (MenuBuiltDelegate is null)
        {
            return;
        }

        AsyncHelper.FireAndForget(() =>
        {
            Task.Delay(100);
            MenuBuiltDelegate.Invoke();
        });
    }

    /// <summary>
    /// Build a single menu item
    /// </summary>
    /// <param name="item">Current menu item</param>
    /// <param name="parentItem">Parent menu item or null if item is part of the top level</param>
    public void BuildMenuItem(IUiMenuItem item, IUiMenuItem? parentItem)
    {
        switch (item)
        {
            // Group item
            case GroupUiMenuItem group:
            {
                // Build the group
                if (parentItem is GroupUiMenuItem parentGroup)
                {
                    BuildGroupUiMenuItem(group, parentGroup);
                }
                else
                {
                    BuildGroupUiMenuItem(group, null);
                }

                // Build group childs
                foreach (var child in group.Childs)
                {
                    BuildMenuItem(child, group);
                }

                break;
            }
            // Command item in a group
            case CommandUiMenuItem command when parentItem is GroupUiMenuItem parentGroup:
                BuildCommandUiMenuItem(command, parentGroup);
                break;
            // Command item top level
            case CommandUiMenuItem command:
                BuildCommandUiMenuItem(command, null);
                break;
            // Separator item in a group
            case SeparatorUiMenuItem separator when parentItem is GroupUiMenuItem parentGroup:
                BuildSeparatorUiMenuItem(separator, parentGroup);
                break;
            // Separator item top level
            case SeparatorUiMenuItem separator:
                BuildSeparatorUiMenuItem(separator, null);
                break;
        }
    }

    /// <summary>
    /// Build the final object for a <see cref="CommandUiMenuItem"/>
    /// </summary>
    /// <param name="item">Command menu item</param>
    /// <param name="parentItem">Parent item or null</param>
    public virtual void BuildCommandUiMenuItem(CommandUiMenuItem item, GroupUiMenuItem? parentItem)
    {
        throw new NotSupportedException("Override this method in your derived class");
    }

    /// <summary>
    /// Build the final object for a <see cref="GroupUiMenuItem"/>
    /// </summary>
    /// <param name="item">Command menu item</param>
    /// <param name="parentItem">Parent item or null</param>
    public virtual void BuildGroupUiMenuItem(GroupUiMenuItem item, GroupUiMenuItem? parentItem)
    {
        throw new NotSupportedException("Override this method in your derived class");
    }

    /// <summary>
    /// Build the final object for a <see cref="SeparatorUiMenuItem"/>
    /// </summary>
    /// <param name="item">Command menu item</param>
    /// <param name="parentItem">Parent item or null</param>
    public virtual void BuildSeparatorUiMenuItem(SeparatorUiMenuItem item, GroupUiMenuItem? parentItem)
    {
        throw new NotSupportedException("Override this method in your derived class");
    }

    /// <summary>
    /// Clear existing menu items
    /// </summary>
    public void Clear()
    {
       _menuItems.Clear();
    }
}
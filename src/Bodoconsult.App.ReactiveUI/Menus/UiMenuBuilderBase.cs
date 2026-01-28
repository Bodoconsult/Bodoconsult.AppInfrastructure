// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.ReactiveUI.Interfaces;

namespace Bodoconsult.App.ReactiveUI.Menus;

/// <summary>
/// Base implementation for <see cref="IUiMenuBuilder"/>
/// </summary>
public abstract class UiMenuBuilderBase : IUiMenuBuilder
{
    private readonly List<IUiMenuItem> _menuItems = new();

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
    public List<IUiMenuItem> MenuItems => _menuItems.ToList();

    /// <summary>
    /// List of all menu items. Must contain at least one element without parent menu item
    /// </summary>
    public List<IUiMenuItem> TopLevelMenuItems => MenuItems.Where(x=> x.Parent==null).ToList();

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
            throw new ArgumentNullException(nameof(item.Name), "item.Name must not be null or string.Empty");
        }

        if (item.Parent == null)
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
            BuildMenuItem(item);
        }
    }

    /// <summary>
    /// Build a single menu item
    /// </summary>
    /// <param name="item">Current menu item</param>
    public virtual void BuildMenuItem(IUiMenuItem item)
    {
        throw new NotSupportedException("Override this method in your derived class");
    }
}
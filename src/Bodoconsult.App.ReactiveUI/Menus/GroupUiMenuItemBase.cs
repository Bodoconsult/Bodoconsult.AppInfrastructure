using Bodoconsult.App.ReactiveUI.Interfaces;

namespace Bodoconsult.App.ReactiveUI.Menus;

/// <summary>
/// A menu group item. Will be transformed to a submenu or a ribbon tab etc. later
/// </summary>
public class GroupUiMenuItemBase : UiMenuItemBase
{
    /// <summary>
    /// Child menu items
    /// </summary>
    private readonly List<IUiMenuItem> _childs = new();

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="name">Name of the menu item or an I18N identifier</param>
    public GroupUiMenuItemBase(string name) : base(name)
    { }

    /// <summary>
    /// Child menu items (readonly access only)
    /// </summary>
    public List<IUiMenuItem> Childs => _childs.ToList();

    /// <summary>
    /// Add a child menu item to the group
    /// </summary>
    /// <param name="menuItem">Child menu item to add</param>
    public void AddChild(IUiMenuItem menuItem)
    {
        _childs.Add(menuItem);
    }
}
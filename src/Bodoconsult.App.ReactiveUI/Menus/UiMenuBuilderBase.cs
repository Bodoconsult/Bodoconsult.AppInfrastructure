using Bodoconsult.App.ReactiveUI.Interfaces;

namespace Bodoconsult.App.ReactiveUI.Menus;

/// <summary>
/// Base implementation for <see cref="IUiMenuBuilder"/>
/// </summary>
public class UiMenuBuilderBase : IUiMenuBuilder
{
    /// <summary>
    /// List of all menu items. Must contain at least one element without parent menu item
    /// </summary>
    public List<IUiMenuItem> MenuItems { get; } = new();

    /// <summary>
    /// Build the menu from the menu items
    /// </summary>
    public virtual void BuildIt()
    {
        throw new NotSupportedException("Override this method in your derived class");
    }
}
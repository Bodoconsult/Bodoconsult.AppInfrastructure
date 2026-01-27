namespace Bodoconsult.App.ReactiveUI.Interfaces;

/// <summary>
/// Interface for menu builder implementations
/// </summary>
public interface IUiMenuBuilder
{
    /// <summary>
    /// List of all menu items. Must contain at least one element without parent menu item
    /// </summary>
    List<IUiMenuItem> MenuItems { get; }

    /// <summary>
    /// Build the menu from the menu items
    /// </summary>
    void BuildIt();

}
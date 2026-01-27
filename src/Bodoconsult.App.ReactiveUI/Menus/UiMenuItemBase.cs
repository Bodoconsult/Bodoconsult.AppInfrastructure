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
    /// Build the menu item
    /// </summary>
    public virtual void BuildIt()
    {
        throw new NotSupportedException("Override in derived classes!");
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.ReactiveUI.Menus;

/// <summary>
/// A separator item
/// </summary>
public class SeparatorUiMenuItem: UiMenuItemBase
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="name">Name of the menu item or an I18N identifier</param>
    public SeparatorUiMenuItem(string name) : base(name)
    { }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Interfaces;

namespace Bodoconsult.App.ReactiveUI.Menus;

/// <summary>
/// A menu item executing a command. Will be transformed to a command button etc. later
/// </summary>
public class CommandUiMenuItem : UiMenuItemBase
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="name">Name of the menu item or an I18N identifier</param>
    public CommandUiMenuItem(string name) : base(name)
    {
        StyleName = "CommandUiMenuItemStyle";
    }

    /// <summary>
    /// Command definition
    /// </summary>
    public IUiCommandDefinition? CommandDefinition { get; set; }

    /// <summary>
    /// File or ressource name to a small 16x16 icon without path
    /// </summary>
    public string? SmallImagePath { get; set; }

    /// <summary>
    /// File or ressource name to a large 32x32 icon without path
    /// </summary>
    public string? LargeImagePath { get; set; }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using BodoCmsConsole.Business.Enums;

namespace BodoCmsConsole.Business.Model;

/// <summary>
/// URL representing a menu item
/// </summary>
public struct MenuUrl
{
    /// <summary>
    /// Menu URL
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Menu name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Menu level
    /// </summary>
    public MenuLevel Level { get; set; }
}
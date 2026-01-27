// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.ReactiveUI.Interfaces;

/// <summary>
/// UI window definition. Contains window related properties like the type of the window, the regions in the window and a factory method for the window
/// </summary>
public struct UiWindowDefinition
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="type">Type of the window</param>
    /// <param name="regions">Regions in this window</param>
    /// <param name="factory">Factory method to create a window instance</param>
    public UiWindowDefinition(Type type, List<string> regions, Func<IUiWindow>? factory)
    {
        WindowType = type;
        Regions = regions;
        Factory = factory;
    }

    /// <summary>
    /// Type of the window
    /// </summary>
    public Type WindowType { get;  }

    /// <summary>
    /// Regions in this window
    /// </summary>
    public List<string> Regions { get; }

    /// <summary>
    /// Factory method to create a window instance
    /// </summary>
    public Func<IUiWindow>? Factory { get; }
}
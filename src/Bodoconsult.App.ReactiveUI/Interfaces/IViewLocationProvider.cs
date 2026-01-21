// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using ReactiveUI;

namespace Bodoconsult.App.ReactiveUI.Interfaces;

/// <summary>
/// Provider delivering viewmodel-view-mappings for the given DefaultViewLocator 
/// </summary>
public interface IViewLocationProvider
{
    /// <summary>
    /// Locator
    /// </summary>
    DefaultViewLocator Locator { get; }

    /// <summary>
    /// Create the viewmodel-view-mappings for the given locator
    /// </summary>
    void CreateMappings();
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Windows.Controls.Ribbon;

namespace Bodoconsult.App.Wpf.Interfaces;

/// <summary>
/// Interface for a service creating a ribbon
/// </summary>
public interface IRibbonService
{
    /// <summary>
    /// The current ribbon control to build
    /// </summary>
    Ribbon CurrentRibbon { get; set; }

    /// <summary>
    /// Add a ribbon item to the quick access bar
    /// </summary>
    /// <param name="ribbonItem"></param>
    void AddQuickAccessItem(RibbonItem ribbonItem);

    /// <summary>
    /// Add a ribbon item to the application menu
    /// </summary>
    /// <param name="ribbonItem"></param>
    void AddApplicationMenuItem(RibbonItem ribbonItem);

    /// <summary>
    /// Add a ribbon item to a tab
    /// </summary>
    /// <param name="ribbonItem"></param>
    void AddTabItem(RibbonItem ribbonItem);

    /// <summary>
    /// Override this method to add your own items to the ribbon. Ribbon starts completely empty otherwise.
    /// </summary>
    void DefineRibbonItems();

    /// <summary>
    /// Build the concrete ribbon. Called normally in the view model of the main menu
    /// </summary>
    void BuildRibbon();
}
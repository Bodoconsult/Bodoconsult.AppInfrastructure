// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Regions;
using ReactiveUI;

namespace Bodoconsult.App.ReactiveUI.Interfaces;

/// <summary>
/// Viewmodel for region based navigation
/// </summary>
public interface IUiRegionViewModel: IRoutableViewModel
{
    /// <summary>
    /// Method based late injection of <see cref="IScreen"/> instance for navigation
    /// </summary>
    /// <param name="screen"></param>
    void InjectScreen(UiRegion  screen);

    /// <summary>
    /// Current UI region
    /// </summary>
    UiRegion? UiRegion { get; }
}
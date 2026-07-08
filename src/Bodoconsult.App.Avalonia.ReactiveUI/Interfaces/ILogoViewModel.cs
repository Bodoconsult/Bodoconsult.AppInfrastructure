// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Avalonia.Media.Imaging;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Regions;
using ReactiveUI;

namespace Bodoconsult.App.Avalonia.ReactiveUI.Interfaces;

/// <summary>
/// Viewmodel interface for start screen with logo and app title
/// </summary>
public interface ILogoViewModel: IUiRegionViewModel
{
    /// <summary>
    /// Menu text for open menu in system tray bar
    /// </summary>
    string AppTitle { get; set; }

    /// <summary>
    /// Current logo to show
    /// </summary>
    Bitmap? Logo { get; set; }

    /// <summary>
    /// Load logo from ressources defined in <see cref="IAppGlobals"/>.AppStartParameter.LogoRessourcePath
    /// </summary>
    void LoadLogoFromRessources();

    /// <summary>
    /// Load logo from ressources defined in <see cref="IAppGlobals"/>.AppStartParameter.LogoRessourcePath
    /// </summary>
    void LoadLogoFromFile(string filename);
}
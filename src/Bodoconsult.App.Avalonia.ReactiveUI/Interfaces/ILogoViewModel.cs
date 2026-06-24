// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Avalonia.Media.Imaging;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.ReactiveUI.Regions;
using ReactiveUI;

namespace Bodoconsult.App.Avalonia.ReactiveUI.Interfaces;

/// <summary>
/// Viewmodel interface for start screen with logo and app title
/// </summary>
public interface ILogoViewModel
{
    /// <summary>
    /// Gets a string token representing the current ViewModel, such as 'login' or 'user'.
    /// </summary>
    string UrlPathSegment { get; }

    /// <summary>
    /// Gets the IScreen that this ViewModel is currently being shown in. This
    /// is usually passed into the ViewModel in the Constructor and saved
    /// as a ReadOnly Property.
    /// </summary>
    IScreen HostScreen { get; }

    /// <summary>
    /// Method based late injection of <see cref="IScreen"/> instance for navigation
    /// </summary>
    /// <param name="screen"></param>
    void InjectScreen(UiRegion screen);

    /// <summary>
    /// UI region the viewmodel is loaded in
    /// </summary>
    UiRegion? UiRegion { get; }

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
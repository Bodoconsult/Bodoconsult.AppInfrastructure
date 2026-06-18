// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Avalonia.Media.Imaging;
using Bodoconsult.App.Abstractions.Interfaces;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Bodoconsult.App.Avalonia.ReactiveUI.ViewModels;

/// <summary>
/// Viewmodel for a logo 
/// </summary>
public partial class LogoViewModel : ReactiveObject
{
    private readonly IAppGlobals _appGlobals;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Current app globals</param>
    public LogoViewModel(IAppGlobals appGlobals)
    {
        _appGlobals = appGlobals;

        var modulInfo = $"{AppTitle} {_appGlobals.AppStartParameter.AppVersion}";
        AppTitle = modulInfo;



    }

    /// <summary>
    /// Menu text for open menu in system tray bar
    /// </summary>
    [Reactive]
    public partial string AppTitle { get; set; }

    /// <summary>
    /// Current logo to show
    /// </summary>
    [Reactive]
    public partial Bitmap? Logo { get; set; }

    /// <summary>
    /// Load logo from ressources defined in <see cref="IAppGlobals"/>.AppStartParameter.LogoRessourcePath
    /// </summary>
    public void LoadLogoFromRessources()
    {
        if (string.IsNullOrEmpty(_appGlobals.AppStartParameter.LogoRessourcePath))
        {
            return;
        }

        if (_appGlobals.AppStartParameter.LogoAssembly == null)
        {
            return;
        }

        var logoStream = _appGlobals.AppStartParameter.LogoAssembly.GetManifestResourceStream(_appGlobals.AppStartParameter.LogoRessourcePath);

        if (logoStream == null)
        {
            return;
        }

        logoStream.Position = 0;
        Logo = new Bitmap(logoStream);
    }

    /// <summary>
    /// Load logo from ressources defined in <see cref="IAppGlobals"/>.AppStartParameter.LogoRessourcePath
    /// </summary>
    public void LoadLogoFromFile(string filename)
    {
        if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
        {
            return;
        }

        var logoStream = new MemoryStream(File.ReadAllBytes(filename));
        logoStream.Position = 0;
        Logo = new Bitmap(logoStream);
    }
}
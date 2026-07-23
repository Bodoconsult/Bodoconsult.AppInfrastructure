// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Reflection;
using Avalonia.Media.Imaging;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Avalonia.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Regions;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Bodoconsult.App.Avalonia.ReactiveUI.ViewModels;

/// <summary>
/// Viewmodel for an image
/// </summary>
public partial class ImageViewModel : ReactiveObject, IImageViewModel
{
    /// <summary>
    /// Gets a string token representing the current view model, such as "login" or "user".
    /// </summary>
    public string UrlPathSegment => "ImageViewModel";

    /// <summary>
    /// UI region the viewmodel is loaded in
    /// </summary>
    public UiRegion? UiRegion { get; private set; }

    /// <summary>
    /// Gets the IScreen that this ViewModel is currently being shown in. This
    /// is usually passed into the ViewModel in the Constructor and saved
    /// as a ReadOnly Property.
    /// </summary>
    public IScreen HostScreen { get; private set; } = new DummyScreen();

    /// <summary>
    /// Current logo to show
    /// </summary>
    [Reactive]
    public partial Bitmap? Bitmap { get; set; }

    /// <summary>
    /// Load bitmap from file
    /// </summary>
    public void LoadBitmapFromFile(string filename)
    {
        if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
        {
            return;
        }

        var bitmapStream = new MemoryStream(File.ReadAllBytes(filename));
        bitmapStream.Position = 0;
        Bitmap = new Bitmap(bitmapStream);
    }

    /// <summary>
    /// Load bitmap
    /// </summary>
    /// <param name="bitmap">Bitmap array to load</param>
    public void LoadBitmap(Memory<byte> bitmap)
    {
        var bitmapStream = new MemoryStream(bitmap.ToArray());
        bitmapStream.Position = 0;
        Bitmap = new Bitmap(bitmapStream);
    }

    /// <summary>
    /// Load logo from ressources defined in <see cref="IAppGlobals"/>.AppStartParameter.LogoRessourcePath
    /// </summary>
    public void LoadLogoFromRessources(Assembly assembly, string ressourcePath)
    {
        if (string.IsNullOrEmpty(ressourcePath))
        {
            return;
        }
        
        var bitmapStream = assembly.GetManifestResourceStream(ressourcePath);

        if (bitmapStream == null)
        {
            return;
        }

        bitmapStream.Position = 0;
        Bitmap = new Bitmap(bitmapStream);
    }

    /// <summary>
    /// Method based late injection of <see cref="IScreen"/> instance for navigation
    /// </summary>
    /// <param name="screen"></param>
    public void InjectScreen(UiRegion screen)
    {
        HostScreen = screen;
        UiRegion = screen;
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Avalonia.Media.Imaging;
using Bodoconsult.App.ReactiveUI.Interfaces;

namespace Bodoconsult.App.Avalonia.ReactiveUI.Interfaces;

/// <summary>
/// Viewmodel interface for start screen with logo and app title
/// </summary>
public interface IImageViewModel : IUiRegionViewModel
{
    /// <summary>
    /// Current bitmap
    /// </summary>
    Bitmap? Bitmap { get; set; }

    /// <summary>
    /// Load bitmap from file
    /// </summary>
    /// <param name="filename">Local filename to load the file from</param>
    void LoadBitmapFromFile(string filename);

    /// <summary>
    /// Load bitmap
    /// </summary>
    /// <param name="bitmap">Bitmap array to load</param>
    public void LoadBitmap(Memory<byte> bitmap);
}
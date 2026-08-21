// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Avalonia.Input.Platform;
using Avalonia.Media.Imaging;
using Bodoconsult.App.ReactiveUI.Interfaces;
using ReactiveUI;
using ReactiveUI.Primitives;

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
    /// Title to show for the image
    /// </summary>
    string Title { get; set; }

    /// <summary>
    /// Button text for the SaveAsBitmap button
    /// </summary>
    string SaveAsBitmapText { get; set; }

    /// <summary>
    /// Button text for the SaveToClipboard button
    /// </summary>
    string SaveToClipboardText { get; set; }

    /// <summary>
    /// Save as bitmap command
    /// </summary>
    ReactiveCommand<RxVoid, RxVoid> SaveAsBitmapCommand { get; }

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

    /// <summary>
    /// Save the bitmap as JPEG file
    /// </summary>
    /// <param name="fileName">Full filename to save the bitmap in</param>
    /// <param name="quality">Quality 0 - 100</param>
    void SaveAsJpeg(string fileName, byte quality);

    /// <summary>
    /// Save the bitmap as PNG file
    /// </summary>
    /// <param name="fileName">Full filename to save the bitmap in</param>
    void SaveAsPng(string fileName);

    /// <summary>
    /// Task starting saving as bitmap
    /// </summary>
    Task<RxVoid> SaveAsBitmapCommandTask();

    /// <summary>
    /// Save the image to clipboard
    /// </summary>
    Task<RxVoid> SaveToClipboardCommandTask();
}
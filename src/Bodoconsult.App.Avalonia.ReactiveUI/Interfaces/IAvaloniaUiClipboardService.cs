// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Avalonia.Media.Imaging;
using Bodoconsult.App.ReactiveUI.Interfaces;

namespace Bodoconsult.App.Avalonia.ReactiveUI.Interfaces;

/// <summary>
/// Enhancements for <see cref="IUiClipboardService"/> for Avalonia
/// </summary>
public interface IAvaloniaUiClipboardService: IUiClipboardService
{
    /// <summary>
    /// Get a <see cref="Bitmap"/> from clipboard
    /// </summary>
    /// <returns>Bitmap or null</returns>

    Task<Bitmap?> GetBitmap();

    /// <summary>
    /// Set a <see cref="Bitmap"/> to the clipboard
    /// </summary>
    /// <param name="bitmap">Bitmap to copy to clipboard</param>
    /// <returns>Bitmap copied to clipboard or null if the bitmap was not copied to clipboard</returns>

    Task<Bitmap?> SetBitmap(Bitmap bitmap);
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Avalonia.Controls;
using Avalonia.Input.Platform;
using Avalonia.Media.Imaging;
using Bodoconsult.App.Avalonia.ReactiveUI.Interfaces;

namespace Bodoconsult.App.Avalonia.ReactiveUI.Services;

/// <summary>
/// Clipboard service based on Avalonia
/// </summary>
public class ClipboardService: IAvaloniaUiClipboardService
{
    private IClipboard? _clipboard;

    /// <summary>
    /// Default ctor
    /// </summary>
    public ClipboardService()
    { }

    /// <summary>
    /// Ctor providing a <see cref="TopLevel"/> instance
    /// </summary>
    /// <param name="topLevel"><see cref="TopLevel"/> instance to use</param>
    public ClipboardService(TopLevel topLevel)
    {
        _clipboard = topLevel.Clipboard;
    }

    /// <summary>
    /// Load the clipboard from <see cref="TopLevel"/> instance
    /// </summary>
    /// <param name="topLevel"><see cref="TopLevel"/> instance</param>
    public void LoadClipboard(TopLevel topLevel)
    {
        _clipboard = topLevel.Clipboard;
    }

    /// <summary>
    /// Get a text from the clipboard
    /// </summary>
    /// <returns>Text from clipboard or null</returns>
    public async Task<string?> GetText()
    {
        if (_clipboard is null)
        {
            return null;
        }

        var text = await _clipboard.TryGetTextAsync();
        return text;

    }

    /// <summary>
    /// Write a text to the clipboard
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public async Task<string?> SetText(string text)
    {
        if (_clipboard is not null)
        {
            await _clipboard.SetTextAsync(text);
            return text;
        }

        return null;
    }

    /// <summary>
    /// Get a PNG file from clipboard
    /// </summary>
    /// <returns>Stream is the PNG image or null</returns>

    public async Task<Stream?> GetPng()
    {
        if (_clipboard is null)
        {
            return null;
        }

        var bitmap = await _clipboard.TryGetBitmapAsync();
        if (bitmap is null)
        {
                
            return null;
        }

        var stream = new MemoryStream();
        bitmap.Save(stream, PngBitmapEncoderOptions.Default);

        stream.Position = 0;

        return stream;
    }

    /// <summary>
    /// Get a JPEG file from clipboard
    /// </summary>
    /// <param name="image">Image to copy to clipboard</param>
    /// <returns>Stream with the PNG image copied to clipboard or null if image was not copied to clipboard</returns>
    public async Task<Stream?> SetJpeg(Stream image)
    {
        return await SetBitmapInternal(image);
    }

    /// <summary>
    /// Get a PNG file from clipboard
    /// </summary>
    /// <param name="image">Image to copy to clipboard</param>
    /// <returns>Stream with the PNG image copied to clipboard or null if image was not copied to clipboard</returns>
    public async Task<Stream?> SetPng(Stream image)
    {
        return await SetBitmapInternal(image);
    }

    private async Task<Stream?> SetBitmapInternal(Stream image)
    {
        if (_clipboard is null)
        {
            return null;
        }

        var bitmap = new Bitmap(image);

        await _clipboard.SetBitmapAsync(bitmap);

        return image;
    }

    /// <summary>
    /// Get a JPEG file from clipboard
    /// </summary>
    /// <returns>Stream is the JPEG image or null</returns>

    public async Task<Stream?> GetJpeg()
    {
        if (_clipboard is null)
        {
            return null;
        }

        var bitmap = await _clipboard.TryGetBitmapAsync();
        if (bitmap is null)
        {

            return null;
        }

        var stream = new MemoryStream();
        bitmap.Save(stream, JpegBitmapEncoderOptions.Default);

        stream.Position = 0;

        return stream;
    }

    /// <summary>
    /// Get a <see cref="Bitmap"/> from clipboard
    /// </summary>
    /// <returns>Stream with the JPEG image or null</returns>

    public async Task<Bitmap?> GetBitmap()
    {
        if (_clipboard is null)
        {
            return null;
        }

        var bitmap = await _clipboard.TryGetBitmapAsync();
        return bitmap ?? null;
    }

    /// <summary>
    /// Set a <see cref="Bitmap"/> to the clipboard
    /// </summary>
    /// <param name="bitmap">Bitmap to copy to clipboard</param>
    /// <returns>Bitmap copied to clipboard or null if the bitmap was not copied to clipboard</returns>
    public async Task<Bitmap?> SetBitmap(Bitmap bitmap)
    {
        if (_clipboard is null)
        {
            
            return null;
        }
        await _clipboard.SetBitmapAsync(bitmap);

        return bitmap;
    }
}
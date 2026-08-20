// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.ReactiveUI.Interfaces;

/// <summary>
/// Interface for creating clipboard related services
/// </summary>
public interface IUiClipboardService
{
    /// <summary>
    /// Get a text from the clipboard
    /// </summary>
    /// <returns>Text from clipboard or null</returns>
    Task<string?> GetText();

    /// <summary>
    /// Write a text to the clipboard
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    Task<string?> SetText(string text);

    /// <summary>
    /// Get a JPEG file from clipboard
    /// </summary>
    /// <returns>Stream is the JPEG image or null</returns>

    Task<Stream?> GetJpeg();

    /// <summary>
    /// Get a PNG file from clipboard
    /// </summary>
    /// <returns>Stream is the PNG image or null</returns>

    Task<Stream?> GetPng();

    /// <summary>
    /// Get a JPEG file from clipboard
    /// </summary>
    /// <param name="image">Image to copy to clipboard</param>
    /// <returns>Stream with the PNG image copied to clipboard or null if image was not copied to clipboard</returns>

    Task<Stream?> SetJpeg(Stream image);

    /// <summary>
    /// Get a PNG file from clipboard
    /// </summary>
    /// <param name="image">Image to copy to clipboard</param>
    /// <returns>Stream with the PNG image copied to clipboard or null if image was not copied to clipboard</returns>

    Task<Stream?> SetPng(Stream image);

}
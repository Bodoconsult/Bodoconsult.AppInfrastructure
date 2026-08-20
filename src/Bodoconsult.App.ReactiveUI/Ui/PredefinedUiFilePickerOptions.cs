// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.ReactiveUI.Ui;

/// <summary>
/// Predefined file picker options
/// </summary>
public static class PredefinedUiFilePickerOptions
{
    /// <summary>
    /// Options for opening bitmaps as PNG or JPEG
    /// </summary>
    public static readonly UiFilePickerOpenOptions BitmapOpenOptions = new()
    {
        Title = "Select image",
        FileTypeFilter =
        [
            new UiFilePickerFileType("PNG Image") { Patterns = ["*.png"] },
            new UiFilePickerFileType("JPEG Image") { Patterns = ["*.jpg", "*.jpeg"] }
        ]
    };

    /// <summary>
    /// Options for opening bitmaps as TXT
    /// </summary>
    public static readonly UiFilePickerOpenOptions TextOpenOptions = new()
    {
        Title = "Select text file",
        FileTypeFilter =
        [
            new UiFilePickerFileType("TXT text file") { Patterns = ["*.txt"] }
        ]
    };

    /// <summary>
    /// Options for saving bitmaps as PNG or JPEG
    /// </summary>
    public static readonly UiFilePickerSaveOptions BitmapSaveOptions = new()
    {
        Title = "Export image",
        FileTypeChoices =
        [
            new UiFilePickerFileType("PNG Image") { Patterns = ["*.png"] },
            new UiFilePickerFileType("JPEG Image") { Patterns = ["*.jpg", "*.jpeg"] }
        ]
    };

    /// <summary>
    /// Options for saving bitmaps as TXT
    /// </summary>
    public static readonly UiFilePickerSaveOptions TextSaveOptions = new()
    {
        Title = "Export as text file",
        FileTypeChoices =
        [
            new UiFilePickerFileType("TXT text file") { Patterns = ["*.txt"] }
        ]
    };
}
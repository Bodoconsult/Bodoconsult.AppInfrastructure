// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.
// Based on Avalonia.Base, Version=12.1.1.0

namespace Bodoconsult.App.ReactiveUI.Ui;

/// <summary>
/// Represents a name mapped to the associated file types (extensions).
/// </summary>
public sealed class UiFilePickerFileType(string? name)
{
    /// <summary>File type name</summary>
    public string Name { get; } = name ?? string.Empty;

    /// <summary>
    /// List of extensions in GLOB format. I.e. "*.png" or "*.*"
    /// </summary>
    /// <remarks>Used on Windows, Linux and Browser platforms.</remarks>
    public IReadOnlyList<string>? Patterns { get; set; }

    /// <summary>List of extensions in MIME format</summary>
    /// <remarks>Used on Android, Linux and Browser platforms</remarks>
    public IReadOnlyList<string>? MimeTypes { get; set; }
}
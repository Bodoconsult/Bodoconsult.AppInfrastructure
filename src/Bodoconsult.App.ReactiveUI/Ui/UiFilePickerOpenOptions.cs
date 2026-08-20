// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.
// Based on Avalonia.Base, Version=12.1.1.0

namespace Bodoconsult.App.ReactiveUI.Ui;

/// <summary>
/// Options class for open a file picker method.
/// </summary>
public class UiFilePickerOpenOptions : UiPickerOptions
{
    /// <summary>
    /// Gets or sets the file type that should be preselected when the dialog is opened.
    /// </summary>
    /// <remarks>
    /// This value should reference one of the items in <see cref="FileTypeFilter" />.
    /// If not set, the first file type in <see cref="FileTypeFilter" /> may be selected by default.
    /// </remarks>
    public UiFilePickerFileType? SuggestedFileType { get; set; }

    /// <summary>
    /// Gets or sets an option indicating whether open picker allows users to select multiple files.
    /// </summary>
    public bool AllowMultiple { get; set; }

    /// <summary>
    /// Gets or sets the collection of file types that the file open picker displays.
    /// </summary>
    public IReadOnlyList<UiFilePickerFileType>? FileTypeFilter { get; set; }
}
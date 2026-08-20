// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.
// Based on Avalonia.Base, Version=12.1.1.0

namespace Bodoconsult.App.ReactiveUI.Ui;

/// <summary>
/// Options for file picker for saving a file
/// </summary>
public sealed class UiFilePickerSaveOptions : UiPickerOptions
{
    /// <summary>
    /// Gets or sets the file type that should be preselected when the dialog is opened.
    /// </summary>
    /// <remarks>
    /// This value should reference one of the items in <see cref="FileTypeChoices" />.
    /// If not set, the first file type in <see cref="FileTypeChoices" /> may be selected by default.
    /// </remarks>
    public UiFilePickerFileType? SuggestedFileType { get; set; }

    /// <summary>
    /// Gets or sets the default extension to be used to save the file.
    /// </summary>
    public string? DefaultExtension { get; set; }

    /// <summary>
    /// Gets or sets the collection of valid file types that the user can choose to assign to a file.
    /// </summary>
    public IReadOnlyList<UiFilePickerFileType>? FileTypeChoices { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether file open picker displays a warning if the user specifies the name of a file that already exists.
    /// </summary>
    public bool? ShowOverwritePrompt { get; set; }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.
// Based on Avalonia.Base, Version=12.1.1.0

namespace Bodoconsult.App.ReactiveUI.Ui;

/// <summary>
/// Common options for file picker methods.
/// </summary>
public abstract class UiPickerOptions
{
    /// <summary>
    /// Gets or sets the text that appears in the title bar of a picker.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the file name that the file picker suggests to the user.
    /// </summary>
    public string? SuggestedFileName { get; set; }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Ui;

namespace Bodoconsult.App.ReactiveUI.Interfaces;

/// <summary>
/// Interface for file dialog services
/// </summary>
public interface IUiFileDialogService
{
    /// <summary>
    /// Open a single file
    /// </summary>
    /// <param name="options">Options for the dialog</param>
    /// <returns>Task providing a filename to open or null</returns>
    Task<IUiStorageFile?> OpenFileAsync(UiFilePickerOpenOptions options);

    /// <summary>
    /// Save a single file
    /// </summary>
    /// <param name="options">Options for the dialog</param>
    /// <returns>Task providing a filename to save in or null</returns>
    Task<IUiStorageFile?> SaveFileAsync(UiFilePickerSaveOptions options);
}
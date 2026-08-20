// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Avalonia.Controls;
using Avalonia;
using Bodoconsult.App.Avalonia.ReactiveUI.Extensions;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Ui;

namespace Bodoconsult.App.Avalonia.ReactiveUI.Services;

/// <summary>
/// Current implementation of <see cref="IUiFileDialogService"/> for Avalonia
/// </summary>
public class FileDialogService : IUiFileDialogService
{
    private  TopLevel? _topLevel;

    /// <summary>
    /// Default ctor
    /// </summary>
    public FileDialogService()
    {
    }

    /// <summary>
    /// Ctor loading a <see cref="TopLevel"/> instance
    /// </summary>
    /// <param name="topLevel"><see cref="TopLevel"/> instance to use. In most case it will be the main window instance</param>
    public FileDialogService(TopLevel topLevel)
    {
        _topLevel = topLevel;
    }

    /// <summary>
    /// Late load the required <see cref="TopLevel"/> instance
    /// </summary>
    /// <param name="topLevel"><see cref="TopLevel"/> instance to use</param>
    public void LoadTopLevel(TopLevel topLevel)
    {
        _topLevel = topLevel;
    }

    /// <summary>
    /// Get the TopLevel instance
    /// </summary>
    /// <param name="visual">Current visual</param>
    /// <returns>TopLevel instance or null</returns>
    public static TopLevel? GetTopLevel(Visual? visual)
    {
        var topLevel = TopLevel.GetTopLevel(visual);
        return topLevel;
    }

    /// <summary>
    /// Open a single file
    /// </summary>
    /// <param name="options">Options for the dialog</param>
    /// <returns>Task providing a filename to open or null</returns>
    public async Task<IUiStorageFile?> OpenFileAsync(UiFilePickerOpenOptions options)
    {
        ArgumentNullException.ThrowIfNull(_topLevel);

        var options1 = options.ToFilePickerOpenOptions();

        var files = await _topLevel.StorageProvider.OpenFilePickerAsync(options1);
        return files.Count >= 1 ? files[0].ToUiStorageFile() : null;
    }

    /// <summary>
    /// Save a single file
    /// </summary>
    /// <param name="options">Options for the dialog</param>
    /// <returns>Task providing a filename to save in or null</returns>
    public async Task<IUiStorageFile?> SaveFileAsync(UiFilePickerSaveOptions options)
    {
        ArgumentNullException.ThrowIfNull(_topLevel);

        var options1 = options.ToFilePickerSaveOptions();

        var result = await _topLevel.StorageProvider.SaveFilePickerAsync(options1);

        return result?.ToUiStorageFile();
    }
}
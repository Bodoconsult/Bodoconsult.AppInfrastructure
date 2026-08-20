//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Bodoconsult.App.Avalonia.ReactiveUI.Regions;
using Bodoconsult.App.Avalonia.ReactiveUI.Services;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Ui;

namespace Bodoconsult.App.Avalonia.ReactiveUI.Extensions;

/// <summary>
/// Extension methods for <see cref="IRegionManager"/>
/// </summary>
public static class AvaloniaUiExtensions
{
    /// <summary>
    /// Create a <see cref="AvaloniaUiRegion"/>
    /// </summary>
    /// <param name="windowState">Avalonia window state</param>
    /// <returns><see cref="UiWindowState"/> created and registered to region manager</returns>
    public static UiWindowState ToUiWindowState(this WindowState windowState)
    {
        return windowState switch
        {
            WindowState.Normal => UiWindowState.Normal,
            WindowState.Minimized => UiWindowState.Minimized,
            WindowState.Maximized => UiWindowState.Maximized,
            _ => throw new ArgumentOutOfRangeException(nameof(windowState), windowState, null)
        };
    }

    /// <summary>
    /// Create a <see cref="AvaloniaUiRegion"/>
    /// </summary>
    /// <param name="uiWindowState">UI window state</param>
    /// <returns><see cref="UiWindowState"/> created and registered to region manager</returns>
    public static WindowState ToWindowState(this UiWindowState uiWindowState)
    {
        return uiWindowState switch
        {
            UiWindowState.Normal => WindowState.Normal,
            UiWindowState.Minimized => WindowState.Minimized,
            UiWindowState.Maximized => WindowState.Maximized,
            _ => throw new ArgumentOutOfRangeException(nameof(uiWindowState), uiWindowState, null)
        };
    }

    /// <summary>
    /// Convert <see cref="IStorageFile"/> to <see cref="IUiStorageFile"/>
    /// </summary>
    /// <param name="storageFile">Current storage file</param>
    /// <returns><see cref="IUiStorageFile"/> instance</returns>
    public static IUiStorageFile ToUiStorageFile(this IStorageFile storageFile)
    {
        return new AvaloniaUiStorageFile(storageFile);
    }

    /// <summary>
    /// Convert <see cref="UiFilePickerOpenOptions"/> to <see cref="FilePickerOpenOptions"/>
    /// </summary>
    /// <param name="options"><see cref="UiFilePickerOpenOptions"/> instance to convert</param>
    /// <returns>Converted <see cref="FilePickerOpenOptions"/> instance</returns>
    public static FilePickerOpenOptions ToFilePickerOpenOptions(this UiFilePickerOpenOptions options)
    {
        var filters = new List<FilePickerFileType>();

        if (options.FileTypeFilter is not null)
        {
            foreach (var filter in options.FileTypeFilter)
            {
                filters.Add(filter.ToFilePickerFileType());
            }
        }

        var result = new FilePickerOpenOptions()
        {
            AllowMultiple = options.AllowMultiple,
            Title = options.Title,
            SuggestedFileName = options.SuggestedFileName,
            FileTypeFilter = filters
        };

        return result;
    }

    /// <summary>
    /// Convert <see cref="FilePickerFileType"/> to <see cref="UiFilePickerFileType"/>
    /// </summary>
    /// <param name="filePickerFileType"><see cref="FilePickerFileType"/> instance to convert</param>
    /// <returns>Converted <see cref="UiFilePickerFileType"/> instance</returns>
    public static FilePickerFileType ToFilePickerFileType(this UiFilePickerFileType filePickerFileType)
    {
        var result = new FilePickerFileType(filePickerFileType.Name)
        {
            MimeTypes = filePickerFileType.MimeTypes,
            Patterns = filePickerFileType.Patterns
        };

        return result;
    }

    /// <summary>
    /// Convert <see cref="UiFilePickerSaveOptions"/> to <see cref="FilePickerSaveOptions"/>
    /// </summary>
    /// <param name="options"><see cref="FilePickerOpenOptions"/> instance to convert</param>
    /// <returns>Converted <see cref="UiFilePickerOpenOptions"/> instance</returns>
    public static FilePickerSaveOptions ToFilePickerSaveOptions(this UiFilePickerSaveOptions options)
    {
        var filters = new List<FilePickerFileType>();

        if (options.FileTypeChoices is not null)
        {
            foreach (var filter in options.FileTypeChoices)
            {
                filters.Add(filter.ToFilePickerFileType());
            }
        }

        var result = new FilePickerSaveOptions()
        {
            Title = options.Title,
            SuggestedFileName = options.SuggestedFileName,
            FileTypeChoices = filters
        };

        return result;
    }
}

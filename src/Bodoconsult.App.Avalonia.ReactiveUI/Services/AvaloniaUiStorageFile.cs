// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Avalonia.Platform.Storage;
using Bodoconsult.App.ReactiveUI.Interfaces;
using SkiaSharp;
using System.Reflection.Metadata;

namespace Bodoconsult.App.Avalonia.ReactiveUI.Services;

/// <summary>
/// <see cref="IUiStorageFile"/> implementation for Avalonia
/// </summary>
public class AvaloniaUiStorageFile : IUiStorageFile
{
    private readonly IStorageFile _storageFile;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="storageFile"></param>
    public AvaloniaUiStorageFile(IStorageFile storageFile)
    {
        _storageFile = storageFile;
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        _storageFile.Dispose();
    }

    /// <summary>
    /// The full path of the storage file
    /// </summary>
    public Uri Path => _storageFile.Path;

    /// <summary>Opens a stream for read access.</summary>
    /// <exception cref="T:System.UnauthorizedAccessException" />
    public async Task<Stream> OpenReadAsync()
    {
        return await _storageFile.OpenReadAsync();
    }

    /// <summary>Opens stream for writing to the file.</summary>
    /// <exception cref="T:System.UnauthorizedAccessException" />
    public async Task<Stream> OpenWriteAsync()
    {
        return await _storageFile.OpenWriteAsync();
    }
}
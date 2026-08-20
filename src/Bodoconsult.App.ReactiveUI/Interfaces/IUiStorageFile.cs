// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.ReactiveUI.Interfaces;

/// <summary>
/// Represents a file in the storgae system
/// </summary>
public interface IUiStorageFile : IDisposable
{
    /// <summary>
    /// The full path of the storage file
    /// </summary>
    Uri Path { get; }

    /// <summary>Opens a stream for read access.</summary>
    /// <exception cref="T:System.UnauthorizedAccessException" />
    Task<Stream> OpenReadAsync();

    /// <summary>Opens stream for writing to the file.</summary>
    /// <exception cref="T:System.UnauthorizedAccessException" />
    Task<Stream> OpenWriteAsync();
}
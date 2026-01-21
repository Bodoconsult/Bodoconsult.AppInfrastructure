// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Wpf.ReactiveUI.Models;

/// <summary>
/// Helper class for transporting data and a title to views
/// </summary>
public class DataContainer<T>
{
    /// <summary>
    /// Title
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// The data object to transport
    /// </summary>
    public T? Data { get; set; }
}
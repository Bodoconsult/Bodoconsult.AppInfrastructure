// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.ViewModels;
using ReactiveUI.Avalonia;

namespace Bodoconsult.App.Avalonia.ReactiveUI.Views;

/// <summary>
/// Copyright window
/// </summary>
public partial class CopyrightWindow : ReactiveWindow<CopyrightViewModel>
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public CopyrightWindow()
    {
        InitializeComponent();
    }
}
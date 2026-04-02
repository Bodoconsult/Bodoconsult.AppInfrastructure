// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Avalonia.ReactiveUI;
using AvaloniaReactiveUiDemoApp.ViewModels;

namespace AvaloniaReactiveUiDemoApp.Views;

/// <summary>
/// Interaktionslogik für SecondView.xaml
/// </summary>
public partial class SecondView :  ReactiveUserControl<SecondViewModel>
{
    public SecondView()
    {
        InitializeComponent();
    }
}
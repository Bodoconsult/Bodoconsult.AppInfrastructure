// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using AvaloniaReactiveUiDemoApp.ViewModels;
using ReactiveUI.Avalonia;

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
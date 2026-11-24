// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using AvaloniaReactiveUiDemoApp.ViewModels;
using AvaloniaReactiveUiDemoApp.Views;
using Bodoconsult.App.Avalonia.ReactiveUI.Controls;
using Bodoconsult.App.Avalonia.ReactiveUI.ViewModels;
using Bodoconsult.App.Avalonia.ReactiveUI.Views;
using Bodoconsult.App.ReactiveUI.ViewModels;
using ReactiveUI;

namespace AvaloniaReactiveUiDemoApp.AppData;

public sealed class ViewModules : IViewModule
{
    /// <inheritdoc/>
    public void RegisterViews(DefaultViewLocator locator)
    {
        var mappingBuilder = locator.CreateMappingBuilder();

        mappingBuilder.Map<AvaloniaReactiveUiDemoAppMainWindowViewModel, MainWindow>();
        mappingBuilder.Map<FirstViewModel, FirstView>();
        mappingBuilder.Map<SecondViewModel, SecondView>();
        mappingBuilder.Map<LogoViewModel, LogoControl>();
        mappingBuilder.Map<CopyrightViewModel, CopyrightWindow>();
        mappingBuilder.Map<ImageViewModel, ImageControl>();
    }
}
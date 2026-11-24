// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.ViewModels;
using ReactiveUI;
using WpfReactiveUiDemoApp.ViewModels;
using WpfReactiveUiDemoApp.Views;

namespace WpfReactiveUiDemoApp.AppData;

public sealed class ViewModules : IViewModule
{
    /// <inheritdoc/>
    public void RegisterViews(DefaultViewLocator locator)
    {
        var mappingBuilder = locator.CreateMappingBuilder();

        mappingBuilder.Map<WpfReactiveUiDemoAppMainWindowViewModel, MainWindow>();
        mappingBuilder.Map<FirstViewModel, FirstView>();
        mappingBuilder.Map<SecondViewModel, SecondView>();
        //mappingBuilder.Map<LogoViewModel, LogoControl>();
        //mappingBuilder.Map<CopyrightViewModel, CopyrightWindow>();
        //mappingBuilder.Map<ImageViewModel, ImageControl>();
    }
}
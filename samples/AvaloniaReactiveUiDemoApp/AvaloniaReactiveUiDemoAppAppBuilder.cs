// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Avalonia.ReactiveUI.App;
using ReactiveUI;
using Bodoconsult.App.ReactiveUI.Interfaces;
using AvaloniaReactiveUiDemoApp.DiContainerProvider;
using AvaloniaReactiveUiDemoApp.ViewModels;
using AvaloniaReactiveUiDemoApp.Views;
using Bodoconsult.App.Avalonia.ReactiveUI.Controls;
using Bodoconsult.App.Avalonia.ReactiveUI.ViewModels;

namespace AvaloniaReactiveUiDemoApp;

public class AvaloniaReactiveUiDemoAppAppBuilder : BaseAvaloniaReactiveUiAppBuilder
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Global app settings</param>
    public AvaloniaReactiveUiDemoAppAppBuilder(IAppGlobals appGlobals) : base(appGlobals)
    { }

    /// <summary>
    /// Load the <see cref="IAppBuilder.DiContainerServiceProviderPackage"/>
    /// </summary>
    public override void LoadDiContainerServiceProviderPackage()
    {
        var factory = new AvaloniaReactiveUiDemoAppProductionDiContainerServiceProviderPackageFactory(AppGlobals);
        DiContainerServiceProviderPackage = factory.CreateInstance();
    }

    /// <summary>
    /// Load view location
    /// </summary>
    /// <param name="locator">The locator to use for the app instance</param>
    public override void LoadViewLocation(DefaultViewLocator locator)
    {
        locator.Map<FirstViewModel, FirstView>(() => new FirstView());
        locator.Map<SecondViewModel, SecondView>(() => new SecondView());
        locator.Map<LogoViewModel, LogoControl>(() => new LogoControl());
    }

    /// <summary>
    /// Create the view model for the main window
    /// </summary>
    public override IRxMainWindowViewModel CreateViewModel()
    {
        var viewModel = AppGlobals.DiContainer.Get<AvaloniaReactiveUiDemoAppMainWindowViewModel>();
        viewModel.HeaderBackColor = TypoColors.DarkBlue;
        viewModel.BodyBackColor = TypoColors.Beige;
        viewModel.AppExe = AppGlobals.AppStartParameter.AppExe;

        // Load the logo now
        viewModel.LoadLogo(AppGlobals.AppStartParameter.LogoAssembly, AppGlobals.AppStartParameter.LogoRessourcePath);

        return viewModel;
    }
}
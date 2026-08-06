// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Avalonia.ReactiveUI.App;
using Bodoconsult.App.ReactiveUI.Interfaces;
using AvaloniaReactiveUiDemoApp.DiContainerProvider;
using AvaloniaReactiveUiDemoApp.ViewModels;
using Bodoconsult.App.Avalonia.ReactiveUI.ViewModels;
using Bodoconsult.App.ReactiveUI.ViewModels;
using ReactiveUI.Builder;

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
    /// <param name="appB">The app builder to use for the app instance</param>
    public override void LoadViewLocation(IReactiveUIBuilder appB)
    {
        appB.WithViewsFromAssembly(this.GetType().Assembly);
        appB.WithViewsFromAssembly(typeof(LogoViewModel).Assembly);
        appB.WithViewsFromAssembly(typeof(CopyrightViewModel).Assembly);
    }

    /// <summary>
    /// Create the view model for the main window
    /// </summary>
    public override IRxMainWindowViewModel CreateViewModel()
    {
        var viewModel = AppGlobals.DiContainer.Get<AvaloniaReactiveUiDemoAppMainWindowViewModel>();
        viewModel.HeaderBackColor = TypoColors.DarkBlue;
        viewModel.BodyBackColor = TypoColors.Beige;
        viewModel.AppExe = AppGlobals.AppStartParameter.AppExe ?? string.Empty;

        // Load the logo now
        if (!string.IsNullOrEmpty(AppGlobals.AppStartParameter.LogoRessourcePath))
        {
            viewModel.LoadLogo(AppGlobals.AppStartParameter.LogoAssembly,
                AppGlobals.AppStartParameter.LogoRessourcePath);
        }

        return viewModel;
    }
}
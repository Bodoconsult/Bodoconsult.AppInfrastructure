// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

using Bodoconsult.App.Wpf.ReactiveUI.App;
using ReactiveUI;
using Bodoconsult.App.ReactiveUI.Interfaces;
using WpfReactiveUiDemoApp.DiContainerProvider;
using WpfReactiveUiDemoApp.ViewModels;
using WpfReactiveUiDemoApp.Views;

namespace WpfReactiveUiDemoApp;

public class WpfReactiveUiDemoAppAppBuilder : BaseWpfReactiveUiAppBuilder
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Global app settings</param>
    public WpfReactiveUiDemoAppAppBuilder(IAppGlobals appGlobals) : base(appGlobals)
    { }

    /// <summary>
    /// Load the <see cref="IAppBuilder.DiContainerServiceProviderPackage"/>
    /// </summary>
    public override void LoadDiContainerServiceProviderPackage()
    {
        var factory = new WpfReactiveUiDemoAppProductionDiContainerServiceProviderPackageFactory(AppGlobals);
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
    }

    /// <summary>
    /// Create the view model for the main window
    /// </summary>
    public override IRxMainWindowViewModel CreateViewModel()
    {
        var viewModel = AppGlobals.DiContainer.Get<WpfReactiveUiDemoAppMainWindowViewModel>();
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
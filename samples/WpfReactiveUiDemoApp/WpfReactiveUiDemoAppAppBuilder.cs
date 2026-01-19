// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Wpf.Interfaces;
using Bodoconsult.App.Wpf.ReactiveUI.App;
using System.Reflection;
using System.Windows.Media;
using WpfReactiveUiDemoApp.DiContainerProvider;

namespace WpfReactiveUiDemoApp;

public class WpfReactiveUiDemoAppAppBuilder : BaseWpfReactiveUiAppBuilder
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Global app settings</param>
    /// <param name="viewAssemblies">List with all assemblies to load views from</param>
    public WpfReactiveUiDemoAppAppBuilder(IAppGlobals appGlobals, List<Assembly> viewAssemblies) : base(appGlobals, viewAssemblies)
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
    /// Create the view model for the main window
    /// </summary>
    public override IMainWindowViewModel CreateViewModel()
    {
        var viewModel = AppGlobals.DiContainer.Get<IMainWindowViewModel>();
        viewModel.HeaderBackColor = Colors.DarkBlue;
        viewModel.BodyBackColor = Colors.Beige;
        viewModel.AppExe = AppGlobals.AppStartParameter.AppExe;

        // Load the logo now
        viewModel.LoadLogo(AppGlobals.AppStartParameter.LogoAssembly, AppGlobals.AppStartParameter.LogoRessourcePath);

        return viewModel;
    }
}
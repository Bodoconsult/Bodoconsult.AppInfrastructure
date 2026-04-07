// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Wpf.AppStarter;
using Bodoconsult.App.Wpf.Interfaces;

// ReSharper disable LocalizableElement

namespace Bodoconsult.App.Wpf.App;

/// <summary>
/// Base class for WinForms based <see cref="IAppBuilder"/> implementations
/// </summary>
public class BaseWpfAppBuilder: BaseAppBuilder
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Current IAppGlobals instance</param>
    public BaseWpfAppBuilder(IAppGlobals appGlobals) : base(appGlobals)
    { }

    /// <summary>
    /// The current view model for the form
    /// </summary>
    public IMainWindowViewModel MainWindowViewModel { get; set; }

    /// <summary>
    /// Start the application
    /// </summary>
    public override void StartApplication()
    {
        // Inject it to UI
        var appStarter = new WpfStarterUi(this, MainWindowViewModel);
        AppStarter = appStarter;

        // Run as singleton app
        if (AppGlobals.AppStartParameter.IsSingletonApp && appStarter.IsAnotherInstance)
        {
            Console.WriteLine($"Another instance of {AppGlobals.AppStartParameter.AppName} is already running! Press any key to proceed!");
            Console.ReadLine();
            Environment.Exit(0);
            return;
        }

        appStarter.Start();

        appStarter.Wait();
    }
}
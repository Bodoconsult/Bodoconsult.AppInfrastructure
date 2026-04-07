// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.WinForms.AppStarter;
using Bodoconsult.App.WinForms.Interfaces;

// ReSharper disable LocalizableElement

namespace Bodoconsult.App.WinForms.App;

/// <summary>
/// Base class for WinForms based <see cref="IAppBuilder"/> implementations
/// </summary>
public class BaseWinFormsAppBuilder: BaseAppBuilder
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Current <see cref="IAppGlobals"/> instance</param>
    public BaseWinFormsAppBuilder(IAppGlobals appGlobals) : base(appGlobals)
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
        var appStarter = new WinFormsStarterUi(this, MainWindowViewModel);
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
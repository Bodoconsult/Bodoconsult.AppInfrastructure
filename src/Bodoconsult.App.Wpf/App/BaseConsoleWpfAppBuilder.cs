// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Wpf.AppStarter;

// ReSharper disable LocalizableElement

namespace Bodoconsult.App.Wpf.App;

/// <summary>
/// Base class for a console app hosting a WPD dispatcher <see cref="IAppBuilder"/> implementations
/// </summary>
public class BaseConsoleWpfAppBuilder: BaseAppBuilder
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Current IAppGlobals instance</param>
    public BaseConsoleWpfAppBuilder(IAppGlobals appGlobals) : base(appGlobals)
    { }

    /// <summary>
    /// Start the application
    /// </summary>
    public override void StartApplication()
    {
        // Inject it to UI
        var appStarter = new ConsoleWpfAppStarterUi(this);
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
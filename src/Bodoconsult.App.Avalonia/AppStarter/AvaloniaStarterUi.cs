// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

using System.Diagnostics;
using System.Diagnostics.Tracing;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.AppStarter;
using Bodoconsult.App.Avalonia.AppStarter.ViewModels;
using Bodoconsult.App.Avalonia.Helpers;
using Bodoconsult.App.Avalonia.Interfaces;
using Bodoconsult.App.Logging;

// ReSharper disable LocalizableElement

namespace Bodoconsult.App.Avalonia.AppStarter;

/// <summary>
/// Implementation of <see cref="IAppStarterUi"/> for WinForms app
/// </summary>
public class AvaloniaStarterUi : BaseAppStarterUi
{
    private readonly AppEventListener _listener;

    private IMainWindowViewModel _viewModel;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appBuilder">Current IAppBuilder instance</param>
    public AvaloniaStarterUi(IAppBuilder appBuilder) : base(appBuilder)
    {

        if (OperatingSystem.IsWindows())
        {
            ConsoleService = new WinConsoleService();
        }
        else
        {
            ConsoleService = new ConsoleService();
        }

        //var minimumLogLevel = Globals.GetLoggingConfiguration().MinimumLogLevel;

        //MinimumLogLevel = LogLevel.Debug;

        //var eventLevel = MapLogLevelToEventLevel(minimumLogLevel);

        var eventLevel = EventLevel.Warning;

        _listener = new AppEventListener(eventLevel);

    }

    /// <summary>
    /// Ctor for using a customized form as main form of the application. The <see cref="IMainWindowViewModel"/> implementation based on <see cref="MainWindowViewModel"/> has to override CreateForm() method
    /// </summary>
    public AvaloniaStarterUi(IAppBuilder appBuilder, IMainWindowViewModel viewModel) : base(appBuilder)
    {

        if (OperatingSystem.IsWindows())
        {
            ConsoleService = new WinConsoleService();
        }
        else
        {
            ConsoleService = new ConsoleService();
        }

        //var minimumLogLevel = Globals.GetLoggingConfiguration().MinimumLogLevel;

        //MinimumLogLevel = LogLevel.Debug;

        //var eventLevel = MapLogLevelToEventLevel(minimumLogLevel);

        var eventLevel = EventLevel.Warning;

        _listener = new AppEventListener(eventLevel);

        _viewModel = viewModel;

    }

    ///// <summary>
    ///// Map the log level to EventSource event level
    ///// </summary>
    ///// <param name="logLevel">Log level</param>
    ///// <returns>Event level for EventSource</returns>
    //private static EventLevel MapLogLevelToEventLevel(LogLevel logLevel)
    //{
    //    switch (logLevel)
    //    {
    //        case LogLevel.Trace:
    //            return EventLevel.LogAlways;
    //        case LogLevel.Debug:
    //            return EventLevel.Verbose;
    //        case LogLevel.Information:
    //            return EventLevel.Informational;
    //        case LogLevel.Warning:
    //            return EventLevel.Warning;
    //        case LogLevel.Error:
    //            return EventLevel.Error;
    //        case LogLevel.Critical:
    //            return EventLevel.Critical;
    //        case LogLevel.None:
    //            return EventLevel.Error;
    //        default:
    //            throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
    //    }
    //}


    /// <summary>
    /// Wait while the application runs
    /// </summary>
    public override void Wait()
    {

        ConsoleService.ConsoleHandle = ConsoleService.CsGetConsoleWindow();
        ConsoleService.CsShowWindow(ConsoleService.ConsoleHandle, ConsoleService.ShowWindowShow);

        _viewModel ??= new MainWindowViewModel(_listener, null);
        _viewModel.LoadAppBuilder(AppBuilder);
        _viewModel.AppVersion = AppBuilder.AppGlobals.AppStartParameter.AppVersion;

        var window = _viewModel.CreateWindow();

        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
        {
            return;
        }

        // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
        // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
        DisableAvaloniaDataAnnotationValidation();
        desktop.MainWindow = window;
        window.Show();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        //// Get an array of plugins to remove
        //var dataValidationPluginsToRemove = bindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        //// remove each entry found
        //foreach (var plugin in dataValidationPluginsToRemove)
        //{
        //    BindingPlugins.DataValidators.Remove(plugin);
        //}
    }

    /// <summary>
    /// Show a message and then terminate the app
    /// </summary>
    /// <param name="message">Message to show before app termination</param>
    /// <param name="appTitle">App title to set</param>
    public override async void TerminateAppWithMessage(string message, string appTitle)
    {
        try
        {
            try
            {
                await AvaloniaStandardDialogHelper.ShowOkMessageBox(appTitle, message);
            }
            catch (Exception exception)
            {
                Debug.Print(exception.ToString());
                // ToDo: messageBox
                //MessageBox.Show($"{exception.Message} {exception.StackTrace}", AppBuilder.AppGlobals.AppStartParameter.AppName);
            }

            _viewModel.ShutDown();
            Environment.Exit(0);
        }
        catch (Exception exception)
        {
            Debug.Print(exception.ToString());
        }
    }


    /// <summary>
    /// Central handling for exceptions
    /// </summary>
    /// <param name="e"></param>
    public override async void HandleException(Exception e)
    {
        try
        {
            if (e == null)
            {
                return;
            }

            try
            {
                var msg = UiMessages.HandleException(e);
                await AvaloniaStandardDialogHelper.ShowOkMessageBox(AppBuilder.AppGlobals.AppStartParameter.AppName, msg);
            }
            catch (Exception exception)
            {
                Debug.Print(exception.ToString());
                await AvaloniaStandardDialogHelper.ShowOkMessageBox(AppBuilder.AppGlobals.AppStartParameter.AppName, $"{exception.Message} {exception.StackTrace}");
            }

            _viewModel?.ShutDown();
            Environment.Exit(0);
        }
        catch (Exception exception)
        {
            Debug.Print(exception.ToString());
        }
    }


    /// <summary>
    /// Current implementation of Dispose()
    /// </summary>
    /// <param name="disposing">Disposing required: true or false</param>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        try
        {
            _listener.Dispose();
        }
        catch //(Exception e)
        {
            // Do nothing
        }
    }
}
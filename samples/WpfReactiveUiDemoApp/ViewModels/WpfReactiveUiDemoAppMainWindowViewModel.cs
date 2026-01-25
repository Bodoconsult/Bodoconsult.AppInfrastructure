// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Windows;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.ReactiveUI.Extensions;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.Wpf.ReactiveUI.AppStarter.ViewModels;
using ReactiveUI.SourceGenerators;
using WpfReactiveUiDemoApp.Views;

namespace WpfReactiveUiDemoApp.ViewModels;

/// <summary>
/// ViewModel for MainWindow window
/// </summary>
public partial class WpfReactiveUiDemoAppMainWindowViewModel : MainWindowViewModel
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="listener">Current app event listener</param>
    /// <param name="translationService">Translation service</param>
    /// <param name="regionManager">Region manager</param>
    public WpfReactiveUiDemoAppMainWindowViewModel(IAppEventListener listener, II18N translationService,
        IRegionManager regionManager) : base(listener, translationService, regionManager)
    { }

    /// <summary>
    /// Create the main form of the application
    /// </summary>
    /// <returns></returns>
    public override Window CreateWindow()
    {
        var w = new MainWindow
        {
            ViewModel = this,
            WindowState = WindowState.Normal,
            Visibility = Visibility.Visible,
        };

        return w;
    }

    [ReactiveCommand]
    public void GoToFirstView()
    {
        try
        {
            Region1?.Navigate(new FirstViewModel(Region1));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [ReactiveCommand]
    public void GoToWindow1()
    {
        try
        {
            Region1?.Navigate(new FirstViewModel(Region1));

            var vm = new Window1ViewModel(RegionManager);

            RegionManager.Navigate(vm, "DocumentRegion");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
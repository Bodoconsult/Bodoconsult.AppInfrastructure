// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Windows;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Wpf.AppStarter.ViewModels;
using WpfApp1.Views;

namespace WpfApp1.ViewModels;

/// <summary>
/// ViewModel for MainWindow window
/// </summary>
public class WpfApp1MainWindowViewModel : MainWindowViewModel
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="listener">Current app event listener</param>
    /// <param name="translationService">Translation service</param>
    public WpfApp1MainWindowViewModel(IAppEventListener listener, II18N translationService) : base(listener, translationService)
    { }

    /// <summary>
    /// Create the main form of the application
    /// </summary>
    /// <returns></returns>
    public override Window CreateWindow()
    {
        var w = new MainWindow
        {
            WindowState = WindowState.Normal,
            Visibility = Visibility.Visible
        };
        w.InjectViewModel(this);
        return w; 
    }
}
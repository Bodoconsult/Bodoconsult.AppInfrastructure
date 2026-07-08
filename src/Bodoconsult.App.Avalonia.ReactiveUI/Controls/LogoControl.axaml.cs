using Avalonia.Controls;
using Bodoconsult.App.Avalonia.ReactiveUI.ViewModels;
using ReactiveUI.Avalonia;

namespace Bodoconsult.App.Avalonia.ReactiveUI.Controls;

/// <summary>
/// Logo cobtrol code behind
/// </summary>
public partial class LogoControl :  ReactiveUserControl<LogoViewModel>
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public LogoControl()
    {
        InitializeComponent();
    }
}
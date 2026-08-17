using Bodoconsult.App.Avalonia.ReactiveUI.ViewModels;
using ReactiveUI.Avalonia;

namespace Bodoconsult.App.Avalonia.ReactiveUI.Controls;

/// <summary>
/// Logo cobtrol code behind
/// </summary>
public partial class ImageControl :  ReactiveUserControl<ImageViewModel>
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public ImageControl()
    {
        InitializeComponent();
    }
}
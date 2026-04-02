using Avalonia.Controls;
using Avalonia.ReactiveUI;
using AvaloniaReactiveUiDemoApp.ViewModels;
using ReactiveUI;

namespace AvaloniaReactiveUiDemoApp.Views;

/// <summary>
/// Interaktionslogik für FirstView.xaml
/// </summary>
public partial class FirstView: ReactiveUserControl<FirstViewModel>
{
    public FirstView()
    {
        InitializeComponent();
            
        //this.WhenAnyValue(x => x.ViewModel).BindTo(this, x => x.DataContext);


        this.WhenActivated(
            d =>
            {
                d(
                    this.Bind(ViewModel, vm => vm.Test, view => view.PathTextBlock.Text)
                );
            });
    }
}
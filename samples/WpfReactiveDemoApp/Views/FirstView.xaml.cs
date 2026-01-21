using ReactiveUI;

namespace WpfReactiveDemoApp.Views;

/// <summary>
/// Interaktionslogik für FirstView.xaml
/// </summary>
public partial class FirstView
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
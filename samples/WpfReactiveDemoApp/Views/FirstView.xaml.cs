using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables.Fluent;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
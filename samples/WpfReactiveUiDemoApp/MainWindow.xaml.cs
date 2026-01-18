// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Reactive.Disposables.Fluent;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Bodoconsult.App.Wpf.ReactiveUI.AppStarter.ViewModels;
using Bodoconsult.App.Wpf.ReactiveUI.Interfaces;
using ReactiveUI;
using WpfReactiveUiDemoApp.ViewModels;

namespace WpfReactiveUiDemoApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void InjectViewModel(MainWindowViewModel viewModel)
        {
            ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

            var rm = ViewModel.RegionManager;

            var region1 = new WpfUiRegion(this.DocumentRegion);
            rm.RegisterRegion(region1);

            //this.WhenActivated(disposables =>
            //{
            //    // Bind the view model router to RoutedViewHost.Router property.
            //    this.OneWayBind(ViewModel, x => x.Router, x => x.DocumentRegion.Router)
            //        .DisposeWith(disposables);
            //});

            rm.Navigate(nameof(this.DocumentRegion), new ViewModel1(this.ViewModel));
        }
    }
}
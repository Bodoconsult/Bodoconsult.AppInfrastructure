// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Bodoconsult.App.Wpf.ReactiveUI.ViewModels;

namespace Bodoconsult.App.Wpf.ReactiveUI.Views
{
    /// <summary>
    /// Interaction logic for MainWindowRibbonControl.xaml
    /// </summary>
    public partial class MainWindowRibbonControl
    {
        private MainWindowRibbonViewModel _model;

        public MainWindowRibbonControl(MainWindowRibbonViewModel model)
        {
            
            _model = model;
            InitializeComponent();
            DataContext = _model;
            model.CurrentRibbon = MainMenuRibbon;
        }

        private void MainMenuRibbon_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var child = VisualTreeHelper.GetChild((DependencyObject)sender, 0) as Grid;
                if (child == null) return;
                var row = child.RowDefinitions[0];
                row.Height = new GridLength(_model.QuickAccessToolbarHeight);

                //var subchild = (Border)child.Children
                //          .Cast<UIElement>().FirstOrDefault(i => Grid.GetRow(i) == 0);

                //var subSubChild = subchild.Child;
            }
            catch
            {
                // Not important
            }
        }
    }
}

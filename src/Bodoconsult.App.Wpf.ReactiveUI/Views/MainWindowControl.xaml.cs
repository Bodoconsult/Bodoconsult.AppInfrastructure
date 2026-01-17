// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Wpf.ReactiveUI.ViewModels;

namespace Bodoconsult.App.Wpf.ReactiveUI.Views
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class MainWindowControl
    {
        public MainWindowControl(MainWindowViewModel model)
        {

            DataContext = model;

            InitializeComponent();
        }
    }
}

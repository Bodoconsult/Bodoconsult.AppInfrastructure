// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Wpf.ReactiveUI.ViewModels;

namespace Bodoconsult.App.Wpf.ReactiveUI.Views
{
    /// <summary>
    /// Interaction logic for MainWindowMenu.xaml
    /// </summary>
    public partial class MenuControl
    {
        public MenuControl(MenuViewModel model)
        {

            DataContext = model;

            InitializeComponent();
        }
    }
}

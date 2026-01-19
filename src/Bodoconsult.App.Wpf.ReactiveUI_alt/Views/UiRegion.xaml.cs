// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Wpf.ReactiveUI.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables.Fluent;

namespace Bodoconsult.App.Wpf.ReactiveUI.Views
{
    /// <summary>
    /// Interaktionslogik für UiRegion.xaml
    /// </summary>
    public partial class UiRegion
    {


        /// <summary>
        /// Default ctor
        /// </summary>
        public UiRegion()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                // Bind the view model router to RoutedViewHost.Router property.
                this.OneWayBind(ViewModel, x => x.Router, x => x.DocumentRegion.Router)
                    .DisposeWith(disposables);
            });
        }
    }
}

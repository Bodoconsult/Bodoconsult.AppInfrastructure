// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Wpf.ReactiveUI.Regions;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Bodoconsult.App.ReactiveUI.Extensions;
using WpfReactiveUiDemoApp.ViewModels;

namespace WpfReactiveUiDemoApp.Views
{
    /// <summary>
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class Window1
    {
        public Window1()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.WhenAnyValue(x => x.ViewModel).ObserveOn(RxSchedulers.MainThreadScheduler).Subscribe(x =>
                {
                    if (x == null)
                    {
                        return;
                    }

                    RegisterAllRouterBindings(x, disposables);
                });
            });
        }

        public void RegisterAllRouterBindings(Window1ViewModel viewModel, CompositeDisposable disposables)
        {
            //if (viewModel == null)
            //{
            //    return;
            //}

            var rm = (WpfRegionManager)viewModel.RegionManager;
            var window = rm.RegisterInstances<Window1, Window1ViewModel>(this, disposables);

            viewModel.Region1 = window.FindRegion(DocumentRegion);
            viewModel.Region2 = window.FindRegion(MenuRegion);

            if (viewModel.Region1 == null)
            {
                throw new ArgumentNullException(nameof(viewModel.Region1));
            }

            if (viewModel.Region2 == null)
            {
                throw new ArgumentNullException(nameof(viewModel.Region2));
            }

            this.OneWayBind(viewModel, p => p.Region1!.Router, xy => xy.DocumentRegion.Router)
                .DisposeWith(disposables);

            this.OneWayBind(viewModel, p => p.Region2!.Router, xy => xy.MenuRegion.Router)
                .DisposeWith(disposables);

            //this.BindCommand(viewModel, x => x.GoToFirstViewCommand, x => x.GoNextButton)
            //    .DisposeWith(disposables);

            //this.BindCommand(viewModel, x => x.GoToWindow1Command, x => x.GoNewWindowButton)
            //    .DisposeWith(disposables);

            //this.BindCommand(viewModel, x => x.Region1.GoBack, x => x.GoBackButton)
            //    .DisposeWith(disposables);

            var vm2 = new SecondViewModel(viewModel.Region2);

            viewModel.Region2.Navigate(vm2);
        }
    }
}
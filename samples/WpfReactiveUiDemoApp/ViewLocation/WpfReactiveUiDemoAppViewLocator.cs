// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Wpf.ReactiveUI.Interfaces;
using ReactiveUI;
using WpfReactiveUiDemoApp.ViewModels;
using WpfReactiveUiDemoApp.Views;

namespace WpfReactiveUiDemoApp.ViewLocation;

public class WpfReactiveUiDemoAppViewLocator : IViewLocator
{
    public IViewFor ResolveView<T>(T viewModel, string contract = null)
    {
        if (viewModel is WpfReactiveUiDemoAppMainWindowViewModel main)
        {
            return new MainWindow { ViewModel = main };
        }
        if (viewModel is FirstViewModel vm1)
        {
            return new FirstView { ViewModel = vm1 };
        }
        //if (viewModel is MainViewModel mainVm)
        //{
        //    return new Window1() { ViewModel = mainVm };
        //}
        //if (viewModel is MainViewModel mainVm)
        //{
        //    return new Window1() { ViewModel = mainVm };
        //}
        //if (viewModel is ViewModel2 vm2)
        //{
        //    return new View2 { ViewModel = vm2 };
        //}
        throw new Exception($"Could not find the view for view model {typeof(T).Name}.");
    }

}
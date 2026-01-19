// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using ReactiveUI;
using WpfReactiveUiDemoApp.ViewModels;
using WpfReactiveUiDemoApp.Views;

namespace WpfReactiveUiDemoApp.ViewLocation;

//public class SimpleViewLocator : IViewLocator
//{
//    public IViewFor ResolveView<T>(T viewModel, string contract = null)
//    {
//        //if (viewModel is MainViewModel mainVm)
//        //{
//        //    return new Window1() { ViewModel = mainVm };
//        //}

//        if (viewModel is MainViewModel main)
//        {
//            return new MainWindow() { ViewModel = main };
//        }
//        if (viewModel is ViewModel1 vm1)
//        {
//            return new View1 { ViewModel = vm1} ;
//        }
//        if (viewModel is ViewModel2 vm2)
//        {
//            return new View2 { ViewModel = vm2 };
//        }
//        throw new Exception($"Could not find the view for view model {typeof(T).Name}.");
//    }

//}
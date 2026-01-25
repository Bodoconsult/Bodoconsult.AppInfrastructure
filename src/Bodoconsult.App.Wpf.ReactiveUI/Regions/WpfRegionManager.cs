// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Regions;
using Bodoconsult.App.Wpf.Helpers;
using Bodoconsult.App.Wpf.ReactiveUI.Extensions;
using ReactiveUI;
using System.Collections.Concurrent;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Windows;
using Bodoconsult.App.Wpf.ReactiveUI.Interfaces;

namespace Bodoconsult.App.Wpf.ReactiveUI.Regions;

/// <summary>
/// WPF implementation of <see cref="IRegionManager"/>
/// </summary>
public class WpfRegionManager : RegionManagerBase
{
    private readonly List<WpfWindowDefinition> _windows = new();

    private readonly ConcurrentDictionary<Type, Func<Window>> _windowsDictionary = new();


    private readonly ConcurrentDictionary<Type, Type> _viewModelBinding = new();

    public void RegisterWindow<T, TViewModel>(List<string> regions, Func<Window>? factory)
    {
        var windowType = typeof(T);

        var wwd = new WpfWindowDefinition(windowType, regions);
        _windows.Add(wwd);

        _viewModelBinding[typeof(TViewModel)] = typeof(T);

        if (factory == null)
        {
            return;
        }

        _windowsDictionary[windowType] = factory;

    }


    //public void RegisterWindow<T, TViewModel>(Func<T, TViewModel>? factory) where T : ReactiveWindow<TViewModel> where TViewModel : class
    //{
    //    var windowType = typeof(T);

    //    var wwd = new WpfWindowDefinition(windowType, regions);
    //    _windows.Add(wwd);

    //    if (factory == null)
    //    {
    //        return;
    //    }

    //    _windowsDictionary[windowType] = () => factory;
    //}

    public WpfUiWindow RegisterInstances<T, TViewModel>(T window, CompositeDisposable disposables) where T : ReactiveWindow<TViewModel> where TViewModel : class
    {

        var type = typeof(T);

        var wwd = _windows.FirstOrDefault(x => x.WindowType == type);

        //if (wwd == default)
        //{

        var childs = WpfHelper.FindVisualChildren<RoutedViewHost>(window).ToList();

        if (childs == null || childs.Count == 0)
        {
            // ReSharper disable once LocalizableElement
            throw new ArgumentNullException(nameof(childs), $"No region childs defined for {type.Name}");
        }

        var uiWindow = this.CreateUiWindow(window);

        foreach (var regionName in wwd.Regions)
        {
            var regionContainer = childs.FirstOrDefault(x => x.Name == regionName);

            if (regionContainer == null)
            {
                throw new ArgumentNullException(nameof(regionName), regionName);
            }

            var region = uiWindow.CreateWpfUiRegion(regionContainer);
            uiWindow.UiRegions.Add(region);

            //window.OneWayBind(region, p => p.Router, xy => regionContainer.Router)
            //    .DisposeWith(disposables);
        }

        return uiWindow;

        //}
    }

    //private WpfUiRegion GetUiWindow(Window window)
    //{


    //    return Regions.FirstOrDefault(x=> x.Key)
    //}

    public override void Navigate<T>(T viewModel, string regionName) where T: class
    {
        var vmType = typeof(T);

        if (!_viewModelBinding.TryGetValue(vmType, out var windowType))
        {
            throw new ArgumentException($"Viewmodel {vmType.Name} not registered with view");
        }

        if (!_windowsDictionary.TryGetValue(windowType, out var func))
        {
            throw new ArgumentException($"Window {windowType.Name} has no factory method registered");
        }

        var view = func.Invoke();

        if (view == null)
        {
            throw new ArgumentNullException(nameof(view));
        }

        if (view is not ReactiveWindow<T> reactiveWindow)
        {
            throw new ArgumentException($"View {view.GetType().Name} is not a ReactiveWindow instance as expected");
        }

        reactiveWindow.ViewModel = viewModel;



        reactiveWindow.WhenAnyValue(x => x.ViewModel).ObserveOn(RxSchedulers.MainThreadScheduler).Subscribe(x =>
        {
            if (x == null)
            {
                return;
            }

            if (x is not IReactiveUiWindowViewModel uvm)

            {
                throw new ArgumentException($"Viewmodel {x.GetType().Name} does not implement IReactiveUiWindowViewModel as expected");
            }



        });

        reactiveWindow.Show();

        //// Now search the region and navigate to
        //window.FindRegion(regionName);
    }
}
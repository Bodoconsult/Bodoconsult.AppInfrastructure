//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

//using System.Windows;
//using ReactiveUI;
//using ReactiveUI.SourceGenerators;

//namespace Bodoconsult.App.Wpf.ReactiveUI.ViewModels;

///// <summary>
///// Viewmodel for UI region
///// </summary>
//public partial class UiRegionViewModel : ReactiveObject, IScreen
//{
//    /// <summary>
//    /// Router
//    /// </summary>
//    [Reactive] private RoutingState _router = new();

//    /// <summary>
//    /// Dependecy property for <see cref="Router"/>
//    /// </summary>
//    public static readonly DependencyProperty RouterProperty =
//        DependencyProperty.Register("Router", typeof(RoutingState), typeof(UiRegionViewModel),
//            new PropertyMetadata(default(Point)));

//}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Interfaces;
using ReactiveUI;
using System.Reactive.Linq;
using ReactiveUI.Primitives;

namespace Bodoconsult.App.ReactiveUI.Regions;

/// <summary>
/// Region of the UI
/// </summary>
public class UiRegion : ReactiveObject, IScreen
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="uiWindow">Current UI window</param>
    /// <param name="regionName">Name of the region to register</param>
    public UiRegion(IUiWindow uiWindow, string regionName)
    {
        UiWindow = uiWindow;
        RegionName = $"{uiWindow.InstanceName}.{regionName}";
        Router = new RoutingState();

        // You can also ask the router to go back. One option is to 
        // execute the default Router.NavigateBack command. Another
        // option is to define your own command with custom
        // canExecute condition as such:
        var canGoBack = Observable.Select(this
                .WhenAnyValue(x => x.Router.NavigationStack.Count), count => count > 0);
        GoBack = ReactiveCommand.CreateFromObservable(
            () => Router.NavigateBack.Execute(RxVoid.Default),
            canGoBack);
    }

    /// <summary>
    /// Region name
    /// </summary>
    public string RegionName { get; }

    /// <summary>
    /// Current UI window
    /// </summary>
    public IUiWindow UiWindow { get; }

    /// <summary>Gets the Router associated with this Screen.</summary>
    public RoutingState Router { get; }

    /// <summary>
    /// The command that navigates a user back
    /// </summary>
    public ReactiveCommand<RxVoid, IRoutableViewModel> GoBack { get; }
}
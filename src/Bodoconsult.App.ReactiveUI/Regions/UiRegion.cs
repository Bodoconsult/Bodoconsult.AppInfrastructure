// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Interfaces;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;

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
    public UiRegion(UiWindow uiWindow, string regionName)
    {
        UiWindow = uiWindow;
        RegionName = regionName;
        Router = new RoutingState();

        // You can also ask the router to go back. One option is to 
        // execute the default Router.NavigateBack command. Another
        // option is to define your own command with custom
        // canExecute condition as such:
        var canGoBack = this
            .WhenAnyValue(x => x.Router.NavigationStack.Count)
            .Select(count => count > 0);
        GoBack = ReactiveCommand.CreateFromObservable(
            () => Router.NavigateBack.Execute(Unit.Default),
            canGoBack);
    }

    /// <summary>
    /// Region name
    /// </summary>
    public string RegionName { get; }

    /// <summary>
    /// Current UI window
    /// </summary>
    public UiWindow UiWindow { get; }

    /// <summary>Gets the Router associated with this Screen.</summary>
    public RoutingState Router { get; }

    /// <summary>
    /// The command that navigates a user back
    /// </summary>
    public ReactiveCommand<Unit, IRoutableViewModel> GoBack { get; }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using ReactiveUI;
using ReactiveUI.Primitives;
using Splat;
using WpfReactiveUiDemoApp.Views;

namespace WpfReactiveUiDemoApp.ViewModels;

public class MainViewModel : ReactiveObject, IScreen
{
    // The Router associated with this Screen.
    // Required by the IScreen interface.
    public RoutingState Router { get; }

    // The command that navigates a user to first view model.
    public ReactiveCommand<RxVoid, RxVoid> GoNextCommand { get; }

    // The command that navigates a user back.
    public ReactiveCommand<RxVoid, RxVoid> GoBackCommand { get; }

    public MainViewModel()
    {
        // Initialize the Router.
        Router = new RoutingState();

        // Router uses Splat.Locator to resolve views for
        // view models, so we need to register our views
        // using AppLocator.CurrentMutable.Register* methods.
        //
        // Instead of registering views manually, you 
        // can use custom IViewLocator implementation,
        // see "View Location" section for details.
        //
        AppLocator.CurrentMutable.Register(() => new FirstView(), typeof(IViewFor<FirstViewModel>));

        // Manage the routing state. Use the Router.Navigate.Execute
        // command to navigate to different view models. 
        //
        // Note, that the Navigate.Execute method accepts an instance 
        // of a view model, this allows you to pass parameters to 
        // your view models, or to reuse existing view models.
        //
        GoNextCommand = ReactiveCommand.CreateFromTask(GoToNext);

        // You can also ask the router to go back. One option is to 
        // execute the default Router.NavigateBack command. Another
        // option is to define your own command with custom
        // canExecute condition as such:

        var source = this.WhenAnyValue(x => x.Router.NavigationStack.Count);
        var canGoBack = source.Select(count => count > 0);
        GoBackCommand = ReactiveCommand.CreateFromTask(GoBack, canGoBack);
    }

    public Task<RxVoid> GoToNext()
    {
        Router.Navigate.Execute(new FirstViewModel(this));
        return Task.FromResult(RxVoid.Default);
    }

    public Task<RxVoid> GoBack()
    {
        Router.NavigateBack.Execute(RxVoid.Default);
        return Task.FromResult(RxVoid.Default);
    }
}
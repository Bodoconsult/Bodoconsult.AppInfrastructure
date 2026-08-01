using System.Reactive.Linq;
using ReactiveUI;
using Splat;
using AvaloniaReactiveUiDemoApp.Views;
using ReactiveUI.Primitives;

namespace AvaloniaReactiveUiDemoApp.ViewModels;

public class MainViewModel : ReactiveObject, IScreen
{
    // The Router associated with this Screen.
    // Required by the IScreen interface.
    public RoutingState Router { get; }

    // The command that navigates a user to first view model.
    public ReactiveCommand<RxVoid, IRoutableViewModel> GoNext { get; }

    // The command that navigates a user back.
    public ReactiveCommand<RxVoid, IRoutableViewModel> GoBack { get; }

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
        GoNext = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new FirstViewModel(this)));

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
}
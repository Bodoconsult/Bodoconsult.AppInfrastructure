using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace WpfReactiveUiDemoApp.ViewModels;

public partial class FirstViewModel : ReactiveObject, IRoutableViewModel
{
    public string UrlPathSegment => "first";

    //private string _name;
    //public string Test
    //{
    //    get => _name;
    //    set => this.RaiseAndSetIfChanged(ref _name, value);
    //}

    /// <summary>
    /// Test text
    /// </summary>
    [Reactive] public partial string Test { get; set; }

    public IScreen HostScreen { get; }

    public FirstViewModel(IScreen screen)
    {
        HostScreen = screen;
        _test = "Blubb";
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace WpfReactiveUiDemoApp.ViewModels;

public partial class SecondViewModel : ReactiveObject, IRoutableViewModel
{
    public string UrlPathSegment => "second";

    /// <summary>
    /// Test text
    /// </summary>
    [Reactive] public partial string Test { get; set; }

    public IScreen HostScreen { get; }

    public SecondViewModel(IScreen screen)
    {
        HostScreen = screen;
        _test = "Mummmpf";
    }
}
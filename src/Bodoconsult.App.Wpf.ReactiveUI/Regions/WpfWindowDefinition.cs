// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using ReactiveUI;
using System.Reactive.Disposables;

namespace Bodoconsult.App.Wpf.ReactiveUI.Regions;

public struct WpfWindowDefinition
{
    public WpfWindowDefinition(Type type, List<string> regions)
    {
        WindowType = type;
        Regions = regions;
    }

    public Type WindowType { get;  }


    public List<string> Regions { get; }
}
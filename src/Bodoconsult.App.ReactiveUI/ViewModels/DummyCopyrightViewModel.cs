// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.ReactiveUI.ViewModels;

/// <summary>
/// Dummy for copyright viewmodel
/// </summary>
public class DummyCopyrightViewModel : CopyrightViewModel
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public DummyCopyrightViewModel() : base(new DummyAppGlobals())
    {
    }
}
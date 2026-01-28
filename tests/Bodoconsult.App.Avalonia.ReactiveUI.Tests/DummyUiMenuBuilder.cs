// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.ReactiveUI.Menus;

namespace Bodoconsult.App.ReactiveUI.Tests;

/// <summary>
/// Dummy implementation of <see cref="UiMenuBuilderBase"/> for testing
/// </summary>
public class DummyUiMenuBuilder : UiMenuBuilderBase
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="translationService">Current translation service</param>
    public DummyUiMenuBuilder(II18N translationService) : base(translationService)
    {
    }
}
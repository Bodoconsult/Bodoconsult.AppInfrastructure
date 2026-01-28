// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using ReactiveUI;

namespace Bodoconsult.App.ReactiveUI.Interfaces;

/// <summary>
/// Dummy implementation of <see cref="IScreen"/>
/// </summary>
public class DummyScreen : IScreen
{
    /// <summary>Gets the Router associated with this Screen.</summary>
    public RoutingState Router { get; } = new();
}
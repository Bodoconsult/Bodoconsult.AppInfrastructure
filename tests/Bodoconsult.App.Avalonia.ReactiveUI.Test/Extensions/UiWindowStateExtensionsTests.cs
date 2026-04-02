// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Avalonia.Controls;
using Bodoconsult.App.Avalonia.ReactiveUI.Extensions;
using Bodoconsult.App.ReactiveUI.Ui;
using NUnit.Framework;

namespace Bodoconsult.App.Avalonia.ReactiveUI.Test.Extensions;

[TestFixture]
public class UiWindowStateExtensionsTests
{
    [Test]
    public void ToWindowState_Normal_ReturnsNormal()
    {
        // Arrange 
        const UiWindowState state = UiWindowState.Normal;

        // Act  
        var result = state.ToWindowState();
        
        // Assert
        Assert.That(result==WindowState.Normal);
    }

    [Test]
    public void ToWindowState_Minimized_ReturnsMinimized()
    {
        // Arrange 
        const UiWindowState state = UiWindowState.Minimized;

        // Act  
        var result = state.ToWindowState();

        // Assert
        Assert.That(result==WindowState.Minimized);
    }

    [Test]
    public void ToWindowState_Maximized_ReturnsMaximized()
    {
        // Arrange 
        const UiWindowState state = UiWindowState.Maximized;

        // Act  
        var result = state.ToWindowState();

        // Assert
        Assert.That(result==WindowState.Maximized);
    }
}
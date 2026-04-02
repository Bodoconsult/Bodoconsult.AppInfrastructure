// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Avalonia.Controls;
using Bodoconsult.App.Avalonia.ReactiveUI.Extensions;
using Bodoconsult.App.ReactiveUI.Ui;
using NUnit.Framework;

namespace Bodoconsult.App.Avalonia.ReactiveUI.Test.Extensions;

[TestFixture]
public class WindowStateExtensionsTests
{
    [Test]
    public void ToWindowState_Normal_ReturnsNormal()
    {
        // Arrange 
        const WindowState state = WindowState.Normal;

        // Act  
        var result = state.ToUiWindowState();

        // Assert
        Assert.That(result==UiWindowState.Normal);
    }

    [Test]
    public void ToWindowState_Minimized_ReturnsMinimized()
    {
        // Arrange 
        const WindowState state = WindowState.Minimized;

        // Act  
        var result = state.ToUiWindowState();

        // Assert
        Assert.That(result==UiWindowState.Minimized);
    }

    [Test]
    public void ToWindowState_Maximized_ReturnsMaximized()
    {
        // Arrange 
        const WindowState state = WindowState.Maximized;

        // Act  
        var result = state.ToUiWindowState();

        // Assert
        Assert.That(result==UiWindowState.Maximized);
    }
}
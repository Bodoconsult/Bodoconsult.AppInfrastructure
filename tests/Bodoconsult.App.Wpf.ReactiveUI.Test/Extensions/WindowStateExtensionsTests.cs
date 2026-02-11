// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Windows;
using Bodoconsult.App.ReactiveUI.Ui;
using Bodoconsult.App.Wpf.ReactiveUI.Extensions;
using NUnit.Framework;

namespace Bodoconsult.App.Wpf.ReactiveUI.Test.Extensions;

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
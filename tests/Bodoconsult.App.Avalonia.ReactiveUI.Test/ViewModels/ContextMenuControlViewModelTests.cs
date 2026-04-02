// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Avalonia.ReactiveUI.Menus;
using Bodoconsult.App.Avalonia.ReactiveUI.ViewModels;
using Bodoconsult.App.ReactiveUI.Tests.Helpers;
using NUnit.Framework;

namespace Bodoconsult.App.Avalonia.ReactiveUI.Test.ViewModels;

[TestFixture]
public class ContextMenuControlViewModelTests
{
    [Test]
    public void Ctor_ValidSetup_PropsSetCorrectly()
    {
        // Arrange 
        var i18N = new DummyI18N();

        var builder = new AvaloniaUiMenuBuilder(i18N);
        MenuBuilderHelper.LoadMenuItems(builder);

        // Act  
        var vm = new ContextMenuControlViewModel();

        // Assert
        Assert.That(vm.MenuItems, Is.Null);
        //Assert.That(vm.MenuItems.Count, Is.EqualTo(builder.MenuItemsSource.Count));
    }

    [Test]
    public void LoadBuilder_ValidSetup_PropsSetCorrectly()
    {
        // Arrange 
        var i18N = new DummyI18N();

        var builder = new AvaloniaUiMenuBuilder(i18N);
        MenuBuilderHelper.LoadMenuItems(builder);

        var vm = new ContextMenuControlViewModel();

        // Act  
        vm.LoadMenuBuilder(builder);

        // Assert
        Assert.That(vm.MenuItems, Is.Not.Null);
        Assert.That(vm.MenuItems.Count, Is.EqualTo(builder.MenuItemsSource.Count));
    }
}
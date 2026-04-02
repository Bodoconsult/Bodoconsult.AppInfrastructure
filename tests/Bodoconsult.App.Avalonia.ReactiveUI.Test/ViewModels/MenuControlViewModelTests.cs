// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Threading;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Avalonia.ReactiveUI.Menus;
using Bodoconsult.App.Avalonia.ReactiveUI.ViewModels;
using Bodoconsult.App.ReactiveUI.Tests.Helpers;
using NUnit.Framework;

namespace Bodoconsult.App.Avalonia.ReactiveUI.Test.ViewModels;

[Apartment(ApartmentState.STA)]
[TestFixture]
public class MenuControlViewModelTests
{
    [Test]
    public void Ctor_ValidSetup_PropsSetCorrectly()
    {
        // Arrange 
        var i18N = new DummyI18N();

        var builder = new AvaloniaUiMenuBuilder(i18N);
        MenuBuilderHelper.LoadMenuItems(builder);

        // Act  
        var vm = new MenuControlViewModel();

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

        var vm = new MenuControlViewModel();

        // Act  
        vm.LoadMenuBuilder(builder);

        // Assert
        Assert.That(vm.MenuItems, Is.Not.Null);
        Assert.That(vm.MenuItems.Count, Is.EqualTo(builder.MenuItemsSource.Count));
    }
}
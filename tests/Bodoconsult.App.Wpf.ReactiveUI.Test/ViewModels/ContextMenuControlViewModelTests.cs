// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.ReactiveUI.Tests.Helpers;
using Bodoconsult.App.Wpf.ReactiveUI.Menus;
using Bodoconsult.App.Wpf.ReactiveUI.ViewModels;
using NUnit.Framework;

namespace Bodoconsult.App.Wpf.ReactiveUI.Test.ViewModels;

[TestFixture]
public class ContextMenuControlViewModelTests
{
    [Test]
    public void Ctor_ValidSetup_PropsSetCorrectly()
    {
        // Arrange 
        var i18N = new DummyI18N();

        var builder = new WpfUiMenuBuilder(i18N);
        MenuBuilderHelper.LoadMenuItems(builder);

        // Act  
        var vm = new ContextMenuControlViewModel();

        // Assert
        Assert.That(vm.MenuItems, Is.Null);
    }

    [Test]
    public void LoadBuilder_ValidSetup_PropsSetCorrectly()
    {
        // Arrange 
        var i18N = new DummyI18N();

        var builder = new WpfUiMenuBuilder(i18N);
        MenuBuilderHelper.LoadMenuItems(builder);

        var vm = new ContextMenuControlViewModel();

        // Act  
        vm.LoadMenuBuilder(builder);

        // Assert
        Assert.That(vm.MenuItems, Is.Not.Null);
        Assert.That(vm.MenuItems.Count, Is.EqualTo(builder.MenuItemsSource.Count));
    }

}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Wpf.ReactiveUI.Menus;
using NUnit.Framework;
using Bodoconsult.App.ReactiveUI.Tests.Helpers;

namespace Bodoconsult.App.Wpf.ReactiveUI.Test.Menus;

[TestFixture]
internal class WpfUiMenuBuilderTests
{
    [Test]
    public void Ctor_ValidSetup_PropsSetCorrectly()
    {
        // Arrange 
        var i18N = new DummyI18N();

        // Act  
        var builder = new WpfUiMenuBuilder(i18N);

        // Assert
        Assert.That(builder.MenuItemsSource.Count, Is.EqualTo(0));
        Assert.That(builder.IsMainMenu, Is.False);
        Assert.That(builder.MenuItems, Is.Not.Null);
        Assert.That(builder.MenuItems.Count, Is.EqualTo(0));
        Assert.That(builder.TopLevelMenuItems.Count, Is.EqualTo(0));
    }

    [Test]
    public void MenuItemsAdd_MultipleMenuItems_MenuItemsAdded()
    {
        // Arrange 
        var i18N = new DummyI18N();

        var builder = new WpfUiMenuBuilder(i18N);

        // Act  
        MenuBuilderHelper.LoadMenuItems(builder);
            
        // Assert
        Assert.That(builder.MenuItemsSource.Count, Is.EqualTo(0));
        Assert.That(builder.IsMainMenu, Is.False);
        Assert.That(builder.MenuItems.Count, Is.Not.EqualTo(0));
        Assert.That(builder.TopLevelMenuItems.Count, Is.Not.EqualTo(0));
    }

    [Test]
    public void BuildIt_MultipleMenuItems_MenuItemAdded()
    {
        // Arrange 
        var i18N = new DummyI18N();

        var builder = new WpfUiMenuBuilder(i18N);

        MenuBuilderHelper.LoadMenuItems(builder);

        // Act  
        builder.BuildIt();

        // Assert
        Assert.That(builder.MenuItemsSource.Count, Is.Not.EqualTo(0));
        //Assert.That(builder.MenuItemsSource.Where(x=> x is MenuItem and ((MenuItem)x)c.Items.Count==0).Count, Is.EqualTo(4));
        //Assert.That(builder.MenuItemsSource.Where(x => x.Items.Count > 0).Count, Is.EqualTo(2));
    }
}
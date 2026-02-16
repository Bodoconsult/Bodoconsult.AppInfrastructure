// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.ReactiveUI.Menus;
using Bodoconsult.App.ReactiveUI.Tests.Helpers;

namespace Bodoconsult.App.ReactiveUI.Tests;

[TestFixture]
public class UiMenuBuilderBaseTests
{
    [Test]
    public void Ctor_ValidSetup_PropsSetCorrectly()
    {
        // Arrange 
        II18N i18N = new DummyI18N();

        // Act  
        var builder = new DummyUiMenuBuilder(i18N);

        // Assert
        Assert.That(builder.MenuItems, Is.Not.Null);
        Assert.That(builder.MenuItems.Count, Is.EqualTo(0));
        Assert.That(builder.TopLevelMenuItems.Count, Is.EqualTo(0));
    }

    [Test]
    public void Add_ValidSetup_PropsSetCorrectly()
    {
        // Arrange 
        II18N i18N = new DummyI18N();

        var builder = new DummyUiMenuBuilder(i18N);

        var item = new GroupUiMenuItem("Test");

        // Act  
        builder.Add(item);

        // Assert
        Assert.That(builder.MenuItems.Count, Is.EqualTo(1));
        Assert.That(builder.TopLevelMenuItems.Count, Is.EqualTo(1));
    }

    [Test]
    public void BuildIt_OneGroup_OneMenuItemCreated()
    {
        // Arrange 
        II18N i18N = new DummyI18N();

        var builder = new DummyUiMenuBuilder(i18N);

        var item = new GroupUiMenuItem("Test");
        builder.Add(item);

        // Act  
        builder.BuildIt();

        // Assert
        Assert.That(builder.NumberOfGroups, Is.EqualTo(1));
        Assert.That(builder.NumberOfCommands, Is.EqualTo(0));
        Assert.That(builder.NumberOfSeparators, Is.EqualTo(0));
    }

    [Test]
    public void BuildIt_MultipleItems_MultipleMenuItemsCreated()
    {
        // Arrange 
        II18N i18N = new DummyI18N();

        var builder = new DummyUiMenuBuilder(i18N);

        MenuBuilderHelper.LoadMenuItems(builder);

        // Act  
        builder.BuildIt();

        // Assert
        Assert.That(builder.NumberOfGroups, Is.EqualTo(2));
        Assert.That(builder.NumberOfCommands, Is.EqualTo(3));
        Assert.That(builder.NumberOfSeparators, Is.EqualTo(1));
    }

    [Test]
    public void TopLevelMenuItems_MultipleItems_MultipleMenuItemsReturned()
    {
        // Arrange 
        II18N i18N = new DummyI18N();

        var builder = new DummyUiMenuBuilder(i18N);

        // Act  
        MenuBuilderHelper.LoadMenuItems(builder);

        // Assert
        Assert.That(builder.MenuItems.Count, Is.Not.EqualTo(0));
        Assert.That(builder.TopLevelMenuItems.Count, Is.Not.EqualTo(0));
    }

    [Test]
    public void Clear_OneGroup_MenuItemsRemoved()
    {
        // Arrange 
        II18N i18N = new DummyI18N();

        var builder = new DummyUiMenuBuilder(i18N);

        var item = new GroupUiMenuItem("Test");
        builder.Add(item);

        Assert.That(builder.MenuItems.Count, Is.EqualTo(1));

        // Act  
        builder.Clear();

        // Assert
        Assert.That(builder.MenuItems.Count, Is.EqualTo(0));
    }
}
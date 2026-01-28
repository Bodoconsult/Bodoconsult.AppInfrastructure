// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.ReactiveUI.Menus;

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
}
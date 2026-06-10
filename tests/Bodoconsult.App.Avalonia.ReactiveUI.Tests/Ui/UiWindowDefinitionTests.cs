// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.


// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Tests.Menus;

namespace Bodoconsult.App.ReactiveUI.Tests.Ui;

[TestFixture]
public class UiWindowDefinitionTests
{
    [Test]
    public void Ctor_ValidSetupNoFactory_PropsSetCorrectly()
    {
        // Arrange 
        var type = typeof(UiWindowDefinition);

        var regions = new List<string>
        {
            "Region1",
            "Region2"
        };

        // Act  
        var wd = new UiWindowDefinition(type, regions, null);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(wd.WindowType, Is.EqualTo(type));
            Assert.That(wd.Regions, Is.Not.Null);
            Assert.That(wd.Regions.Count, Is.EqualTo(regions.Count));
            Assert.Null(wd.Factory);
        }
    }

    [Test]
    public void Ctor_ValidSetupWithFactory_PropsSetCorrectly()
    {
        // Arrange 
        var type = typeof(UiWindowDefinition);

        var regions = new List<string>
        {
            "Region1",
            "Region2"
        };

        // Act  
        var wd = new UiWindowDefinition(type, regions, () => new DummyUiWindow());

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(wd.WindowType, Is.EqualTo(type));
            Assert.NotNull(wd.Regions);
            Assert.That(wd.Regions.Count, Is.EqualTo(regions.Count));
            Assert.Null(wd.Factory);
        }
    }
}
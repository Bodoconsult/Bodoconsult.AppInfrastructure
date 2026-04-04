// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.


// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Regions;
using Bodoconsult.App.ReactiveUI.Tests.Menus;

namespace Bodoconsult.App.ReactiveUI.Tests.Regions;

[TestFixture]
public class UiRegionTests
{
    [Test]
    public void Ctor_ValidSetup_PropsSetCorrectly()
    {
        // Arrange 
        const string regionName = "test";
        var window = new DummyUiWindow
        {
            InstanceName = "Dummy"
        };

        // Act  
        var rmb = new UiRegion(window, regionName);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(rmb.UiWindow, Is.EqualTo(window));
            Assert.That(rmb.Router, Is.Not.Null);
            Assert.That(rmb.RegionName, Is.EqualTo($"{window.InstanceName}.{regionName}"));
        }
    }
}
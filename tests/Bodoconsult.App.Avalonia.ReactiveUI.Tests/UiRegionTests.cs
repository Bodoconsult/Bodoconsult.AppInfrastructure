// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Regions;

namespace Bodoconsult.App.ReactiveUI.Tests;

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
            Name = "Dummy"
        };

        // Act  
        var rmb = new UiRegion(window, regionName);

        // Assert
        Assert.That(rmb.UiWindow, Is.EqualTo(window));
        Assert.That(rmb.Router, Is.Not.Null);
        Assert.That(rmb.RegionName, Is.EqualTo($"{window.Name}.{regionName}"));
    }

}
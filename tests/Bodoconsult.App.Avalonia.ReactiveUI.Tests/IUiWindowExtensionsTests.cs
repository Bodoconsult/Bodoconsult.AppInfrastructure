// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Extensions;

namespace Bodoconsult.App.ReactiveUI.Tests;

[TestFixture]
public class IUiWindowExtensionsTests
{
    [Test]
    public void RegisterRegion_ValidSetupNoFactory_RegionRegistered()
    {
        // Arrange 
        var rmb = new DummyRegionManager();

        var regions = new List<string>
        {
            "Region1",
            "Region2"
        };

        rmb.RegisterWindow<DummyUiWindow, DummyUiWindowViewModel>(regions, null);

        var window = new DummyUiWindow();

        rmb.RegisterWindowInstances(window);

        // Act
        var region = window.CreateUiRegion(regions[0]);

        // Assert
        Assert.That(region, Is.Not.Null);
        rmb.RegisterRegion(region);
        Assert.That(rmb.WindowDefinitions.Count, Is.EqualTo(1));
        Assert.That(rmb.ViewModelBindings.Count, Is.EqualTo(1));
        Assert.That(rmb.Windows.Count, Is.EqualTo(1));
        Assert.That(rmb.Regions.Count, Is.EqualTo(1));
    }

}
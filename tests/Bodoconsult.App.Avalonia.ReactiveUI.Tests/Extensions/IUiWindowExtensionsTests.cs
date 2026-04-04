// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Extensions;
using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Tests.Menus;
using Bodoconsult.App.ReactiveUI.Tests.TestData;

namespace Bodoconsult.App.ReactiveUI.Tests.Extensions;

[TestFixture]
public class IUiWindowExtensionsTests
{
    [Test]
    public void RegisterRegion_ValidSetupNoFactory_RegionRegistered()
    {
        // Arrange 
        const string instanceName = "Blubb";

        var rmb = new DummyRegionManager();

        var regions = new List<string>
        {
            "Region1",
            "Region2"
        };

        rmb.RegisterWindow<DummyUiWindow, DummyUiWindowViewModel>(regions, null);

        var window = new DummyUiWindow
        {
            RegionManager = rmb,
            InstanceName = instanceName
        };

        rmb.RegisterWindowInstances(window, instanceName);

        // Act
        var region = window.CreateUiRegion(regions[0]);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(region, Is.Not.Null);
            ArgumentNullException.ThrowIfNull(region);
            Assert.That(rmb.WindowDefinitions.Count, Is.EqualTo(1));
            Assert.That(rmb.ViewModelBindings.Count, Is.EqualTo(1));
            Assert.That(rmb.Windows.Count, Is.EqualTo(1));
            Assert.That(rmb.Regions.Count, Is.EqualTo(1));
        }
    }
}
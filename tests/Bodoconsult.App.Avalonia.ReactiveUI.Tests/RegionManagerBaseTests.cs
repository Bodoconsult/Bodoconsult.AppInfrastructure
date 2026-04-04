// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.ReactiveUI.Interfaces;
using Bodoconsult.App.ReactiveUI.Regions;

namespace Bodoconsult.App.ReactiveUI.Tests;

[TestFixture]
public class RegionManagerBaseTests
{
    [Test]
    public void Ctor_ValidSetup_PropsSetCorrectly()
    {
        // Arrange 

        // Act  
        var rmb = new DummyRegionManager();

        // Assert
        Assert.That(rmb.ViewModelBindings, Is.Not.Null);
        Assert.That(rmb.ViewModelBindings.Count, Is.EqualTo(0));
        Assert.That(rmb.WindowDefinitions, Is.Not.Null);
        Assert.That(rmb.WindowDefinitions.Count, Is.EqualTo(0));
        Assert.That(rmb.Regions, Is.Not.Null);
        Assert.That(rmb.Regions.Count, Is.EqualTo(0));
        Assert.That(rmb.Windows, Is.Not.Null);
        Assert.That(rmb.Windows.Count, Is.EqualTo(0));
    }

    [Test]
    public void RegisterWindow_ValidSetupNoFactory_PropsSetCorrectly()
    {
        // Arrange 
        var rmb = new DummyRegionManager();

        var regions = new List<string>
        {
            "Region1",
            "Region2"
        };

        // Act  
        rmb.RegisterWindow<DummyUiWindow, DummyUiWindowViewModel>(regions, null);

        // Assert
        Assert.That(rmb.WindowDefinitions.Count, Is.EqualTo(1));
        Assert.That(rmb.ViewModelBindings.Count, Is.EqualTo(1));

        var vmb = rmb.ViewModelBindings[0];

        Assert.That(vmb.Key, Is.EqualTo(typeof(DummyUiWindowViewModel)));
        Assert.That(vmb.Value, Is.EqualTo(typeof(DummyUiWindow)));

        var wd = rmb.WindowDefinitions[0];

        Assert.That(wd.WindowType, Is.EqualTo(typeof(DummyUiWindow)));
        Assert.That(wd.Regions.Count, Is.EqualTo(regions.Count));
        Assert.That(wd.Regions[0], Is.EqualTo(regions[0]));
        Assert.That(wd.Regions[1], Is.EqualTo(regions[1]));
        Assert.That(wd.Factory, Is.Null);
    }

    [Test]
    public void RegisterWindow_ValidSetupWithFactory_PropsSetCorrectly()
    {
        // Arrange 
        var rmb = new DummyRegionManager();

        var regions = new List<string>
        {
            "Region1",
            "Region2"
        };

        Func<IUiWindow> factory = () => new DummyUiWindow();

        // Act  
        rmb.RegisterWindow<DummyUiWindow, DummyUiWindowViewModel>(regions, factory);

        // Assert
        Assert.That(rmb.WindowDefinitions.Count, Is.EqualTo(1));
        Assert.That(rmb.ViewModelBindings.Count, Is.EqualTo(1));

        var vmb = rmb.ViewModelBindings[0];

        Assert.That(vmb.Key, Is.EqualTo(typeof(DummyUiWindowViewModel)));
        Assert.That(vmb.Value, Is.EqualTo(typeof(DummyUiWindow)));

        var wd = rmb.WindowDefinitions[0];

        Assert.That(wd.WindowType, Is.EqualTo(typeof(DummyUiWindow)));
        Assert.That(wd.Regions.Count, Is.EqualTo(regions.Count));
        Assert.That(wd.Regions[0], Is.EqualTo(regions[0]));
        Assert.That(wd.Regions[1], Is.EqualTo(regions[1]));
        Assert.That(wd.Factory, Is.Not.Null);
    }

    [Test]
    public void RegisterWindow_ValidSetupNoFactory_Throws()
    {
        // Arrange 
        var rmb = new DummyRegionManager();

        var regions = new List<string>
        {
            "Region1",
            "Region2"
        };

        // Act and assert
        Assert.Throws<ArgumentException>(() =>
        {
            rmb.RegisterWindow<DummyUiWindow, DummyUiWindowViewModel>(regions, null);
            rmb.RegisterWindow<DummyUiWindow, DummyUiWindowViewModel>(regions, null);
        });
    }

    [Test]
    public void RegisterWindowInstance_ValidSetupNoFactory_WindowRegistered()
    {
        // Arrange 
        var rmb = new DummyRegionManager();
        const string instanceName = "Blubb";

        var regions = new List<string>
        {
            "Region1",
            "Region2"
        };

        rmb.RegisterWindow<DummyUiWindow, DummyUiWindowViewModel>(regions, null);

        var window = new DummyUiWindow();

        // Act  
        rmb.RegisterWindowInstances(window, instanceName);

        // Assert
        Assert.That(rmb.WindowDefinitions.Count, Is.EqualTo(1));
        Assert.That(rmb.ViewModelBindings.Count, Is.EqualTo(1));
        Assert.That(rmb.Windows.Count, Is.EqualTo(1));
    }

    [Test]
    public void RegisterRegion_ValidSetupNoFactory_RegionRegistered()
    {
        // Arrange 
        var rmb = new DummyRegionManager();
        const string instanceName = "Blubb";

        var regions = new List<string>
        {
            "Region1",
            "Region2"
        };

        rmb.RegisterWindow<DummyUiWindow, DummyUiWindowViewModel>(regions, null);

        var window = new DummyUiWindow();

        rmb.RegisterWindowInstances(window, instanceName);

        var region = new UiRegion(window, regions[0]);

        // Act  
        rmb.RegisterRegion(region);

        // Assert
        Assert.That(rmb.WindowDefinitions.Count, Is.EqualTo(1));
        Assert.That(rmb.ViewModelBindings.Count, Is.EqualTo(1));
        Assert.That(rmb.Windows.Count, Is.EqualTo(1));
        Assert.That(rmb.Regions.Count, Is.EqualTo(1));
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.DependencyInjection;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.DataProtection.DependencyInjection;
using Bodoconsult.App.DependencyInjection;
using Bodoconsult.App.Test.Helpers;

namespace Bodoconsult.App.Test.DependencyInjection;

[TestFixture]
internal class NoDataProtectionDiContainerServiceProviderTests
{
    private static NoDataProtectionDiContainerServiceProvider CreateProvider()
    {
        var destinationFolderPath = TestHelper.TempPath;
        var loggerProvider = new NoDataProtectionDiContainerServiceProvider(destinationFolderPath);
        return loggerProvider;
    }

    [Test]
    public void Ctor_DefaultSetup_PropsSetCorrectly()
    {
        // Arrange

        // Act  
        var provider = CreateProvider();

        // Assert
        Assert.That(provider, Is.Not.Null);
    }

    [Test]
    public void AddServices_DefaultSetup_ServicesAdded()
    {
        // Arrange
        var provider = CreateProvider();

        var container = new DiContainer();

        Assert.That(container.ServiceCollection.Count, Is.EqualTo(0));

        // Act  
        provider.AddServices(container);
        container.BuildServiceProvider();

        // Assert
        Assert.That(container.ServiceCollection.Count, Is.Not.EqualTo(0));

        var dpm = container.Get<IDataProtectionManagerFactory>();
        Assert.That(dpm, Is.Not.Null);

    }

    [Test]
    public void CreateInstance_DefaultSetup_InstanceCreated()
    {
        // Arrange
        var filePath = Path.Combine(TestHelper.TempPath, "blubb.dat");

        var provider = CreateProvider();

        var container = new DiContainer();

        Assert.That(container.ServiceCollection.Count, Is.EqualTo(0));

        provider.AddServices(container);
        container.BuildServiceProvider();

        Assert.That(container.ServiceCollection.Count, Is.Not.EqualTo(0));

        var dpm = container.Get<IDataProtectionManagerFactory>();
        Assert.That(dpm, Is.Not.Null);

        // Act 
        var instance = dpm.CreateInstance(filePath);

        // Assert
        Assert.That(instance, Is.Not.Null);
    }
}
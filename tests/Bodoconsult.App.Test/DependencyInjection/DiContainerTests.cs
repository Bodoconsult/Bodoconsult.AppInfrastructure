// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Runtime.InteropServices;
using Bodoconsult.App.Abstractions.DependencyInjection;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.DataProtection;
using Bodoconsult.App.Test.Helpers;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

namespace Bodoconsult.App.Test.DependencyInjection;

[TestFixture]
internal class DiContainerTests
{

    [Test]
    public void Ctor_DefaultSetup_PropsSetCorrectly()
    {
        // Arrange

        // Act  
        var diContainer = new DiContainer();

        // Assert
        Assert.That(diContainer.ServiceCollection.Count, Is.EqualTo(0));
        Assert.That(diContainer.ServiceProvider, Is.Null);
    }

    [Test]
    public void BuildServiceProvider_DefaultSetup_ServicesAddedAndServiceProviderBuilt()
    {
        // Arrange
        var diContainer = new DiContainer();

        Assert.That(diContainer.ServiceCollection.Count, Is.Zero);

        // Now add the services provided by a MS builder class
        var builder = diContainer.ServiceCollection.AddDataProtection()
                .SetApplicationName("TestApp")
                .PersistKeysToFileSystem(new DirectoryInfo(TestHelper.TempPath));

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            builder.ProtectKeysWithDpapi();
        }

        // Add your own classes
        diContainer.AddSingleton<IFileProtectionService, NoFileProtectionService>();
        diContainer.AddSingleton<IDataProtectionServiceFactory, DataProtectionServiceFactory>();
        diContainer.AddSingleton<IDataProtectionManagerFactory, DataProtectionManagerFactory>();

        // Act  
        diContainer.BuildServiceProvider();

        // Assert
        Assert.That(diContainer.ServiceCollection.Count, Is.Not.EqualTo(0));
        Assert.That(diContainer.ServiceProvider, Is.Not.Null);

        // Try to use an instance
        var dpm1 = diContainer.Get<IDataProtectionManagerFactory>();
        Assert.That(dpm1, Is.Not.Null);

        var dpm2 = diContainer.Get<IDataProtectionManagerFactory>();
        Assert.That(dpm2, Is.Not.Null);

        Assert.That(dpm1, Is.SameAs(dpm2));
    }


    [Test]
    public void Clear_DefaultSetup_ContainerCleared()
    {
        // Arrange
        var diContainer = new DiContainer();

        Assert.That(diContainer.ServiceCollection.Count, Is.EqualTo(0));

        var builder = diContainer.ServiceCollection.AddDataProtection()
                .SetApplicationName("TestApp")
                .PersistKeysToFileSystem(new DirectoryInfo(TestHelper.TempPath))
            ;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            builder.ProtectKeysWithDpapi();
        }

        diContainer.AddSingleton<IFileProtectionService, NoFileProtectionService>();
        diContainer.AddSingleton<IDataProtectionServiceFactory, DataProtectionServiceFactory>();
        diContainer.AddSingleton<IDataProtectionManagerFactory, DataProtectionManagerFactory>();

        diContainer.BuildServiceProvider();

        Assert.That(diContainer.ServiceCollection.Count, Is.Not.EqualTo(0));
        Assert.That(diContainer.ServiceProvider, Is.Not.Null);

        // Act  
        diContainer.ClearAll();

        // Assert
        Assert.That(diContainer.ServiceCollection.Count, Is.EqualTo(0));
        Assert.That(diContainer.ServiceProvider, Is.Null);
    }
}
// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.DataProtection;
using Bodoconsult.App.Test.App;

namespace Bodoconsult.App.Test.DataProtection;

internal class DataProtectionServiceTests
{
    private const string Key = "MyKey";
    private const string Secret = "Blubb";
    private const string AppName = "MyApp";

    [Test]
    public void CreateInstance_ValidPath_InstanceCreated()
    {
        // Arrange 
        var path = Globals.Instance.AppStartParameter.DataPath;

        // Act  
        var instance = DataProtectionService.CreateInstance(path, AppName);

        // Assert
        Assert.That(instance, Is.Not.Null);
    }

    [Test]
    public void Protect_ValidPath_InstanceCreated()
    {
        // Arrange 
        var path = Globals.Instance.AppStartParameter.DataPath;
        var instance = DataProtectionService.CreateInstance(path, AppName);

        // Act  
        var result = instance.Protect(Key, Secret);

        // Assert
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Unprotect_ValidPath_InstanceCreated()
    {
        // Arrange 
        var path = Globals.Instance.AppStartParameter.DataPath;
        var instance = DataProtectionService.CreateInstance(path, AppName);

        var encrypted = instance.Protect(Key, Secret);

        // Act  
        var result = instance.Unprotect(Key, encrypted);

        // Assert
        Assert.That(result, Is.Not.Null);
    }

}
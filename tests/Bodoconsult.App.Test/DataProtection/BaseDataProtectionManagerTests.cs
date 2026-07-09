// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.DataProtection;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Test.App;
using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsTCPIP;

namespace Bodoconsult.App.Test.DataProtection;

/// <summary>
/// Base class for <see cref="DataProtectionManager"/> tests
/// </summary>
internal abstract class BaseDataProtectionManagerTests
{

    protected const string Key = "MyKey";
    protected const string Key2 = "MyKey2";
    protected const string Key3 = "MyKey3";
    protected const string Secret = "Blubb";
    protected const string Secret2 = "Blabb";
    protected const string AppName = "MyApp";

    protected IFileProtectionService FileProtectionService;

    protected string Extension;

    private int _count;

    protected string ReadStringDelegate(string message)
    {
        _count++;
        Debug.Print(message);
        return $"Secret{_count}";
    }


    [Test]
    public void Ctor_ValidSetup_PropsSetCorrectly()
    {
        // Arrange 
        var path = Globals.Instance.AppStartParameter.DataPath;
        var instance = DataProtectionService.CreateInstance(path, AppName);

        var filePath = Path.Combine(path, $"appData.{Extension}");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        // Act  
        var dpm = new DataProtectionManager(instance, FileProtectionService, filePath);

        // Assert
        Assert.That(dpm, Is.Not.Null);
        Assert.That(dpm.FilePath, Is.EqualTo(filePath));
        dpm.Dispose();
    }

    [Test]
    public void Protect_ValidSetup_SecretStoredCorrectly()
    {
        // Arrange 
        var path = Globals.Instance.AppStartParameter.DataPath;
        var instance = DataProtectionService.CreateInstance(path, AppName);

        var filePath = Path.Combine(path, $"appData.{Extension}");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        var dpm = new DataProtectionManager(instance, FileProtectionService, filePath);

        // Act  
        dpm.Protect(Key, Secret);

        // Assert
        Wait.Until(() => File.Exists(filePath));
        Assert.That(File.Exists(filePath), Is.EqualTo(true));
        dpm.Dispose();
    }

    [Test]
    public void Protect_ValidSetupSecretUpdated_SecretStoredCorrectly()
    {
        // Arrange 
        var path = Globals.Instance.AppStartParameter.DataPath;
        var instance = DataProtectionService.CreateInstance(path, AppName);

        var filePath = Path.Combine(path, $"appData.{Extension}");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        var dpm = new DataProtectionManager(instance, FileProtectionService, filePath);

        dpm.Protect(Key, Secret);
        dpm.Protect(Key, Secret2);

        // Act  
        var result = dpm.Unprotect(Key);

        // Assert
        Assert.That(result, Is.EqualTo(Secret2));
        dpm.Dispose();
    }

    [Test]
    public void Unprotect_ValidSetup_SecretUnprotectedCorrectly()
    {
        // Arrange 
        var path = Globals.Instance.AppStartParameter.DataPath;
        var instance = DataProtectionService.CreateInstance(path, AppName);

        var filePath = Path.Combine(path, $"appData.{Extension}");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        var dpm = new DataProtectionManager(instance, FileProtectionService, filePath);

        dpm.Protect(Key, Secret);

        // Act  
        var result = dpm.Unprotect(Key);

        // Assert
        Assert.That(result, Is.EqualTo(Secret));
        dpm.Dispose();
    }

    [Test]
    public void LoadValues_ValidSetup_SecretUnprotectedCorrectly()
    {
        // Arrange 
        var path = Globals.Instance.AppStartParameter.DataPath;
        var instance = DataProtectionService.CreateInstance(path, AppName);

        var filePath = Path.Combine(path, $"appData.{Extension}");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        var dpm = new DataProtectionManager(instance, FileProtectionService, filePath);

        dpm.Protect(Key, Secret);

        Wait.Until(() => File.Exists(filePath));

        dpm.ClearAll();

        Assert.That(dpm.Values.Count, Is.EqualTo(0));

        // Act
        dpm.LoadValues();

        var result = dpm.Unprotect(Key);

        // Assert
        Assert.That(result, Is.EqualTo(Secret));
        dpm.Dispose();
    }

    [Test]
    public void AddKey_ValidSetup_SecretUnprotectedCorrectly()
    {
        // Arrange 
        var path = Globals.Instance.AppStartParameter.DataPath;
        var instance = DataProtectionService.CreateInstance(path, AppName);

        var filePath = Path.Combine(path, $"appData.{Extension}");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        var dpm = new DataProtectionManager(instance, FileProtectionService, filePath);

        dpm.AddKey(Key);
        dpm.AddKey(Key3);
        dpm.AddKey(Key2);

        // Act
        var result = dpm.Keys.Count;

        // Assert
        Assert.That(result, Is.EqualTo(3));
        dpm.Dispose();
    }

    [Test]
    public void SaveValues_OnlyAddKey_FileNotSaved()
    {
        // Arrange 
        var path = Globals.Instance.AppStartParameter.DataPath;
        var instance = DataProtectionService.CreateInstance(path, AppName);

        var filePath = Path.Combine(path, $"appData.{Extension}");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        Assert.That(File.Exists(filePath), Is.False);

        var dpm = new DataProtectionManager(instance, FileProtectionService, filePath);

        Assert.That(File.Exists(filePath), Is.False);

        dpm.AddKey(Key);
        dpm.AddKey(Key3);
        dpm.AddKey(Key2);

        Assert.That(File.Exists(filePath), Is.False);

        // Act
        dpm.SaveValues();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(File.Exists(filePath), Is.False);

            // Assert
            Wait.Until(() => File.Exists(filePath));
            Assert.That(File.Exists(filePath), Is.False);
        }

        dpm.Dispose();
    }

    [Test]
    public void AskForInitialLoadValues_ValidSetup_SecretUnprotectedCorrectly()
    {
        // Arrange 
        var path = Globals.Instance.AppStartParameter.DataPath;
        var instance = DataProtectionService.CreateInstance(path, AppName);

        var filePath = Path.Combine(path, $"appData.{Extension}");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        var dpm = new DataProtectionManager(instance, FileProtectionService, filePath);
        dpm.ReadStringDelegate = ReadStringDelegate;

        dpm.AddKey(Key);
        dpm.AddKey(Key3);
        dpm.AddKey(Key2);

        // Act
        dpm.AskForInitialLoadValues();

        // Assert
        Wait.Until(() => File.Exists(filePath));
        Assert.That(File.Exists(filePath), Is.True);
    }
}
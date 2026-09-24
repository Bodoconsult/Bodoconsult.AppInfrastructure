// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.DataProtection;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Test.App;
using Bodoconsult.App.Test.TestData;
using System.Diagnostics;

namespace Bodoconsult.App.Test.DataProtection;

/// <summary>
/// Base class for <see cref="DataProtectionManager"/> tests
/// </summary>
internal abstract class BaseDataProtectionManagerTests
{
    private readonly string _path = Globals.Instance.AppStartParameter.DataPath ?? Path.GetTempPath();

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
        var instance = DataProtectionService.CreateInstance(_path, AppName);

        var filePath = Path.Combine(_path, $"appData.{Extension}");
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
        var instance = DataProtectionService.CreateInstance(_path, AppName);

        var filePath = Path.Combine(_path, $"appData.{Extension}");
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
        var instance = DataProtectionService.CreateInstance(_path, AppName);

        var filePath = Path.Combine(_path, $"appData.{Extension}");
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
        var instance = DataProtectionService.CreateInstance(_path, AppName);

        var filePath = Path.Combine(_path, $"appData.{Extension}");
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
        var instance = DataProtectionService.CreateInstance(_path, AppName);

        var filePath = Path.Combine(_path, $"appData.{Extension}");
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
        CreateDataProtectionManager(out var dpm);

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
        var filePath = CreateDataProtectionManager(out var dpm);

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
        var filePath = CreateDataProtectionManager(out var dpm);

        // Act
        dpm.AskForInitialLoadValues();

        // Assert
        Wait.Until(() => File.Exists(filePath));
        Assert.That(File.Exists(filePath), Is.True);
    }

    [Test]
    public void Protect_EntityMultipleSecrets_PropsWithDataProtectionSecretAttributeProtected()
    {
        // Arrange 
        const string secret = "Secret";
        const string secret2 = "Secret2";
        const string name = "BlubbEps1";

        var entity = new EntityWithSecrets
        {
            Name = name,
            Secret = secret,
            Secret2 = secret2
        };

        CreateDataProtectionManager(out var dpm);

        // Act  
        dpm.Protect(entity);

        // Assert
        Assert.That(entity.Name, Is.EqualTo(name));
        Assert.That(entity.Secret, Is.Not.EqualTo(secret));
        Assert.That(entity.Secret2, Is.Not.EqualTo(secret2));
        Assert.That(entity.Secret, Is.Not.EqualTo(entity.Secret2));
    }

    [Test]
    public void Unprotect_EntityMultipleSecrets_PropsWithDataProtectionSecretAttributeProtected()
    {
        // Arrange 
        const string secret = "Secret";
        const string secret2 = "Secret2";
        const string name = "BlubbEps2";

        var entity = new EntityWithSecrets
        {
            Name = name,
            Secret = secret,
            Secret2 = secret2
        };

        CreateDataProtectionManager(out var dpm);

        dpm.Protect(entity);

        Assert.That(entity.Name, Is.EqualTo(name));
        Assert.That(entity.Secret, Is.Not.EqualTo(secret));
        Assert.That(entity.Secret2, Is.Not.EqualTo(secret2));
        Assert.That(entity.Secret, Is.Not.EqualTo(entity.Secret2));

        // Act  
        dpm.Unprotect(entity);

        // Assert
        Assert.That(entity.Name, Is.EqualTo(name));
        Assert.That(entity.Secret, Is.EqualTo(secret));
        Assert.That(entity.Secret2, Is.EqualTo(secret2));

    }

    [Test]
    public void Protect_EntityMultipleSecretsWithUid_PropsWithDataProtectionSecretAttributeProtected()
    {
        // Arrange 
        const string secret = "Secret";
        const string secret2 = "Secret2";
        var uid = Guid.NewGuid();

        var entity = new EntityWithUidWithSecrets
        {
            Uid = uid,
            Secret = secret,
            Secret2 = secret2
        };

        CreateDataProtectionManager(out var dpm);

        // Act  
        dpm.Protect(entity);

        // Assert
        Assert.That(entity.Uid, Is.EqualTo(uid));
        Assert.That(entity.Secret, Is.Not.EqualTo(secret));
        Assert.That(entity.Secret2, Is.Not.EqualTo(secret2));
        Assert.That(entity.Secret, Is.Not.EqualTo(entity.Secret2));
    }

    [Test]
    public void Unprotect_EntityMultipleSecretsWithUid_PropsWithDataProtectionSecretAttributeProtected()
    {
        // Arrange 
        const string secret = "Secret";
        const string secret2 = "Secret2";
        var uid = Guid.NewGuid();

        var entity = new EntityWithUidWithSecrets
        {
            Uid = uid,
            Secret = secret,
            Secret2 = secret2
        };

        CreateDataProtectionManager(out var dpm);

        dpm.Protect(entity);

        Assert.That(entity.Uid, Is.EqualTo(uid));
        Assert.That(entity.Secret, Is.Not.EqualTo(secret));
        Assert.That(entity.Secret2, Is.Not.EqualTo(secret2));
        Assert.That(entity.Secret, Is.Not.EqualTo(entity.Secret2));

        // Act  
        dpm.Unprotect(entity);

        // Assert
        Assert.That(entity.Uid, Is.EqualTo(uid));
        Assert.That(entity.Secret, Is.EqualTo(secret));
        Assert.That(entity.Secret2, Is.EqualTo(secret2));
    }

    private string CreateDataProtectionManager(out DataProtectionManager dpm)
    {
        var instance = DataProtectionService.CreateInstance(_path, AppName);

        var filePath = Path.Combine(_path, $"appData.{Extension}");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        dpm = new DataProtectionManager(instance, FileProtectionService, filePath);
        dpm.ReadStringDelegate = ReadStringDelegate;

        dpm.AddKey(Key);
        dpm.AddKey(Key3);
        dpm.AddKey(Key2);
        return filePath;
    }
}
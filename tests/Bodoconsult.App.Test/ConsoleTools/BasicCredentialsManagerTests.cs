// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.DataProtection.ConsoleTools;
using Bodoconsult.App.Helpers;

namespace Bodoconsult.App.Test.ConsoleTools;

[TestFixture]
internal class BasicCredentialsManagerTests
{
    private void CheckFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    [Test]
    public void Ctor_ValidSetup_PropsSetCorrectly()
    {
        // Arrange 

        // Act  
        var m = new BasicCredentialsManager();

        // Assert
        Assert.That(m.Credentials, Is.Null);
    }

    [Test]
    public void Init_NoDataSaved_DoesNotThrow()
    {
        // Arrange 
        var m = new BasicCredentialsManager
        {
            FolderPath = Path.GetTempPath()
        };

        CheckFile(m.FilePath);

        // Act  
        Assert.DoesNotThrow(() =>
        {
            m.Init();
        });
            
        // Assert
        Assert.That(m.Credentials, Is.Not.Null);
    }

    [Test]
    public void Init_NoDataSaved_Throws()
    {
        // Arrange 
        var m = new BasicCredentialsManager();

        // Act  
        Assert.Throws<ArgumentNullException>(() =>
        {
            m.Init();
        });

        // Assert
        Assert.That(m.Credentials, Is.Null);
    }

    [Test]
    public void Protect_NoDataSaved_DoesNotThrow()
    {
        // Arrange 
        var m = new BasicCredentialsManager
        {
            FolderPath = Path.GetTempPath()
        };
        m.Init();
        
        Assert.That(m.Credentials, Is.Not.Null);
        const string s1 = "test1";
        m.Credentials.Password = s1;

        const string s2 = "test2";
        m.Credentials.Url = s2;

        const string s3 = "test3";
        m.Credentials.Username = s3;

        CheckFile(m.FilePath);

        // Act  
        m.Protect();

        // Assert
        Wait.Until(() => File.Exists(m.FilePath));
        Assert.That(File.Exists(m.FilePath), Is.True);

        Assert.That(m.Credentials.Password, Is.EqualTo(s1));
        Assert.That(m.Credentials.Url, Is.EqualTo(s2));
        Assert.That(m.Credentials.Username, Is.EqualTo(s3));

        CheckFile(m.FilePath);
    }

    [Test]
    public void Init_LoadExistingFile_DoesNotThrow()
    {
        // Arrange 
        var m = new BasicCredentialsManager
        {
            FolderPath = Path.GetTempPath()
        };

        CheckFile(m.FilePath);

        m.Init();

        Assert.That(m.Credentials, Is.Not.Null);
        const string s1 = "test1";
        m.Credentials.Password = s1;

        const string s2 = "test2";
        m.Credentials.Url = s2;

        const string s3 = "test3";
        m.Credentials.Username = s3;

        m.Protect();

        Wait.Until(() => File.Exists(m.FilePath));
        //Assert.That(File.Exists(m.FilePath), Is.True);

        // Act. next app start  
        var m1 = new BasicCredentialsManager
        {
            FolderPath = Path.GetTempPath()
        };
        m1.Init();
        m1.Unprotect();

        // Assert
        Assert.That(m1.Credentials, Is.Not.Null);
        Assert.That(m1.Credentials.Password, Is.EqualTo(s1));
        Assert.That(m1.Credentials.Url, Is.EqualTo(s2));
        Assert.That(m1.Credentials.Username, Is.EqualTo(s3));


        CheckFile(m.FilePath);
    }
}
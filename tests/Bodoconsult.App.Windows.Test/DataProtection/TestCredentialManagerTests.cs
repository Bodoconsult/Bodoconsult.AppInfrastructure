// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.IO;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Windows.DataProtection;
using NUnit.Framework;

namespace Bodoconsult.App.Windows.Test.DataProtection;

[TestFixture]
internal class TestCredentialManagerTests
{
    [Test]
    public void LoadCredentials_ValidCredentials_CredentialsLoaded()
    {
        // Arrange 
        var cm = new TestCredentialManager<TestCredentials>("Bodoconsult.App.Windows.Test");

        var cred = new TestCredentials
        {
            Password = "Blubb",
            AppToken = "Blabb"
        };

        var json = SimpleJsonHelper.Serialize(cred);

        var importFileName = Path.Combine(FileSystemHelper.GetAppPath(), "cred.json");

        FileSystemHelper.DeleteFile(importFileName);
        FileSystemHelper.DeleteFile(cm.CredentialFileName);

        File.WriteAllText(importFileName, json);
        
        // Act  
        cm.LoadCredentials();

        // Assert
        Assert.That(cm.Credentials, Is.Not.Null);
        Assert.That(File.Exists(importFileName), Is.False);

        // Act 2
        var cm2 = new TestCredentialManager<TestCredentials>("Bodoconsult.App.Windows.Test");
        cm2.LoadCredentials();

        // Assert 2
        Assert.That(cm2.Credentials, Is.Not.Null);
    }
}
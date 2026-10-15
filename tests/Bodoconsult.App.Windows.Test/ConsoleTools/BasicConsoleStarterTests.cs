// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System.IO;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Windows.ConsoleTools;
using Bodoconsult.App.Windows.Test.Helpers;
using NUnit.Framework;

namespace Bodoconsult.App.Windows.Test.ConsoleTools;

[TestFixture]
internal class BasicConsoleStarterTests
{
    [Test]
    public void HashAppPassword_ValidPassword_HashCreated()
    {
        // Arrange 
        string[] args = [];
        var starter = new BasicConsoleStarter("Basic", args);

        DeleteFile(starter.HashFileName);

        const string pwd = "test";

        // Act  
        starter.HashAppPassword(pwd);

        // Assert
        Assert.That(starter.AppPasswordHash, Is.Not.Null);
        Assert.That(starter.AppPasswordHash.Length, Is.Not.Zero);
    }

    [Test]
    public void LoadAppPasswordHash_ValidPassword_HashLoaded()
    {
        // Arrange 
        string[] args = [];
        var starter = new BasicConsoleStarter("Basic", args);

        DeleteFile(starter.HashFileName);

        const string pwd = "test";

        starter.HashAppPassword(pwd);

        starter = new BasicConsoleStarter("Basic", args);

        // Act  
        starter.LoadAppPasswordHash();

        // Assert
        Assert.That(starter.AppPasswordHash, Is.Not.Null);
        Assert.That(starter.AppPasswordHash.Length, Is.Not.Zero);
    }

    [Test]
    public void ValidateAppPassword_ValidPassword_HashValidated()
    {
        // Arrange 
        string[] args = [];
        var starter = new BasicConsoleStarter("Basic", args);

        const string pwd = "test";

        DeleteFile(starter.HashFileName);

        starter.HashAppPassword(pwd);

        // Act  
        var result = starter.ValidateAppPassword(pwd);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void SaveSmtpMailAccount_ValidMailAccount_FileSaved()
    {
        // Arrange 
        var ma = new SmtpMailAccount();
        string[] args = [];
        var starter = new BasicConsoleStarter("Basic", args);

        DeleteFile(starter.SmtpMailAccountFileName);

        // Act  
        starter.SaveSmtpMailAccount(ma);

        // Assert
        Assert.That(File.Exists(starter.SmtpMailAccountFileName));
    }

    [Test]
    public void LoadSmtpMailAccount_ValidMailAccount_FileLoaded()
    {
        // Arrange 
        var ma = new SmtpMailAccount();
        string[] args = [];
        var starter = new BasicConsoleStarter("Basic", args);

        DeleteFile(starter.SmtpMailAccountFileName);

        starter.SaveSmtpMailAccount(ma);

        // Act  
        var result = starter.LoadSmtpMailAccount();

        // Assert
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void LoadSmtpConfig_ValidFile_FileLoaded()
    {
        // Arrange 
        var ma = new SmtpMailAccount();
        string[] args = [];
        var starter = new BasicConsoleStarter("Basic", args);

        DeleteFile(starter.SmtpMailAccountFileName);

        starter.SaveSmtpMailAccount(ma);

        // Act and assert
        Assert.DoesNotThrow(starter.LoadSmtpConfig);
    }

    [Test]
    public void LoadSmtpConfig_ValidImport_FileLoaded()
    {
        // Arrange 
        var ma = new SmtpMailAccount();
        string[] args = [];
        var starter = new BasicConsoleStarter("Basic", args);

        DeleteFile(starter.SmtpMailAccountFileName);
        DeleteFile(starter.SmtpMailAccountImportFileName);

        JsonHelper.SaveAsFile(starter.SmtpMailAccountImportFileName, ma);

        Assert.That(File.Exists(starter.SmtpMailAccountImportFileName), Is.True);

        // Act  
        Assert.DoesNotThrow(starter.LoadSmtpConfig);

        // Assert
        Assert.That(File.Exists(starter.SmtpMailAccountImportFileName), Is.False);
    }

    [Test]
    public void LoadO365Config_ValidFile_FileLoaded()
    {
        // Arrange 
        var ma = new O365MailAccount();
        string[] args = [];
        var starter = new BasicConsoleStarter("Basic", args);

        DeleteFile(starter.O365MailAccountFileName);

        starter.SaveO365MailAccount(ma);

        // Act and assert
        Assert.DoesNotThrow(starter.LoadO365Config);
    }

    [Test]
    public void LoadO365Config_ValidImport_FileLoaded()
    {
        // Arrange 
        var ma = new O365MailAccount();
        string[] args = [];
        var starter = new BasicConsoleStarter("Basic", args);

        DeleteFile(starter.O365MailAccountFileName);
        DeleteFile(starter.O365MailAccountImportFileName);

        JsonHelper.SaveAsFile(starter.O365MailAccountImportFileName, ma);

        Assert.That(File.Exists(starter.O365MailAccountImportFileName), Is.True);

        // Act  
        Assert.DoesNotThrow(starter.LoadO365Config);

        // Assert
        Assert.That(File.Exists(starter.O365MailAccountImportFileName), Is.False);
    }

    [Test]
    public void SaveO365MailAccount_ValidMailAccount_FileSaved()
    {
        // Arrange 
        var ma = new O365MailAccount();
        string[] args = [];
        var starter = new BasicConsoleStarter("Basic", args);

        DeleteFile(starter.O365MailAccountFileName);

        // Act  
        starter.SaveO365MailAccount(ma);

        // Assert
        Assert.That(File.Exists(starter.O365MailAccountFileName));
    }

    [Test]
    public void LoadO365MailAccount_ValidMailAccount_FileLoaded()
    {
        // Arrange 
        var ma = new O365MailAccount();
        string[] args = [];
        var starter = new BasicConsoleStarter("Basic", args);

        DeleteFile(starter.O365MailAccountFileName);

        starter.SaveO365MailAccount(ma);

        // Act  
        var result = starter.LoadO365MailAccount();

        // Assert
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void LoadWebCredentialsConfig_ValidFile_FileLoaded()
    {
        // Arrange 
        var ma = new WebCredentials();
        string[] args = [];
        var starter = new BasicConsoleStarter("Basic", args);

        DeleteFile(starter.WebCredentialsFileName);

        starter.SaveWebCredentials(ma);

        // Act and assert
        Assert.DoesNotThrow(starter.LoadWebCredentialsConfig);
    }

    [Test]
    public void LoadWebCredentialsConfig_ValidImport_FileLoaded()
    {
        // Arrange 
        var ma = new WebCredentials();
        string[] args = [];
        var starter = new BasicConsoleStarter("Basic", args);

        DeleteFile(starter.WebCredentialsFileName);
        DeleteFile(starter.WebCredentialsImportFileName);

        JsonHelper.SaveAsFile(starter.WebCredentialsImportFileName, ma);

        Assert.That(File.Exists(starter.WebCredentialsImportFileName), Is.True);

        // Act  
        Assert.DoesNotThrow(starter.LoadWebCredentialsConfig);

        // Assert
        Assert.That(File.Exists(starter.WebCredentialsImportFileName), Is.False);
    }

    [Test]
    public void SaveWebCredentials_ValidMailAccount_FileSaved()
    {
        // Arrange 
        var ma = new WebCredentials();
        string[] args = [];
        var starter = new BasicConsoleStarter("Basic", args);

        DeleteFile(starter.WebCredentialsFileName);

        // Act  
        starter.SaveWebCredentials(ma);

        // Assert
        Assert.That(File.Exists(starter.WebCredentialsFileName));
    }

    [Test]
    public void LoadWebCredentials_ValidMailAccount_FileLoaded()
    {
        // Arrange 
        var ma = new WebCredentials();
        string[] args = [];
        var starter = new BasicConsoleStarter("Basic", args);

        DeleteFile(starter.WebCredentialsFileName);

        starter.SaveWebCredentials(ma);

        // Act  
        var result = starter.LoadWebCredentials();

        // Assert
        Assert.That(result, Is.Not.Null);
    }

    private static void DeleteFile(string fileName)
    {
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
    }
}
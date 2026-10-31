// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System.IO;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Helpers;
using Bodoconsult.Web.Mail.Helpers;
using JobExecuter.Dal.App;
using JobExecuter.Test.Helpers;
using NUnit.Framework;

namespace JobExecuter.Test.App;

[TestFixture]
internal class AppStarterTests
{
    [Test]
    public void HashAppPassword_ValidPassword_HashCreated()
    {
        // Arrange 
        string[] args = [];
        var starter = new JobExcuterStarter(args);

        FileSystemHelper.DeleteFile(starter.HashFileName);

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
        var starter = new JobExcuterStarter(args);

        FileSystemHelper.DeleteFile(starter.HashFileName);

        const string pwd = "test";

        starter.HashAppPassword(pwd);

        starter = new JobExcuterStarter(args);

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
        var starter = new JobExcuterStarter(args);

        const string pwd = "test";

        FileSystemHelper.DeleteFile(starter.HashFileName);

        starter.HashAppPassword(pwd);

        // Act  
        var result = starter.ValidateAppPassword(pwd);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void CryptJson_ValidJsonFile_CryptedFileCreated()
    {
        // Arrange 
        var jsonFile = Path.Combine(TestDataHelper.TestDataPath, "job.json");
        var cjsonFile = Path.Combine(TestDataHelper.TestDataPath, "job.cjson");

        FileSystemHelper.DeleteFile(cjsonFile);

        string[] args = [];
        var starter = new JobExcuterStarter(args);

        FileSystemHelper.DeleteFile(starter.HashFileName);

        const string pwd = "test";

        starter.HashAppPassword(pwd);

        starter.ValidateAppPassword(pwd);

        // Act  
        starter.CryptJson(jsonFile);

        // Assert
        Assert.That(File.Exists(cjsonFile), Is.True);
    }

    [Test]
    public void DecryptJson_ValidJsonFile_CryptedFileCreated()
    {
        // Arrange 
        var jsonFile = Path.Combine(TestDataHelper.TestDataPath, "job.json");
        var cjsonFile = Path.Combine(TestDataHelper.TestDataPath, "job.cjson");

        FileSystemHelper.DeleteFile(cjsonFile);

        string[] args = [];
        var starter = new JobExcuterStarter(args);

        FileSystemHelper.DeleteFile(starter.HashFileName);

        const string pwd = "test";

        starter.HashAppPassword(pwd);

        starter.ValidateAppPassword(pwd);

        starter.CryptJson(jsonFile);

        // Act  
        var job = starter.DecryptJson(cjsonFile);

        // Assert
        Assert.That(job, Is.Not.Null);
    }

    [Test]
    public void SaveSmtpMailAccount_ValidMailAccount_FileSaved()
    {
        // Arrange 
        var ma = new SmtpMailAccount();
        string[] args = [];
        var starter = new JobExcuterStarter(args);

        FileSystemHelper.DeleteFile(starter.SmtpMailAccountFileName);

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
        var starter = new JobExcuterStarter(args);

        FileSystemHelper.DeleteFile(starter.SmtpMailAccountFileName);

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
        var starter = new JobExcuterStarter(args);

        FileSystemHelper.DeleteFile(starter.SmtpMailAccountFileName);

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
        var starter = new JobExcuterStarter(args);

        FileSystemHelper.DeleteFile(starter.SmtpMailAccountFileName);
        FileSystemHelper.DeleteFile(starter.SmtpMailAccountImportFileName);

        JsonHelper.SaveAsFile(starter.SmtpMailAccountImportFileName, ma);

        Assert.That(File.Exists(starter.SmtpMailAccountImportFileName), Is.True);

        // Act  
        Assert.DoesNotThrow(starter.LoadSmtpConfig);

        // Assert
        Assert.That(File.Exists(starter.SmtpMailAccountImportFileName), Is.False);
    }


    [Test]
    public void RunJobTask_ValidJsonFile_CryptedFileCreated()
    {
        // Arrange 
        var jsonFile = Path.Combine(TestDataHelper.TestDataPath, "job.json");
        var cjsonFile = Path.Combine(TestDataHelper.TestDataPath, "job.cjson");

        FileSystemHelper.DeleteFile(cjsonFile);

        string[] args = [];
        var starter = new JobExcuterStarter(args);

        FileSystemHelper.DeleteFile(starter.HashFileName);

        const string pwd = "test";

        starter.HashAppPassword(pwd);

        starter.ValidateAppPassword(pwd);

        starter.CryptJson(jsonFile);

        // Act  
        var result = starter.RunJobTask(cjsonFile);

        // Assert
        Assert.That(result, Is.Zero);
    }
}
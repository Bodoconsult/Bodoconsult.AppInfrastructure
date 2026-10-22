Bodoconsult.App.Windows
==========================

# Overview

## What does the library

Bodoconsult.App.Windows provides features related to Microsoft Windows operating system.

Current features are:

>   [Logging to OS event log via EventLogLoggingProviderConfigurator](#logging-to-os-event-log-via-eventlogloggingproviderconfigurator) 

>   [Sending TOAST messages via WindowsToastMessagingService](#sending-toast-messages-via-windowstoastmessagingservice)

>   [Using clipboard with Clipboard class](#using-clipboard-with-clipboard-class)

>   [Using DataProtectionService to protect critical string data](#using-dataprotectionservice-to-protect-sensible-string-data)

>   [Using TestCredentialManager to protect secrets for unit test environments](#using-testcredentialmanager-to-protect-secrets-for-unit-test-environments)

>   [Icon extraction as bitmap](#using-iconsasfileshelper-to-get-gif-images-from-an-app-icon)

>   [Reading data from url files (get the included link address in file)](#using-filesystemurl-classes-extract-a-link-address)
  

## How to use the library

The source code contain a NUnit test classes, the following source code is extracted from. The samples below show the most helpful use cases for the library.

# Logging to OS event log via EventLogLoggingProviderConfigurator 

With EventLogLoggingProviderConfigurator you can enhance the logging system from Bodoconsult.App based on IAppLoggerProxy with logging to OS event log.

``` csharp

```


# Sending TOAST messages via WindowsToastMessagingService

``` csharp
[Test]
public void SendSimpleToastMessage_NotifyRequestRecord_NotificationIsShown()
{
    // Arrange 
    var request = new NotifyRequestRecord
    {
        Text = "Das ist eine Message",
        Title = "Title"
    };

    var s = new WindowsToastMessagingService();

    // Act and assert
    Assert.DoesNotThrow(() =>
    {
        s.SendSimpleToastMessage(request);
    });
}
```
# Using clipboard with Clipboard class

``` csharp
[Test]
public void TestSetText()
{
    // Arrange
    const string text = "CopyToClipboard";

    // Act
    Clipboard.SetText(text);

    var result = Clipboard.GetText();


    // Assert
    Assert.That(result, Is.EqualTo(text));
}
```
# Using DataProtectionService to protect sensible string data 

``` csharp
[SetUp]
public void Setup()
{
    _service = new DataProtectionService();
}

[Test]
public void ProtectUnprotect_CurrentUser_Successful()
{
    const string secret = "Test123!";

    _service.CurrentDataProtectionScope = DataProtectionScope.CurrentUser;

    //Encrypt the data.
    var encryptedSecret = _service.ProtectString(secret);
    Debug.Print($"The encrypted byte array is: {ArrayHelper.GetStringFromArray(encryptedSecret)}");

    // Decrypt the data and store in a byte array.
    var originalData = _service.UnprotectString(encryptedSecret);
    Debug.Print($"The original data is: {originalData}"); 

    Assert.That(originalData, Is.EqualTo(secret));
}

[Test]
public void ProtectUnprotect_LocalMachine_Successful()
{
    const string secret = "Test123!";

    _service.CurrentDataProtectionScope = DataProtectionScope.LocalMachine;

    //Encrypt the data.
    var encryptedSecret = _service.ProtectString(secret);
    Debug.Print($"The encrypted byte array is: {ArrayHelper.GetStringFromArray(encryptedSecret)}");

    // Decrypt the data and store in a byte array.
    var originalData = _service.UnprotectString(encryptedSecret);
    Debug.Print($"The original data is: {originalData}");

    Assert.That(originalData, Is.EqualTo(secret));
}
```

# Using TestCredentialManager to protect secrets for unit test environments

``` csharp
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
```

# Using FileSystemUrl classes: extract a link address

``` csharp
[Test]
public void TestRead()
{
    // Arrange
    var url = Path.Combine(TestHelper.TestDataPath, "Bodoconsult.url");

    var fri = new FileInfo(url);

    var urlFile = new FileSystemUrl(fri);

    // Act
    urlFile.Read();

    // Assert
    Assert.That(urlFile.Url, Is.EqualTo("http://www.bodoconsult.de/"));
    Assert.That(urlFile.Caption, Is.EqualTo("Bodoconsult"));
}
```

# Using IconsAsFilesHelper to get GIF images from an app icon

``` csharp
[Test]
public void SaveIcons_OfficeDocuments_IconExtracted()
{
    var iconDocx = Path.Combine(TestHelper.OutputPath, "docx.gif");
    if (File.Exists(iconDocx))
    {
        File.Delete(iconDocx);
    }

    var iconXlsx = Path.Combine(TestHelper.OutputPath, "xlsx.gif");
    if (File.Exists(iconXlsx))
    {
        File.Delete(iconXlsx);
    }


    var icons = new IconsAsFilesHelper {IconPath = TestHelper.OutputPath};

    var path = Path.Combine(TestHelper.TestDataPath, "Test.docx");

    var fri = new FileInfo(path);
    icons.AddExtension(fri);


    path = Path.Combine(TestHelper.TestDataPath, "Test.xlsx");

    fri = new FileInfo(path);
    icons.AddExtension(fri);

    icons.SaveIcons();

    Assert.That(File.Exists(iconDocx));
    Assert.That(File.Exists(iconXlsx));
}
```

# About us

Bodoconsult (<http://www.bodoconsult.de>) is a Munich based software development company from Germany.

Robert Leisner is senior software developer at Bodoconsult. See his profile on <http://www.bodoconsult.de/Curriculum_vitae_Robert_Leisner.pdf>.


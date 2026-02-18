// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.StringToFileServices;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Test.Helpers;

namespace Bodoconsult.App.Test.StringToFileServices;

public class StringToFileServiceTests
{
    private readonly string _filePath = Path.Combine(TestHelper.TempPath, "test.config");

    [Test]
    public void Ctor_ValidSetup_PropsSetCorrectly()
    {
        // Arrange 

        // Act  
        var service = new StringToFileService();

        // Assert
        Assert.That(string.IsNullOrEmpty( service.FilePath));
    }

    [Test]
    public void WriteToFile_ValidSetup_FileWritten()
    {
        // Arrange 
        const string item = "Blubb";

        var service = new StringToFileService
        {
            FilePath = _filePath
        };

        service.Start();

        // Act  
        service.WriteToFile(item);

        Wait.Until(() => File.Exists(_filePath));

        // Assert
        service.Stop();

        Assert.That(File.Exists(_filePath));
    }

    [Test]
    public void GetFileContent_ValidSetup_FileRead()
    {
        // Arrange 
        const string item = "Blubb";

        var service = new StringToFileService
        {
            FilePath = _filePath
        };

        service.Start();

        service.WriteToFile(item);

        Wait.Until(() => File.Exists(_filePath));

        Assert.That(File.Exists(_filePath));

        // Act  
        var result = service.GetFileContent();

        // Assert
        service.Stop();

        Assert.That(result, Is.EqualTo(item));
    }
}
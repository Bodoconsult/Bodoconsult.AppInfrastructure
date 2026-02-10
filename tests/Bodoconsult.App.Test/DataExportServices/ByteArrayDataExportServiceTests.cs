// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.DataExportServices;
using System.Text;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Test.Helpers;

namespace Bodoconsult.App.Test.DataExportServices;

[TestFixture]
internal class ByteArrayDataExportServiceTests
{
    [SetUp]
    public void SetUp()
    {
        TestHelper.CleanTempPath();

    }

    [Test]
    public void Ctor_ValidDefaultSetup_PropsSetCorretctly()
    {
        // Arrange 

        // Act  
        var service = new ByteArrayDataExportService
        {
            FileExtension = "bin"
        };

        // Assert
        Assert.That(service.CurrentFilePath, Is.Null);
    }

    [Test]
    public void Ctor_ValidNonDefaultSetup_PropsSetCorretctly()
    {
        // Arrange 

        // Act  
        var service = new ByteArrayDataExportService()
        {
            FileName = "Export",
            FileExtension = "bin"
        };

        // Assert
        Assert.That(service.CurrentFilePath, Is.Null);
    }

    [Test]
    public void CreateCurrentFilePath_ValidDefaultSetup_ValidPathReturned()
    {
        // Arrange 
        var service = new ByteArrayDataExportService
        {
            FileExtension = "bin"
        };

        // Act  
        var result = service.CreateCurrentFilePath();

        // Assert
        Assert.That(string.IsNullOrEmpty(result), Is.False);
    }

    [Test]
    public void Add_ValidDefaultSetup_FileWritten()
    {
        // Arrange 
        const string text = "Blubb";

        var data = Encoding.UTF8.GetBytes(text);

        var service = new ByteArrayDataExportService
        {
            FileExtension = "bin"
        };
        service.Start();

        // Act  
        service.Add(data);

        service.Stop();

        // Assert
        Assert.That(string.IsNullOrEmpty(service.CurrentFilePath), Is.False);
        Assert.That(File.Exists(service.CurrentFilePath));
        Assert.That(service.RowCounter, Is.GreaterThanOrEqualTo(1));

        FileSystemHelper.RunInDebugMode(service.CurrentFilePath);
    }


    [Test]
    public void Add_ValidDefaultSetup1000_FileWritten()
    {
        // Arrange 
        const string text = "Blubb\r\n";

        var data = Encoding.UTF8.GetBytes(text);

        var service = new ByteArrayDataExportService
        {
            FileExtension = "bin"
        };
        service.Start();

        // Act
        for (var i = 0; i < 1000; i++)
        {
            service.Add(data);
        }

        service.Stop();

        // Assert
        Assert.That(string.IsNullOrEmpty(service.CurrentFilePath), Is.False);
        Assert.That(File.Exists(service.CurrentFilePath));
        Assert.That(service.RowCounter, Is.GreaterThanOrEqualTo(1000));

        FileSystemHelper.RunInDebugMode(service.CurrentFilePath);
    }


    [Test]
    public void Add_ValidDefaultSetup1000000_FileWritten()
    {
        // Arrange 
        const string text = "Blubb\r\n";

        var data = Encoding.UTF8.GetBytes(text);

        var service = new ByteArrayDataExportService
        {
            FileExtension = "bin"
        };
        service.Start();

        // Act
        for (var i = 0; i < 1000000; i++)
        {
            service.Add(data);
        }

        service.Stop();

        // Assert
        Assert.That(string.IsNullOrEmpty(service.CurrentFilePath), Is.False);
        Assert.That(File.Exists(service.CurrentFilePath));
        Assert.That(service.RowCounter, Is.GreaterThanOrEqualTo(1000000));

        FileSystemHelper.RunInDebugMode(service.CurrentFilePath);
    }
}